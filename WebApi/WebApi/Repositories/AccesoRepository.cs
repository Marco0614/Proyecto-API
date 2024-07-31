using WebApi.DTOs;
using WebApi.Interfaces;
using WebApi.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;


namespace WebApi.Repositories
{
    public class AccesoRepository : BaseRepository<Usuario>, IAccesoRepository
    {
        public AccesoRepository(DbpruebaContext context) : base(context)
        {
        }

        public async Task<Usuario?> GetUsuarioCorreo(string correo)
        {
            return await Context.Usuarios.FirstOrDefaultAsync(x => x.Correo == correo);
        }

        public async Task<SP> CreateUser(Usuario resource)
        {
            var paramNom = new SqlParameter("@Nombre", resource.Nombre);
            var paramCorreo = new SqlParameter("@Correo", resource.Correo);
            var paramClave = new SqlParameter("@Clave", resource.Clave);

            var responseSP = await Context.Set<SP>()
                .FromSql($"EXECUTE [dbo].[AgregarUsuario] {paramNom}, {paramCorreo}, {paramClave}").ToListAsync();

            return responseSP.FirstOrDefault();

        }
        public async Task<bool> DeleteUser(int idUsuario)
        {
            var paramIdUsuario = new SqlParameter("@IdUsuario", idUsuario);
            var result = await Context.Database.ExecuteSqlRawAsync("EXECUTE [dbo].[EliminarUsuario] @IdUsuario", paramIdUsuario);
            return result > 0;
        }

        public async Task<bool> UpdateUser(Usuario resource)
        {
            var paramIdUsuario = new SqlParameter("@IdUsuario", resource.IdUsuario);
            var paramNom = new SqlParameter("@Nombre", resource.Nombre);
            var paramCorreo = new SqlParameter("@Correo", resource.Correo);
            var paramClave = new SqlParameter("@Clave", resource.Clave);

            var result = await Context.Database.ExecuteSqlRawAsync("EXECUTE [dbo].[ActualizarUsuario] @IdUsuario, @Nombre, @Correo, @Clave", paramIdUsuario, paramNom, paramCorreo, paramClave);
            return result > 0;
        }
    }
}
