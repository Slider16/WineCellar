using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WineCellar.API.Entities;

namespace WineCellar.API.Models
{
    public abstract class WineForManipulationDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        public string Vineyard { get; set; }
        public string VineyardId { get; set; }
        public string Location { get; set; }
        public int Year { get; set; }        
        public ImageFile ImageFile { get; set; }
        public byte[] ContentImage { get; set; }

        [MaxLength(1500, ErrorMessage = "Maximum length for notes is 1500 characters.")]
        public virtual string Notes { get; set; }
        public List<WinePurchaseForCreationDto> WinePurchases { get; set; }
          = new List<WinePurchaseForCreationDto>();
    }
}
