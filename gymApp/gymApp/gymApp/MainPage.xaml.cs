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
            /*var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
            player.Load("hallOfFame.mp3");
            player.Play();*/
            
        }

        private void registration_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new pages.WorkoutSelectionPage(), false);
        }

        private void login_Clicked(object sender, EventArgs e)
        {
            
            Application.Current.MainPage = new pages.MenuPage();
            //Navigation.PushModalAsync(new pages.MenuPage(), false);
        }
    }
}
