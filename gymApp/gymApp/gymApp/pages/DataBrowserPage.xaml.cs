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
    public partial class DataBrowserPage : TabbedPage
    {
        public DataBrowserPage()
        {
            InitializeComponent();
            ShowListViews();
        }

        private void ShowListViews()
        {
            List<BodyStats> BodyStatsList = new List<BodyStats>();
            var BodyStats = App.DatabaseBodyStats.SelectSortedByDate().Result;
            foreach (var BodyStat in BodyStats)
            {
                BodyStatsList.Add(new BodyStats { Combination = "Váha: "+BodyStat.Weight + "Kg Obvod pasu: "+ BodyStat.WaistCircumference + "CM Obvod stehna: "+ BodyStat.ThighCircumference + "CM Obvod bicepsu: " + BodyStat.BicepsCircumference+"CM", Date = BodyStat.Date});
            }
            BodyStatsLV.ItemsSource = BodyStatsList;


            List<PersonalRecord> PersonalRecordList = new List<PersonalRecord>();
            var personalRecords = App.DatabasePersonalRecord.SelectSortedByDate().Result;
            foreach (var personalRecord in personalRecords)
            {
                PersonalRecordList.Add(new PersonalRecord { Combination = "Benčpres: "+ personalRecord.Benchpress+ "Kg Mrtvý tah: "+ personalRecord.Deathlift+ "Kg Dřep: " + personalRecord.Deathlift+ "Kg", Date = personalRecord.Date });
            }
            PersonalRecordsLV.ItemsSource = PersonalRecordList;
        }
    }
}