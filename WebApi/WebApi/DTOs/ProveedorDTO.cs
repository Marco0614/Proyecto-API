using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApi.DTOs
{
    public class ProveedoreDTO
    {
        public int IdProveedor { get; set; }

        [Required(ErrorMessage = "El nombre es requerido.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El teléfono de contacto es requerido.")]
        [RegularExpression(@"^[0-9]{8}$", ErrorMessage = "El teléfono de contacto debe tener exactamente 8 números.")]
        public string TelefonoContacto { get; set; }

        [Required(ErrorMessage = "El correo es requerido.")]
        [EmailAddress(ErrorMessage = "El correo debe ser una dirección de correo electrónico válida.")]
        [RegularExpression(@"^(.*@gmail\.com|.*@uamcr\.net|.*@hotmail\.com)$", ErrorMessage = "El correo debe ser de los dominios gmail.com, uamcr.net o hotmail.com.")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "La dirección es requerida.")]
        [StringLength(200, ErrorMessage = "La dirección no puede exceder los 200 caracteres.")]
        public string Direccion { get; set; }

        public ICollection<ProductoDTO> Productos { get; set; }
    }
}
