using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstMobileApp.Module.DataBase
{
	public static class TaskExtensions
	{
		// NOTE: Async void is intentional here. This provides a way
		// to call an async method from the constructor while
		// communicating intent to fire and forget, and allow
		// handling of exceptions
		public static async void SafeFireAndForget(this Task task, bool returnToCallingContext, Action<Exception> onException = null)
		{
			try
			{
				await task.ConfigureAwait(returnToCallingContext);
			}

			// if the provided action is not null, catch and
			// pass the thrown exception
			catch (Exception ex) when (onException != null)
			{
				onException(ex);
			}
		}
	}

	public class DataBase<T> where T : new()
	{
		SQLiteAsyncConnection Database => GetLazyInitializer().Value;
		bool initialized = false;

		private readonly string DatabaseFilename;
		private readonly SQLiteOpenFlags Flags;
		private string DatabasePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), DatabaseFilename);

		public DataBase(string databaseFilename, SQLiteOpenFlags flags)
		{
			this.DatabaseFilename = databaseFilename;
			this.Flags = flags;

			InitializeAsync().SafeFireAndForget(false);
		}
		private Lazy<SQLiteAsyncConnection> GetLazyInitializer()
		{
			Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
			{
				return new SQLiteAsyncConnection(DatabasePath, Flags);
			});

			return lazyInitializer;
		}

		async Task InitializeAsync()
		{
			try
			{
				if (!System.IO.File.Exists(DatabasePath))
				{
					CopyDataBaseFileToAPK();
				}

				if (!initialized)
				{
					if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(T).Name))
					{
						await Database.CreateTablesAsync(CreateFlags.None, typeof(T)).ConfigureAwait(false);
					}
					initialized = true;
				}
			}
			catch
			{
				throw new ArgumentException($"initialization of data base failed: {DatabasePath}");
			}
			
		}
		public void CopyDataBaseFileToAPK()
		{
			using (BinaryReader br = new BinaryReader(Android.App.Application.Context.Assets.Open(DatabaseFilename)))
			{
				using (BinaryWriter bw = new BinaryWriter(new FileStream(DatabasePath, FileMode.Create)))
				{
					byte[] buffer = new byte[2048];
					int len = 0;
					while ((len = br.Read(buffer, 0, buffer.Length)) > 0)
					{
						bw.Write(buffer, 0, len);
					}
				}
			}
		}
		public Task<List<T>> GetItemsAsync()
		{
			return Database.Table<T>().ToListAsync();
		}
		public Task<int> UpdateItemAsync(T item)
		{
			return Database.UpdateAsync(item);
		}
		public Task<int> InsertItemAsync(T item)
		{
			return Database.InsertAsync(item);
		}
		public Task<int> DeleteItemAsync(T item)
		{
			return Database.DeleteAsync(item);
		}
	}
}
