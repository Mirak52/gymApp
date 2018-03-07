using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gymApp.classes
{
    public class BodyStatsDatabase
    {
        public SQLiteAsyncConnection database;

        public BodyStatsDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<BodyStats>().Wait();
        }
        public Task<List<BodyStats>> SelectLastBodyStats()
        {
            return database.QueryAsync<BodyStats>("select * FROM [BodyStats] ORDER BY ID_excercise DESC LIMIT 1");
        }
      
        public Task<int> SaveItemAsync(BodyStats item)
        {
            if (item.ID_bodyStats != 0)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                return database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(Excercise item)
        {
            return database.DeleteAsync(item);
        }
    }
}
