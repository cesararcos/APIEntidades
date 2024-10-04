using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace APIEntidades.Utilities
{
    public class Utilities : IUtilities
    {
        private readonly IConfiguration _configuration;

        public Utilities(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerarToken(string usuario)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, usuario), // Subjeto (usuario)
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // ID del token
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]!)); // La clave secreta debe ser la misma usada para validar el token
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtConfig = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(jwtConfig); // Devuelve el token generado
        }
    }
}
