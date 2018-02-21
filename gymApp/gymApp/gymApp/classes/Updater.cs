using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gymApp.classes
{
    public class Updater
    {
        [PrimaryKey, AutoIncrement]   
        public int ID_updater { get; set; }
        public string lastUpdate { get; set; }
    }
}
