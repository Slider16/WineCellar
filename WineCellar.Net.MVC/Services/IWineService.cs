using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WineCellar.Net.MVC.Data.Entities;

namespace WineCellar.Net.MVC.Services
{
    public interface IWineService
    {
        Task<Wine> GetWineAsync();
        Task<IEnumerable<Wine>> GetWinesAsync();
    }
}
