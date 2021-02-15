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

        public async Task<IEnumerable<Wine>> GetWinesAsync()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<Wine>>
                (await _httpClient.GetStreamAsync($"api/wine"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<IEnumerable<Wine>> GetWinesForPriceAdjustAsync()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<Wine>>
                (await _httpClient.GetStreamAsync($"api/Wine/priceadjust"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
        

        public async Task<Wine> AddWineAsync(Wine wine)
        {
            var wineJson = new StringContent(JsonSerializer.Serialize(wine), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/wine", wineJson).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<Wine>(await response.Content.ReadAsStreamAsync());
            }

            return null;            
        }

        public async Task<Wine> GetWineByIdAsync(string wineId)
        {
            var response = await _httpClient.GetAsync($"api/wine/{wineId}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<Wine>(await response.Content.ReadAsStreamAsync());
            }

            return null;
        }

        public async Task DeleteWineAsync(string wineId)
        {
            var response = await _httpClient.DeleteAsync($"api/wine/{wineId}").ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error deleting wine with id: {wineId}.");
        }

        public async Task UpdateWineAsync(Wine wine)
        {
            var wineJson = new StringContent(JsonSerializer.Serialize(wine), Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PutAsync($"api/wine/{wine.Id}", wineJson).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error updating wine with id: {wine.Id}");
        }

        public Task GetVineyardsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
