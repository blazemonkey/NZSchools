using NZSchools.WebServiceApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZSchools.WebServiceApi.Services.DatabaseService
{
    public interface IDatabaseService
    {
        Task<IEnumerable<Directory>> GetDirectories();
        Task<Directory> GetDirectoryById(int id);
        Task<bool> UpdateDirectory(Directory directory);
        Task<bool> AddDirectory(Directory directory);
        Task<bool> DeleteDirectory(int id);
    }
}
