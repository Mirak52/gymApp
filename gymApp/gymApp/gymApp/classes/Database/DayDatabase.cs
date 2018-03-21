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
        public Task<List<Day>> SelectLastBodyStats()
        {
            return database.QueryAsync<Day>("select * FROM [Day] ORDER BY ID_bodyStats DESC LIMIT 1");
        }

        public Task<int> SaveItemAsync(Day item)
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

        public Task<int> DeleteItemAsync(Day item)
        {
            return database.DeleteAsync(item);
        }
    }
}

