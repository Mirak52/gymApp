using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gymApp.classes
{
    public class Day
    {
        [PrimaryKey, AutoIncrement]
        public int ID_Day{ get; set; }
        public int ID_TrainingUnit { get; set; }
        public string MainExcercise { get; set; }
        public string State { get; set; } = "0";
    }
}
