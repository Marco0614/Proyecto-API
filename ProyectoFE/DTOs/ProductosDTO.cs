using System.ComponentModel.DataAnnotations;

namespace ProyectoFE.DTOs
{
    public class ProductosDTO
    {
        public int IdProducto { get; set; }

        [Required(ErrorMessage = "El nombre es requerido.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El nombre debe contener solo letras.")]
        public string Nombre { get; set; }

        public int? IdProveedor { get; set; }

        [StringLength(50, ErrorMessage = "La marca no puede exceder los 50 caracteres.")]
        public string Marca { get; set; }

        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "El precio debe ser un número válido.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0.")]
        public decimal? Precio { get; set; }
    }
}
