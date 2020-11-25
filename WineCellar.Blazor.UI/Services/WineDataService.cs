using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WineCellar.Blazor.Shared.Models;

namespace WineCellar.Blazor.UI.Services
{
    public class WineDataService : IWineDataService
    {
        private readonly HttpClient _httpClient;

        public WineDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IEnumerable<Wine>> GetAllWinesAsync()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<Wine>>
                (await _httpClient.GetStreamAsync($"api/wines"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
    }
}
