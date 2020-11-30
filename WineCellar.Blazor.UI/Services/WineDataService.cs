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

        public async Task<Wine> AddWineAsync(Wine wine)
        {
            var wineJson = new StringContent(JsonSerializer.Serialize(wine), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/wines", wineJson).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<Wine>(await response.Content.ReadAsStreamAsync());
            }

            return null;            
        }

        public async Task<Wine> GetWineByIdAsync(string wineId)
        {
            var response = await _httpClient.GetAsync($"api/wines/{wineId}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<Wine>(await response.Content.ReadAsStreamAsync());
            }

            return null;
        }

        public async Task DeleteWine(string wineId)
        {
            var response = await _httpClient.DeleteAsync($"api/wines/{wineId}").ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error deleting wine with id: {wineId}.");
        }
    }
}
