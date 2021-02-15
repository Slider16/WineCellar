using System.Collections.Generic;
using System.Threading.Tasks;
using WineCellar.Blazor.Shared.Models;

namespace WineCellar.Blazor.UI.Services
{
    public interface IVendorDataService
    {
        Task<IEnumerable<Vendor>> GetVendorsAsync();

        Task<Vendor> AddVendorAsync(Vendor vendor);

        Task UpdateVendorAsync(Vendor vendor);

        Task<Vendor> GetVendorByIdAsync(string vendorId);

        Task DeleteVendor(string vendorId);
    }
}