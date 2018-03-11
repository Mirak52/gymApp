using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace gymApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void registration_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new pages.registration(),false);
        }

        private void login_Clicked(object sender, EventArgs e)
        {
           //Navigation.PushModalAsync(new pages.WorkoutSelectionPage(), false);
           Navigation.PushModalAsync(new pages.MenuPage(), false);
        }
    }
}
