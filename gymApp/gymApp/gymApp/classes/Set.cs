using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gymApp.classes
{
    public class Set
    {
        public int ID_set { get; set; }
        public int ID_excercisePK { get; set; }
        public string ExcerciseName { get; set; }
        public string Reps { get; set; }
        public int Weight { get; set; }
        public int ID_day { get; set; }
    }
}
