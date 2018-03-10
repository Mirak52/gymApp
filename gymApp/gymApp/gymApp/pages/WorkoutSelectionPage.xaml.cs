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
            Navigation.PushModalAsync(new TrainingCreatorPage(2), false);
        }
    }
}