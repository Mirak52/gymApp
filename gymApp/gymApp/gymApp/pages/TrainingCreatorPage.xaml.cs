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
            SetPicker();
            SetDefaultTime();
            CountTotalTrainingTime();
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

        private void SetPicker()
        {
            var muscles = new List<string>();
            muscles.Add("Vyber zadávání");
            muscles.Add("Prsa");
            muscles.Add("Záda");
            muscles.Add("Nohy");
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
            Information.TextColor = Color.Black;
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
            MakeListOfBasicExcercise();
            MakeListOfSupplementExcercise();
            MakeListOfCompensationExcercise();
            
        }
        /* public int ID_set { get; set; }
         public int ID_excercisePK { get; set; }
         public int Reps { get; set; }*/


        public List<Set> sets = new List<Set>();
        private List<Excercise> excercises;
        public Random rnd = new Random();
        private void MakeListOfBasicExcercise()
        {
            int RepsInSet = 0;
            int excerciseRandomNumber = 0;
            for (int excerciseNum = 0; excerciseNum <= totalTime.BasicExceNum-1; excerciseNum++)
            {
                RepsInSet = Convert.ToInt32(BasicSetExcerciseNumber.Value);
                switch (Muscles.SelectedItem)
                {
                    case "Prsa":
                        excercises = App.DatabaseExcercise.SelectByRegionAndSpecification(1,1).Result;
                        break;
                    case "Záda":
                        excercises = App.DatabaseExcercise.SelectByRegionAndSpecification(6,1).Result;
                        break;
                    case "Nohy":
                        excercises = App.DatabaseExcercise.SelectByRegionAndSpecification(7,1).Result;
                        break;
                }
                excerciseRandomNumber = rnd.Next(0, excercises.Count());
                excerciseRandomNumber = TestGeneratedNumber(excerciseRandomNumber);
                for (int excerciseSet = 0; excerciseSet <= totalTime.BasicSetsNum-1; excerciseSet++)
                {
                    if (VolumePower.IsToggled)
                    {
                        RepsInSet = RepsInSet + 5;
                        sets.Add(new Set { ID_excercisePK = excercises[excerciseRandomNumber].ID_excercise, Reps = RepsInSet });
                        RepsInSet = RepsInSet - 5 - excerciseSet;
                    }
                    else
                    {
                        RepsInSet = RepsInSet + 5;
                        sets.Add(new Set { ID_excercisePK = excercises[excerciseRandomNumber].ID_excercise, Reps = RepsInSet });
                        RepsInSet = RepsInSet - 5 - excerciseSet;
                    }
                }
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

            switch (Muscles.SelectedItem)
            {
                case "Prsa":
                    excercises = App.DatabaseExcercise.SelectByRegionAndSpecification(1, 2).Result;
                    SupllementExcercises = App.DatabaseExcercise.SelectBicepsAndShoulders().Result;
                    foreach(var supplementExcercise in SupllementExcercises)
                    {
                        excercises.Add(new Excercise { ID_excercise = supplementExcercise.ID_excercise });
                    }
                    break;
                case "Záda":
                    excercises = App.DatabaseExcercise.SelectByRegionAndSpecification(6, 2).Result;
                    SupllementExcercises = App.DatabaseExcercise.SelectTricepsAndForearm().Result;
                    foreach (var supplementExcercise in SupllementExcercises)
                    {
                        excercises.Add(new Excercise { ID_excercise = supplementExcercise.ID_excercise });
                    }
                    break;
                case "Nohy":
                    excercises = App.DatabaseExcercise.SelectByRegionAndSpecification(7, 2).Result;
                    SupllementExcercises = App.DatabaseExcercise.SelectBellyAndCalf().Result;
                    foreach (var supplementExcercise in SupllementExcercises)
                    {
                        excercises.Add(new Excercise { ID_excercise = supplementExcercise.ID_excercise });
                    }
                    break;
            }
            for (int excerciseNum = 0; excerciseNum <= totalTime.BasicExceNum - 1; excerciseNum++)
            {
                excerciseRandomNumber = rnd.Next(0, excercises.Count());
                excerciseRandomNumber = TestGeneratedNumber(excerciseRandomNumber);
                for (int excerciseSet = 0; excerciseSet <= totalTime.BasicSetsNum - 1; excerciseSet++)
                {
                    if (VolumePower.IsToggled)
                    {
                        sets.Add(new Set { ID_excercisePK = excercises[excerciseRandomNumber].ID_excercise, Reps = 10 });
                    }
                    else
                    {
                        sets.Add(new Set { ID_excercisePK = excercises[excerciseRandomNumber].ID_excercise, Reps = 8 });
                    }
                }
            }
        }
        
        private void MakeListOfCompensationExcercise()
        {
            randomNumbersList.Clear();
            int excerciseRandomNumber = 0;
            excercises = App.DatabaseExcercise.SelectCompensationExcercises().Result;

            for (int excerciseNum = 0; excerciseNum <= totalTime.BasicExceNum - 1; excerciseNum++)
            {
                excerciseRandomNumber = rnd.Next(0, excercises.Count());
                excerciseRandomNumber = TestGeneratedNumber(excerciseRandomNumber);
                for (int excerciseSet = 0; excerciseSet <= totalTime.BasicSetsNum - 1; excerciseSet++)
                {
                    sets.Add(new Set { ID_excercisePK = excercises[excerciseRandomNumber].ID_excercise, Reps = 10 });
                }
            }
        }
        private void VolumePower_Toggled(object sender, ToggledEventArgs e)
        {
            CountTotalTrainingTime();
        }
    }
}