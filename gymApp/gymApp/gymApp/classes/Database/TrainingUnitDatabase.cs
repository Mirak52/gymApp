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

        public Task<List<TrainingUnit>> SelectLastID()
        {
            return database.QueryAsync<TrainingUnit>("select ID_TrainingUnit FROM [TrainingUnit] order by ID_TrainingUnit DESC LIMIT 1");
        }
        public Task<List<TrainingUnit>> SelectLastIDWhitZeroState()
        {
            return database.QueryAsync<TrainingUnit>("select ID_TrainingUnit FROM [TrainingUnit] Where State = 0 order by ID_TrainingUnit DESC LIMIT 1");
        }
        public Task<List<TrainingUnit>> UpdateTrainingUnitState(int ID)
        {
            return database.QueryAsync<TrainingUnit>("UPDATE [TrainingUnit] SET State = 1 WHERE ID_TrainingUnit = " + ID);
        }
        public Task<List<TrainingUnit>> Select()
        {
            return database.QueryAsync<TrainingUnit>("SELECT * FROM [TrainingUnit]");
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
