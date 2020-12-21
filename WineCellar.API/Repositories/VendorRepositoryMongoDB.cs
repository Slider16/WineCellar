using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using WineCellar.API.Entities;
using WineCellar.API.Interfaces;

namespace WineCellar.API.Repositories
{
    public class VendorRepositoryMongoDB : IVendorRepository
    {
        private readonly IMongoCollection<Vendor> _vendors;

        public VendorRepositoryMongoDB(IWineCellarDatabaseSettings settings)            
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _vendors = database.GetCollection<Vendor>(settings.VendorsCollectionName);
        }

        public async Task<Vendor> CreateVendorAsync(Vendor vendor)
        {
            await _vendors.InsertOneAsync(vendor);
            return vendor;
        }

        public async Task DeleteVendorAsync(Vendor vendorIn) =>
            await _vendors.DeleteOneAsync(vendor => vendor.Id == vendorIn.Id).ConfigureAwait(false);

        public async Task DeleteVendorAsync(string id) =>
            await _vendors.DeleteOneAsync(vendor => vendor.Id == id).ConfigureAwait(false);

        public async Task<IEnumerable<Vendor>> GetVendorsAsync()
        {
            var vendors = await _vendors.FindAsync(vendor => true).ConfigureAwait(false);
            return await vendors.ToListAsync().ConfigureAwait(false);
        }

        public async Task<Vendor> GetVendorAsync(string id)
        {
            var task =  await _vendors.FindAsync(vendor => vendor.Id == id).ConfigureAwait(false);
            return await task.FirstOrDefaultAsync().ConfigureAwait(false);            
        }

        public async Task UpdateVendorAsync(string id, Vendor vendorIn) =>
            await _vendors.ReplaceOneAsync(vendor => vendor.Id == id, vendorIn).ConfigureAwait(false);
    }
}
