﻿using SQLite;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstMobileApp
{
	public class DataBaseHandler
	{
		public DataBase<Scale> ScaleDataBase;
		public DataBase<Tuning> TuningDataBase;
		public ObservableCollection<Scale> ScaleCollection;
		public ObservableCollection<Tuning> TuningCollection;
		public DataBaseHandler()
		{
			ScaleCollection = GetScalesFromDataBaseAsync();
			TuningCollection = GetTuningsFromDataBaseAsync();
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
			return new ObservableCollection<Tuning>(tunings.OrderBy(s => s.Description));
		}

		public void AddTuningToDB(Tuning newTuning)
		{
			TuningDataBase.InsertItemAsync(newTuning);
			TuningCollection.Add(newTuning);
		}

		public void RemoveTuningFromDB(Tuning deadTuning)
		{
			TuningDataBase.DeleteItemAsync(deadTuning);
			TuningCollection.Remove(deadTuning);
		}
	}
}
