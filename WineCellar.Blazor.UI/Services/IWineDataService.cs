using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WineCellar.Blazor.Shared.Models;

namespace WineCellar.Blazor.UI.Services
{
    public interface IWineDataService
    {
        Task<IEnumerable<Wine>> GetAllWinesAsync();

        Task<Wine> AddWineAsync(Wine wine);

        Task<Wine> GetWineByIdAsync(string wineId);

        Task DeleteWine(string wineId);
    }
}
