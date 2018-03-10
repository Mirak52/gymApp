using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gymApp.classes
{
    public class ExcerciseRegionDatabase
    {
        public SQLiteAsyncConnection database;

        public ExcerciseRegionDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<ExcerciseRegion>().Wait();
        }
        public Task<List<ExcerciseRegion>> SelectRegions()
        {
            return database.QueryAsync<ExcerciseRegion>("select * FROM [ExcerciseRegion]");
        }
        // Query
        public Task<List<ExcerciseRegion>> GetItemsAsync()
        {
            return database.Table<ExcerciseRegion>().ToListAsync();
        }

        // Query using SQL query string
        public Task<List<ExcerciseRegion>> GetItemsNotDoneAsync()
        {
            return database.QueryAsync<ExcerciseRegion>("SELECT * FROM [ExcerciseRegion] WHERE [Done] = 0");
        }

        // Query using LINQ <3
        public Task<ExcerciseRegion> GetItemAsync(int id)
        {
            return database.Table<ExcerciseRegion>().Where(i => i.ID_region == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(ExcerciseRegion item)
        {
            if (item.ID_region != 0)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                return database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(ExcerciseRegion item)
        {
            return database.DeleteAsync(item);
        }
    }
}
