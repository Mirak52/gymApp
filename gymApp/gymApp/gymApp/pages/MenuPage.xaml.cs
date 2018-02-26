using gymApp.classes;
using Newtonsoft.Json;
using RestSharp.Portable;
using RestSharp.Portable.Authenticators;
using RestSharp.Portable.HttpClient;
using RestSharp.Portable.HttpClient.Impl;

using RestSharp.Portable.WebRequest.Impl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
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


            var url = "https://student.sps-prosek.cz/~bastlma14/gymApp/item.php";
            var jsonInput = "";
            /*
            Task task = new Task(() => {
                jsonInput = AccessTheWebAsync(url).Result;
            });
            task.Start();
            task.Wait();

            
       
            var convertLocal = JsonConvert.DeserializeObject<List<Excercise>>(jsonInput);
            foreach(var Exercise in convertLocal)
            {
                //App.DatabaseExcercise.SaveItemAsync(Exercise);
            }
            
            var exercises = App.DatabaseExcercise.QueryCustom().Result;
            List<Excercise> excercise = new List<Excercise>();
            foreach (var exercise in exercises) {
                excercise.Add(new Excercise() { Name = exercise.Name, Region = exercise.Region, Description = exercise.Description, Tip = exercise.Tip });
            }
            listview.ItemsSource = excercise;
            */

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
                    Test.Text = date.AddDays(5).ToString();
                    DateTime test = date.AddDays(5);
                    if (updatedDay.AddDays(5) <= DateTime.Today)
                    {
                        saveExcercises();
                        exerciseSearch.BackgroundColor = Color.HotPink;
                    }
                    else
                    {
                        exerciseSearch.BackgroundColor = Color.Yellow;
                    }
                }
            }
        }
        private void saveExcercises()
        {
            var jsonInput = "";

            Task task = new Task(() => {
                jsonInput = AccessTheWebAsync().Result;
            });
            task.Start();
            task.Wait();

            var convertLocal = JsonConvert.DeserializeObject<List<Excercise>>(jsonInput);
            foreach (var Exercise in convertLocal)
            {
                App.DatabaseExcercise.SaveItemAsync(Exercise);
            }
        }
        private void Hiit_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new HiitPage(), false);
        }
    
        private void exerciseSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            exerciseSearch.BackgroundColor = Color.Aqua;

        }
        async Task<string> AccessTheWebAsync()
        {
            HttpClient client = new HttpClient();
            Task<string> getStringTask = client.GetStringAsync("https://student.sps-prosek.cz/~bastlma14/gymApp/item.php");

            string urlContents = await getStringTask;
            return urlContents;
        }

    }
}