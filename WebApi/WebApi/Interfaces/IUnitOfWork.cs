namespace WebApi.Interfaces
{
    public interface IUnitOfWork
    {
        // SE AGREGAN LAS INTERFACES ACA, EJEMPLO IPROVEDORESREPOSITORY Y SE LES ASIGNA UN NOMBRE COMO EST AHI
        IAccesoRepository Acceso { get; }
        IInventarioRepository Inventario { get; }
        // IParametroRepository Parametro { get; }
        IProductoRepository Producto { get; } 
        IProveedorRepository Proveedor { get; } 
        //IUsuarioRepository Usuario { get; } 

        Task<int> SaveChangesAsync();

    }
}
