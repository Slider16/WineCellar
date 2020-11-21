using System.Collections.Generic;
using System.Threading.Tasks;
using WineCellar.Net.API.Entities;

namespace WineCellar.Net.API.Repositories
{
    public interface IVendorRepository
    {
        Task<Vendor> CreateVendorAsync(Vendor vendor);
        Task DeleteVendorAsync(Vendor vendorIn);
        Task DeleteVendorAsync(string id);
        Task<IEnumerable<Vendor>> GetVendorsAsync();
        Task<Vendor> GetVendorAsync(string id);
        Task UpdateVendorAsync(string id, Vendor vendorIn);
    }
}