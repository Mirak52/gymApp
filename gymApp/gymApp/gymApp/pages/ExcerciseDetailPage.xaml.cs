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
    public partial class ExcerciseDetailPage : ContentPage
    {
        public int ID_excercise;
        public ExcerciseDetailPage(int id_excercise)
        {
            InitializeComponent();
            ID_excercise = id_excercise;
            ShowDetailedExcercise(id_excercise);
            var lastID = App.DatabaseExcercise.GetLastID().Result;
        }

        private void ShowDetailedExcercise(int id_excercise)
        {
            var Excercise = App.DatabaseExcercise.SelectDetailedExcercise(id_excercise).Result;
            NameExcercise.Text= Excercise[0].Name;
            TipsText.Text = Excercise[0].Tip;
            DescriptionText.Text = Excercise[0].Description;
        }

        private void GoToLeft_Clicked(object sender, EventArgs e)
        {
            if(ID_excercise-1 < 1)
            {
                var lastID = App.DatabaseExcercise.GetLastID().Result;
                ID_excercise = lastID[0].ID_excercise;
                ShowDetailedExcercise(ID_excercise);
            }
            else
            {
                ID_excercise--;
                ShowDetailedExcercise(ID_excercise);
            }
        }

        private void GoToRight_Clicked(object sender, EventArgs e)
        {
            var lastID = App.DatabaseExcercise.GetLastID().Result;
            if (ID_excercise + 1 > lastID[0].ID_excercise)
            {
                ID_excercise = 1;
                ShowDetailedExcercise(ID_excercise);
            }
            else
            {
                ID_excercise++;
                ShowDetailedExcercise(ID_excercise);
            }
        }
    }
}