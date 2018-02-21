using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
namespace gymApp.classes
{
    public class UpdaterDatabase
    {
        public SQLiteAsyncConnection database;

        public UpdaterDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Updater>().Wait();
        }

        public Task<int> SaveItemAsync(Updater item)
        {
            if (item.ID_updater != 0)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                return database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(Updater item)
        {
            return database.DeleteAsync(item);
        }
        public Task<List<Updater>> Select()
        {
            return database.QueryAsync<Updater>("SELECT * FROM [Updater]");
        }
        public Task<List<Updater>> update(DateTime date)
        {
            return database.QueryAsync<Updater>("UPDATE [Updater] SET lastUpdate= '" + date + "'");
        }

        public Task<List<Updater>> insertToday(string today)
        {
            return database.QueryAsync<Updater>("INSERT INTO [Updater] (lastUpdate) VALUES ('" + today + "')");
        }
    }
}
