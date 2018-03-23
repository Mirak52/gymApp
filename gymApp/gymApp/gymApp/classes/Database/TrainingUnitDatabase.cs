using SQLite;
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
