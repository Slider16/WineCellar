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
        public string Vineyard { get; set; }

        public string Location { get; set; }
        
        [Required]                
        public int Year { get; set; }

        public string Notes { get; set; }
    }
}
