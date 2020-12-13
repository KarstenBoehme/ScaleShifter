using System.Linq;
using System.Reactive.Subjects;

namespace MyFirstMobileApp
{
	public class Model
	{
        public BehaviorSubject<Model> ModelSubject;
        public FretBoard FretBoard { get; private set; }
		public DataBaseHandler DataBaseHandler { get; private set; }
		public FredBoardGrid FretBoardGrid { get; private set; }

        public Model()
        {
            DataBaseHandler = new DataBaseHandler();
            FretBoard = new FretBoard();
			FretBoardGrid = new FredBoardGrid(FretBoard);
            ModelSubject = new BehaviorSubject<Model>(this);

            AppPreferences.Load(this);

            UpdateFretboardUIGrid();
            FretBoardGrid.UpdateCapo();
        }

        public void UpdateFretboardUIGrid()
        {
            EnumCollectionCreator<GuitarString>.GetEnumCollection().ToList()
                .ForEach(s => FretBoardGrid.UpdateGrid(s));
            ModelSubject.OnNext(this);
        }

        public void UpdateFretboardUIGrid(GuitarString guitarString)
        {
            FretBoardGrid.UpdateGrid(guitarString);
            ModelSubject.OnNext(this);
        }

        public void RefreshModel()
        {
            FretBoard.SetScale(FretBoard.Scale, FretBoard.Key);
            FretBoard.SetTuning(FretBoard.Tuning);
            FretBoardGrid.UpdateCapo();
            UpdateFretboardUIGrid();
        }
    }
}
