using System.ComponentModel.DataAnnotations;

namespace WebApi.DTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "El correo es requerido.")]
        [EmailAddress(ErrorMessage = "El correo debe ser una dirección de correo electrónico válida.")]
        [RegularExpression(@"^(.*@gmail\.com|.*@uamcr\.net|.*@hotmail\.com)$", ErrorMessage = "El correo debe ser de los dominios gmail.com, uamcr.net o hotmail.com.")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "La clave es requerida.")]
        //[RegularExpression(@"^(?=(?:.*[A-Za-z]){2})(?=(?:.*\d){2})(?=(?:.*[@$!%*?&]){2}).{6,}$", ErrorMessage = "La clave debe tener al menos dos letras, dos números y dos caracteres especiales.")]
        public string Clave { get; set; }
    }
}
