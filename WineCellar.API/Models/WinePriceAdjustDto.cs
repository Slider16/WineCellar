using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using WineCellar.API.Entities;

namespace WineCellar.API.Models
{
    // Using this to demonstrate Projections of a subset of fields for a given entity
    public class WinePriceAdjustDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("vineyard")]
        public string Vineyard { get; set; }

        [BsonElement("sellPrice")]
        public decimal SellPrice { get; set; }

        [BsonElement("purchases")]
        public List<WinePurchase> WinePurchases { get; set; }

        public WinePriceAdjustDto()
        {
            WinePurchases = new List<WinePurchase>();
        }
    }

}
