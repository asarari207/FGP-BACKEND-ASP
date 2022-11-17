using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ForG.Models
{
    public class Acg
    {
        [BsonId]
        public ObjectId _id { get; set; }
        public string SiteID { get; set; }
        public string Status { get; set; }
        public string ProgressTime { get; set; }
        public string DoneTime { get; set; }
    }
}
