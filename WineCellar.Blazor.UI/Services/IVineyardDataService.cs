using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WineCellar.Blazor.Shared.Models;

namespace WineCellar.Blazor.UI.Services
{
    public interface IVineyardDataService
    {
        Task<IEnumerable<Vineyard>> GetVineyardsAsync();

        Task<Vineyard> AddVineyardAsync(Vineyard vineyard);

        Task UpdateVineyardAsync(Vineyard vineyard);

        Task<Vineyard> GetVineyardByIdAsync(string vineyardId);

        Task DeleteVineyard(string vineyardId);

    }
}
