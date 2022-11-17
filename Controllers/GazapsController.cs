using ForG.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ForG.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GazapsController : ControllerBase
    {
        private readonly IConfiguration _config;

        private readonly IMongoCollection<TwoG> _Mongo;
        public GazapsController(IConfiguration configuration, MongoClient mongoClient)
        {
            _config = configuration;
            _Mongo = mongoClient.GetDatabase(_config.GetConnectionString("Mongodb")).GetCollection<TwoG>("gzpools");

        }

        [HttpGet]
        public JsonResult Get()
        {
           
            var data = _Mongo.AsQueryable();

            return new JsonResult(data);
        }

        [HttpPost]
        public JsonResult Post(TwoG gazapool)
        {
        
            _Mongo.InsertOne(gazapool);
            return new JsonResult($"{gazapool.SiteID} is in the table");
        }

        [HttpPut("{id}")]
        public JsonResult Edit(string id)
        {
         
            var filter = Builders<TwoG>.Filter.Eq("_id", new BsonObjectId(id));
            var getone = _Mongo.Find(filter);
            foreach (var rult in getone.ToList())
            {
                if (rult.Status == "Done")
                {
                    var update = Builders<TwoG>.Update.Set("Status", "In progress");
                    _Mongo.UpdateOne(filter, update);
                }
                else
                {

                    var update = Builders<TwoG>.Update.Set("Status", "Done");
                    _Mongo.UpdateOne(filter, update);
                }

            }


            return new JsonResult("Done");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(string id)
        {
           
            var filter = Builders<TwoG>.Filter.Eq("_id", new BsonObjectId(id));
            _Mongo.DeleteOne(filter);

            return new JsonResult("Deleted Successfully");
        }
    }
}
