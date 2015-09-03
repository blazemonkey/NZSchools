using NZSchools.Models;
using NZSchools.Services.FileReaderService;
using NZSchools.Services.JsonService;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;

namespace NZSchools.Services.SqlLiteService
{
    public class SqlLiteService : ISqlLiteService
    {
        private readonly SQLiteAsyncConnection _conn;
        private readonly IFileReaderService _fileReader;
        private readonly IJsonService _json;

        public SqlLiteService(IFileReaderService fileReader, IJsonService json)
        {
            _conn = new SQLiteAsyncConnection("nzschools.db");
            _fileReader = fileReader;
            _json = json;
        }

        public async Task InitDb()
        {
            var createTasks = new Task[]
            {
                _conn.CreateTableAsync<Directory>(),
                _conn.CreateTableAsync<Region>()
            };

            Task.WaitAll(createTasks);
            await InsertDataAsync();
        }

        private async Task InsertDataAsync()
        {
            await InsertDirectories();
            await InsertRegions();
        }        

        private async Task InsertDirectories()
        {
            if (await _conn.Table<Directory>().CountAsync() == 0)
            {
                var json = await _fileReader.ReadFile(Package.Current.InstalledLocation, "directories.json");
                var directories = _json.Deserialize<List<Directory>>(json);
                await _conn.InsertAllAsync(directories);
            }
        }

        private async Task InsertRegions()
        {
            if (await _conn.Table<Region>().CountAsync() == 0)
            {
                var regions = Region.CreateList();
                await _conn.InsertAllAsync(regions);
            }
        }

        public async Task<IEnumerable<Directory>> GetDirectories()
        {
            return await _conn.Table<Directory>().Where(x => x.RegionalCouncil != "Unknown").ToListAsync();
        }

        public async Task<IEnumerable<Directory>> GetFavourites()
        {
            return await _conn.Table<Directory>().Where(x => x.RegionalCouncil != "Unknown").Where(x => x.IsFavourites).ToListAsync();
        }

        public async Task<IEnumerable<Region>> GetRegions()
        {
            return await _conn.Table<Region>().OrderBy(x => x.Order).ToListAsync();
        }

        public async Task UpdateFavourites(Directory directory)
        {
            await _conn.UpdateAsync(directory);
        }

        public async Task ClearLocalDb()
        {
            // await _conn.DropTableAsync<Directory>();
            // await _conn.DropTableAsync<Region>();
            await InitDb();
        }
    }
}
