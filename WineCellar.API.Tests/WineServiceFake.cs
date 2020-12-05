using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WineCellar.Net.API.Entities;
using WineCellar.Net.API.Repositories;

namespace WineCellar.Net.API.Tests
{
    public class WineServiceFake : IWineRepository
    {

        private List<Wine> _wineList;

        public WineServiceFake()
        {
            var shiningShirazPurchases = new List<WinePurchase>()
            {
                new WinePurchase() { VendorId = "5e02d372f9f3d53f306b5f65", PurchaseDate = new DateTime(2019, 10, 20), PurchasePrice = 21 },
                new WinePurchase() { VendorId = "5e02d372f9f3d53f306b5f65", PurchaseDate = new DateTime(2019, 11, 12), PurchasePrice = 23 },
                new WinePurchase() { VendorId = "5e02d372f9f3d53f306b5f65", PurchaseDate = new DateTime(2019, 8, 14), PurchasePrice = 18 },
                new WinePurchase() { VendorId = "5e02d372f9f3d53f306b5f65", PurchaseDate = new DateTime(2019, 9, 16), PurchasePrice = 19 }
            };
            
            
            _wineList = new List<Wine>()
            {
                new Wine() { Id = "1234", Name = "Jose Cabernet", Vineyard = "Grapes Galore Vineyard", Location = "Spain", Notes = "This is an excellent wine for shrimp and meats.", Year = 1995 },
                new Wine() { Id = "4567", Name = "Perfect Pinot", Vineyard = "Italianio Gardens", Location = "Italy", Notes = "Great with steaks and with steamed vegetables.", Year = 2001 },
                new Wine() { Id = "0910", Name = "Mind Blowing Merlot", Vineyard = "Hills of Southern California Vineyards", Location = "California", Notes = "Just goes great with everything.", Year = 2008 },
                new Wine() { Id = "1112", Name = "Marvelous Malbec", Vineyard = "Aussie Audacity Vineyards", Location = "Australia", Notes = "A wine for a quiet evening.", Year = 2011 },
                new Wine() { Id = "1314", Name = "Shining Shiraz", Vineyard = "Aussie Audacity Vineyards", Location = "Australia", Notes = "This will get the party going.", Year = 2017, WinePurchases = shiningShirazPurchases } 
            };
        }

        public WineServiceFake(List<Wine> wineList)
        {
            _wineList = wineList;
        }

        public async Task<Wine> CreateWineAsync(Wine wine)
        {
            wine.Id = "5e097a48de9973754407f7b2";
            return await Task.FromResult(wine);
        }

        public async Task DeleteWineAsync(Wine wineIn)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteWineAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Wine>> GetWinesAsync()
        {
            return await Task.FromResult(_wineList).ConfigureAwait(false);
        }

        public async Task<Wine> GetWineAsync(string id)
        {
            return await Task.FromResult(_wineList.Where(wine => wine.Id == id).FirstOrDefault()).ConfigureAwait(false);
        }

        public async Task UpdateWineAsync(string id, Wine wineIn)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> WineExists(string wineId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Wine>> GetWinesByVineyardAsync(string vineyard)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Wine>> GetWinesByYearAsync(int year)
        {
            throw new NotImplementedException();
        }
    }
}
