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
        [PrimaryKey, AutoIncrement]
        public int ID_set { get; set; }
        public int ID_excercisePK { get; set; }
        public int Reps { get; set; }
        public int Weight { get; set; }
    }
}
