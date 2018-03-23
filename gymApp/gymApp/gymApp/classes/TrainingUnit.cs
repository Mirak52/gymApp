using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gymApp.classes
{
    public class TrainingUnit
    {
        [PrimaryKey, AutoIncrement]
        public int ID_TrainingUnit { get; set; }
        public string Title { get; set; }
        public string CreatedDate { get; set; }
        public int state { get; set; }
    }
}
