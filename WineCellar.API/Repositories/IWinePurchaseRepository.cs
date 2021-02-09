using System.Collections.Generic;
using System.Threading.Tasks;
using WineCellar.API.Entities;

namespace WineCellar.API.Repositories
{
    public interface IWinePurchaseRepository
    {
        Task<IEnumerable<WinePurchase>> GetWinePurchasesByWineIdAsync(string wineId);
        Task<WinePurchase> GetWinePurchaseAsync(string winePurchaseId);
        Task<WinePurchase> CreateWinePurchaseAsync(WinePurchase winePurchase);
        Task UpdateWinePurchaseAsync(string id, WinePurchase winePurchaseIn);
        Task DeleteWinePurchaseAsync(string id);
        Task DeleteWinePurchasesByWineIdAsync(string id);
    }
}
