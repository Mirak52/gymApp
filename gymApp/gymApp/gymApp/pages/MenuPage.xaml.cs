using gymApp.classes;
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
            CheckUpdater();
            var data = App.DatabaseRegions.SelectRegions().Result;
            var datad = App.DatabaseExcercise.SelectExcercise().Result;
            ShowExcercises();
            
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
            EmployeeView.IsGroupingEnabled = true;
          
            List<Group> Groups = new List<Group>();
            bool madeGroup = true;
            int groupID=0;
            int regionID =1;
            var regions = App.DatabaseRegions.SelectRegions().Result;
            foreach (var region in regions) {
                var excercises = App.DatabaseExcercise.SelectExcerciseByRegion(regionID).Result;
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
                regionID++;
            }
            EmployeeView.ItemsSource = Groups;

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
            
            foreach (var excercise in convertedJsonExcercise)
            {
                Excercise excerciseData = new Excercise();
                //excerciseData.ID_excercise = excercise.ID_excercise;
                excerciseData.Name = excercise.Name;
                excerciseData.Region = excercise.Region;
                excerciseData.Tip = excercise.Tip;
                excerciseData.Description = excercise.Description;
                App.DatabaseExcercise.SaveItemAsync(excerciseData);
            }
            var convertedJsonRegion = JsonConvert.DeserializeObject<List<ExcerciseRegion>>(RegionJson);
            foreach (var Region in convertedJsonRegion)
            {
                ExcerciseRegion excerciseRegionData = new ExcerciseRegion();
                //excerciseRegionData.ID_region = Region.ID_region;
                excerciseRegionData.Region = Region.Region;
                App.DatabaseRegions.SaveItemAsync(excerciseRegionData);
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
                EmployeeView.IsGroupingEnabled = false;
                EmployeeView.ItemsSource = "";
                EmployeeView.ItemsSource = data;
            }
        }

        async Task<string> GetExcerciseJson()
        {
            HttpClient client = new HttpClient();
            Task<string> getStringTask = client.GetStringAsync("https://student.sps-prosek.cz/~bastlma14/gymApp/Excercises.php");
            string urlContents = await getStringTask;
            return urlContents;
        }
        async Task<string> GetRegionJson()
        {
            HttpClient client = new HttpClient();
            Task<string> getStringTask = client.GetStringAsync("https://student.sps-prosek.cz/~bastlma14/gymApp/Excercises.php?action=1");
            string urlContents = await getStringTask;
            return urlContents;
        }

        private void EmployeeView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (EmployeeView.SelectedItem is Excercise selectedExcersise)
            {
                var excercise = App.DatabaseExcercise.SelectByName(selectedExcersise.Name).Result;
                Navigation.PushModalAsync(new ExcerciseDetailPage(excercise[0].ID_excercise), false);
            }
        }
    }
}