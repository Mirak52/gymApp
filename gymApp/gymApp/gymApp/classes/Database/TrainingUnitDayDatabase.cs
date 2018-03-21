using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gymApp.classes.Database
{
    public class TrainingUnitDayDatabase
    {
        public SQLiteAsyncConnection database;

        public TrainingUnitDayDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<TrainingUnitDay>().Wait();
        }
        public Task<int> SaveItemAsync(TrainingUnitDay item)
        {
            if (item.ID_Day != 0)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                return database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(TrainingUnitDay item)
        {
            return database.DeleteAsync(item);
        }
    }
}
