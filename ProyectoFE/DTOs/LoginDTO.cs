using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ProyectoFE.DTOs
{
    public class LoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }

    }

    public class LoginDtoResponse
    {
        public string token { get; set; }
    }
}


