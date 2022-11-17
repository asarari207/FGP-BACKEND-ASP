using ForG.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace ForG.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegsController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IMongoCollection<Users> _Mongo;
        public RegsController(IConfiguration configuration, MongoClient mongoClient)
        {
            _config = configuration;
            _Mongo = mongoClient.GetDatabase(_config.GetConnectionString("Mongodb")).GetCollection<Users>("fuser");
        }

        [HttpPost]
        public JsonResult Post(Users users)
        {
          
            users.Password = BCrypt.Net.BCrypt.HashPassword(users.Password);
            _Mongo.InsertOne(users);

            return new JsonResult($"{users.Username} Welcome to 4G-Project Solutions");
        }
    }
}
