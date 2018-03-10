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
    public partial class StatsPage : ContentPage
    {
        public StatsPage()
        {
            InitializeComponent();
            SetPicker();
        }

        private void SetPicker()
        {
            var picker = new List<string>();
            picker.Add("Vyber zadávání");
            picker.Add("Sledované údaje");
            picker.Add("Maximálky");
            
            Picker.ItemsSource = picker;
            Picker.SelectedIndex = 0;
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(Picker.SelectedIndex == 1)
            {
                Stats.IsVisible = true;
                BodyStats.IsVisible = true;
                Records.IsVisible = false;
            }
            else if(Picker.SelectedIndex == 2)
            {
                Stats.IsVisible = true;
                BodyStats.IsVisible = false;
                Records.IsVisible = true;
            }
        }

        private void SendB_Clicked(object sender, EventArgs e)
        {
            SaveBodyStats();
        }

        private void SaveBodyStats()
        {
            if(string.IsNullOrEmpty(HeightE.Text) && string.IsNullOrEmpty(WeightE.Text) && string.IsNullOrEmpty(WaistE.Text) && string.IsNullOrEmpty(ThighE.Text) && string.IsNullOrEmpty(BicepsE.Text))
            {
                ErrorL.Text = "Nevložil si data";
            }
            else
            {
                ErrorL.Text = "SLEDOVANÉ ÚDAJE";
                var bodyStats = App.DatabaseBodyStats.SelectLastBodyStats().Result;
                BodyStats bodyStat = new BodyStats();
                bodyStat.Weight = bodyStats[0].Weight;
                bodyStat.Height = bodyStats[0].Height;
                bodyStat.WaistCircumference = bodyStats[0].WaistCircumference;
                bodyStat.ThighCircumference = bodyStats[0].ThighCircumference;
                bodyStat.BicepsCircumference = bodyStats[0].BicepsCircumference;
                SaveBodyStatData(bodyStat);
                ErrorL.Text = "Úspěšně uloženo";
            }
        }
        private void SaveBodyStatData(BodyStats bodyStat)
        {
            if (App.IsNumber(WeightE.Text)) { bodyStat.Weight = App.setNumber(WeightE.Text); } else { ErrorL.Text = "Zadej pouze číslo"; }
            if (App.IsNumber(HeightE.Text)) { bodyStat.Height = App.setNumber(HeightE.Text); } else { ErrorL.Text = "Zadej pouze číslo"; }
            if (App.IsNumber(WaistE.Text)) { bodyStat.WaistCircumference = App.setNumber(WaistE.Text); } else { ErrorL.Text = "Zadej pouze číslo"; }
            if (App.IsNumber(ThighE.Text)) { bodyStat.ThighCircumference = App.setNumber(ThighE.Text); } else { ErrorL.Text = "Zadej pouze číslo"; }
            if (App.IsNumber(BicepsE.Text)) { bodyStat.BicepsCircumference = App.setNumber(BicepsE.Text); } else { ErrorL.Text = "Zadej pouze číslo"; }
            bodyStat.Date = DateE.Date.ToString();
            bodyStat.Date = bodyStat.Date.Remove(bodyStat.Date.Length - 12);
            App.DatabaseBodyStats.SaveItemAsync(bodyStat);
        }
        private void SendR_Clicked(object sender, EventArgs e)
        {
            SavePersonalRecord();
        }
        private void SavePersonalRecord()
        {
            if (string.IsNullOrEmpty(BenchpressE.Text) && string.IsNullOrEmpty(DeathliftE.Text) && string.IsNullOrEmpty(SquatE.Text))
            {
                ErrorL.Text = "Nevložil si data";
            }
            else
            {
                ErrorL.Text = "SLEDOVANÉ ÚDAJE";
                var highestData = App.DatabasePersonalRecord.SelectHighestRecord().Result;

                PersonalRecord personalRecord = new PersonalRecord();
                personalRecord.Benchpress = highestData[0].Benchpress;
                personalRecord.Deathlift = highestData[0].Deathlift;
                personalRecord.Squat = highestData[0].Squat;
                SavePersonalRecordData(personalRecord);
              
                ErrorL.Text = "Úspěšně uloženo";
            }
        }
        private void SavePersonalRecordData(PersonalRecord personalRecord)
        {
            //order = order.Remove(order.Length - 1);
            if (App.IsNumber(BenchpressE.Text)) { personalRecord.Benchpress = App.setNumber(BenchpressE.Text); } else { ErrorL.Text = "Zadej pouze číslo";}
            if (App.IsNumber(DeathliftE.Text)) { personalRecord.Deathlift = App.setNumber(DeathliftE.Text); } else { ErrorL.Text = "Zadej pouze číslo"; }
            if (App.IsNumber(SquatE.Text)) { personalRecord.Squat = App.setNumber(SquatE.Text); } else { ErrorL.Text = "Zadej pouze číslo"; }
            personalRecord.Date = DateR.Date.ToString();
            personalRecord.Date= personalRecord.Date.Remove(personalRecord.Date.Length - 12);
            App.DatabasePersonalRecord.SaveItemAsync(personalRecord);
        }
    }
}