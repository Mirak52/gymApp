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
    public partial class TrainingCreatorPage : ContentPage
    {
        public TrainingCreatorTimer totalTime = new TrainingCreatorTimer();
        public TrainingCreatorPage(int TypeOfTraining)
        {
            InitializeComponent();
            if (TypeOfTraining == 1)
            {
                QuickTraining.IsVisible = true;
                SetPickerQuick();
                SetDefaultTime();
                CountTotalTrainingTime();
                
            }
            else if (TypeOfTraining == 2)
            {
                SetPickerPlan();
                CheckPersonalRecords();
                QuickTraining.IsVisible = false;
                LongPlan.IsVisible = true;
            }
        }

        private void SetPickerPlan()
        {
            var days = new List<string>();
            days.Add("Vyber počet dnů");
            for (int i = 2; i <= 7; i++)
            {
                days.Add(i.ToString());
            }
            DaysNumbers.ItemsSource = days;
            DaysNumbers.SelectedIndex = 0;
        }

        private void SetDefaultTime()
        {
            totalTime.BasicExceNum = Convert.ToInt32(BasicExcerciseNumber.Value);
            totalTime.BasicSetsNum = Convert.ToInt32(BasicSetExcerciseNumber.Value);
            totalTime.SupplementExceNum = Convert.ToInt32(SupplementExcerciseNumber.Value);
            totalTime.SupplementSetsNum = Convert.ToInt32(SupplementSetExcerciseNumber.Value);
            totalTime.CompensationExceNum = Convert.ToInt32(CompensatorExcerciseNumber.Value);
            totalTime.CompensationSetsNum = Convert.ToInt32(CompensatorSetExcerciseNumber.Value);
        }

        private void SetPickerQuick()
        {
            var muscles = new List<string>();
            muscles.Add("Vyber zadávání");
            muscles.Add("Prsa, ramena, biceps");
            muscles.Add("Záda, triceps");
            muscles.Add("Nohy, břicho");
            Muscles.ItemsSource = muscles;
            Muscles.SelectedIndex = 0;
        }

        private void BasicExcerciseNumber_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            SetIntValueToLabel(BasicExcerciseNumber, BasicNumber);
            totalTime.BasicExceNum = Convert.ToInt32(BasicExcerciseNumber.Value);
            CountTotalTrainingTime();
        }
        private void BasicSetExcerciseNumber_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            SetIntValueToLabel(BasicSetExcerciseNumber, BasicSetNumber);
            totalTime.BasicSetsNum = Convert.ToInt32(BasicSetExcerciseNumber.Value);
            CountTotalTrainingTime();
        }

        private void SupplementExcerciseNumber_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            SetIntValueToLabel(SupplementExcerciseNumber, SupplementNumber);
            totalTime.SupplementExceNum = Convert.ToInt32(SupplementExcerciseNumber.Value);
            CountTotalTrainingTime();
        }
        private void SupplementSetExcerciseNumber_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            SetIntValueToLabel(SupplementSetExcerciseNumber, SupplementSetNumber);
            totalTime.SupplementSetsNum = Convert.ToInt32(SupplementSetExcerciseNumber.Value);
            CountTotalTrainingTime();
        }

        private void CompensatorExcerciseNumber_ValueChanged_1(object sender, ValueChangedEventArgs e)
        {
            SetIntValueToLabel(CompensatorExcerciseNumber, CompensationNumber);
            totalTime.CompensationExceNum = Convert.ToInt32(CompensatorExcerciseNumber.Value);
            CountTotalTrainingTime();
        }
        private void CompensatorExcerciseNumber_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            SetIntValueToLabel(CompensatorSetExcerciseNumber, CompensationSetNumber);
            totalTime.CompensationSetsNum = Convert.ToInt32(CompensatorSetExcerciseNumber.Value);
            CountTotalTrainingTime();
        }


        public int setNumber;
        private void SetIntValueToLabel(Slider slider, Label label)
        {
            setNumber = Convert.ToInt32(slider.Value);
            slider.Value = setNumber;
            label.Text = setNumber.ToString();
        }
        private void CountTotalTrainingTime()
        {
            if (VolumePower.IsToggled)
            {
                totalTime.totalTime = (totalTime.BasicExceNum * totalTime.BasicSetsNum * 90)
                                    + (totalTime.SupplementExceNum * totalTime.SupplementSetsNum * 60)
                                    + (totalTime.CompensationExceNum * totalTime.CompensationSetsNum * 45);
            }
            else
            {
                totalTime.totalTime = (totalTime.BasicExceNum * totalTime.BasicSetsNum * 120)
                                    + (totalTime.SupplementExceNum * totalTime.SupplementSetsNum * 90)
                                    + (totalTime.CompensationExceNum * totalTime.CompensationSetsNum * 60);
            }
            Information.Text = "Odhadovaná doba tréninku: " + ReturnTimeInFormat(totalTime.totalTime)+ " min";
        }
        private string ReturnTimeInFormat(int timeParameter)
        {
            int minute = 0;
            int second = 0;
            string timeFormated;
            minute = timeParameter / 60;
            second = timeParameter - minute * 60;
            timeFormated = TimeAdjustment(minute) + ":" + TimeAdjustment(second);
            return timeFormated;
        }
        private string TimeAdjustment(int timeParameter)
        {
            string time;
            if (timeParameter <= 9)
            {
                time = "0" + timeParameter.ToString();
            }
            else
            {
                time = timeParameter.ToString();
            }
            return time;
        }

        private void Create_Clicked(object sender, EventArgs e)
        {
            if (Muscles.SelectedIndex == 0)
            {
                Information.TextColor = Color.Red;
                Information.Text = "Nezadal si partii kterou chceš cvičit";
            }
            else
            {
                CreateTraining();
            }
        }
       
        private void CreateTraining()
        {
            sets.Clear();
            MakeListOfBasicExcercise();
            MakeListOfSupplementExcercise();
            MakeListOfCompensationExcercise();
            Navigation.PushModalAsync(new WorkoutPlayerPage(sets), false);
        }
        public List<Set> sets = new List<Set>();
        private List<Excercise> excercises;
        public Random rnd = new Random();
        private void MakeListOfBasicExcercise()
        {
            double RepsInSet = 0;
            int excerciseRandomNumber = 0;
            switch (Muscles.SelectedIndex)
            {
                case 1:
                    excercises = App.DatabaseExcercise.SelectByRegionAndSpecification(1, 1).Result;
                    break;
                case 2:
                    excercises = App.DatabaseExcercise.SelectByRegionAndSpecification(6, 1).Result;
                    break;
                case 3:
                    excercises = App.DatabaseExcercise.SelectByRegionAndSpecification(7, 1).Result;
                    break;
            }
            for (int excerciseNum = 1; excerciseNum <= BasicExcerciseNumber.Value; excerciseNum++)
            {
                RepsInSet = Convert.ToInt32(BasicSetExcerciseNumber.Value);
              
                excerciseRandomNumber = rnd.Next(0, excercises.Count());
                
                RepsInSet = RepsInSet + 5;
                for (int excerciseSet = 1; excerciseSet <= BasicSetExcerciseNumber.Value; excerciseSet++)
                {
                    if (VolumePower.IsToggled)
                    {
                        sets.Add(new Set { ID_excercisePK = excercises[excerciseRandomNumber].ID_excercise, Reps = Convert.ToInt32(RepsInSet).ToString() });
                        RepsInSet = RepsInSet / 1.15;
                    }
                    else
                    {
                        
                        sets.Add(new Set { ID_excercisePK = excercises[excerciseRandomNumber].ID_excercise, Reps = Convert.ToInt32(RepsInSet).ToString() });
                        RepsInSet = RepsInSet / 1.3;
                    }
                    
                }
                excercises.RemoveAt(excerciseRandomNumber);
            }
        }
        public List<int> randomNumbersList = new List<int>();
        private int TestGeneratedNumber(int excerciseRandomNumber)
        {
            if (randomNumbersList.Contains(excerciseRandomNumber))
            {
                excerciseRandomNumber = rnd.Next(0, excercises.Count());
                TestGeneratedNumber(excerciseRandomNumber);
            }
            randomNumbersList.Add(excerciseRandomNumber);
            return excerciseRandomNumber;          
        }
        private List<Excercise> SupllementExcercises;
        private void MakeListOfSupplementExcercise()
        {
            randomNumbersList.Clear();
            int excerciseRandomNumber = 0;
            switch (Muscles.SelectedIndex)
            {
                case 1:
                    excercises = App.DatabaseExcercise.SelectByRegionAndSpecification(1, 2).Result;
                    SupllementExcercises = App.DatabaseExcercise.SelectBicepsAndShoulders().Result;
                    foreach(var supplementExcercise in SupllementExcercises)
                    {
                        excercises.Add(new Excercise { ID_excercise = supplementExcercise.ID_excercise });
                    }
                    break;
                case 2:
                    excercises = App.DatabaseExcercise.SelectByRegionAndSpecification(6, 2).Result;
                    SupllementExcercises = App.DatabaseExcercise.SelectTricepsAndForearm().Result;
                    foreach (var supplementExcercise in SupllementExcercises)
                    {
                        excercises.Add(new Excercise { ID_excercise = supplementExcercise.ID_excercise });
                    }
                    break;
                case 3:
                    excercises = App.DatabaseExcercise.SelectByRegionAndSpecification(7, 2).Result;
                    SupllementExcercises = App.DatabaseExcercise.SelectBellyAndCalf().Result;
                    foreach (var supplementExcercise in SupllementExcercises)
                    {
                        excercises.Add(new Excercise { ID_excercise = supplementExcercise.ID_excercise });
                    }
                    break;
            }
            for (int excerciseNum = 1; excerciseNum <= SupplementExcerciseNumber.Value; excerciseNum++)
            {
                excerciseRandomNumber = rnd.Next(0, excercises.Count());
                for (int excerciseSet = 1; excerciseSet <= SupplementSetExcerciseNumber.Value; excerciseSet++)
                {
                    if (VolumePower.IsToggled)
                    {
                        sets.Add(new Set { ID_excercisePK = excercises[excerciseRandomNumber].ID_excercise, Reps = "10" });
                    }
                    else
                    {
                        sets.Add(new Set { ID_excercisePK = excercises[excerciseRandomNumber].ID_excercise, Reps = "8" });
                    }
                }
                excercises.RemoveAt(excerciseRandomNumber);
            }
        }
        
        private void MakeListOfCompensationExcercise()
        {
            randomNumbersList.Clear();
            int excerciseRandomNumber = 0;
            excercises = App.DatabaseExcercise.SelectCompensationExcercises().Result;

            for (int excerciseNum = 1; excerciseNum <= CompensatorExcerciseNumber.Value; excerciseNum++)
            {
                excerciseRandomNumber = rnd.Next(0, excercises.Count());
                for (int excerciseSet = 1; excerciseSet <= CompensatorSetExcerciseNumber.Value; excerciseSet++)
                {
                    sets.Add(new Set { ID_excercisePK = excercises[excerciseRandomNumber].ID_excercise, Reps = "10" });
                }
                excercises.RemoveAt(excerciseRandomNumber);
            }
           
        }
        private void VolumePower_Toggled(object sender, ToggledEventArgs e)
        {
            CountTotalTrainingTime();
        }
        private void CheckPersonalRecords()
        {
            var records= App.DatabasePersonalRecord.SelectHighestRecord().Result;
            if(records[0].Benchpress == 0 || records[0].Deathlift == 0 || records[0].Squat == 0)
            {
                UseInsertedData.IsVisible = false;
                UseData.IsVisible = false;
            }
        }
        private void UseData_Toggled(object sender, ToggledEventArgs e)
        {
            if (UseData.IsToggled) {
                var records = App.DatabasePersonalRecord.SelectHighestRecord().Result;
                BenchpressE.IsVisible = false;
                DeathliftE.IsVisible = false;
                SquatE.IsVisible = false;
                RecordInformations.IsVisible = true;
                RecordInformations.Text = "Zadaná data" +
                    " Bench-press: "+ records[0].Benchpress+ "Kg \r\n Deathlift: " + records[0].Deathlift+ "Kg \r\n Dřep: " + records[0].Squat+"Kg";
            }
            else
            {
                BenchpressE.IsVisible = true;
                DeathliftE.IsVisible = true;
                SquatE.IsVisible = true;
                RecordInformations.IsVisible = false;
            }
        }
        private void GenerateTraining_Clicked(object sender, EventArgs e)
        {
            ChechUserInput();
        }
        private void ChechUserInput()
        {
            if(UseData.IsToggled && DaysNumbers.SelectedIndex != 0)
            {
                SetRecordsClass(false);
            }
            else if(!UseData.IsToggled && !string.IsNullOrEmpty(BenchpressE.Text) && !string.IsNullOrEmpty(DeathliftE.Text) && !string.IsNullOrEmpty(SquatE.Text)){
                SetRecordsClass(true);
            }
            else
            {
                Warning.TextColor = Color.Red;
                Warning.Text = "Zkontroluj si vložené údaje";
            }
        }
        private void SetRecordsClass(bool EntryUsed)
        {
            if (EntryUsed) {
                SaveRecordsToDatabase();   
            }
            else
            {
                var Records = App.DatabasePersonalRecord.SelectHighestRecord().Result;
                personalRecord.Squat = Records[0].Squat;
                personalRecord.Deathlift= Records[0].Deathlift;
                personalRecord.Benchpress= Records[0].Benchpress;
            }
            GenerateTrainingPlan();


        }
        public PersonalRecord personalRecord = new PersonalRecord();
        private void SaveRecordsToDatabase() {
            personalRecord.Benchpress = App.setNumber(BenchpressE.Text);
            personalRecord.Deathlift = App.setNumber(DeathliftE.Text);
            personalRecord.Squat = App.setNumber(SquatE.Text);
            personalRecord.Date = DateTime.Today.ToString();
            App.DatabasePersonalRecord.SaveItemAsync(personalRecord);
        }
        private void GenerateTrainingPlan()
        {

        }
    }
}