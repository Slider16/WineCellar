using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WineCellar.Blazor.Shared.Models
{
    public class Wine
    {
        public string Id { get; set; }
        
        [Required]
        public string Name { get; set; }

        [Required]
        public string VineyardId { get; set; }
        
        [Required]                
        public int Year { get; set; }

        public decimal SellPrice { get; set; }

        [Required]
        public string Notes { get; set; }

        public bool ShowPurchases { get; set; }

        public bool ShowWinePurchaseForm { get; set; }

        public List<WinePurchase> WinePurchases { get; set; } =
            new List<WinePurchase>();
    }
}
