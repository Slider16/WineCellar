using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WineCellar.API.Entities;

namespace WineCellar.API.Filters
{
    public class WinesFilter
    {
        public int Year { get; set; }
        
        public int Bin { get; set; }

        public string Vineyard { get; set; }


        public FilterDefinition<Wine> ToFilterDefinition()
        {
            var filterDefinition = Builders<Wine>.Filter.Empty;  // equivalent to new BsonDocument()

            if (Year > 0)
            {
                filterDefinition &= Builders<Wine>.Filter.Eq(w => w.Year, Year);
            }

            if (Bin > 0)
            {
                filterDefinition &= Builders<Wine>.Filter.Eq(w => w.Bin, Bin);
            }

            if (!string.IsNullOrEmpty(Vineyard))
            {
                filterDefinition &= Builders<Wine>.Filter.Where(w => w.Vineyard.StartsWith(Vineyard));
            }            

            return filterDefinition;
        }

        public SortDefinition<Wine> ToSortDefinition()
        {
            var sortDefinition = Builders<Wine>.Sort.Ascending(w => w.Name);

            return sortDefinition;
        }
    }
}
