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
            Navigation.PushAsync(new pages.registration());

        }
    }
}
