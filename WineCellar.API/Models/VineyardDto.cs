using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WineCellar.API.Entities;

namespace WineCellar.API.Models
{
    public class VineyardDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public List<LinkDto> Links { get; set; }
            = new List<LinkDto>();
    }
}
