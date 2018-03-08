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
    public partial class StatsPage : ContentPage
    {
        public StatsPage()
        {
            InitializeComponent();
            SetPicker();
        }

        private void SetPicker()
        {
            var monkeyList = new List<string>();
            monkeyList.Add("Sledované údaje");
            monkeyList.Add("Maximálky");
            Picker.ItemsSource = monkeyList;
            
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var pick = Picker.SelectedIndex;
            if(pick == 0)
            {
                Stats.IsVisible = true;
                BodyStats.IsVisible = true;
                Records.IsVisible = false;
            }
            else if(pick == 1)
            {
                Stats.IsVisible = true;
                BodyStats.IsVisible = false;
                Records.IsVisible = true;
            }
        }

        private void SendB_Clicked(object sender, EventArgs e)
        {

        }

        private void SendR_Clicked(object sender, EventArgs e)
        {

        }
    }
}