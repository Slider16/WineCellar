using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace WineCellar.API.Entities
{
    public class Wine
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("vineyardid")]
        public string VineyardId { get; set; }

        [BsonElement("vineyard")]
        public string Vineyard { get; set; }

        [BsonElement("location")]
        public string Location { get; set; }

        [BsonElement("year")]
        public int Year { get; set; }

        [BsonElement("bin")]
        public int Bin { get; set; }

        [BsonElement("sellPrice")]
        public decimal SellPrice { get; set; }

        [BsonElement("imagefile")]
        public ImageFile ImageFile { get; set; }

        [BsonElement("contentimage")]
        public byte[] ContentImage { get; set; }

        [BsonElement("notes")]
        public string Notes { get; set; }

        [BsonElement("__v")]
        public int __v { get; set; }
    }
}