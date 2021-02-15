using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WineCellar.API.Entities;
using WineCellar.API.Interfaces;

namespace WineCellar.API.Repositories
{
    public class VineyardRepositoryMongoDB : IVineyardRepository
    {
        private readonly IMongoCollection<Vineyard> _vineyards;

        public VineyardRepositoryMongoDB(IWineCellarDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _vineyards = database.GetCollection<Vineyard>(settings.VineyardsCollectionName);
        }

        public async Task<Vineyard> CreateVineyardAsync(Vineyard vineyard)
        {
            await _vineyards.InsertOneAsync(vineyard);
            return vineyard;
        }

        public async Task DeleteVineyardAsync(string id) =>
            await _vineyards.DeleteOneAsync(vineyard => vineyard.Id == id).ConfigureAwait(false);

        public async Task<Vineyard> GetVineyardAsync(string id)
        {
            var task = await _vineyards.FindAsync(vineyard => vineyard.Id == id).ConfigureAwait(false);
            return await task.FirstOrDefaultAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<Vineyard>> GetVineyardsAsync()
        {
            var vineyards = await _vineyards.FindAsync(vineyard => true).ConfigureAwait(false);
            return await vineyards.ToListAsync().ConfigureAwait(false);
        }

        public async Task UpdateVineyardAsync(string id, Vineyard vineyardIn) =>
            await _vineyards.ReplaceOneAsync(vineyard => vineyard.Id == id, vineyardIn).ConfigureAwait(false);

        public async Task<bool> VineyardExists(string vineyardId)
        {
            if (string.IsNullOrEmpty(vineyardId))
            {
                throw new ArgumentNullException(nameof(vineyardId));
            }

            var task = await _vineyards.FindAsync(vineyard => vineyard.Id == vineyardId).ConfigureAwait(false);
            return (await task.FirstOrDefaultAsync().ConfigureAwait(false) != null);
        }
    }
}
