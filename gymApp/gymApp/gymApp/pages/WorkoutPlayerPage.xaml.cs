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
        public WorkoutPlayerPage(List<Set> sets)
        {
            InitializeComponent();
            CreateListView(sets);
        }

        private void CreateListView(List<Set> sets)
        {
            List<Set> excerciseList = new List<Set>();
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
                    reps.Remove(0, 1);
                    excerciseList.Add(new Set { ExcerciseName= number.ToString(), Reps= reps });
                    number = set.ID_excercisePK;
                    reps = null;
                    reps = reps + " - " + set.Reps;
                }
            }
            excerciseList.Add(new Set { ExcerciseName = number.ToString(), Reps = reps });

        }

        private void Done_Clicked(object sender, EventArgs e)
        {

        }
    }
}