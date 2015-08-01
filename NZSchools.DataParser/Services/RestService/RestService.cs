using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZSchools.DataParser.Services.RestService
{
    public class RestService : IRestService
    {
        public async Task<IEnumerable<T>> GetApi<T>(string apiUrl, string resourceUrl)
        {
            var client = new RestClient(apiUrl);
            var request = new RestRequest(resourceUrl, Method.GET);

            var response = await client.ExecuteTaskAsync<List<T>>(request);
            if (!string.IsNullOrEmpty(response.ErrorMessage))
            {
                Logger.Error(response.ErrorMessage);
                return null;
            }

            return response.Data;
        }

        public async Task<IEnumerable<T>> GetApi<T>(string apiUrl, string resourceUrl, int id)
        {
            var client = new RestClient(apiUrl);
            var request = new RestRequest(resourceUrl + "/{id}", Method.GET);
            request.AddParameter("id", id);

            var response = await client.ExecuteTaskAsync<List<T>>(request);
            if (!string.IsNullOrEmpty(response.ErrorMessage))
            {
                Logger.Error(response.ErrorMessage);
                return null;
            }

            return response.Data;
        }

        public async Task<bool> PostApi(string apiUrl, string resourceUrl, string resource)
        {
            var client = new RestClient(apiUrl);
            var request = new RestRequest(resourceUrl, Method.POST);
            request.AddParameter("application/json; charset=utf-8", resource, ParameterType.RequestBody);
            request.RequestFormat = DataFormat.Json;

            var response = await client.ExecutePostTaskAsync(request);
            if (!string.IsNullOrEmpty(response.ErrorMessage))
            {
                Logger.Error(response.ErrorMessage);
                return false;
            }

            return true;
        }

        public async Task<bool> PutApi(string apiUrl, string resourceUrl, string resource)
        {
            var client = new RestClient(apiUrl);
            var request = new RestRequest(resourceUrl, Method.PUT);
            request.AddParameter("application/json; charset=utf-8", resource, ParameterType.RequestBody);
            request.RequestFormat = DataFormat.Json;

            var response = await client.ExecuteTaskAsync(request);
            if (!string.IsNullOrEmpty(response.ErrorMessage))
            {
                Logger.Error(response.ErrorMessage);
                return false;
            }

            return true;
        }
    }
}
