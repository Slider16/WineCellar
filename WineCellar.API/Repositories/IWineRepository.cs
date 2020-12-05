using System.Collections.Generic;
using System.Threading.Tasks;
using WineCellar.Net.API.Entities;

namespace WineCellar.Net.API.Repositories
{
    public interface IWineRepository
    {
        Task<IEnumerable<Wine>> GetWinesAsync();
        Task<IEnumerable<Wine>> GetWinesByVineyardAsync(string vineyard);
        Task<IEnumerable<Wine>> GetWinesByYearAsync(int year);
        Task<Wine> GetWineAsync(string id);
        Task<Wine> CreateWineAsync(Wine wine);
        Task UpdateWineAsync(string id, Wine wineIn);
        Task DeleteWineAsync(Wine wineIn);
        Task DeleteWineAsync(string id);
        Task<bool> WineExists(string wineId);
    }
}
