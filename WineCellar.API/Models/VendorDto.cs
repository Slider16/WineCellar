using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WineCellar.Net.API.Models
{
    public class VendorDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }       
        public string Phone { get; set; }
        public Uri WebSite { get; set; }
        public List<LinkDto> Links { get; set; } 
            = new List<LinkDto>();

    }
}
