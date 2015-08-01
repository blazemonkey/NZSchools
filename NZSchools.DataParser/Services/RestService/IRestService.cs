using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZSchools.DataParser.Services.RestService
{
    public interface IRestService
    {
        Task<IEnumerable<T>> GetApi<T>(string apiUrl, string resourceUrl);
        Task<IEnumerable<T>> GetApi<T>(string apiUrl, string resourceUrl, int id);
        Task<bool> PostApi(string apiUrl, string resourceUrl, string resource);
        Task<bool> PutApi(string apiUrl, string resourceUrl, string resource);
    }
}
