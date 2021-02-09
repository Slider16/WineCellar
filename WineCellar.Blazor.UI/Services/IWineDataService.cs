using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WineCellar.Blazor.Shared.Models;

namespace WineCellar.Blazor.UI.Services
{
    public interface IWineDataService
    {
        Task<IEnumerable<Wine>> GetWinesAsync();

        Task<IEnumerable<Wine>> GetWinesForPriceAdjustAsync();

        Task<Wine> AddWineAsync(Wine wine);

        Task UpdateWineAsync(Wine wine);

        Task<Wine> GetWineByIdAsync(string wineId);

        Task DeleteWineAsync(string wineId);

        Task GetVineyardsAsync();
    }
}
