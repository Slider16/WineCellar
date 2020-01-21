using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WineCellar.Net.MVC.Data.Entities;


namespace WineCellar.Net.MVC.Services
{
    public class WineAPIService : IWineService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public WineAPIService(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration ??
                throw new ArgumentNullException(nameof(configuration));

            _httpClient = httpClient ??
                throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<Wine> GetWineAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Wine>> GetWinesAsync()
        {
            var httpResponse = await _httpClient.GetAsync("api/wines").ConfigureAwait(false);
            httpResponse.EnsureSuccessStatusCode();

            //var pagingHeader = httpResponse.Headers.Where(h => h.Key == "X-Pagination").FirstOrDefault().Value.ToString();

            //StringValues paginationHeader = string.Empty;
            //IHeaderDictionary headers = httpResponse.Headers;
            //headers.TryGetValue("X-Pagination", out paginationHeader)

            //var pagination = JsonConvert.DeserializeObject(pagingHeader);

            var responseAsString = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

            var collection = JsonConvert.DeserializeObject<IEnumerable<Wine>>(responseAsString).AsQueryable();

            return collection;
        }
    }
}
