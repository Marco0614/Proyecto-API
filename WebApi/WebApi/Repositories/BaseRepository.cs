using Microsoft.EntityFrameworkCore;
using WebApi.Interfaces;
using WebApi.Models;
using WebApi.Data;

namespace WebApi.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected DbpruebaContext Context { get; set; }

        protected BaseRepository(DbpruebaContext context)
        {
            Context = context;
        }  
        
        public IQueryable<TEntity> GetAll() => Context.Set<TEntity>().AsNoTracking();

        public async Task<TEntity?> GetByCorreo(String correo) => await Context.Set<TEntity>().FindAsync(correo);

        public async Task<TEntity?> GetById(int id) => await Context.Set<TEntity>().FindAsync(id);

        public void Create(TEntity entity) => Context.Set<TEntity>().Add(entity);

     
        public void Update(TEntity entity) => Context.Set<TEntity>().Update(entity);

       
        public void Delete(TEntity entity) => Context.Set<TEntity>().Remove(entity);
    }
}
