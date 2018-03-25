using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
namespace gymApp.classes
{
    public class BodyStats
    {
        [PrimaryKey, AutoIncrement]
        public int ID_bodyStats { get; set; }
        public int Weight{ get; set; }
        public int Height{ get; set; }
        public int WaistCircumference{ get; set; }
        public int ThighCircumference { get; set; }
        public int BicepsCircumference { get; set; }
        public string Date { get; set; }
        public string Combination { get; set; }
    }
}
