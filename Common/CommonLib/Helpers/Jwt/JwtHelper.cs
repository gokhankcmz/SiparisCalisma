using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using CommonLib.Models.ErrorModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace CommonLib.Helpers.Jwt
{
    public static class JwtHelper
    {
        public static void ConfigureJwt(this IServiceCollection services, IConfiguration
            configuration)
        {
            var provider = "Bearer";
            var jwtSettings = configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings.GetSection("secret").Value;
            services.AddAuthentication(opt => {
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(provider, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidIssuer = jwtSettings.GetSection("validIssuer").Value,
                        ValidAudience = jwtSettings.GetSection("validAudience").Value,
                        IssuerSigningKey = new
                            SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                    };
                });
        }
        
        public static string GetClaimOrThrow(this IHeaderDictionary headerDictionary, string claimType, Exception exception=null)
        {
            var token = headerDictionary["Authorization"].ToString();
            token = token.Replace("Bearer ", "");
            exception ??= new UnAuthorized();
            try
            {
                var tokenDecoded = new JwtSecurityToken(token);
                if (tokenDecoded.ValidTo < DateTime.UtcNow) throw exception;
                Claim claim = tokenDecoded.Claims.FirstOrDefault(x => x.Type.Equals(claimType));
                return claim.Value;

            }
            catch (Exception e)
            {
                throw exception;
            }

        }
    }
}