using Android.Net;
using gymApp.classes;
using Plugin.Connectivity;
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

        private void CheckFirstRunStatus()
        {
            var updater = App.DatabaseUpdater.Select().Result;
            if (!updater.Any())
            {
                if (!DoIHaveInternet())
                {
                    Informations.IsVisible = true;
                    Indicator.IsVisible = false;
                    Informations.Text = "Pro první vstup je nutný internet";
                }
                else
                {
                    Application.Current.MainPage = new pages.MenuPage();
                }
            }
            else {
                Application.Current.MainPage = new pages.MenuPage();
            }
          

        }
        public bool DoIHaveInternet()
        {
            return CrossConnectivity.Current.IsConnected;
        }

        private void login_Clicked_1(object sender, EventArgs e)
        {
            CheckFirstRunStatus();
        }
        private void login_Pressed(object sender, EventArgs e)
        {
            Indicator.IsVisible = true;
            Informations.Text= "Kontroluji aktualizace";
            Informations.IsVisible = true;
        }
    }
}
