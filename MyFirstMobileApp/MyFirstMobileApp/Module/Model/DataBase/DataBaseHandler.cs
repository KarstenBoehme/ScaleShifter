using MyFirstMobileApp.Module.Properties;
using SQLite;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstMobileApp.Module.DataBase
{
	public class DataBaseHandler
	{
		private DataBase<Scale> ScaleDataBase;
		private DataBase<Tuning> TuningDataBase;
		private DataBase<Freq> FrequencyDataBase;

		public ObservableCollection<Scale> ScaleCollection;
		public ObservableCollection<Tuning> TuningCollection;
		public ObservableCollection<Freq> FrequencyCollection;
		public DataBaseHandler()
		{
			ScaleCollection = GetScalesFromDataBaseAsync();
			TuningCollection = GetTuningsFromDataBaseAsync();
			FrequencyCollection = GetFrequenciesFromDataBaseAsync();
		}

		private ObservableCollection<Scale> GetScalesFromDataBaseAsync()
		{
			ScaleDataBase = new DataBase<Scale>("SQScales.db", SQLiteOpenFlags.ReadOnly);
			var scales = Task.Run(async () => await ScaleDataBase.GetItemsAsync()).Result;
			return new ObservableCollection<Scale>(scales.OrderBy(s => s.Name));
		}

		private ObservableCollection<Tuning> GetTuningsFromDataBaseAsync()
		{
			TuningDataBase = new DataBase<Tuning>("SQTunings.db", SQLiteOpenFlags.ReadWrite);
			var tunings = Task.Run(async () => await TuningDataBase.GetItemsAsync()).Result;
			return new ObservableCollection<Tuning>(tunings.OrderBy(s => s.Notes));
		}

		private ObservableCollection<Freq> GetFrequenciesFromDataBaseAsync()
		{
			FrequencyDataBase = new DataBase<Freq>("SQFrequency.db", SQLiteOpenFlags.ReadWrite);
			var freqs = Task.Run(async () => await FrequencyDataBase.GetItemsAsync()).Result;
			return new ObservableCollection<Freq>(freqs);
		}

		public void AddTuningToDB(Tuning newTuning)
		{
			TuningDataBase.InsertItemAsync(newTuning);
			TuningCollection.Add(newTuning);
		}

		public void RemoveTuningFromDB(Tuning obsoleteTuning)
		{
			TuningDataBase.DeleteItemAsync(obsoleteTuning);
			TuningCollection.Remove(obsoleteTuning);
		}
	}
}
