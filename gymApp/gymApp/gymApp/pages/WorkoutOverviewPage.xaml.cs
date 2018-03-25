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

        
        private int day_ID = 0;
        private void ShowSetsInDay(int iD_Day, string mainExcercise)
        {
            day_ID = iD_Day;
            MainExcerciseName.Text = mainExcercise;
            List<Set> setsList = new List<Set>();
            var sets = App.DatabaseSet.SelectSetsByTrainingUnit(iD_Day).Result;
            string detail;
            foreach (var set in sets)
             {
                if(set.Weight != 0){detail = set.Reps + "x" + set.Weight +"KG";}else{ detail = set.Reps; }
                var excercise = App.DatabaseExcercise.SelectDetailedExcercise(set.ID_excercisePK).Result;
                setsList.Add(new Set { ExcerciseName = excercise[0].Name, Reps = detail, ID_set = set.ID_set, ID_excercisePK= excercise[0].ID_excercise });
             }
            SetsLV.ItemsSource = null;
            SetsLV.ItemsSource = setsList;
        }
        private int set_ID = 0;
        private int set_PK = 0;
        private void ShowExcercise(int iD_set, int iD_excercisePK)
        {
            set_ID = iD_set;
            set_PK = iD_excercisePK;
            SetExcercisePicker();
            var IsWeight = App.DatabaseSet.SelectByID(iD_set).Result;
            if(IsWeight[0].Weight != 0){WeightE.IsVisible = true;} else{ WeightE.IsVisible = false;}
        }

        private void SetExcercisePicker()
        {
            var excercises = App.DatabaseExcercise.SelectExcercise().Result;
            string NameOfSelectedItem = null;
            ExcerciseP.Items.Add("null");
            foreach (var excercise in excercises)
            {
                ExcerciseP.Items.Add(excercise.Name);
                if(excercise.ID_excercise == set_PK)
                {
                    NameOfSelectedItem = excercise.Name;
                }
            }
            ExcerciseP.Items[0] = NameOfSelectedItem;
            ExcerciseP.SelectedItem = NameOfSelectedItem;
        }
        private void UpdateSet()
        {
            var excercise = App.DatabaseExcercise.SelectByName(ExcerciseP.SelectedItem.ToString()).Result;
            set_PK = excercise[0].ID_excercise;
            Informations.IsVisible = true;
            if (WeightE.IsVisible && !string.IsNullOrEmpty(RepsE.Text) && !string.IsNullOrEmpty(WeightE.Text))
            {
                Set set = new Set();
                set.ID_set = set_ID;
                set.ID_day = day_ID;
                set.ID_excercisePK = set_PK;
                set.Reps = RepsE.Text;
                set.Weight = App.setNumber(WeightE.Text);
                App.DatabaseSet.UpdateRepsAndExcerciseAndWeight(set_PK, RepsE.Text, App.setNumber(WeightE.Text) ,set_ID);
                RepsE.Text = "";
                WeightE.Text = "";
                BackToSets.Text = "zpět";
                Informations.Text = "Úspěšně uloženo";
               
            }
            else
            {
                if (!string.IsNullOrEmpty(RepsE.Text))
                {
                    App.DatabaseSet.UpdateRepsAndExcercise(set_PK, RepsE.Text, set_ID);
                    RepsE.Text = "";
                    WeightE.Text = "";
                    BackToSets.Text = "zpět";
                    Informations.Text = "Úspěšně uloženo";
                }
                else
                {
                    Informations.Text = "Nezadal si všechny údaje";
                }
            }
            
        }
        private void ExcerciseChange_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            BackToSets.Text = "ULOŽIT";
        }

        private void TrainingUnitsLV_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (TrainingUnitsLV.SelectedItem is TrainingUnit selectedTrainingUnit)
            {
                TrainingUnitsLV.SelectedItem = null;
                TrainingUnits.IsVisible = false;
                Days.IsVisible = true;
                ShowDaysInTrainigUnit(selectedTrainingUnit.ID_TrainingUnit, selectedTrainingUnit.Title);
            }
        }
        private void DaysLV_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (DaysLV.SelectedItem is Day selectedDay)
            {
                Days.IsVisible = false;
                Sets.IsVisible = true;
                DaysLV.SelectedItem = null;
                ShowSetsInDay(selectedDay.ID_Day, selectedDay.MainExcercise);
            }
        }
        private void SetsLV_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (SetsLV.SelectedItem is Set selectedSet)
            {
                Sets.IsVisible = false;
                ExcerciseUpdater.IsVisible = true;
                SetsLV.SelectedItem = null;
                ShowExcercise(selectedSet.ID_set, selectedSet.ID_excercisePK);
            }
        }

        

        private void BackToTraininUnits_Clicked(object sender, EventArgs e)
        {
            TrainingUnits.IsVisible = true;
            Days.IsVisible = false;
        }
        private void BackToDays_Clicked(object sender, EventArgs e)
        {
            Sets.IsVisible = false;
            Days.IsVisible = true;
        }

        private void BackToSets_Clicked(object sender, EventArgs e)
        {
            if(BackToSets.Text == "ULOŽIT")
            {
                UpdateSet();
            }
            else
            {
                Sets.IsVisible = true;
                ExcerciseUpdater.IsVisible = false;
                ShowSetsInDay(day_ID, MainExcerciseName.Text);
            }
        }
    }
}