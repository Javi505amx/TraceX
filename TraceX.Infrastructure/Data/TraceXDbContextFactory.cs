using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TraceX.Infrastructure.Data;

public class TraceXDbContextFactory : IDesignTimeDbContextFactory<TraceXDbContext>
{
    public TraceXDbContext CreateDbContext(string[] args)
    {
        // Ruta directa y absoluta (copiada de tu error)
        var basePath = @"C:\dev\learning\Tracex\TraceX\TraceX.Api";

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<TraceXDbContext>();
        var connectionString = configuration.GetConnectionString("TraceXDbConnection");

        optionsBuilder.UseSqlServer(connectionString);

        return new TraceXDbContext(optionsBuilder.Options);
    }
}