using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WineCellar.Net.API.Entities;
using WineCellar.Net.API.Interfaces;

namespace WineCellar.Net.API.Repositories
{
    public class WineRepositoryMongoDB : IWineRepository
    {
        private readonly IMongoCollection<Wine> _wines;
        
        public WineRepositoryMongoDB(IWineCellarDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _wines = database.GetCollection<Wine>(settings.WinesCollectionName);
        }

        public async Task<Wine> CreateWineAsync(Wine wine)
        {
            await _wines.InsertOneAsync(wine).ConfigureAwait(false);
            return wine;
        }

        public async Task DeleteWineAsync(Wine wineIn) =>        
            await _wines.DeleteOneAsync(wine => wine.Id == wineIn.Id).ConfigureAwait(false);

        public async Task DeleteWineAsync(string id) =>
            await _wines.DeleteOneAsync(wine => wine.Id == id).ConfigureAwait(false);

        public async Task<IEnumerable<Wine>> GetWinesAsync()
        {
            var wines = await _wines.FindAsync(wine => true).ConfigureAwait(false);
            return await wines.ToListAsync().ConfigureAwait(false);
        }

        public async Task<Wine> GetWineAsync(string id)
        {
            var task = await _wines.FindAsync(wine => wine.Id == id).ConfigureAwait(false);
            return await task.FirstOrDefaultAsync().ConfigureAwait(false);
        }

        public async Task UpdateWineAsync(string id, Wine wineIn) =>
            await _wines.ReplaceOneAsync(wine => wine.Id == id, wineIn).ConfigureAwait(false);

        public async Task<bool> WineExists(string wineId)
        {
            if (string.IsNullOrEmpty(wineId))
            {
                throw new ArgumentNullException(nameof(wineId));
            }

            var task = await _wines.FindAsync(wine => wine.Id == wineId).ConfigureAwait(false);
            return (await task.FirstOrDefaultAsync().ConfigureAwait(false) != null);
        }
    }
}
