using gymApp.classes;
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

            MainPage = new NavigationPage(new gymApp.MainPage());
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
    }
}
