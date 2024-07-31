using WebApi.Interfaces;
using WebApi.Models;
using WebApi.Data;

namespace WebApi.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbpruebaContext _context;

        private IAccesoRepository _acceso = default!;
        private IInventarioRepository _inventario = default!;
        //private IUsuarioRepository _usuario = default!;
        private IProveedorRepository _proveedor = default!;
        private IProductoRepository _producto = default!;

        public IAccesoRepository Acceso => _acceso ?? new AccesoRepository(_context);
        public IInventarioRepository Inventario => _inventario ?? new InventarioRepository(_context);
       //public IUsuarioRepository Usuario => _usuario ?? new UsuarioRepository(_context);
        public IProveedorRepository Proveedor => _proveedor ?? new ProveedorRepository(_context);
        public IProductoRepository Producto => _producto ?? new ProductoRepository(_context);



        public UnitOfWork(DbpruebaContext context)
        {
            _context = context;
            
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
