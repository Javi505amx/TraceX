using Microsoft.EntityFrameworkCore;
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
        public DbSet<Machine> Machines { get; set; }
    }

}
