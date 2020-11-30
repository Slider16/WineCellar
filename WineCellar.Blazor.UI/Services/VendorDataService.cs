using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WineCellar.Blazor.Shared.Models;

namespace WineCellar.Blazor.UI.Services
{
    public class VendorDataService : IVendorDataService
    {

        private readonly HttpClient _httpClient;

        public VendorDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Vendor>> GetAllVendorsAsync()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<Vendor>>
                (await _httpClient.GetStreamAsync($"api/vendors"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
        
        public async Task<Vendor> AddVendorAsync(Vendor vendor)
        {
            var vendorJson = new StringContent(JsonSerializer.Serialize(vendor), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/vendors", vendorJson).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<Vendor>(await response.Content.ReadAsStreamAsync());
            }

            return null;
        }
        
        public async Task<Vendor> GetVendorByIdAsync(string vendorId)
        {
            var response = await _httpClient.GetAsync($"api/vendors/{vendorId}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<Vendor>(await response.Content.ReadAsStreamAsync());
            }

            return null;
        }

        public async Task DeleteVendor(string vendorId)
        {
            var response = await _httpClient.DeleteAsync($"api/vendors/{vendorId}").ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error deleting vendor with id: {vendorId}.");
        }

        public async Task UpdateVendorAsync(Vendor vendor)
        {
            var vendorJson = new StringContent(JsonSerializer.Serialize(vendor), Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/vendors/{vendor.Id}", vendorJson).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error updating wine with id: {vendor.Id}");
        }
    }
}
