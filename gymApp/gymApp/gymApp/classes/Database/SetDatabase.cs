using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gymApp.classes.Database
{
    public class SetDatabase
    {
        public SQLiteAsyncConnection database;

        public SetDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<BodyStats>().Wait();
        }
        public Task<List<Set>> SelectLastBodyStats()
        {
            return database.QueryAsync<Set>("select * FROM [Set] ORDER BY ID_bodyStats DESC LIMIT 1");
        }

        public Task<int> SaveItemAsync(Set item)
        {
            if (item.ID_set != 0)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                return database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(Set item)
        {
            return database.DeleteAsync(item);
        }
    }
}
