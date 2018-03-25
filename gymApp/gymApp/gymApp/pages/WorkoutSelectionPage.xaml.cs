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
	public partial class WorkoutSelectionPage : ContentPage
	{
		public WorkoutSelectionPage ()
		{
			InitializeComponent ();
		}

        private void QuickTrain_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new TrainingCreatorPage(1), false);
        }

        private void WorkoutPlan_Clicked(object sender, EventArgs e)
        {
            var plan = App.DatabaseTrainingUnit.SelectLastID().Result;
            if (plan.Count == 1)
            {
                if (plan[0].state == 0)
                {
                    var LastDay = App.DatabaseDay.SelectFirstActiveDay().Result;
                    var sets = App.DatabaseSet.SelectSetsByTrainingUnit(LastDay[0].ID_Day).Result;
                    List<Set> setsList = new List<Set>();
                    foreach (var set in sets)
                    {                      
                        var excercise = App.DatabaseExcercise.SelectDetailedExcercise(set.ID_excercisePK).Result;
                        setsList.Add(new Set {ID_day= set.ID_day, Reps = set.Reps, Weight=  set.Weight, ID_excercisePK = excercise[0].ID_excercise });
                    }
                    Navigation.PushModalAsync(new WorkoutPlayerPage(setsList), false);
                }
                else
                {
                    Navigation.PushModalAsync(new TrainingCreatorPage(2), false);
                }
            }
            else
            {
                Navigation.PushModalAsync(new TrainingCreatorPage(2), false);
            }
        }
    }
}