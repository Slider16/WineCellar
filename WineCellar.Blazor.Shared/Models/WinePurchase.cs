using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WineCellar.Blazor.Shared.Models
{
    public class WinePurchase
    {
        public string Id { get; set; }
        
        public string WineId { get; set; }
        
        [Required]
        public string VendorId { get; set; }
        
        [Required]
        public DateTimeOffset? PurchaseDate { get; set; }
        
        [Required]
        public decimal? PurchasePrice { get; set; }
        
        public decimal? ResearchedPrice { get; set; }

        public string VendorName { get; set; }
    }
}
