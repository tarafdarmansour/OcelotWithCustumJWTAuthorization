using AuthenticationService.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        private IEnumerable<Claim> _getClaims(User user)
        {
            var list = new List<Claim>();
            list.Add(new Claim(ClaimTypes.Name, user.Username));

            return list;
        }
        public string GenerateToken(User user)
        {
            var secretKey = Encoding.UTF8.GetBytes(_configuration.GetValue<string>("SecretKey")); // longer that 16 character
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature);

            var claims = _getClaims(user);

            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration.GetValue<string>("Issuer"),
                Audience = _configuration.GetValue<string>("Audience"),
                IssuedAt = DateTime.Now,
                NotBefore = DateTime.Now.AddMinutes(_configuration.GetValue<int>("NotBeforeMinutes")),
                Expires = DateTime.Now.AddMinutes(_configuration.GetValue<int>("ExpirationMinutes")),
                SigningCredentials = signingCredentials,
                Subject = new ClaimsIdentity(claims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateJwtSecurityToken(descriptor);

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
    }
}
