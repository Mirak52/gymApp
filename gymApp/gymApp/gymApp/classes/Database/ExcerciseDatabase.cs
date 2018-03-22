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
        public Task<List<Excercise>> SelectExcercise()
        {
            return database.QueryAsync<Excercise>("select * FROM [Excercise]");
        }
        public Task<List<Excercise>> SelectExcerciseByParameter(string parameter)
        {
            return database.QueryAsync<Excercise>("select * FROM [Excercise] where Name LIKE '%" + parameter + "%'");
        }
        public Task<List<Excercise>> SelectByName(string parameter)
        {
            return database.QueryAsync<Excercise>("select ID_excercise FROM [Excercise] where Name ='" + parameter + "'");
        }

        // Query SELECT  * FROM gym_excercise ORDER BY ID_excercise DESC LIMIT 1
        public Task<List<Excercise>> SelectDetailedExcercise(int ID)
        {
            return database.QueryAsync<Excercise>("SELECT * FROM [Excercise] WHERE [ID_excercise] = " + ID);
        }
        public Task<List<Excercise>> GetLastID()
        {
            return database.QueryAsync<Excercise>("SELECT ID_excercise FROM [Excercise] ORDER BY [ID_excercise] DESC LIMIT 1");
        }
        public Task<List<Excercise>> SelectExcerciseByRegion(int region)
        {
            return database.QueryAsync<Excercise>("SELECT * FROM [Excercise] WHERE [Region] = "+region);
        }
        public Task<List<Excercise>> SelectExcerciseByRegionWithoutMainExcercise(int region,string name)
        {
            return database.QueryAsync<Excercise>("SELECT * FROM [Excercise] WHERE [Region] = " + region + " AND Name NOT LIKE '%" + name + "%'");
        }
        public Task<List<Excercise>> SelectMainExcerciseByRegionAndName(int region, string name)
        {
            return database.QueryAsync<Excercise>("SELECT * FROM [Excercise] WHERE [Region] = " + region + " AND Name LIKE '%" + name + "%'");
        }

        public Task<List<Excercise>> SelectBicepsAndShoulders()
        {
            return database.QueryAsync<Excercise>("SELECT * FROM [Excercise] WHERE [Region] = '" + 3 + "'OR [Region] = " + 2);
        }
        public Task<List<Excercise>> SelectTricepsAndForearm()
        {
            return database.QueryAsync<Excercise>("SELECT * FROM [Excercise] WHERE [Region] = '" + 4 + "'OR [Region] = " + 5);
        }
        public Task<List<Excercise>> SelectBellyAndCalf()
        {
            return database.QueryAsync<Excercise>("SELECT * FROM [Excercise] WHERE [Region] = '" + 9 + "'OR [Region] = " + 8);
        }
        public Task<List<Excercise>> SelectCompensationExcercises()
        {
            return database.QueryAsync<Excercise>("SELECT * FROM [Excercise] WHERE [Specification] = " + 3);
        }
        public Task<List<Excercise>> SelectByRegionAndSpecification(int region,int specification)
        {
            return database.QueryAsync<Excercise>("SELECT * FROM [Excercise] WHERE [Region] = '" + region+ "'AND [Specification] = " + specification);
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
