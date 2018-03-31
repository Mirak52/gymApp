using gymApp.classes;
using gymApp.classes.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace gymApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
           // var rootPage = new NavigationPage(new pages.MenuPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
        
        private static ExcerciseDatabase _databaseEx;
        public static ExcerciseDatabase DatabaseExcercise
        {
            get
            {
                if (_databaseEx == null)
                {
                    _databaseEx = new ExcerciseDatabase(DependencyService.Get<IFileHelper>().GetLocalFilePath("TodoSQLite.db3"));
                }
                return _databaseEx;
            }
        }
        private static BodyStatsDatabase _databaseBS;
        public static BodyStatsDatabase DatabaseBodyStats
        {
            get
            {
                if (_databaseBS == null)
                {
                    _databaseBS = new BodyStatsDatabase(DependencyService.Get<IFileHelper>().GetLocalFilePath("TodoSQLite.db3"));
                }
                return _databaseBS;
            }
        }
        private static UpdaterDatabase _databaseUp;
        public static UpdaterDatabase DatabaseUpdater
        {
            get
            {
                if (_databaseUp == null)
                {
                    _databaseUp = new UpdaterDatabase(DependencyService.Get<IFileHelper>().GetLocalFilePath("TodoSQLite.db3"));
                }
                return _databaseUp;
            }
        }
        private static ExcerciseRegionDatabase _databaseReg;
        public static ExcerciseRegionDatabase DatabaseRegions
        {
            get
            {
                if (_databaseReg == null)
                {
                    _databaseReg = new ExcerciseRegionDatabase(DependencyService.Get<IFileHelper>().GetLocalFilePath("TodoSQLite.db3"));
                }
                return _databaseReg;
            }
        }

        

        private static PersonalRecordDatabase _databaseRec;
        public static PersonalRecordDatabase DatabasePersonalRecord
        {
            get
            {
                if (_databaseRec == null)
                {
                    _databaseRec = new PersonalRecordDatabase(DependencyService.Get<IFileHelper>().GetLocalFilePath("TodoSQLite.db3"));
                }
                return _databaseRec;
            }
        }
        private static DayDatabase _databaseDay;
        public static DayDatabase DatabaseDay
        {
            get
            {
                if (_databaseDay == null)
                {
                    _databaseDay = new DayDatabase(DependencyService.Get<IFileHelper>().GetLocalFilePath("TodoSQLite.db3"));
                }
                return _databaseDay;
            }
        }
        private static TrainingUnitDatabase _databaseTrainingUnit;
        public static TrainingUnitDatabase DatabaseTrainingUnit
        {
            get
            {
                if (_databaseTrainingUnit == null)
                {
                    _databaseTrainingUnit = new TrainingUnitDatabase(DependencyService.Get<IFileHelper>().GetLocalFilePath("TodoSQLite.db3"));
                }
                return _databaseTrainingUnit;
            }
        }
        private static SetDatabase _databaseSet;
        public static SetDatabase DatabaseSet
        {
            get
            {
                if (_databaseSet == null)
                {
                    _databaseSet = new SetDatabase(DependencyService.Get<IFileHelper>().GetLocalFilePath("TodoSQLite.db3"));
                }
                return _databaseSet;
            }
        }

        public static bool IsNumber(string parameter)
        {
            int number;
            bool isNumeric = int.TryParse(parameter, out number);
            return isNumeric;
        }
        internal static int setNumber(string text)
        {
            int number;
            bool isNumeric = int.TryParse(text, out number);
            return number;
        }
    }
}
