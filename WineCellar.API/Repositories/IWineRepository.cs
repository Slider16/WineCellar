using System.Collections.Generic;
using System.Threading.Tasks;
using WineCellar.API.Filters;
using WineCellar.API.Entities;
using WineCellar.API.Models;

namespace WineCellar.API.Repositories
{
    public interface IWineRepository
    {
        Task<IEnumerable<Wine>> GetWinesAsync(WinesFilter filters);

        // Not needed since implementing WinesFilter class
        //Task<IEnumerable<Wine>> GetWinesByYearAsync(int year);
        Task<IEnumerable<Wine>> GetWinesByVineyardAsync(string vineyard);
        Task<IEnumerable<WinePriceAdjustDto>> GetWinesForPriceAdjust();
        Task<Wine> GetWineAsync(string id);
        Task<Wine> CreateWineAsync(Wine wine);
        Task UpdateWineAsync(string id, Wine wineIn);
        Task DeleteWineAsync(string id);
        Task<bool> WineExists(string wineId);
    }
}
