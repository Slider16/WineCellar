using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WineCellar.API.Models
{
    public class WinePurchaseDto    
    {
        public string Id { get; set; }
        
        public string VendorId { get; set; }
        
        public string WineId { get; set; }        
        
        public DateTimeOffset PurchaseDate { get; set; }
        
        public decimal PurchasePrice { get; set; }
        
        public decimal ResearchedPrice { get; set; }
    }
}
