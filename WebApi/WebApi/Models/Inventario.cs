using System;

namespace WebApi.Models
{
    public partial class Inventario
    {
        public int IdMovimiento { get; set; }
        public string TipoMovimiento { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public DateTime FechaMovimiento { get; set; }
        public DateTime? FechaCaducidad { get; set; }

        public virtual Producto Producto { get; set; }
    }
}
