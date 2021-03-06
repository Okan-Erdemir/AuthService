﻿using AuthService.WebApi.Models;
using AuthService.WebApi.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.WebApi.Services
{
    public class UserService : IUserService
    {
        private readonly SecretModel _config;

        public UserService(IOptions<SecretModel> appSettings)
        {
            _config = appSettings.Value;
        }

        private List<UserModel> _users = new List<UserModel>
        {
            new UserModel { Id = 1, FirstName = "Test", LastName = "test", Username = "test", Password = "test",Email="test@test.com" }
        };

        public AuthResponseModel Authenticate(AuthRequestModel model)
        {
            var user = _users.SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);

            // return null if user not found
            if (user == null) return null;

            var token = generateJwtToken(user);

            return new AuthResponseModel()
            {
                FirstName = user.FirstName,
                Id = user.Id,
                LastName = user.LastName,
                Token = token,
                Username = user.Username
            };
        }

        public UserModel GetById(int id)
        {
            return _users.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<UserModel> GetAll()
        {
            return _users;
        }
        private string generateJwtToken(UserModel user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
