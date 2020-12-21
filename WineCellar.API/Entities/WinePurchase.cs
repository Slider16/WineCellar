using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace WineCellar.API.Entities
{
    public class WinePurchase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        [BsonElement("vendorId")]
        public string VendorId { get; set; }

        [BsonElement("purchaseDate")]
        public DateTime PurchaseDate { get; set; }

        [BsonElement("purchasePrice")]
        public decimal PurchasePrice { get; set; }

        // We need this constructor in order to properly generate a mongoDb objectid for this sub-document
        public WinePurchase()
        {
            Id = ObjectId.GenerateNewId().ToString();
        }

    }
}