using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WineCellar.API.Entities
{
    public class Vineyard
    {
        public string Name { get; set; }
        public Address Address { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }

    }
}
