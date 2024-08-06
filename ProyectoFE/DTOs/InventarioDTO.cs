using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;


namespace ProyectoFE.DTOs
{
    public class InventarioDTO : IValidatableObject
    {
        public int IdMovimiento { get; set; }

        [Required(ErrorMessage = "El tipo de movimiento es requerido.")]
        [StringLength(50, ErrorMessage = "El tipo de movimiento no puede exceder los 50 caracteres.")]
        public string TipoMovimiento { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El IdProducto debe ser un número positivo.")]
        public int IdProducto { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser un número positivo.")]
        public int Cantidad { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que cero.")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "La fecha de movimiento es requerida.")]
        [DataType(DataType.Date)]
        public DateTime FechaMovimiento { get; set; }

        [DataType(DataType.Date)]
        public DateTime? FechaCaducidad { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FechaMovimiento < DateTime.Today)
            {
                yield return new ValidationResult("La fecha de movimiento debe ser mayor o igual a la fecha actual.", new[] { nameof(FechaMovimiento) });
            }

            if (FechaCaducidad.HasValue && FechaCaducidad <= FechaMovimiento)
            {
                yield return new ValidationResult("La fecha de caducidad debe ser mayor que la fecha de movimiento.", new[] { nameof(FechaCaducidad) });
            }

            if (FechaCaducidad.HasValue && FechaCaducidad <= DateTime.Today)
            {
                yield return new ValidationResult("La fecha de caducidad debe ser mayor que la fecha actual.", new[] { nameof(FechaCaducidad) });
            }
        }
    }
}

