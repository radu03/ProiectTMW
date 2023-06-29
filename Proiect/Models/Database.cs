using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Proiect.Models
{
    public class Database
    {
        const string name = "Jokes.db";

        SQLiteAsyncConnection connection;

        public async Task InitializeAsync()
        {
            if (connection is null)
            {
                var path = Path.Combine(FileSystem.AppDataDirectory, name); 
                
                var flags = SQLite.SQLiteOpenFlags.ReadWrite
                    | SQLite.SQLiteOpenFlags.Create
                    | SQLite.SQLiteOpenFlags.SharedCache;
                connection = new SQLiteAsyncConnection(path, flags);
                await connection.CreateTableAsync<Joke>();
            }
        }

        public async Task<IList<string>> GetTypesAsync()
        {
            await InitializeAsync();
            return await connection.QueryScalarsAsync<string>("select distinct [type] from [Joke]");
        }

        public async Task<IList<Joke>> GetJokesByTypesAsync(string type)
        {
            await InitializeAsync();
            return await connection.Table<Joke>()
               .Where(joke => joke.type == type)
               .ToListAsync();
        }

        public async Task<IList<string>> GetJokes()
        {
            await InitializeAsync();
            return await connection.QueryScalarsAsync<string>("select distinct [id] from [Joke]");

        }

        public async Task<Joke?> GetJokeById(int id)
        {
            await InitializeAsync();
            return await connection.Table<Joke>()
                .Where(joke => joke.id == id)
                .FirstAsync();
        }

        public async Task SaveJokeAsync(Joke joke)
        {
            await InitializeAsync();
           
            await connection.InsertAsync(joke);
                
            
        }

        public async Task RemoveJokeAsync(Joke joke)
        {
            await connection.DeleteAsync<Joke>(joke);
        }

    }
}
