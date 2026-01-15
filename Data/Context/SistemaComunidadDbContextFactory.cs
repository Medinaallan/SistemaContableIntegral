using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SistemaComunidad.Data.Context;

public class SistemaComunidadDbContextFactory : IDesignTimeDbContextFactory<SistemaComunidadDbContext>
{
    public SistemaComunidadDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<SistemaComunidadDbContext>();
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=SistemaComunidad;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True");

        return new SistemaComunidadDbContext(optionsBuilder.Options);
    }
}
