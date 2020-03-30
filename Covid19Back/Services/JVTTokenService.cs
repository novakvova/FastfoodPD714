using Covid19Back.Abstract;
using Covid19Back.DTO;
using Covid19Back.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Covid19Back.Services
{
    public class JWTTokenService : IJWTTokenService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly UserManager<DbUser> _userManager;
        public JWTTokenService(ApplicationDbContext context,
            IConfiguration configuration,
            UserManager<DbUser> userManager)
        {
            _context = context;
            _configuration = configuration;
            _userManager = userManager;
        }
        public string CreateRefreshToken(DbUser user)
        {
            return "";
        }
        public string CreateToken(DbUser user)
        {
            var roles = _userManager.GetRolesAsync(user).Result;
            var claims = new List<Claim>()
            {
                //new Claim(JwtRegisteredClaimNames.Sub, user.Id)
                new Claim("id", user.Id.ToString()),
                new Claim("name", user.UserName)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim("roles", role));
            }

            string jwtTokenSecretKey = this._configuration.GetValue<string>("SecretPhrase");

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenSecretKey));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var jwt = new JwtSecurityToken(
                signingCredentials: signingCredentials,
                claims: claims,
                expires: DateTime.Now.AddYears(1));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
        public async Task<TokensDTO> RefreshAuthToken(string oldAuthToken, string refreshToken)
        {
            return null;
        }

    }
}
