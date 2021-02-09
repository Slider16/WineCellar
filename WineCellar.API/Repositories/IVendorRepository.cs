using System.Collections.Generic;
using System.Threading.Tasks;
using WineCellar.API.Entities;

namespace WineCellar.API.Repositories
{
    public interface IVendorRepository
    {
        Task<Vendor> CreateVendorAsync(Vendor vendor);
        Task DeleteVendorAsync(string id);
        Task<IEnumerable<Vendor>> GetVendorsAsync();
        Task<Vendor> GetVendorAsync(string id);
        Task UpdateVendorAsync(string id, Vendor vendorIn);
    }
}