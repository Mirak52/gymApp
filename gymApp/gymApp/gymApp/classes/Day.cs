﻿using SQLite;
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
        public int EstimatedTime { get; set; }
        public string MainExcercise { get; set; }
        public int State { get; set; }
    }
}