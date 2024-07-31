using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.DTOs;
using WebApi.Interfaces;
using WebApi.Models;
using WebApi.Data;

namespace WebApi.Repositories
{
    public class InventarioRepository : BaseRepository<Inventario>, IInventarioRepository
    {
        public InventarioRepository(DbpruebaContext context) : base(context)
        {
        }

        public async Task<List<Inventario>> GetInventarioAsync()
        {
            return await Context.Inventarios.ToListAsync();
        }

        public async Task<Inventario> GetInventarioID(int id)
        {
            return await Context.Inventarios.FirstOrDefaultAsync(x => x.IdMovimiento == id);
        }

        public async Task<SP> CreateInventario(InventarioDTO resource)
        {
            var paramTipoMov = new SqlParameter("@TipoMovimiento", resource.TipoMovimiento);
            var paramidprod = new SqlParameter("@IdProducto", resource.IdProducto);
            var paramcant = new SqlParameter("@Cantidad", resource.Cantidad);
            var paramprecio = new SqlParameter("@Precio", resource.Precio);
            var paramFecha = new SqlParameter("@FechaMovimiento", resource.FechaMovimiento);
            var paramFechaCad = new SqlParameter("@FechaCaducidad", resource.FechaCaducidad);

            var responseSP = await Context.Set<SP>()
                .FromSqlRaw($"EXECUTE [dbo].[AgregarMovimientoInventario] @TipoMovimiento, @IdProducto, @Cantidad, @Precio, @FechaMovimiento, @FechaCaducidad",
                            paramTipoMov, paramidprod, paramcant, paramprecio, paramFecha, paramFechaCad)
                .ToListAsync();

            return responseSP.FirstOrDefault();
        }

        public async Task<SP> UpdateInventario(InventarioDTO resource)
        {
            var paramid = new SqlParameter("@IdMovimiento", resource.IdMovimiento);
            var paramTipoMov = new SqlParameter("@TipoMovimiento", resource.TipoMovimiento);
            var paramidprod = new SqlParameter("@IdProducto", resource.IdProducto);
            var paramcant = new SqlParameter("@Cantidad", resource.Cantidad);
            var paramprecio = new SqlParameter("@Precio", resource.Precio);
            var paramFecha = new SqlParameter("@FechaMovimiento", resource.FechaMovimiento);
            var paramFechaCad = new SqlParameter("@FechaCaducidad", resource.FechaCaducidad);

            var responseSP = await Context.Set<SP>()
                .FromSqlRaw($"EXECUTE [dbo].[ActualizarMovimientoInventario] @IdMovimiento, @TipoMovimiento, @IdProducto, @Cantidad, @Precio, @FechaMovimiento, @FechaCaducidad",
                            paramid, paramTipoMov, paramidprod, paramcant, paramprecio, paramFecha, paramFechaCad)
                .ToListAsync();

            return responseSP.FirstOrDefault();
        }

        public async Task<SP> DeleteInventario(int id)
        {
            var paramid = new SqlParameter("@IdMovimiento", id);

            var responseSP = await Context.Set<SP>()
                .FromSqlRaw($"EXECUTE [dbo].[EliminarMovimientoInventario] @IdMovimiento", paramid)
                .ToListAsync();

            return responseSP.FirstOrDefault();
        }

        public async Task<List<Inventario>> GetInventariosByProductIdAsync(int productId)
        {
            return await Context.Inventarios
                .Where(i => i.IdProducto == productId)
                .ToListAsync();
        }
    }
}
