using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WineCellar.API.Entities;
using WineCellar.API.Repositories;

namespace WineCellar.API.Tests
{
    public class WinePurchaseServiceFake : IWinePurchaseRepository
    {
        private List<WinePurchase> _winePurchaseList;

        private IMongoCollection<WinePurchase> _winePurchaseCollection;

        public WinePurchaseServiceFake()
        {

        }

        public WinePurchaseServiceFake(List<WinePurchase> winePurchaseList)
        {
            _winePurchaseList = winePurchaseList;
        }

        public Task<WinePurchase> CreateWinePurchaseAsync(WinePurchase winePurchase)
        {
            throw new NotImplementedException();
        }

        public Task DeleteWinePurchaseAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteWinePurchasesByWineIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<WinePurchase> GetWinePurchaseAsync(string winePurchaseId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<WinePurchase>> GetWinePurchasesByWineIdAsync(string wineId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateWinePurchaseAsync(string id, WinePurchase winePurchaseIn)
        {
            throw new NotImplementedException();
        }
    }
}
