using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gymApp.classes
{
    public class Excercise
    { 
        [PrimaryKey, AutoIncrement]
        public int ID_excercise { get; set; }
        public string Name{ get; set; }
        public string Region { get; set; }
        public string Description { get; set; }
        public string Tip { get; set; }
    }
}

