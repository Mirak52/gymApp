using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
namespace gymApp.classes
{
    public class ExcerciseDatabase
    {
        public SQLiteAsyncConnection database;

        public ExcerciseDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Excercise>().Wait();
        }
        public Task<List<Excercise>> QueryCustom()
        {
            return database.QueryAsync<Excercise>("select * FROM [Excercise]");
        }
        // Query
        public Task<List<Excercise>> GetItemsAsync()
        {
            return database.Table<Excercise>().ToListAsync();
        }

        // Query using SQL query string
        public Task<List<Excercise>> GetItemsNotDoneAsync()
        {
            return database.QueryAsync<Excercise>("SELECT * FROM [Excercise] WHERE [Done] = 0");
        }

        // Query using LINQ <3
        public Task<Excercise> GetItemAsync(int id)
        {
            return database.Table<Excercise>().Where(i => i.ID_excercise == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(Excercise item)
        {
            if (item.ID_excercise != 0)
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
