using gymApp.classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace gymApp.pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WorkoutOverviewPage : ContentPage
    {
        public WorkoutOverviewPage()
        {
            InitializeComponent();
            CreateListView();
            var test = App.DatabaseSet.Select().Result;
            var test1 = App.DatabaseDay.Select().Result;
            var test2 = App.DatabaseTrainingUnit.Select().Result;
        }

        private void CreateListView()
        {
            List<TrainingUnit> TrainingUnitList = new List<TrainingUnit>();
            var TrainingUnits = App.DatabaseTrainingUnit.Select().Result;
            foreach (var TrainingUnit in TrainingUnits)
            {
                TrainingUnitList.Add(new TrainingUnit { Title = TrainingUnit.Title, CreatedDate= TrainingUnit.CreatedDate, ID_TrainingUnit= TrainingUnit.ID_TrainingUnit});
            }
            TrainingUnitsLV.ItemsSource = TrainingUnitList;
        }
        private void TrainingUnitsLV_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (TrainingUnitsLV.SelectedItem is TrainingUnit selectedTrainingUnit)
            {

                TrainingUnits.IsVisible = false;
                Days.IsVisible = true;
                ShowDaysInTrainigUnit(selectedTrainingUnit.ID_TrainingUnit, selectedTrainingUnit.Title);
            }
        }

        private void ShowDaysInTrainigUnit(int trainingUnit, string Title)
        {
            List<Day> daysList = new List<Day>();
            var days = App.DatabaseDay.SelectSetsByTrainingUnit(trainingUnit).Result;
            TrainingUnitName.Text = Title;
            int DayInPlan = 1;
            string state = null;
            foreach (var day in days)
            {
                if (day.State == "0")
                { state = "Neodcvičeno"; } else{ state = "Splněno"; }
                daysList.Add(new Day { MainExcercise = "Den: " + DayInPlan.ToString() + " Hlavní cvik: " + day.MainExcercise, State = state, ID_Day = day.ID_Day });
                DayInPlan++;
            }
            DaysLV.ItemsSource = daysList;
        }

        private void backView_Clicked(object sender, EventArgs e)
        {
            TrainingUnits.IsVisible = true;
            Days.IsVisible = false;
        }

        private void ShowSetsInDay(int iD_Day, string mainExcercise)
        {
            MainExcerciseName.Text = mainExcercise;
            List<Set> setsList = new List<Set>();
            var sets = App.DatabaseSet.SelectSetsByTrainingUnit(iD_Day).Result;
            string detail;
            foreach (var set in sets)
             {
                if(set.Weight != 0){detail = set.Reps + "x" + set.Weight;}else{ detail = set.Reps; }
                var excercise = App.DatabaseExcercise.SelectDetailedExcercise(set.ID_excercisePK).Result;
                setsList.Add(new Set { ExcerciseName = excercise[0].Name, Reps = detail });
             }
            SetsLV.ItemsSource = setsList;
        }

        private void DaysLV_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (DaysLV.SelectedItem is Day selectedDay)
            {
                Days.IsVisible = false;
                Sets.IsVisible = true;
                ShowSetsInDay(selectedDay.ID_Day, selectedDay.MainExcercise);
            }
        }
        private void SetsLV_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }
    }
}