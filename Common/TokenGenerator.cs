using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using ApiExtranjeros.Models;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ApiExtranjeros.Common
{
    public  class TokenGenerator
    {
        private readonly AppSettings _appSettings;
        public TokenGenerator(IOptions<Configuration> configuration)
        {
            _appSettings = configuration.Value.appSettings;
        }
        
   
        public string GenerateJwtToken(string username)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Name, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JWT_SECRET_KEY));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _appSettings.JWT_ISSUER_TOKEN,
                audience: _appSettings.JWT_AUDIENCE_TOKEN,
                claims: claims,
                //notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_appSettings.JWT_EXPIRE_MINUTES)),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
