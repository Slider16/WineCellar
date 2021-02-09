using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WineCellar.API.Filters;
using WineCellar.API.Entities;
using WineCellar.API.Models;
using WineCellar.API.Repositories;

namespace WineCellar.API.Tests
{
    public class WineServiceFake : IWineRepository
    {

        private List<Wine> _wineList;

        private IMongoCollection<Wine> _wineCollection;

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
                new Wine() { Id = "1234", Name = "Jose Cabernet", Notes = "This is an excellent wine for shrimp and meats.", Year = 1995 },
                new Wine() { Id = "4567", Name = "Perfect Pinot", Notes = "Great with steaks and with steamed vegetables.", Year = 2001 },
                new Wine() { Id = "0910", Name = "Mind Blowing Merlot", Notes = "Just goes great with everything.", Year = 2008 },
                new Wine() { Id = "1112", Name = "Marvelous Malbec", Notes = "A wine for a quiet evening.", Year = 2011 },
                new Wine() { Id = "1314", Name = "Shining Shiraz", Notes = "This will get the party going.", Year = 2017} 
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

        public async Task DeleteWineAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Wine>> GetWinesAsync(WinesFilter filters)
        {
            var filterDefinition = filters.ToFilterDefinition();

            // Get a BsonDocumentSerializer needed by the Render function
            var wineSerializer = BsonSerializer.SerializerRegistry.GetSerializer<Wine>();
            var serializerRegistry = new BsonSerializerRegistry();

            try
            {
                var filterBsonDocument = filterDefinition.Render(wineSerializer, serializerRegistry);
                var year = filterBsonDocument?.Elements?.ToList().FirstOrDefault(e => e.Name == "year").Value?.AsInt32;
                var bin = filterBsonDocument?.Elements?.ToList().FirstOrDefault(e => e.Name == "bin").Value?.AsInt32;

                var vineyard = filterBsonDocument?.Elements?.ToList().FirstOrDefault(e => e.Name == "vineyard").Value?.AsString;

                if (!string.IsNullOrEmpty(vineyard))
                {
                    await GetWinesByVineyardAsync(vineyard).ConfigureAwait(false);
                }


                if (year > 0)
                {
                    _wineList = _wineList.Where(w => w.Year == year)?.ToList();
                }

                if (bin > 0)
                {
                    _wineList = _wineList.Where(w => w.Bin == bin)?.ToList();
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message},   InnerExec Message: {ex.InnerException?.Message}");
            }
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


        public async Task<IEnumerable<Wine>> GetWinesByVineyardAsync(string vineyard)
        {
            return await Task.FromResult(_wineList.Where(wine => wine.Vineyard.StartsWith(vineyard)).ToList()).ConfigureAwait(false);
        }

        Task<IEnumerable<WinePriceAdjustDto>> IWineRepository.GetWinesForPriceAdjust()
        {
            throw new NotImplementedException();
        }

        // Not needed since implementing WinesFilter class
        //public async Task<IEnumerable<Wine>> GetWinesByYearAsync(int year)
        //{
        //    return await Task.FromResult(_wineList.Where(wine => wine.Year == year).ToList()).ConfigureAwait(false);
        //}
    }
}
