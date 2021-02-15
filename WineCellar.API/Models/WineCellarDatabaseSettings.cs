using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WineCellar.API.Interfaces;

namespace WineCellar.API.Models
{
    public class WineCellarDatabaseSettings : IWineCellarDatabaseSettings
    {
        public string WinesCollectionName { get; set; }
        public string WinePurchasesCollectionName { get; set; }
        public string VendorsCollectionName { get; set; }
        public string VineyardsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
