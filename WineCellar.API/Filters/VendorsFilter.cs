using Microsoft.CodeAnalysis;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WineCellar.API.Entities;

namespace WineCellar.API.Filters
{
    public class VendorsFilter
    {
        private string VendorName { get; set; }

        private string City { get; set; }

        public FilterDefinition<Vendor> ToFilterDefinition()
        {
            var filterDefinition = Builders<Vendor>.Filter.Empty;

            // Filter code and logic here
            if (!string.IsNullOrEmpty(VendorName))
            {
                filterDefinition &= Builders<Vendor>.Filter.Eq(v => v.Name, VendorName);
            }

            if (!string.IsNullOrEmpty(City))
            {
                filterDefinition &= Builders<Vendor>.Filter.Eq(v => v.City, City);
            }


            return filterDefinition;
        }
    }
}
