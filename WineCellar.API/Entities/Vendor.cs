using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WineCellar.API.Entities;

namespace WineCellar.API.Entities
{
    public class Vendor
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("address")]
        public string Address { get; set; }
        
        [BsonElement("city")]
        public string City { get; set; }

        [BsonElement("state")]
        public string State { get; set; }

        [BsonElement("zipcode")]
        public string ZipCode { get; set; }

        [BsonElement("country")]
        public string Country { get; set; }

        [BsonElement("phone")]
        public string Phone { get; set; }

        [BsonElement("website")]
        public string Website { get; set; }

        [BsonElement("__v")]
        public int __v { get; set; }

        public Address AddressNew { get; set; }
    }
}
