using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ForG.Models;

namespace ForG.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IMongoCollection<Users> _Mongo;

        public LoginController(IConfiguration configuration, MongoClient mongoClient)
        {
            _config = configuration;
            _Mongo = mongoClient.GetDatabase(_config.GetConnectionString("Mongodb")).GetCollection<Users>("fuser");

        }


        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] Login login)
        {
            var user = Authenticate(login);

            if (user != null)
            {
                var token = Generate(user);
                return Ok(token);
            }

            return NotFound("User not found or The Password is incorrect");
        }


        private string Generate(Users user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
             
                new Claim("Username", user.Username),
                new Claim("Email", user.Email),
                new Claim("Role", user.Role),

            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Audience"],
              claims,
              expires: DateTime.Now.AddMinutes(15),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private Users Authenticate(Login userLogin)
        {
           
            var dbList = _Mongo.AsQueryable();

            var currentUser = dbList.FirstOrDefault(o => o.Username.ToLower() == userLogin.Username.ToLower());
            if (currentUser != null)
            {
                if (BCrypt.Net.BCrypt.Verify(userLogin.Password, currentUser.Password))
                {
                    return currentUser;
                }
                return null;

            }

            return null;
        }

    }
}
