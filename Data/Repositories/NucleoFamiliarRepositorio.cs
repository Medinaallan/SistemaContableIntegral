using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SistemaComunidad.Data.Context;
using SistemaComunidad.Data.Entities;
using SistemaComunidad.Data.Interfaces;

namespace SistemaComunidad.Data.Repositories;

public class NucleoFamiliarRepositorio : Repositorio<NucleoFamiliar>, INucleoFamiliarRepositorio
{
    public NucleoFamiliarRepositorio(SistemaComunidadDbContext context) : base(context)
    {
    }

    public override async Task<NucleoFamiliar?> ObtenerPorIdAsync(int id)
    {
        return await _dbSet
            .Include(n => n.MiembrosFamiliares.Where(m => m.Activo))
                .ThenInclude(m => m.Persona)
            .Include(n => n.Miembros.Where(p => p.Activo))
            .FirstOrDefaultAsync(n => n.Id == id);
    }

    public async Task<NucleoFamiliar?> ObtenerConMiembrosAsync(int id)
    {
        return await _dbSet
            .Include(n => n.MiembrosFamiliares.Where(m => m.Activo))
                .ThenInclude(m => m.Persona)
            .Include(n => n.Miembros.Where(p => p.Activo))
            .FirstOrDefaultAsync(n => n.Id == id);
    }

    public override async Task<IEnumerable<NucleoFamiliar>> ObtenerTodosAsync()
    {
        return await _dbSet
            .Include(n => n.MiembrosFamiliares.Where(m => m.Activo))
            .OrderBy(n => n.Nombre)
            .ToListAsync();
    }

    public async Task<IEnumerable<NucleoFamiliar>> ObtenerTodosConMiembrosAsync()
    {
        return await _dbSet
            .Include(n => n.MiembrosFamiliares.Where(m => m.Activo))
                .ThenInclude(m => m.Persona)
            .Include(n => n.Miembros.Where(p => p.Activo))
            .Where(n => n.Activo)
            .OrderBy(n => n.Nombre)
            .ToListAsync();
    }

    public async Task<IEnumerable<NucleoFamiliar>> BuscarPorNombreAsync(string nombre)
    {
        var nombreBusqueda = nombre.ToLower();
        return await _dbSet
            .Include(n => n.MiembrosFamiliares.Where(m => m.Activo))
            .Where(n => n.Activo && n.Nombre.ToLower().Contains(nombreBusqueda))
            .OrderBy(n => n.Nombre)
            .ToListAsync();
    }
}
