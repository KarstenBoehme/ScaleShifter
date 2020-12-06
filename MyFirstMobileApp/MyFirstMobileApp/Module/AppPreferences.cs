using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;

namespace MyFirstMobileApp
{
	public static class AppPreferences
	{
		private static string TuningPreference => "Tuning";
        private static int tuningIndex;

        private static string CapoPreference => "Capo";
        private static int capoPos;

        private static string KeyPreference => "Key";
        private static string key;

        private static string ScalePreference => "Scale";
        private static int scaleIndex;

        private static string SemitonePreference => "Semitone";
        private static string semitoneSetting;

        private static string NoteDisplayPreference => "NoteDisplay";
        private static string noteDisplaySetting;

        public static void Load(Model model)
        {
            semitoneSetting = Preferences.Get(SemitonePreference, SemiStepSettings.SHARP.ToString());
            Settings.SemiStepSettings = EnumCollectionCreator<SemiStepSettings>.GetEnumCollection().Single(s => s.ToString() == semitoneSetting);

            noteDisplaySetting = Preferences.Get(NoteDisplayPreference, KeyDisplayingSettings.ALL.ToString());
            Settings.KeyDisplayingSettings = EnumCollectionCreator<KeyDisplayingSettings>.GetEnumCollection().Single(s => s.ToString() == noteDisplaySetting);

            tuningIndex = Preferences.Get(TuningPreference, 1);
            Tuning tuning = model.DataBaseHandler.TuningCollection.Any(t => t.Index == tuningIndex) ?
                                     model.DataBaseHandler.TuningCollection.Single(t => t.Index == tuningIndex) :
                                     model.DataBaseHandler.TuningCollection.Single(t => t.Description.Contains("Standard"));
            model.FretBoard.Tuning = (Tuning)tuning.Clone();

            key = Preferences.Get(KeyPreference, Key.C.ToString());
            model.FretBoard.Key = Keys.ListOfKeys.Single(k => k.ToString() == key);

            scaleIndex = Preferences.Get(ScalePreference, 1);
            model.FretBoard.Scale = model.DataBaseHandler.ScaleCollection.Single(s => s.Index == scaleIndex);

            capoPos = Preferences.Get(CapoPreference, 0);
            model.FretBoard.CapoPosition = capoPos;
        }

        public static void Save(Model model)
        {
            Preferences.Set(NoteDisplayPreference, Settings.KeyDisplayingSettings.ToString());
            Preferences.Set(SemitonePreference, Settings.SemiStepSettings.ToString());

            Preferences.Set(ScalePreference, model.FretBoard.Scale.Index);
            Preferences.Set(KeyPreference, model.FretBoard.Key.ToString());
            Preferences.Set(TuningPreference, model.FretBoard.Tuning.Index);
            Preferences.Set(CapoPreference, model.FretBoard.CapoPosition);
        }
    }
}
