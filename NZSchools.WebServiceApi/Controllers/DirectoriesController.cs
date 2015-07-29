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
            return null;
        }

        public async Task<Directory> Get(int id)
        {
            return null;
        }

        [HttpPut]
        public async Task<bool> Update(Directory directory)
        {
            if (directory == null)
                throw new ArgumentNullException("directory");

            return false;
        }

        [HttpPost]
        public async Task<bool> Add(Directory directory)
        {
            if (directory == null)
                throw new ArgumentNullException("directory");

            return false;
        }
    }
}
