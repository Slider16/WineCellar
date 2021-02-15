using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WineCellar.Blazor.Shared.Models;

namespace WineCellar.Blazor.UI.Services
{
    public interface IWinePurchaseDataService
    {
        Task<IEnumerable<WinePurchase>> GetWinePurchasesByWineIdAsync(string wineId);

        Task<WinePurchase> GetWinePurchaseByIdAsync(string winePurchaseId);

        Task<WinePurchase> AddWinePurchaseAsync(WinePurchase winePurchase);

        Task UpdateWinePurchaseAsync(WinePurchase winePurchase);

        Task DeleteWinePurchaseAsync(string winePurchaseId);
    }
}
