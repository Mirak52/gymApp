using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gymApp.classes
{
    public class PersonalRecordDatabase
    {
        public SQLiteAsyncConnection database;

        public PersonalRecordDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<PersonalRecord>().Wait();
        }
        public Task<List<PersonalRecord>> SelectHighestRecord()
        {
            return database.QueryAsync<PersonalRecord>("select MAX(Benchpress) Benchpress,MAX(Deathlift) Deathlift,MAX(Squat) Squat FROM [PersonalRecord]");
        }
        public Task<List<PersonalRecord>> SelectAll()
        {
            return database.QueryAsync<PersonalRecord>("select * FROM [PersonalRecord]");
        }

        public Task<int> SaveItemAsync(PersonalRecord item)
        {
            if (item.ID_personalRecord != 0)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                return database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(PersonalRecord item)
        {
            return database.DeleteAsync(item);
        }
    }
}
