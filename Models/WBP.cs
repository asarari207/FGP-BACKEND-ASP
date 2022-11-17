using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ForG.Models
{
    public class WBP
    {
        [BsonId]
        public ObjectId _id { get; set; }
        public string SiteID { get; set; }
        public string Status { get; set; }
        public string ProgressTime { get; set; }
        public string DoneTime { get; set; }
    }
}
