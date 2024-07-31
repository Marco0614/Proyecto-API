using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.DTOs;
using WebApi.Interfaces;
using WebApi.Models;
using WebApi.Data;

namespace WebApi.Repositories
{
    public class ProductoRepository : BaseRepository<Producto>, IProductoRepository
    {
        public ProductoRepository(DbpruebaContext context) : base(context)
        {
        }

        public async Task<List<Producto>> GetProductosAsync()
        {
            return await Context.Productos.ToListAsync();
        }

        public async Task<Producto?> GetProductoByIdAsync(int id)
        {
            return await Context.Productos.FirstOrDefaultAsync(p => p.IdProducto == id);
        }

        public async Task<Producto> GetProductoByNombreAsync(string nombre)
        {
            return await Context.Productos.FirstOrDefaultAsync(p => p.Nombre == nombre);
        }


        public async Task<SP> CreateProductoAsync(ProductoDTO producto)
        {
            var paramNombre = new SqlParameter("@Nombre", producto.Nombre);
            var paramIdProveedor = new SqlParameter("@IdProveedor", producto.IdProveedor);
            var paramMarca = new SqlParameter("@Marca", producto.Marca);
            var paramPrecio = new SqlParameter("@Precio", producto.Precio);

            var responseSP = await Context.Set<SP>()
                .FromSqlRaw("EXECUTE [dbo].[AgregarProducto] @Nombre, @IdProveedor, @Marca, @Precio",
                            paramNombre, paramIdProveedor, paramMarca, paramPrecio)
                .ToListAsync();

            return responseSP.FirstOrDefault();
        }

        public async Task<SP> UpdateProductoAsync(ProductoDTO producto)
        {
            var paramIdProducto = new SqlParameter("@IdProducto", producto.IdProducto);
            var paramNombre = new SqlParameter("@Nombre", producto.Nombre);
            var paramIdProveedor = new SqlParameter("@IdProveedor", producto.IdProveedor);
            var paramMarca = new SqlParameter("@Marca", producto.Marca);
            var paramPrecio = new SqlParameter("@Precio", producto.Precio);

            var responseSP = await Context.Set<SP>()
                .FromSqlRaw("EXECUTE [dbo].[ActualizarProducto] @IdProducto, @Nombre, @IdProveedor, @Marca, @Precio",
                            paramIdProducto, paramNombre, paramIdProveedor, paramMarca, paramPrecio)
                .ToListAsync();

            return responseSP.FirstOrDefault();
        }

        public async Task<SP> DeleteProductoAsync(int id)
        {
            var paramIdProducto = new SqlParameter("@IdProducto", id);

            var responseSP = await Context.Set<SP>()
                .FromSqlRaw("EXECUTE [dbo].[EliminarProducto] @IdProducto", paramIdProducto)
                .ToListAsync();

            return responseSP.FirstOrDefault();
        }
    }
}
