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
    public class ProveedorRepository : BaseRepository<Proveedore>, IProveedorRepository
    {
        public ProveedorRepository(DbpruebaContext context) : base(context)
        {
        }

        public async Task<List<Proveedore>> GetProveedoresAsync()
        {
            return await Context.Proveedores.ToListAsync();
        }

        public async Task<Proveedore?> GetProveedorByIdAsync(int id)
        {
            return await Context.Proveedores.FirstOrDefaultAsync(p => p.IdProveedor == id);
        }

        public async Task<Proveedore?> GetProveedorByNombreAsync(string nombre)
        {
            return await Context.Proveedores.FirstOrDefaultAsync(p => p.Nombre == nombre);
        }


        public async Task<SP> CreateProveedorAsync(ProveedoreDTO proveedor)
        {
            var paramNombre = new SqlParameter("@Nombre", proveedor.Nombre);
            var paramTelefono = new SqlParameter("@TelefonoContacto", proveedor.TelefonoContacto ?? (object)DBNull.Value);
            var paramCorreo = new SqlParameter("@Correo", proveedor.Correo ?? (object)DBNull.Value);
            var paramDireccion = new SqlParameter("@Direccion", proveedor.Direccion ?? (object)DBNull.Value);

            var responseSP = await Context.Set<SP>()
                .FromSqlRaw("EXECUTE [dbo].[AgregarProveedor] @Nombre, @TelefonoContacto, @Correo, @Direccion",
                            paramNombre, paramTelefono, paramCorreo, paramDireccion)
                .ToListAsync();

            return responseSP.FirstOrDefault();
        }

        public async Task<SP> UpdateProveedorAsync(ProveedoreDTO proveedor)
        {
            var paramIdProveedor = new SqlParameter("@IdProveedor", proveedor.IdProveedor);
            var paramNombre = new SqlParameter("@Nombre", proveedor.Nombre);
            var paramTelefono = new SqlParameter("@TelefonoContacto", proveedor.TelefonoContacto ?? (object)DBNull.Value);
            var paramCorreo = new SqlParameter("@Correo", proveedor.Correo ?? (object)DBNull.Value);
            var paramDireccion = new SqlParameter("@Direccion", proveedor.Direccion ?? (object)DBNull.Value);

            var responseSP = await Context.Set<SP>()
                .FromSqlRaw("EXECUTE [dbo].[ActualizarProveedor] @IdProveedor, @Nombre, @TelefonoContacto, @Correo, @Direccion",
                            paramIdProveedor, paramNombre, paramTelefono, paramCorreo, paramDireccion)
                .ToListAsync();

            return responseSP.FirstOrDefault();
        }

        public async Task<SP> DeleteProveedorAsync(int id)
        {
            var paramIdProveedor = new SqlParameter("@IdProveedor", id);

            var responseSP = await Context.Set<SP>()
                .FromSqlRaw("EXECUTE [dbo].[EliminarProveedor] @IdProveedor", paramIdProveedor)
                .ToListAsync();

            return responseSP.FirstOrDefault();
        }
    }
}
