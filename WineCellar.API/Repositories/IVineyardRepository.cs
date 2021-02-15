using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using WineCellar.API.Entities;

namespace WineCellar.API.Repositories
{
    public interface IVineyardRepository
    {
        Task<IEnumerable<Vineyard>> GetVineyardsAsync();
        Task<Vineyard> GetVineyardAsync(string id);
        Task<Vineyard> CreateVineyardAsync(Vineyard vineyard);
        Task UpdateVineyardAsync(string id, Vineyard vineyardIn);
        Task DeleteVineyardAsync(string id);
        Task<bool> VineyardExists(string vineyardId);
    }
}