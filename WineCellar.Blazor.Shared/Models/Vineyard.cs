using System;
using System.Collections.Generic;
using System.Text;

namespace WineCellar.Blazor.Shared.Models
{
    public class Vineyard
    {        
        public string Id { get; set; }        
        public string Name { get; set; }        
        public Address Address { get; set; }        
        public decimal Longitude { get; set; }        
        public decimal Latitude { get; set; }

        public Vineyard()
        {
            Address = new Address();
        }
    }
}
