using Microsoft.EntityFrameworkCore;
using WebApi.DTOs;
using WebApi.Models;

namespace WebApi.Data
{
   
    public partial class DbpruebaContext : DbContext
    {
        public DbpruebaContext()
        {
        }

        public DbpruebaContext(DbContextOptions<DbpruebaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Producto> Productos { get; set; }
        public virtual DbSet<Proveedore> Proveedores { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<Inventario> Inventarios { get; set; } // Agregar DbSet para Inventario
        public virtual DbSet<ErrorLog> ErrorLogs { get; set; }  // Agregar DbSet para ErrorLog

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(e => e.IdProducto).HasName("PK__Producto__09889210F88AE934");
                entity.ToTable("Producto");
                entity.Property(e => e.Marca).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.Nombre).HasMaxLength(256);
                entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.Productos)
                    .HasForeignKey(d => d.IdProveedor)
                    .HasConstraintName("FK__Producto__IdProv__3B75D760");
            });

            modelBuilder.Entity<Proveedore>(entity =>
            {
                entity.HasKey(e => e.IdProveedor).HasName("PK__Proveedo__E8B631AF977E9206");
                entity.Property(e => e.Correo).HasMaxLength(256);
                entity.Property(e => e.Direccion).HasMaxLength(500);
                entity.Property(e => e.Nombre).HasMaxLength(256);
                entity.Property(e => e.TelefonoContacto).HasMaxLength(50);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario).HasName("PK__Usuario__5B65BF97A501370B");
                entity.ToTable("Usuario");
                entity.Property(e => e.Clave).HasMaxLength(100).IsUnicode(false);
                entity.Property(e => e.Correo).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.Nombre).HasMaxLength(50).IsUnicode(false);
            });

            modelBuilder.Entity<Inventario>(entity =>
            {
                entity.HasKey(e => e.IdMovimiento).HasName("PK__Inventario__IdMovimiento");
                entity.ToTable("Inventario");
                entity.Property(e => e.TipoMovimiento).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.FechaMovimiento).HasColumnType("date");
                entity.Property(e => e.FechaCaducidad).HasColumnType("date");

                entity.HasOne(d => d.Producto)
                    .WithMany()
                    .HasForeignKey(d => d.IdProducto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Inventario__IdProducto");
            });

            modelBuilder.Entity<ErrorLog>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__ErrorLog__Id");
                entity.ToTable("ErrorLog");
                entity.Property(e => e.Controller).HasMaxLength(100);
                entity.Property(e => e.Action).HasMaxLength(100);
                entity.Property(e => e.Message).HasMaxLength(4000);
                entity.Property(e => e.StackTrace).HasMaxLength(4000);
                entity.Property(e => e.Timestamp).HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);

            modelBuilder.Entity<SP>(Entity =>
            {
                Entity.HasNoKey();
            });
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

