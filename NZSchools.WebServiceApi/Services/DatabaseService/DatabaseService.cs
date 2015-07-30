using NZSchools.WebServiceApi.DAL;
using NZSchools.WebServiceApi.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace NZSchools.WebServiceApi.Services.DatabaseService
{
    public class DatabaseService : IDatabaseService
    {
        private readonly SchoolContext _db;

        public DatabaseService()
        {
            _db = new SchoolContext();
        }

        public async Task<IEnumerable<Directory>> GetDirectories()
        {
            return await _db.Directories.ToListAsync();
        }

        public async Task<Directory> GetDirectoryById(int id)
        {
            return await _db.Directories.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> UpdateDirectory(Directory directory)
        {
            var find = await _db.Directories.FindAsync(directory.Id);
            if (find == null)
                return false;

            find.Name = directory.Name;
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddDirectory(Directory directory)
        {
            var find = await _db.Directories.FindAsync(directory.Id);
            if (find != null)
                return false;

            _db.Directories.Add(directory);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteDirectory(int id)
        {
            var find = await _db.Directories.FindAsync(id);
            if (find == null)
                return false;

            _db.Directories.Remove(find);
            await _db.SaveChangesAsync();

            return true;
        }
    }
}