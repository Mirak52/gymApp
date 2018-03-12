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
    public partial class WorkoutPlayerPage : ContentPage
    {
        private List<Excercise> excercise;

        public WorkoutPlayerPage(List<Set> sets)
        {
            InitializeComponent();
            CreateListView(sets);
            StartTimer();
        }
        private bool TimerRun= true;
        private int actualTime = 0;
        private void StartTimer()
        {
            actualTime = 0;
            TimerL.Text = ReturnTimeInFormat(actualTime);
            Device.StartTimer(TimeSpan.FromSeconds(1), () => {
                actualTime++;
                if (TimerRun)
                {
                    TimerL.Text = ReturnTimeInFormat(actualTime);
                    return TimerRun; //continue
                }

                actualTime = 0;
                return TimerRun; //not continue
            });
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
        private List<Set> excerciseList = new List<Set>();
        private void CreateListView(List<Set> sets)
        {
            excerciseList.Clear();
            int number = sets[0].ID_excercisePK;
            string reps = null;

            foreach (var set in sets)
            {
                if(set.ID_excercisePK == number)
                {
                    reps = reps +" - " + set.Reps;
                }
                else
                {
                    excercise = App.DatabaseExcercise.SelectDetailedExcercise(number).Result;
                    reps = reps.Substring(2);
                    excerciseList.Add(new Set { ExcerciseName = excercise[0].Name, Reps= reps });
                    number = set.ID_excercisePK;
                    reps = null;
                    reps = reps + " - " + set.Reps;
                }
            }
            reps = reps.Substring(2);
            excercise = App.DatabaseExcercise.SelectDetailedExcercise(number).Result;
            excerciseList.Add(new Set { ExcerciseName = excercise[0].Name, Reps = reps });
            ExcercisesLV.ItemsSource = null;
            ExcercisesLV.ItemsSource = excerciseList;
        }
        private void Done_Clicked(object sender, EventArgs e)
        {
            excerciseList.RemoveAt(0);
            if (excerciseList.Count() == 0)
            {
                ExcercisesLV.ItemsSource = null;
                TimerRun = false;
                Done.Text = "Úspěšně splněno";
                Done.IsEnabled = false;
            }
            else
            {
                ExcercisesLV.ItemsSource = null;
                ExcercisesLV.ItemsSource = excerciseList;
                actualTime = 0;
                TimerL.Text = ReturnTimeInFormat(actualTime);
            } 
        }

        private void ExcercisesLV_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (ExcercisesLV.SelectedItem is Set selectedExcersise)
            {
                var excercise = App.DatabaseExcercise.SelectByName(selectedExcersise.ExcerciseName).Result;
                Navigation.PushModalAsync(new ExcerciseDetailPage(excercise[0].ID_excercise), false);
            }
        }
    }
}