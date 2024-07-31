using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.DTOs;
using WebApi.Models;

namespace WebApi.Interfaces
{
    public interface IProveedorRepository : IBaseRepository<Proveedore>
    {
        Task<List<Proveedore>> GetProveedoresAsync();
        Task<Proveedore?> GetProveedorByNombreAsync(string nombre);
        Task<Proveedore?> GetProveedorByIdAsync(int id);
        Task<SP> CreateProveedorAsync(ProveedoreDTO proveedor);
        Task<SP> UpdateProveedorAsync(ProveedoreDTO proveedor);
        Task<SP> DeleteProveedorAsync(int id);
    }
}