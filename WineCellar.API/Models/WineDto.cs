using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WineCellar.API.Entities;

namespace WineCellar.API.Models
{
    public class WineDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Vineyard { get; set; }
        public string VineyardId { get; set; }
        public string Location { get; set; }
        public int Year { get; set; }
        public int Bin { get; set; }
        public string Notes { get; set; }
        public List<LinkDto> Links { get; set; } 
            = new List<LinkDto>();
        public List<WinePurchaseDto> WinePurchases { get; set; }
            = new List<WinePurchaseDto>();
    }
}
