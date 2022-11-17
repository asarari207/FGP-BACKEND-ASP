using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ForG.Models
{
    public class Users
    {
        [BsonId]
        public ObjectId _id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
