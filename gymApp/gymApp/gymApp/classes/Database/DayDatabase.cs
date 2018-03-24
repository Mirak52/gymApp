using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gymApp.classes.Database
{
    public class DayDatabase
    {
        public SQLiteAsyncConnection database;

        public DayDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Day>().Wait();
        }
        public Task<List<Day>> SelectLastID()
        {
            return database.QueryAsync<Day>("select ID_Day FROM [Day] order by ID_day DESC LIMIT 1");
        }
        public Task<List<Day>> Select()
        {
            return database.QueryAsync<Day>("SELECT * FROM [Day]");
        }
        public Task<List<Day>> SelectSetsByTrainingUnit(int trainingUnit)
        {
            return database.QueryAsync<Day>("select * FROM [Day] WHERE ID_TrainingUnit = '" + trainingUnit + "'" + "order by ID_Day asc");
        }
        public Task<int> SaveItemAsync(Day item)
        {
                return database.InsertAsync(item);
        }

        public Task<int> DeleteItemAsync(Day item)
        {
            return database.DeleteAsync(item);
        }
    }
}

