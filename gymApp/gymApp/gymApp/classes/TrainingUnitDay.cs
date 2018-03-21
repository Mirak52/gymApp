using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gymApp.classes
{
    public class TrainingUnitDay
    {
        [PrimaryKey, AutoIncrement]
        public int ID_TrainDay { get; set; }
        public int ID_Day { get; set; }
        public int ID_trainingUnit { get; set; }
    }
}
