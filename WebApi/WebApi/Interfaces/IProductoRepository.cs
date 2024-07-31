using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.DTOs;
using WebApi.Models;

namespace WebApi.Interfaces
{
    public interface IProductoRepository : IBaseRepository<Producto>
    {
        Task<List<Producto>> GetProductosAsync();
        Task<Producto?> GetProductoByIdAsync(int id);
        Task<Producto> GetProductoByNombreAsync(string nombre);

        Task<SP> CreateProductoAsync(ProductoDTO producto);
        Task<SP> UpdateProductoAsync(ProductoDTO producto);
        Task<SP> DeleteProductoAsync(int id);

    }
}
