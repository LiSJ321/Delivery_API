using Delivery_BLL.Configs;
using Delivery_BLL.Services.IServices;
using Delivery_DAL.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
namespace Delivery_BLL.Services
{
    public class JwtService : IJwtService
    {

        public readonly IConfiguration _configuration;
        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public JwtSecurityToken CreateToken(User user)
        {
            var jwtConfig = _configuration.GetSection("Jwt").Get<JwtConfig>();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SigningKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claimsList = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };
            var expires = DateTime.Now.AddMinutes(jwtConfig.Expires);
            var token = new JwtSecurityToken(
                issuer: jwtConfig.Issuer,
                audience: jwtConfig.Audience,
                claims: claimsList,
                notBefore: DateTime.Now,
                expires: expires,
                signingCredentials: credentials
            );
            return token;
        }
    }
}
