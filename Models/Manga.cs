using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MangaShopApi.Models
{
    public class Manga
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonElement("title")]
        public string Title { get; set; } = null!;
        [BsonElement("author")]
        public string Author { get; set; } = null!;
        [BsonElement("genres")]
        public List<string> Genres { get; set; } = new();
        [BsonElement("subgenres")]
        public List<string> Subgenres { get; set; } = new();
        [BsonElement("volume_count")]
        public int VolumeCount { get; set; }
        [BsonElement("editorial")]
        public string Editorial { get; set; } = null!;
        [BsonElement("description")]
        public string Description { get; set; } = null!;
        [BsonElement("price")]
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Price { get; set; }
        [BsonElement("stock")]
        public int Stock { get; set; }
        [BsonElement("cover_image_url")]
        public string? CoverImageUrl { get; set; }

        [BsonElement("release_date")]
        public DateTime ReleaseDate { get; set; }
    }
}