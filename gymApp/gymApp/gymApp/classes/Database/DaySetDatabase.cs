using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gymApp.classes.Database
{
    public class DaySetDatabase
    {
        public SQLiteAsyncConnection database;

        public DaySetDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Day>().Wait();
        }
        public Task<int> SaveItemAsync(DaySet item)
        {
            if (item.ID != 0)
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
