using gymApp.classes;
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
        private void login_Clicked_1(object sender, EventArgs e)
        {
            Application.Current.MainPage = new pages.MenuPage();
        }
        private void login_Pressed(object sender, EventArgs e)
        {
            Indicator.IsVisible = true;
            Informations.IsVisible = true;
        }
    }
}
