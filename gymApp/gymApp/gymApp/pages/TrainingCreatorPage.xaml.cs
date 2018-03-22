using gymApp.classes;
using gymApp.classes.Functions;
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
        public List<Set> sets = new List<Set>();
        private List<Excercise> excercises;
        public Random rnd = new Random();
        public TrainingCreatorTimer totalTime = new TrainingCreatorTimer();
        public int excerciseRandomNumber;
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
            for (int i = 3; i <= 6; i++)
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
        
        private void MakeListOfBasicExcercise()
        {
            double RepsInSet = 0;
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
            CreatePlan();
        }
        public PersonalRecord personalRecord = new PersonalRecord();
        private void SaveRecordsToDatabase() {
            personalRecord.Benchpress = App.setNumber(BenchpressE.Text);
            personalRecord.Deathlift = App.setNumber(DeathliftE.Text);
            personalRecord.Squat = App.setNumber(SquatE.Text);
            personalRecord.Date = DateTime.Today.ToString();
            App.DatabasePersonalRecord.SaveItemAsync(personalRecord);
        }
        public WeightInfluence weightInfluence = new WeightInfluence();
        private void CreatePlan()
        {
            CreatePhaseOne();
            //CreatePhaseTwo();
            //CreatePhaseThree();
            
        }
        public List<double> ListOfInfluence = new List<double>();
        private void CreatePhaseOne()
        {
            int Training = 1;
            int Volume = 0;
            weightInfluence.Influence = 0.4;
            
            for (int i = 0; i <= 4* App.setNumber(DaysNumbers.SelectedItem.ToString()); i++) // Počítání tréninkových jednotek (dnů)
            {
                double Influencer = App.setNumber(DaysNumbers.SelectedItem.ToString()) * 4;
                Influencer = Influencer / 3;
                Influencer = 30 / Influencer; // 70-40% maxima
                Influencer = Influencer / 100;
                int ID_lastDay;
                switch (Training) //výběr partie která se bude cvičit
                {
                    
                    case 1:
                        ID_lastDay = CreateTrainingDay(Training);
                        Volume++;
                        sets.Clear();
                        var excercises = App.DatabaseExcercise.SelectExcerciseByRegionWithoutMainExcercise(1, "Benč").Result;
                        sets = GenerateWarmUp(excercises);

                        excercises = App.DatabaseExcercise.SelectMainExcerciseByRegionAndName(1,"Benč").Result;
                        SetWeightInfluence(Volume, i, Influencer);
                        ListOfInfluence.Add(weightInfluence.Influence);
                        int RepsInSet = 12;
                        for (int excerciseSet = 1; excerciseSet <= 5; excerciseSet++)
                        {
                            RepsInSet = RepsInSet - 2;
                            sets.Add(new Set { ID_excercisePK = excercises[0].ID_excercise, Reps = (RepsInSet).ToString(), Weight = Convert.ToInt32(personalRecord.Benchpress * weightInfluence.Influence) });
                        }
                        excercises = App.DatabaseExcercise.SelectBicepsAndShoulders().Result;
                        sets = GenerateConclusion(excercises);
                        Training++;
                        break;
                    case 2:
                        Volume++;
                        ID_lastDay = CreateTrainingDay(Training);
                        sets.Clear();
                        excercises = App.DatabaseExcercise.SelectExcerciseByRegionWithoutMainExcercise(6, "Mrtvý tah").Result;
                        sets = GenerateWarmUp(excercises);

                        excercises = App.DatabaseExcercise.SelectMainExcerciseByRegionAndName(6, "Mrtvý tah").Result;
                        SetWeightInfluence(Volume, i, Influencer);
                        ListOfInfluence.Add(weightInfluence.Influence);
                        RepsInSet = 12;
                        for (int excerciseSet = 1; excerciseSet <= 5; excerciseSet++)
                        {
                            RepsInSet = RepsInSet - 2;
                            sets.Add(new Set { ID_excercisePK = excercises[0].ID_excercise, Reps = (RepsInSet).ToString(), Weight = Convert.ToInt32(personalRecord.Benchpress * weightInfluence.Influence) });
                        }
                        excercises = App.DatabaseExcercise.SelectTricepsAndForearm().Result;
                        sets = GenerateConclusion(excercises);
                        Training++;
                        break;
                    case 3:
                        Volume++;
                        ID_lastDay = CreateTrainingDay(Training);
                        sets.Clear();
                        excercises = App.DatabaseExcercise.SelectExcerciseByRegionWithoutMainExcercise(7, "Dřep").Result;
                        sets = GenerateWarmUp(excercises);

                        excercises = App.DatabaseExcercise.SelectMainExcerciseByRegionAndName(7, "Dřep").Result;
                        SetWeightInfluence(Volume, i, Influencer);
                        ListOfInfluence.Add(weightInfluence.Influence);
                        RepsInSet = 12;
                        for (int excerciseSet = 1; excerciseSet <= 5; excerciseSet++)
                        {
                            RepsInSet = RepsInSet - 2;
                            sets.Add(new Set { ID_excercisePK = excercises[0].ID_excercise, Reps = (RepsInSet).ToString(), Weight = Convert.ToInt32(personalRecord.Benchpress * weightInfluence.Influence) });
                        }
                        excercises = App.DatabaseExcercise.SelectBellyAndCalf().Result;
                        sets = GenerateConclusion(excercises);
                        Training =1;
                        SaveSetsToDatabase(sets, ID_lastDay);
                        break;
                }                  
            }

            var bla1 = App.DatabaseExcercise.SelectExcercise().Result;
            var bla = App.DatabaseSet.Select().Result;
        }

        private void SaveSetsToDatabase(List<Set> sets, int iD_lastDay)
        {
            foreach(var set in sets)
            {
                Set setDatabase = new Set();
                setDatabase.ID_excercisePK = set.ID_excercisePK;
                setDatabase.Reps = set.Reps;
                setDatabase.Weight = set.Weight;
                App.DatabaseSet.SaveItemAsync(setDatabase);
            }
        }

        private int CreateTrainingDay(int training)
        {
            Day day = new Day();
            switch (training)
            {
                case 1:
                    day.MainExcercise = "Benčpres";
                    break;
                case 2:
                    day.MainExcercise = "Mrtvý tah";
                    break;
                case 3:
                    day.MainExcercise = "Dřep";
                    break;
            }
            App.DatabaseDay.SaveItemAsync(day);
            var id = App.DatabaseDay.SelectLastID().Result;
            return id[0].ID_Day;
        }

        private List<Set> GenerateWarmUp(List<Excercise> excercises)
        {
            for (int excerciseNum = 1; excerciseNum <= 2; excerciseNum++)
            {
                excerciseRandomNumber = rnd.Next(0, excercises.Count());
                int RepsInSet = 12;
                for (int excerciseSet = 1; excerciseSet <= 4; excerciseSet++)
                {
                        RepsInSet= RepsInSet - 2;
                        sets.Add(new Set { ID_excercisePK = excercises[excerciseRandomNumber].ID_excercise, Reps = (RepsInSet).ToString() });
                }
                excercises.RemoveAt(excerciseRandomNumber);
            }
            return sets;
        }
        private List<Set> GenerateConclusion(List<Excercise> excercises)
        {
            for (int excerciseNum = 1; excerciseNum <= 4; excerciseNum++)
            {
                excerciseRandomNumber = rnd.Next(0, excercises.Count());
                int RepsInSet = 10;
                for (int excerciseSet = 1; excerciseSet <= 4; excerciseSet++)
                {
                    if(excerciseSet > 2)
                    {
                        RepsInSet = RepsInSet - 2;
                    }
                    sets.Add(new Set { ID_excercisePK = excercises[excerciseRandomNumber].ID_excercise, Reps = (RepsInSet).ToString() });
                }
                excercises.RemoveAt(excerciseRandomNumber);
            }
            return sets;
        }

        private void CreatePhaseTwo()
        {
            throw new NotImplementedException();
        }
        private void CreatePhaseThree()
        {
            throw new NotImplementedException();
        }

        private void SetWeightInfluence(int Volume, int i, double Influencer)
        {
            Volume++;
            if (Volume == 4)//Výběr zda se jedná o lehký nebo těžký trénink
            {
                weightInfluence.Influence = weightInfluence.Influence + Influencer * 2;
                Volume = 0;
                weightInfluence.Influence = weightInfluence.Influence - Influencer;
            }
            else
            {
                if (4 * App.setNumber(DaysNumbers.SelectedItem.ToString()) - 6 < i)
                {
                    if (4 * App.setNumber(DaysNumbers.SelectedItem.ToString()) - 3 < i){
                        weightInfluence.Influence = weightInfluence.Influence + Influencer * 2;
                    }
                    else
                    {
                        weightInfluence.Influence = weightInfluence.Influence - Influencer;
                    }
                }
                else
                {
                    weightInfluence.Influence = weightInfluence.Influence + Influencer;
                }
            }
        }
    }
}