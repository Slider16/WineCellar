using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WineCellar.API.Entities;

namespace WineCellar.API.Models
{
    public class VineyardForManipulationDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        public Address Address { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }

    }
}
