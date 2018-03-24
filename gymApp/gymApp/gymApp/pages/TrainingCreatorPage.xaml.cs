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
        public int DayNumber;
        public int SetIDNumber;
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
            if(UseData.IsToggled && DaysNumbers.SelectedIndex != 0 && !string.IsNullOrEmpty(TrainNameE.Text))
            {
                SetRecordsClass(false);
            }
            else if(!UseData.IsToggled && !string.IsNullOrEmpty(BenchpressE.Text) && !string.IsNullOrEmpty(DeathliftE.Text) && !string.IsNullOrEmpty(SquatE.Text) && !string.IsNullOrEmpty(TrainNameE.Text))
            {               
                SetRecordsClass(true);
            }
            else
            {
                Warning.TextColor = Color.Red;
                Warning.Text = "Zkontroluj si vložené údaje";
            }
        }

        private void ShowGenerating()
        {
            LongPlan.IsVisible = false;
            Generating.IsVisible = true;
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
            CreateTrainingUnit();

            CreatePhaseOne();
            CreatePhaseTwo();
            CreatePhaseThree();

            SaveSetsToDatabase();
            
            Indicator.IsVisible = false;
            var test = App.DatabaseSet.Select().Result;
            var test1 = App.DatabaseDay.Select().Result;
            var test2 = App.DatabaseTrainingUnit.Select().Result;
            Informations.Text = "Vytvořeno";
            Overview.IsVisible = true;
        }
        public List<double> ListOfInfluence = new List<double>();
        private int Volume = 0;
        private int trainingUnitID = 0;

        private void CreatePhaseOne()
        {
            int Training = 1;
            var ID_lastDay = App.DatabaseDay.SelectLastID().Result;
            if(ID_lastDay.Count == 0) { DayNumber = 0; } else { DayNumber = ID_lastDay[0].ID_Day; }


            var ID_LastSet = App.DatabaseSet.SelectLastID().Result;
            if (ID_LastSet.Count == 0) { SetIDNumber = 0; } else { SetIDNumber = ID_LastSet[0].ID_set; }
            


            double Influencer = App.setNumber(DaysNumbers.SelectedItem.ToString()) * 4;
            Influencer = Influencer / 3;
            Influencer = 30 / Influencer; // 70-40% maxima
            Influencer = Influencer / 100;
            
            weightInfluence.Influence = 0.4 + Influencer ;
            for (int i = 1; i <= 1+4* App.setNumber(DaysNumbers.SelectedItem.ToString()); i++) // Počítání tréninkových jednotek (dnů)
            {
                DayNumber++;
                 switch (Training) //výběr partie která se bude cvičit
                 {

                     case 1:
                        CreateTrainingDay(Training);
                        Volume++;
                        SetWeightInfluence(Influencer, i);
                        ListOfInfluence.Add(weightInfluence.Influence);
                        //sets.Clear();
                        var excercises = App.DatabaseExcercise.SelectExcerciseByRegionWithoutMainExcercise(1, "Benč").Result;
                        sets = GenerateWarmUp(excercises);

                        excercises = App.DatabaseExcercise.SelectMainExcerciseByRegionAndName(1,"Benč").Result;
                        int RepsInSet = 12;
                        double totalWeight = 0;
                        double weightAplifier = 0;
                        for (double excerciseSet = 1; excerciseSet <= 5; excerciseSet++)
                        {
                            SetIDNumber++;
                            if (Convert.ToInt32(personalRecord.Benchpress * weightInfluence.Influence*10) < 20)
                            {
                                totalWeight = 20;
                            }
                            else
                            {
                                totalWeight = Convert.ToInt32(personalRecord.Benchpress * weightInfluence.Influence);
                                weightAplifier = totalWeight *(0.5 + (excerciseSet / 10));
                                if(weightAplifier < 20)
                                {
                                    weightAplifier = 20;
                                }
                            }
                            RepsInSet = RepsInSet - 2;
                            sets.Add(new Set { ID_excercisePK = excercises[0].ID_excercise, Reps = (RepsInSet).ToString(), ID_day = DayNumber, ID_set = SetIDNumber, Weight = Convert.ToInt32(weightAplifier) });
                        }
                        excercises = App.DatabaseExcercise.SelectBicepsAndShoulders().Result;
                        sets = GenerateConclusion(excercises,4);
                        Training++;
                        //SaveSetsToDatabase(sets, ID_lastDay);
                        break;
                     case 2:
                        CreateTrainingDay(Training);
                        Volume++;
                        SetWeightInfluence(Influencer, i);
                        ListOfInfluence.Add(weightInfluence.Influence);
                        excercises = App.DatabaseExcercise.SelectExcerciseByRegionWithoutMainExcercise(6, "Mrtvý tah").Result;
                        sets = GenerateWarmUp(excercises);

                        excercises = App.DatabaseExcercise.SelectMainExcerciseByRegionAndName(6, "Mrtvý tah").Result;
                        RepsInSet = 12;
                        totalWeight = 0;
                        weightAplifier = 0;
                        for (double excerciseSet = 1; excerciseSet <= 5; excerciseSet++)
                        {
                            SetIDNumber++;
                            if (Convert.ToInt32(personalRecord.Benchpress * weightInfluence.Influence * 10) < 20)
                            {
                                totalWeight = 20;
                            }
                            else
                            {
                                totalWeight = Convert.ToInt32(personalRecord.Benchpress * weightInfluence.Influence);
                                weightAplifier = totalWeight * (0.5 + (excerciseSet / 10));
                                if (weightAplifier < 20)
                                {
                                    weightAplifier = 20;
                                }
                            }
                            RepsInSet = RepsInSet - 2;
                            sets.Add(new Set { ID_excercisePK = excercises[0].ID_excercise, Reps = (RepsInSet).ToString(), ID_set = SetIDNumber, Weight = Convert.ToInt32(weightAplifier) });
                        }
                        excercises = App.DatabaseExcercise.SelectBicepsAndShoulders().Result;
                        sets = GenerateConclusion(excercises, 4);
                        Training++;
                        //SaveSetsToDatabase(sets, ID_lastDay);
                        break;
                     case 3:
                        CreateTrainingDay(Training);
                        Volume++;
                        SetWeightInfluence(Influencer,i);
                        ListOfInfluence.Add(weightInfluence.Influence);

                        excercises = App.DatabaseExcercise.SelectExcerciseByRegionWithoutMainExcercise(7, "Dřep").Result;
                         sets = GenerateWarmUp(excercises);

                         excercises = App.DatabaseExcercise.SelectMainExcerciseByRegionAndName(7, "Dřep").Result;
                         RepsInSet = 12;
                         totalWeight = 0;
                         weightAplifier = 0;
                        for (double excerciseSet = 1; excerciseSet <= 5; excerciseSet++)
                        {
                            SetIDNumber++;
                            if (Convert.ToInt32(personalRecord.Benchpress * weightInfluence.Influence * 10) < 20)
                            {
                                totalWeight = 20;
                            }
                            else
                            {
                                totalWeight = Convert.ToInt32(personalRecord.Benchpress * weightInfluence.Influence);
                                weightAplifier = totalWeight * (0.5 + (excerciseSet / 10));
                                if (weightAplifier < 20)
                                {
                                    weightAplifier = 20;
                                }
                            }
                            RepsInSet = RepsInSet - 2;
                            sets.Add(new Set { ID_excercisePK = excercises[0].ID_excercise, Reps = (RepsInSet).ToString(),ID_set = SetIDNumber, ID_day = DayNumber, Weight = Convert.ToInt32(weightAplifier) });
                        }
                        excercises = App.DatabaseExcercise.SelectBicepsAndShoulders().Result;
                        sets = GenerateConclusion(excercises, 4);
                        Training=1;
                        //SaveSetsToDatabase(sets, ID_lastDay);
                        break;
                 }                
                
            }
        }
        private void CreatePhaseTwo()
        {
            int Training = 1;

            double Influencer = App.setNumber(DaysNumbers.SelectedItem.ToString()) * 4;
            Influencer = Influencer / 3;
            Influencer = 30 / Influencer; // 70-40% maxima
            Influencer = Influencer / 100;
            weightInfluence.Influence = 0.5 + Influencer;
            for (int i = 1; i <= 1 + 4 * App.setNumber(DaysNumbers.SelectedItem.ToString()); i++) // Počítání tréninkových jednotek (dnů)
            {
                DayNumber++;
                switch (Training) //výběr partie která se bude cvičit
                {
                    case 1:
                        CreateTrainingDay(Training);
                        Volume++;
                        SetWeightInfluence(Influencer, i);
                        ListOfInfluence.Add(weightInfluence.Influence);
                        //sets.Clear();
                        var excercises = App.DatabaseExcercise.SelectExcerciseByRegionWithoutMainExcercise(1, "Benč").Result;
                        sets = GenerateWarmUp(excercises);

                        excercises = App.DatabaseExcercise.SelectMainExcerciseByRegionAndName(1, "Benč").Result;
                        int RepsInSet = 10;
                        double totalWeight = 0;
                        double weightAplifier = 0;
                        for (double excerciseSet = 1; excerciseSet <= 6; excerciseSet++)
                        {
                            SetIDNumber++;
                            if (Convert.ToInt32(personalRecord.Benchpress * weightInfluence.Influence * 10) < 20)
                            {
                                totalWeight = 20;
                            }
                            else
                            {
                                totalWeight = Convert.ToInt32(personalRecord.Benchpress * weightInfluence.Influence);
                                weightAplifier = totalWeight * (0.4 + (excerciseSet / 10));
                                if (weightAplifier < 20)
                                {
                                    weightAplifier = 20;
                                }
                            }
                            if(RepsInSet > 2)
                            {
                                RepsInSet = RepsInSet - 2;
                            }
                            sets.Add(new Set { ID_excercisePK = excercises[0].ID_excercise, Reps = (RepsInSet).ToString(), ID_day = DayNumber,ID_set = SetIDNumber, Weight = Convert.ToInt32(weightAplifier) });
                        }
                        excercises = App.DatabaseExcercise.SelectBicepsAndShoulders().Result;
                        sets = GenerateConclusion(excercises, 3);
                        Training++;
                        //SaveSetsToDatabase(sets, ID_lastDay);
                        break;
                    case 2:
                        Volume++;
                        SetWeightInfluence(Influencer, i);
                        ListOfInfluence.Add(weightInfluence.Influence);
                        CreateTrainingDay(Training);
                        //sets.Clear();
                        excercises = App.DatabaseExcercise.SelectExcerciseByRegionWithoutMainExcercise(6, "Mrtvý tah").Result;
                        sets = GenerateWarmUp(excercises);

                        excercises = App.DatabaseExcercise.SelectMainExcerciseByRegionAndName(6, "Mrtvý tah").Result;
                        RepsInSet = 10;
                        totalWeight = 0;
                        weightAplifier = 0;
                        for (double excerciseSet = 1; excerciseSet <= 6; excerciseSet++)
                        {
                            SetIDNumber++;
                            if (Convert.ToInt32(personalRecord.Benchpress * weightInfluence.Influence * 10) < 20)
                            {
                                totalWeight = 20;
                            }
                            else
                            {
                                totalWeight = Convert.ToInt32(personalRecord.Benchpress * weightInfluence.Influence);
                                weightAplifier = totalWeight * (0.4 + (excerciseSet / 10));
                                if (weightAplifier < 20)
                                {
                                    weightAplifier = 20;
                                }
                            }
                            if (RepsInSet > 2)
                            {
                                RepsInSet = RepsInSet - 2;
                            }
                            sets.Add(new Set { ID_excercisePK = excercises[0].ID_excercise, Reps = (RepsInSet).ToString(), ID_day = DayNumber, ID_set = SetIDNumber, Weight = Convert.ToInt32(weightAplifier) });
                        }
                        excercises = App.DatabaseExcercise.SelectBicepsAndShoulders().Result;
                        sets = GenerateConclusion(excercises, 3);
                        Training++;
                        //SaveSetsToDatabase(sets, ID_lastDay);
                        break;
                    case 3:
                        Volume++;
                        SetWeightInfluence(Influencer, i);
                        ListOfInfluence.Add(weightInfluence.Influence);


                        CreateTrainingDay(Training);
                        //sets.Clear();
                        excercises = App.DatabaseExcercise.SelectExcerciseByRegionWithoutMainExcercise(7, "Dřep").Result;
                        sets = GenerateWarmUp(excercises);

                        excercises = App.DatabaseExcercise.SelectMainExcerciseByRegionAndName(7, "Dřep").Result;
                        RepsInSet = 10;
                        totalWeight = 0;
                        weightAplifier = 0;
                        for (double excerciseSet = 1; excerciseSet <= 6; excerciseSet++)
                        {
                            SetIDNumber++;
                            if (Convert.ToInt32(personalRecord.Benchpress * weightInfluence.Influence * 10) < 20)
                            {
                                totalWeight = 20;
                            }
                            else
                            {
                                totalWeight = Convert.ToInt32(personalRecord.Benchpress * weightInfluence.Influence);
                                weightAplifier = totalWeight * (0.4 + (excerciseSet / 10));
                                if (weightAplifier < 20)
                                {
                                    weightAplifier = 20;
                                }
                            }
                            if (RepsInSet > 2)
                            {
                                RepsInSet = RepsInSet - 2;
                            }
                            sets.Add(new Set { ID_excercisePK = excercises[0].ID_excercise, Reps = (RepsInSet).ToString(), ID_day = DayNumber, ID_set = SetIDNumber, Weight = Convert.ToInt32(weightAplifier) });
                        }
                        excercises = App.DatabaseExcercise.SelectBicepsAndShoulders().Result;
                        sets = GenerateConclusion(excercises, 3);
                        Training=1;
                        // SaveSetsToDatabase(sets, ID_lastDay);
                        break;
                }

            }
        }
        private void CreatePhaseThree()
        {
            int Training = 1;

            double Influencer = App.setNumber(DaysNumbers.SelectedItem.ToString()) * 4;
            Influencer = Influencer / 3;
            Influencer = 60 / Influencer; // 70-40% maxima
            Influencer = Influencer / 100;
            weightInfluence.Influence = 0.6 + Influencer;
            for (int i = 1; i <= 1 + 4 * App.setNumber(DaysNumbers.SelectedItem.ToString()); i++) // Počítání tréninkových jednotek (dnů)
            {
                DayNumber++;
                switch (Training) //výběr partie která se bude cvičit
                {
                    case 1:
                        CreateTrainingDay(Training);
                        Volume++;
                        SetWeightInfluence(Influencer, i);
                        ListOfInfluence.Add(weightInfluence.Influence);
                        //sets.Clear();
                        var excercises = App.DatabaseExcercise.SelectExcerciseByRegionWithoutMainExcercise(1, "Benč").Result;
                        sets = GenerateWarmUp(excercises);

                        excercises = App.DatabaseExcercise.SelectMainExcerciseByRegionAndName(1, "Benč").Result;
                        int RepsInSet = 10;
                        double totalWeight = 0;
                        double weightAplifier = 0;
                        for (double excerciseSet = 1; excerciseSet <= 7; excerciseSet++)
                        {
                            SetIDNumber++;
                            if (Convert.ToInt32(personalRecord.Benchpress * weightInfluence.Influence * 10) < 20)
                            {
                                totalWeight = 20;
                            }
                            else
                            {
                                totalWeight = Convert.ToInt32(personalRecord.Benchpress * weightInfluence.Influence);
                                weightAplifier = totalWeight * (0.3 + (excerciseSet / 10));
                                if (weightAplifier < 20)
                                {
                                    weightAplifier = 20;
                                }
                            }
                            if (RepsInSet > 2){ RepsInSet = RepsInSet - 2;}
                            if(excerciseSet == 7){RepsInSet = RepsInSet - 1;}
                            sets.Add(new Set { ID_excercisePK = excercises[0].ID_excercise, Reps = (RepsInSet).ToString(), ID_day = DayNumber, ID_set = SetIDNumber, Weight = Convert.ToInt32(weightAplifier) });
                        }
                        excercises = App.DatabaseExcercise.SelectBicepsAndShoulders().Result;
                        sets = GenerateConclusion(excercises, 2);
                        Training++;
                        // SaveSetsToDatabase(sets, ID_lastDay);
                        break;
                    case 2:
                        Volume++;
                        SetWeightInfluence(Influencer, i);
                        ListOfInfluence.Add(weightInfluence.Influence);
                        CreateTrainingDay(Training);
                        //sets.Clear();
                        excercises = App.DatabaseExcercise.SelectExcerciseByRegionWithoutMainExcercise(6, "Mrtvý tah").Result;
                        sets = GenerateWarmUp(excercises);

                        excercises = App.DatabaseExcercise.SelectMainExcerciseByRegionAndName(6, "Mrtvý tah").Result;
                        RepsInSet = 10;
                        totalWeight = 0;
                        weightAplifier = 0;
                        for (double excerciseSet = 1; excerciseSet <= 7; excerciseSet++)
                        {
                            SetIDNumber++;
                            if (Convert.ToInt32(personalRecord.Benchpress * weightInfluence.Influence * 10) < 20)
                            {
                                totalWeight = 20;
                            }
                            else
                            {
                                totalWeight = Convert.ToInt32(personalRecord.Benchpress * weightInfluence.Influence);
                                weightAplifier = totalWeight * (0.3 + (excerciseSet / 10));
                                if (weightAplifier < 20)
                                {
                                    weightAplifier = 20;
                                }
                            }
                            if (RepsInSet > 2) { RepsInSet = RepsInSet - 2; }
                            if (excerciseSet == 7) { RepsInSet = RepsInSet - 1; }
                            sets.Add(new Set { ID_excercisePK = excercises[0].ID_excercise, Reps = (RepsInSet).ToString(), ID_day = DayNumber, ID_set = SetIDNumber, Weight = Convert.ToInt32(weightAplifier) });
                        }
                        excercises = App.DatabaseExcercise.SelectBicepsAndShoulders().Result;
                        sets = GenerateConclusion(excercises, 2);
                        Training++;
                        //SaveSetsToDatabase(sets, ID_lastDay);
                        break;
                    case 3:
                        Volume++;
                        SetWeightInfluence(Influencer, i);
                        ListOfInfluence.Add(weightInfluence.Influence);


                        CreateTrainingDay(Training);
                        //sets.Clear();
                        excercises = App.DatabaseExcercise.SelectExcerciseByRegionWithoutMainExcercise(7, "Dřep").Result;
                        sets = GenerateWarmUp(excercises);

                        excercises = App.DatabaseExcercise.SelectMainExcerciseByRegionAndName(7, "Dřep").Result;
                        RepsInSet = 10;
                        totalWeight = 0;
                        weightAplifier = 0;
                        for (double excerciseSet = 1; excerciseSet <= 7; excerciseSet++)
                        {
                            SetIDNumber++;
                            if (Convert.ToInt32(personalRecord.Benchpress * weightInfluence.Influence * 10) < 20)
                            {
                                totalWeight = 20;
                            }
                            else
                            {
                                totalWeight = Convert.ToInt32(personalRecord.Benchpress * weightInfluence.Influence);
                                weightAplifier = totalWeight * (0.3 + (excerciseSet / 10));
                                if (weightAplifier < 20)
                                {
                                    weightAplifier = 20;
                                }
                            }
                            if (RepsInSet > 2) { RepsInSet = RepsInSet - 2; }
                            if (excerciseSet == 7) { RepsInSet = RepsInSet - 1; }
                            sets.Add(new Set { ID_excercisePK = excercises[0].ID_excercise, Reps = (RepsInSet).ToString(), ID_day = DayNumber, ID_set = SetIDNumber, Weight = Convert.ToInt32(weightAplifier) });
                        }
                        excercises = App.DatabaseExcercise.SelectBicepsAndShoulders().Result;
                        sets = GenerateConclusion(excercises, 2);
                        Training=1;
                        //SaveSetsToDatabase(sets, ID_lastDay);
                        break;
                }
            }
            
        }

        private void SaveSetsToDatabase()
        {
            foreach(var set in sets)
            {
                Set setDatabase = new Set();
                setDatabase.ID_set = set.ID_set;
                setDatabase.ID_excercisePK = set.ID_excercisePK;
                setDatabase.ID_day = set.ID_day;
                setDatabase.Reps = set.Reps;
                setDatabase.Weight = set.Weight;
                App.DatabaseSet.SaveItemAsync(setDatabase);
            }
        }

        private void CreateTrainingUnit()
        {
            TrainingUnit trainingUnit = new TrainingUnit();
            trainingUnit.state = 0;
            trainingUnit.Title = TrainNameE.Text;
            trainingUnit.CreatedDate = DateTime.Today.ToString();
            App.DatabaseTrainingUnit.SaveItemAsync(trainingUnit);
            var id = App.DatabaseTrainingUnit.SelectLastID().Result;
            trainingUnitID = id[0].ID_TrainingUnit;
        }
        private void CreateTrainingDay(int training)
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
            day.ID_TrainingUnit= trainingUnitID;
            App.DatabaseDay.SaveItemAsync(day);
        }

        private List<Set> GenerateWarmUp(List<Excercise> excercises)
        {
            for (int excerciseNum = 1; excerciseNum <= 2; excerciseNum++)
            {
                excerciseRandomNumber = rnd.Next(0, excercises.Count());
                int RepsInSet = 12;
                for (int excerciseSet = 1; excerciseSet <= 4; excerciseSet++)
                {
                        SetIDNumber++;
                        RepsInSet = RepsInSet - 2;
                        sets.Add(new Set { ID_excercisePK = excercises[excerciseRandomNumber].ID_excercise, ID_day = DayNumber, ID_set = SetIDNumber, Reps = (RepsInSet).ToString() });
                }
                excercises.RemoveAt(excerciseRandomNumber);
            }
            return sets;
        }
        private List<Set> GenerateConclusion(List<Excercise> excercises,int setting)
        {
            for (int excerciseNum = 1; excerciseNum <= setting; excerciseNum++)
            {
                excerciseRandomNumber = rnd.Next(0, excercises.Count());
                int RepsInSet = 10;
                for (int excerciseSet = 1; excerciseSet <= setting; excerciseSet++)
                {
                    SetIDNumber++;
                    if (excerciseSet > 2)
                    {
                        RepsInSet = RepsInSet - 2;
                    }
                    sets.Add(new Set { ID_excercisePK = excercises[excerciseRandomNumber].ID_excercise, ID_day = DayNumber, ID_set = SetIDNumber, Reps = (RepsInSet).ToString() });
                }
                excercises.RemoveAt(excerciseRandomNumber);
            }
            return sets;
        }
        public bool test = true;
        private void SetWeightInfluence(double Influencer, int i)
        {
            if (test)
            {
                if (Volume == 4)//Výběr zda se jedná o lehký nebo těžký trénink
                {
                    weightInfluence.Influence = weightInfluence.Influence + Influencer * 2;
                    Volume = 0;
                }
                else if (Volume == 1)
                {
                    weightInfluence.Influence = weightInfluence.Influence - Influencer;
                }
            }
            if (4 * App.setNumber(DaysNumbers.SelectedItem.ToString()) - 6 == i)
            {
                //test = false;   
                    weightInfluence.Influence = weightInfluence.Influence - Influencer;
            }
            if (4 * App.setNumber(DaysNumbers.SelectedItem.ToString()) - 3 == i)
            {
                test = false;
                weightInfluence.Influence = weightInfluence.Influence + Influencer * 2;
            }
            if (4 * App.setNumber(DaysNumbers.SelectedItem.ToString()) == i)
            {
                test = false;
                weightInfluence.Influence = weightInfluence.Influence + Influencer ;
            }
        }

        private void GenerateTraining_Pressed(object sender, EventArgs e)
        {
            if (UseData.IsToggled && DaysNumbers.SelectedIndex != 0 && !string.IsNullOrEmpty(TrainNameE.Text))
            {
                ShowGenerating();
            }
            else if (!UseData.IsToggled && !string.IsNullOrEmpty(BenchpressE.Text) && !string.IsNullOrEmpty(DeathliftE.Text) && !string.IsNullOrEmpty(SquatE.Text) && !string.IsNullOrEmpty(TrainNameE.Text))
            {
                ShowGenerating();
            }
        }

        private void Overview_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new WorkoutOverviewPage(), false);
        }
    }
}