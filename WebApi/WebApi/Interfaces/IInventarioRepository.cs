using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.DTOs;
using WebApi.Models;

namespace WebApi.Interfaces
{
    public interface IInventarioRepository : IBaseRepository<Inventario>
    {
        Task<List<Inventario>> GetInventarioAsync();
        Task<Inventario> GetInventarioID(int id);
        Task<SP> CreateInventario(InventarioDTO resource);
        Task<SP> UpdateInventario(InventarioDTO resource);
        Task<SP> DeleteInventario(int id);

        Task<List<Inventario>> GetInventariosByProductIdAsync(int productId);
    }
}
