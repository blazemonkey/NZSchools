using NZSchools.WebServiceApi.Models;
using NZSchools.WebServiceApi.Services.DatabaseService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace NZSchools.WebServiceApi.Controllers
{
    public class DirectoriesController : ApiController
    {
        private readonly IDatabaseService _db;

        public DirectoriesController(IDatabaseService db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Directory>> Get()
        {
            return await _db.GetDirectories();
        }

        public async Task<Directory> Get(int id)
        {
            return await _db.GetDirectoryById(id);
        }

        [HttpPut]
        public async Task<bool> Update(Directory directory)
        {
            if (directory == null)
                throw new ArgumentNullException("directory");

            var result = await _db.UpdateDirectory(directory);
            return result;
        }

        [HttpPost]
        public async Task<bool> Add(Directory directory)
        {
            if (directory == null)
                throw new ArgumentNullException("directory");

            var result = await _db.AddDirectory(directory);
            return result;
        }

        [HttpDelete]
        public async Task<bool> Delete(int id)
        {
            var result = await _db.DeleteDirectory(id);
            return result;
        }
    }
}
