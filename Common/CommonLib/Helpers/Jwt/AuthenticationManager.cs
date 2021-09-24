﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using CommonLib.Models;
using CommonLib.Models.ErrorModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CommonLib.Helpers.Jwt
{
    public class AuthenticationManager<T> where T: Document
    {
        private readonly IConfiguration _configuration;
        public AuthenticationManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Authenticate(T document)
        {
            if (document == null) return null;
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var tokenDescriptor = new SecurityTokenDescriptor
            {

                Subject = new ClaimsIdentity(new Claim[]
                {
                    new (ClaimTypes.NameIdentifier, document.Id.ToString()),
                }),
                Expires = DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings.GetSection("expires").Value)),
                Issuer = jwtSettings.GetSection("validIssuer").Value,
                Audience= jwtSettings.GetSection("validAudience").Value,
                SigningCredentials = GetSigningCredentials()

            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        

        private SigningCredentials GetSigningCredentials()
        {
            var key =
                Encoding.UTF8.GetBytes(_configuration.GetSection("JwtSettings").GetSection("secret").Value);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }
    }

}
