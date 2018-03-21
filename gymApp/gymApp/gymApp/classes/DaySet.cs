using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gymApp.classes
{
    public class DaySet
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int ID_Day { get; set; }
        public string ID_set{ get; set; }
    }
}
