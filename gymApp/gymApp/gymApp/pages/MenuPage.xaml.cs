﻿using gymApp.classes;
using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using System.Linq;
using System.Net.Http;

using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace gymApp.pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : TabbedPage
    {
        public MenuPage ()
        {
            InitializeComponent();
            CheckFirstRun();
            ShowStats();
            CheckUpdater();
            ShowExcercises();
            var data = App.DatabaseExcercise.SelectExcercise().Result;
        }

        public void ShowStats()
        {
            var bodyStats = App.DatabaseBodyStats.SelectLastBodyStats().Result;
            var highestLift = App.DatabasePersonalRecord.SelectHighestRecord().Result;
            WeightL.Text = bodyStats[0].Weight.ToString() + " kg ";
            HeightL.Text = bodyStats[0].Height.ToString() + " cm" ;
            WaistL.Text = bodyStats[0].WaistCircumference.ToString() + " cm";
            ThighL.Text = bodyStats[0].ThighCircumference.ToString() + " cm";
            BicepsL.Text = bodyStats[0].BicepsCircumference.ToString() + " cm";

            BenchL.Text = highestLift[0].Benchpress.ToString() + " kg";
            DeathL.Text = highestLift[0].Deathlift.ToString() + " kg";
            SquatL.Text = highestLift[0].Squat.ToString() + " kg";
        }

        private void CheckFirstRun()
        {
            var bodyStats = App.DatabaseBodyStats.SelectLastBodyStats().Result;
            if(bodyStats.Count == 0) {
                BodyStats bodyStat = new BodyStats();
                bodyStat.Height = 0;
                bodyStat.Weight = 0;
                bodyStat.WaistCircumference = 0;
                bodyStat.ThighCircumference = 0;
                bodyStat.BicepsCircumference = 0;
                App.DatabaseBodyStats.SaveItemAsync(bodyStat);
            }
            var Records = App.DatabasePersonalRecord.SelectHighestRecord().Result;
            if (Records.Count == 0)
            {
                PersonalRecord personalRecord = new PersonalRecord();
                personalRecord.Benchpress = 0;
                personalRecord.Deathlift = 0;
                personalRecord.Squat = 0;
                App.DatabasePersonalRecord.SaveItemAsync(personalRecord);

            }
        }

        public DateTime date;
        private void CheckUpdater()
        {
            var updater = App.DatabaseUpdater.Select().Result;
            if (!updater.Any())
            {
                DateTime Today = DateTime.Today;
                App.DatabaseUpdater.insertToday(Today.ToString());
                saveExcercises();
                exerciseSearch.BackgroundColor = Color.Blue;

            }
            else { 
                foreach (var data in updater)
                {
                    DateTime updatedDay = DateTime.Parse(data.lastUpdate);
                    if (updatedDay.AddDays(5) <= DateTime.Today)
                    {
                        saveExcercises();
                    }
                }
            } 
        }

        private void ShowExcercises()
        {
            ExcercisesLV.IsGroupingEnabled = true;
          
            List<Group> Groups = new List<Group>();
            bool madeGroup = true;
            int groupID=0;
            var regions = App.DatabaseRegions.SelectRegions().Result;
            foreach (var region in regions) {
                var excercises = App.DatabaseExcercise.SelectExcerciseByRegion(region.ID_region).Result;
                foreach (var excercise in excercises)
                {
                    if (madeGroup)
                    {
                        Groups.Add(new Group(region.Region) { new Excercise { Name = excercise.Name } });
                    }
                    else
                    {
                        Groups[groupID].Add(new Excercise { Name = excercise.Name });
                    }
                    madeGroup = false;
                }
                madeGroup = true;
                groupID++;
            }
            ExcercisesLV.ItemsSource = Groups;

        }

        private void saveExcercises()
        {
            var ExcercisesJson = "";
            var RegionJson = "";
            Task task = new Task(() => {
                ExcercisesJson = GetExcerciseJson().Result;
                RegionJson = GetRegionJson().Result;
            });
            task.Start();
            task.Wait();
            var convertedJsonExcercise = JsonConvert.DeserializeObject<List<Excercise>>(ExcercisesJson);
            var convertedJsonRegion = JsonConvert.DeserializeObject<List<ExcerciseRegion>>(RegionJson);
            foreach (var Region in convertedJsonRegion)
            {
                ExcerciseRegion excerciseRegionData = new ExcerciseRegion();
                excerciseRegionData.ID_region = Region.ID_region;
                excerciseRegionData.Region = Region.Region;
                App.DatabaseRegions.SaveItemAsync(excerciseRegionData);
            }
            foreach (var excercise in convertedJsonExcercise)
            {
                Excercise excerciseData = new Excercise();
                //excerciseData.ID_excercise = excercise.ID_excercise;
                excerciseData.Name = excercise.Name;
                excerciseData.Region = excercise.Region;
                excerciseData.Tip = excercise.Tip;
                excerciseData.Description = excercise.Description;
                excerciseData.Specification = excercise.Specification;
                App.DatabaseExcercise.SaveItemAsync(excerciseData);
            }
        }
        private void Hiit_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new HiitPage(), false);
        }
    
        private void exerciseSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                ShowExcercises();
            }
            else
            {
                searchAndShowExcercise(e.NewTextValue);
            }
        }
        
        private void searchAndShowExcercise(string searchText)
        {
            if(searchText.Length > 2)
            {
                var data = App.DatabaseExcercise.SelectExcerciseByParameter(searchText).Result;
                ExcercisesLV.IsGroupingEnabled = false;
                ExcercisesLV.ItemsSource = "";
                ExcercisesLV.ItemsSource = data;
            }
        }
        public static string ApiUrlExcercise = "https://student.sps-prosek.cz/~bastlma14/gymApp/Excercises.php";
        public static string ApiUrlRegions = "https://student.sps-prosek.cz/~bastlma14/gymApp/Excercises.php?action=1";
        async Task<string> GetExcerciseJson()
        {
            HttpClient client = new HttpClient();
            Task<string> getStringTask = client.GetStringAsync(ApiUrlExcercise);
            string urlContents = await getStringTask;
            return urlContents;
        }
        async Task<string> GetRegionJson()
        {
            HttpClient client = new HttpClient();
            Task<string> getStringTask = client.GetStringAsync(ApiUrlRegions);
            string urlContents = await getStringTask;
            return urlContents;
        }

        private void ExcercisesLV_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ExcercisesLV.SelectedItem = null;
            if (ExcercisesLV.SelectedItem is Excercise selectedExcersise)
            {
                var excercise = App.DatabaseExcercise.SelectByName(selectedExcersise.Name).Result;
                Navigation.PushModalAsync(new ExcerciseDetailPage(excercise[0].ID_excercise), false);
            }
        }

        private void Workout_Clicked(object sender, EventArgs e)
        {
            var plan = App.DatabaseTrainingUnit.SelectLastIDWhitZeroState().Result;
            if (plan.Count == 1)
            {
                if (plan[0].state == 0)
                {
                    var LastDay = App.DatabaseDay.SelectFirstActiveDay().Result;
                    if (LastDay.Count == 0)
                    {

                    }
                    else
                    {
                        var sets = App.DatabaseSet.SelectSetsByTrainingUnit(LastDay[0].ID_Day).Result;
                        List<Set> setsList = new List<Set>();
                        foreach (var set in sets)
                        {
                            var excercise = App.DatabaseExcercise.SelectDetailedExcercise(set.ID_excercisePK).Result;
                            setsList.Add(new Set { ID_day = set.ID_day, Reps = set.Reps, Weight = set.Weight, ID_excercisePK = excercise[0].ID_excercise });
                        }
                        Navigation.PushModalAsync(new WorkoutPlayerPage(setsList), false);
                    }
                }
                else
                {
                    Navigation.PushModalAsync(new TrainingCreatorPage(2), false);
                }
            }
            else
            {
                Navigation.PushModalAsync(new TrainingCreatorPage(2), false);
            }
        }

        private void AddStats_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new StatsPage(), false);
        }
        private void TrainingOverview_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new WorkoutOverviewPage(), false);
        }

        private void DataBrowser_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new DataBrowserPage(), false);
        }

        private void QuickTraining_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new TrainingCreatorPage(1), false);
        }
    }
}