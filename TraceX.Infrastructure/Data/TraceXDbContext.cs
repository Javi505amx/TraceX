using Microsoft.EntityFrameworkCore;
using TraceX.Domain.Common;
using TraceX.Domain.Entities;

namespace TraceX.Infrastructure.Data
{
    public class TraceXDbContext : DbContext
    {
        // Constructor que recibe opciones (como la cadena de conexion)
        public TraceXDbContext(DbContextOptions options) : base(options)
        {

        }

        // Aqui esta la magia! Esta propiedad representa la tabla en SQL.
        // EF Core la llamara "machines" en la base de datos automaticamente.
        public DbSet<Machine> Machines => Set<Machine>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<User> Users => Set<User>();
        public DbSet<WorkOrder> WorkOrders => Set<WorkOrder>();




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Filtro gobal para softDelete(ignora registros donde IsDeleted == True)
            modelBuilder.Entity<Machine>().HasQueryFilter(m => !m.IsDeleted);
            modelBuilder.Entity<Product>().HasQueryFilter(p => !p.IsDeleted);
            modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Snapshot con ToList() para no alterar la colección durante la iteración
            var entries = ChangeTracker.Entries<BaseEntity>().ToList();

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTimeOffset.UtcNow;
                        entry.Entity.IsActive = true;
                        entry.Entity.IsDeleted = false;
                        break;

                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTimeOffset.UtcNow;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged; // Cancela el DELETE
                        entry.Entity.IsDeleted = true;       // Marca IsDeleted = true
                        entry.Entity.DeletedAt = DateTimeOffset.UtcNow;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }

}
