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
    public class ACsController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IMongoCollection<Acg> _Mongo;

        public ACsController(IConfiguration configuration, MongoClient mongoClient)
        {
            _config = configuration;
            _Mongo = mongoClient.GetDatabase(_config.GetConnectionString("Mongodb")).GetCollection<Acg>("acs");
        }

        [HttpGet]
        public JsonResult Get()
        {
            
            var data = _Mongo.AsQueryable();
           
            return new JsonResult(data);
        }

        [HttpPost]
        public JsonResult Post(Acg gazapool)
        {
            
            _Mongo.InsertOne(gazapool);
            return new JsonResult($"{gazapool.SiteID} is in the table");
        }

        [HttpPut("{id}")]
        public JsonResult Edit(string id)
        {
        
            var filter = Builders<Acg>.Filter.Eq("_id", new BsonObjectId(id));
            var getone = _Mongo.Find(filter);
             foreach(var rult in getone.ToList())
            {
                if(rult.Status == "Done")
                {
                    var update = Builders<Acg>.Update.Set("Status", "In progress");
                    _Mongo.UpdateOne(filter, update);
                }
                else
                {

                    var update = Builders<Acg>.Update.Set("Status", "Done");
                    _Mongo.UpdateOne(filter, update);
                }
                
            }
            
     

            return new JsonResult("Done");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(string id)
        {
       
            var filter = Builders<Acg>.Filter.Eq("_id", new BsonObjectId(id));
            _Mongo.DeleteOne(filter);

            return new JsonResult("Deleted Successfully");
        }
    }
}
