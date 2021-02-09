using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WineCellar.API.Filters;
using WineCellar.API.Helpers;
using WineCellar.API.Entities;
using WineCellar.API.Interfaces;
using WineCellar.API.Models;

namespace WineCellar.API.Repositories
{
    public class WineRepositoryMongoDB : IWineRepository
    {        
        private readonly IMongoCollection<Wine> _wines;
        
        public WineRepositoryMongoDB(IWineCellarDatabaseSettings settings)
        {

            var connectionString = settings.ConnectionString;
            var mongoClientSettings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));

            var commandStartedLogger = new LoggerFactory().CreateLogger<CommandStartedEvent>();
            mongoClientSettings.ClusterConfigurator = builder => builder.Subscribe(new MongoEventsLogger(commandStartedLogger));

            
            var client = new MongoClient(mongoClientSettings);
            var database = client.GetDatabase(settings.DatabaseName);

            _wines = database.GetCollection<Wine>(settings.WinesCollectionName);
        }

        //   Implemented "Filters Class" that builds filters and gives options of combining filter options Bin and Year.
        //        See "Using the MongoDB C# v2 Driver" on Pluralsight by Wes Higbee.  Module 2 Combining Filter Conditions
        public async Task<IEnumerable<Wine>> GetWinesAsync(WinesFilter filters)
        {
            var filterDefinition = filters.ToFilterDefinition();
            var sortDefinition = filters.ToSortDefinition();

            var findOptions = new FindOptions<Wine, Wine>() 
            { 
                Sort = sortDefinition                 
            };

            var wines = await _wines.FindAsync(filterDefinition, findOptions).ConfigureAwait(false);
            return await wines.ToListAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<Wine>> GetWinesByVineyardAsync(string vineyard)
        {
            var task = await _wines.FindAsync(Builders<Wine>.Filter.Where(f => f.Vineyard.StartsWith(vineyard)));
            return await task.ToListAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Uses MongoDb's Projection to pull only needed fields from the database for price adjusting
        /// </summary>
        /// <returns>Returns an IEnumerable of WinePriceAdjustDto</returns>
        public async Task<IEnumerable<WinePriceAdjustDto>> GetWinesForPriceAdjust()
        {
            return await _wines.Find(wine => true)
                               .Project(w => new WinePriceAdjustDto
                               {
                                   Id = w.Id,
                                   Name = w.Name,
                                   SellPrice = w.SellPrice,
                                   Vineyard = w.Vineyard,                                   
                               })
                               .ToListAsync();            
        }

        public async Task<Wine> GetWineAsync(string id)
        {            
            var task = await _wines.FindAsync(wine => wine.Id == id).ConfigureAwait(false);
            return await task.FirstOrDefaultAsync().ConfigureAwait(false);
        }
        public async Task<Wine> CreateWineAsync(Wine wine)
        {
            await _wines.InsertOneAsync(wine).ConfigureAwait(false);
            return wine;
        }

        public async Task UpdateWineAsync(string id, Wine wineIn) =>
            await _wines.ReplaceOneAsync(wine => wine.Id == id, wineIn).ConfigureAwait(false);

        public async Task DeleteWineAsync(string id) =>
            await _wines.DeleteOneAsync(wine => wine.Id == id).ConfigureAwait(false);

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
