using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WineCellar.Blazor.Shared.Models;

namespace WineCellar.Blazor.AppAssembly.Services
{
    public interface IWineDataService
    {
        Task<IEnumerable<Wine>> GetAllWinesAsync();
    }
}
