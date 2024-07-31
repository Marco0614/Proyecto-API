using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using WebApi.Models;


namespace WebAPI.Custom
{
    public class Configuraciones
    {
        private readonly IConfiguration _configuration;

        public Configuraciones(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Método para encriptar contraseña usando bcrypt
        public string EncriptarPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Método para verificar la contraseña usando bcrypt
        public bool VerificarPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        // Método para generar un token JWT
        public string generarJWT(Usuario modelo)
        {
            // Crear las claims (reclamaciones) del usuario para el token JWT
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, modelo.IdUsuario.ToString()),
                new Claim(ClaimTypes.Email, modelo.Correo)
                // Puedes agregar más claims según las necesidades de tu aplicación
            };

            // Obtener la clave de seguridad para firmar el token desde la configuración
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]));

            // Configurar las credenciales para firmar el token
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            // Crear el token JWT con la configuración especificada
            var jwtConfig = new JwtSecurityToken(
                claims: userClaims,
                expires: DateTime.UtcNow.AddMinutes(10),  // Token expira después de 10 minutos, cambiar a dos segun indicacion de profe
                signingCredentials: credentials
            );

            // Escribir el token JWT como una cadena
            return new JwtSecurityTokenHandler().WriteToken(jwtConfig);
        }
    }
}
