using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WineCellar.Net.MVC.ViewModels
{
    public class WinePurchaseViewModel
    {
        public string Id { get; set; }
        public string VendorId { get; set; }
        public DateTimeOffset PurchaseDate { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal ResearchedPrice { get; set; }
    }
}
