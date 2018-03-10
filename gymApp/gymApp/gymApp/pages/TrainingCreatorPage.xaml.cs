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
            CountTotalTrainingTime();
        }

        private void VolumePower_Toggled(object sender, ToggledEventArgs e)
        {
            CountTotalTrainingTime();
        }
    }
}