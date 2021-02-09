using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WineCellar.Blazor.Shared.Models;

namespace WineCellar.Blazor.UI.Services
{
    public class VineyardDataService : IVineyardDataService
    {
        private readonly HttpClient _httpClient;

        public VineyardDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Vineyard>> GetVineyardsAsync()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<Vineyard>>
                (await _httpClient.GetStreamAsync($"api/vineyard"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<Vineyard> AddVineyardAsync(Vineyard vineyard)
        {
            var vineyardJson = new StringContent(JsonSerializer.Serialize(vineyard), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/vineyard", vineyardJson).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<Vineyard>(await response.Content.ReadAsStreamAsync());
            }

            return null;
        }

        public async Task<Vineyard> GetVineyardByIdAsync(string vineyardId)
        {
            var response = await _httpClient.GetAsync($"api/vineyard/{vineyardId}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<Vineyard>(await response.Content.ReadAsStreamAsync());
            }

            return null;
        }

        public async Task DeleteVineyard(string vineyardId)
        {
            var response = await _httpClient.DeleteAsync($"api/vineyard/{vineyardId}").ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error deleting vineyard with id: {vineyardId}.");
        }

        public async Task UpdateVineyardAsync(Vineyard vineyard)
        {
            var vineyardJson = new StringContent(JsonSerializer.Serialize(vineyard), Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/vineyard/{vineyard.Id}", vineyardJson).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error updating wine with id: {vineyard.Id}");
        }

    }
}
