using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WineCellar.Net.API.Entities;

namespace WineCellar.Net.API.Repositories
{
    public class WinePurchaseRepositoryMongoDB : IWinePurchaseRepository
    {
        //private readonly IMongoCollection<Wine> _wines;

        //public WinePurchaseServiceMongoDB(IWineCellarDatabaseSettings settings)
        //{
        //    var client = new MongoClient(settings.ConnectionString);
        //    var database = client.GetDatabase(settings.DatabaseName);
            
        //    _wines = database.GetCollection<Wine>(settings.WinesCollectionName);
        //}
        public Task<WinePurchase> CreateWinePurchaseAsync(WinePurchase winePurchase)
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

        public Task<IEnumerable<WinePurchase>> GetWinePurchasesAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateWinePurchaseAsync(string id, WinePurchase winePurchaseIn)
        {
            throw new NotImplementedException();
        }
    }
}
