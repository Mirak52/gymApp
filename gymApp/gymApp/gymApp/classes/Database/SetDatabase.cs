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
            database.CreateTableAsync<Set>().Wait();
        }
        public Task<List<Set>> Select()
        {
            return database.QueryAsync<Set>("SELECT * FROM [Set] order by ID_set asc");
        }
        public Task<List<Set>> SelectByID(int ID)
        {
            return database.QueryAsync<Set>("SELECT * FROM [Set] WHERE ID_set = " + ID);
        }
        public Task<List<Set>> SelectSetsByTrainingUnit(int day)
        {
            return database.QueryAsync<Set>("select * FROM [Set] WHERE ID_day = '" + day + "'"+ "order by ID_set asc");
        }
        public Task<List<Set>> SelectLastID()
        {
            return database.QueryAsync<Set>("select ID_set FROM [Set] order by ID_set DESC LIMIT 1");
        }
        public Task<int> SaveItemAsync(Set item)
        {
                return database.InsertAsync(item);
        }
        public Task<int> SaveListAsync(List<Set> items)
        {
            return database.InsertAllAsync(items);
        }
        public Task<List<Set>> UpdateRepsAndExcercise(int PK, string Reps, int ID)
        {
            return database.QueryAsync<Set>("UPDATE [Set] SET ID_excercisePK = "+ PK + ", Reps = '" + Reps + "' WHERE ID_set = "+ ID);
        }
        public Task<List<Set>> UpdateRepsAndExcerciseAndWeight(int PK, string Reps,int Weight, int ID)
        {
            return database.QueryAsync<Set>("UPDATE [Set] SET ID_excercisePK = " + PK + ", Reps = '" + Reps + "', Weight = " + Weight + " WHERE ID_set = " + ID);
        }

        public Task<int> DeleteItemAsync(Set item)
        {
            return database.DeleteAsync(item);
        }
    }
}
