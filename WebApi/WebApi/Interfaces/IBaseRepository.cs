using System.Threading.Tasks;

namespace WebApi.Interfaces
{
    public interface IBaseRepository <TEntity>
    {
        IQueryable<TEntity> GetAll ();
        Task<TEntity?> GetByCorreo(string correo);
        Task<TEntity?> GetById (int id);

        void Create(TEntity entity);

       
        void Update(TEntity entity);

        
        void Delete(TEntity entity);
    }
}
