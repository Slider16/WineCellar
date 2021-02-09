using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WineCellar.Blazor.Shared.Models;

namespace WineCellar.Blazor.UI.Services
{
    public class WinePurchaseDataService : IWinePurchaseDataService
    {
        private readonly HttpClient _httpClient;

        public WinePurchaseDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<IEnumerable<WinePurchase>> GetWinePurchasesByWineIdAsync(string wineId)
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<WinePurchase>>
                (await _httpClient.GetStreamAsync($"api/winepurchase/purchases/{wineId}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }


        public async Task<WinePurchase> AddWinePurchaseAsync(WinePurchase winePurchase)
        {
            var winePurchaseJson = new StringContent(JsonSerializer.Serialize(winePurchase), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/winepurchase", winePurchaseJson).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<WinePurchase>(await response.Content.ReadAsStreamAsync());
            }

            return null;
        }

        public async Task UpdateWinePurchaseAsync(WinePurchase winePurchase)
        {
            var winePurchaseJson = new StringContent(JsonSerializer.Serialize(winePurchase), Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/winepurchase/{winePurchase.Id}", winePurchaseJson).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error updating wine purchase with id: {winePurchase.Id}");
        }

        public async Task<WinePurchase> GetWinePurchaseByIdAsync(string winePurchaseId)
        {
            var response = await _httpClient.GetAsync($"api/winepurchase/{winePurchaseId}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<WinePurchase>(await response.Content.ReadAsStreamAsync());
            }

            return null;
        }

        public async Task DeleteWinePurchaseAsync(string winePurchaseId)
        {
            var response = await _httpClient.DeleteAsync($"api/winepurchase/{winePurchaseId}").ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error deleting wine purchase with id: {winePurchaseId}.");
        }

    }
}
