﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gymApp.classes.Database
{
    public class TrainingUnitDatabase
    {
        public SQLiteAsyncConnection database;

        public TrainingUnitDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<TrainingUnit>().Wait();
        }
        public Task<int> SaveItemAsync(TrainingUnit item)
        {
            if (item.ID_TrainingUnit != 0)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                return database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(TrainingUnit item)
        {
            return database.DeleteAsync(item);
        }
    }
}