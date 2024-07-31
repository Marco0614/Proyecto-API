using WebApi.Models;
using WebApi.DTOs;

namespace WebApi.Interfaces
{
    public interface IAccesoRepository : IBaseRepository<Usuario>
    {

        //Task<List<Usuario>> GetUsuario();
        Task<Usuario?> GetUsuarioCorreo(string correo);
        Task<SP> CreateUser(Usuario resource);
        Task<bool> DeleteUser(int idUsuario);
        Task<bool> UpdateUser(Usuario resource);

    }
}
