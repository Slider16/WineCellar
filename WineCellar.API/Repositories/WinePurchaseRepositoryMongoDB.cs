using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WineCellar.API.Entities;
using WineCellar.API.Interfaces;

namespace WineCellar.API.Repositories
{
    public class WinePurchaseRepositoryMongoDB : IWinePurchaseRepository
    {
        private readonly IMongoCollection<Wine> _wines;

        public WinePurchaseRepositoryMongoDB(IWineCellarDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _wines = database.GetCollection<Wine>(settings.WinesCollectionName);
        }
        public async Task<WinePurchase> CreateWinePurchaseAsync(WinePurchase winePurchase)
        {
            //var wineToUpdate = await _wines.FindAsync(wine => wine.Id == wineId).ConfigureAwait(false);
            //_wines.FindOneAndUpdateAsync(wine => wine.Id == wineId, new Document)
            throw new NotImplementedException();
            
        }

        public Task DeleteWinePurchaseAsync(WinePurchase winePurchaseIn)
        {
            throw new NotImplementedException();
        }

        public Task DeleteWinePurchaseAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<WinePurchase> GetWinePurchaseAsync(string wineId, string winePurchaseId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<WinePurchase>> GetWinePurchasesAsync(string wineId)
        {
            //_wines.Aggregate<Wine>()
            //     .Lookup<Wine, WinePurchase, WinePurchase>(_wines,
            //     wine => wine.Id,
            //     winePurchase => winePurchase.Id,
            //     wineP => wineP.PurchaseDate);

            throw new NotImplementedException();            
           
        }

        public Task UpdateWinePurchaseAsync(string id, WinePurchase winePurchaseIn)
        {
            throw new NotImplementedException();
        }
    }
}
