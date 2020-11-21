using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WineCellar.Net.API.Models
{
    public class WinePurchaseForManipulationDto
    {
        [Required(ErrorMessage = "Vendor id is required")]
        public string VendorId { get; set; }

        [Required(ErrorMessage = "Purchase date is required")]
        public DateTime PurchaseDate { get; set; }

        [Required(ErrorMessage = "Purchase price is required")]
        public decimal PurchasePrice { get; set; }

        public decimal ResearchedPrice { get; set; }
    }
}
