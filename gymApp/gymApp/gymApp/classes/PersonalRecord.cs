using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
namespace gymApp.classes
{
    public class PersonalRecord
    {
        [PrimaryKey, AutoIncrement]
        public int ID_personalRecord { get; set; }
        public int Benchpress { get; set; }
        public int Deathlift { get; set; }
        public int Squat { get; set; }
        public string Date { get; set; }
    }
}
