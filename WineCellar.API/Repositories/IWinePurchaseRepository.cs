using System.Collections.Generic;
using System.Threading.Tasks;
using WineCellar.Net.API.Entities;

namespace WineCellar.Net.API.Repositories
{
    public interface IWinePurchaseRepository
    {
        Task<IEnumerable<WinePurchase>> GetWinePurchasesAsync();
        Task<WinePurchase> GetWinePurchaseAsync(string wineId, string winePurchaseId);
        Task<WinePurchase> CreateWinePurchaseAsync(WinePurchase winePurchase);
        Task UpdateWinePurchaseAsync(string id, WinePurchase winePurchaseIn);
        Task DeleteWinePurchaseAsync(WinePurchase winePurchaseIn);
        Task DeleteWinePurchaseAsync(string id);
    }
}
