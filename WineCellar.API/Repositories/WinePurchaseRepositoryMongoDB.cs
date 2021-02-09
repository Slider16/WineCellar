using MongoDB.Bson;
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
        private readonly IMongoCollection<WinePurchase> _winepurchases;

        public WinePurchaseRepositoryMongoDB(IWineCellarDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _winepurchases = database.GetCollection<WinePurchase>(settings.WinePurchasesCollectionName);
        }
        public async Task<WinePurchase> CreateWinePurchaseAsync(WinePurchase winePurchase)
        {
            await _winepurchases.InsertOneAsync(winePurchase).ConfigureAwait(false);
            return winePurchase;           
        }
        public async Task UpdateWinePurchaseAsync(string id, WinePurchase winePurchaseIn) =>
            await _winepurchases.ReplaceOneAsync(wp => wp.Id == id, winePurchaseIn).ConfigureAwait(false);        

        public async Task DeleteWinePurchaseAsync(string winePurchaseId) =>
            await _winepurchases.DeleteOneAsync(winepurchase => winepurchase.Id == winePurchaseId).ConfigureAwait(false);

        public async Task DeleteWinePurchasesByWineIdAsync(string wineId) =>
            await _winepurchases.DeleteManyAsync(winepurchase => winepurchase.WineId == wineId).ConfigureAwait(false);
        

        public async Task<WinePurchase> GetWinePurchaseAsync(string winePurchaseId)
        {
            var task = await _winepurchases.FindAsync(winepurchase => winepurchase.Id == winePurchaseId).ConfigureAwait(false);
            return await task.FirstOrDefaultAsync().ConfigureAwait(false);
        }


        public async Task<IEnumerable<WinePurchase>> GetWinePurchasesByWineIdAsync(string wineId)
        {
            var filter = Builders<WinePurchase>.Filter.Eq("wineId", ObjectId.Parse(wineId));
            return (await _winepurchases.FindAsync(filter).ConfigureAwait(false)).ToList();
        }
    }
}
