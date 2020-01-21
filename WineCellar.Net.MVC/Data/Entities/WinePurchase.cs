using System;
using System.Collections.Generic;
namespace WineCellar.Net.MVC.Data.Entities
{
    public class WinePurchase
    {        
        public string Id { get; set; }       
        public string VendorId { get; set; }        
        public DateTime PurchaseDate { get; set; }
        public decimal PurchasePrice { get; set; }
    }
}
