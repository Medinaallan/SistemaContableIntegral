using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SistemaComunidad.Data.Context;
using SistemaComunidad.Data.Entities;
using SistemaComunidad.Data.Interfaces;

namespace SistemaComunidad.Data.Repositories;

public class PersonaRepositorio : Repositorio<Persona>, IPersonaRepositorio
{
    public PersonaRepositorio(SistemaComunidadDbContext context) : base(context)
    {
    }

    public override async Task<Persona?> ObtenerPorIdAsync(int id)
    {
        return await _dbSet
            .Include(p => p.NucleoFamiliar)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public override async Task<IEnumerable<Persona>> ObtenerTodosAsync()
    {
        return await _dbSet
            .Include(p => p.NucleoFamiliar)
            .OrderBy(p => p.Apellidos)
            .ThenBy(p => p.Nombres)
            .ToListAsync();
    }

    public async Task<IEnumerable<Persona>> ObtenerPorNucleoFamiliarAsync(int nucleoFamiliarId)
    {
        return await _dbSet
            .Where(p => p.NucleoFamiliarId == nucleoFamiliarId)
            .OrderBy(p => p.Apellidos)
            .ThenBy(p => p.Nombres)
            .ToListAsync();
    }

    public async Task<IEnumerable<Persona>> BuscarPorNombreAsync(string nombre)
    {
        var nombreBusqueda = nombre.ToLower();
        return await _dbSet
            .Include(p => p.NucleoFamiliar)
            .Where(p => (p.Nombres.ToLower() + " " + p.Apellidos.ToLower()).Contains(nombreBusqueda))
            .OrderBy(p => p.Apellidos)
            .ThenBy(p => p.Nombres)
            .ToListAsync();
    }

    public async Task<Persona?> ObtenerPorIdentidadAsync(string identidad)
    {
        return await _dbSet
            .Include(p => p.NucleoFamiliar)
            .FirstOrDefaultAsync(p => p.IdentidadNacional == identidad);
    }

    public async Task<IEnumerable<Persona>> ObtenerActivosAsync()
    {
        return await _dbSet
            .Include(p => p.NucleoFamiliar)
            .Where(p => p.Activo && p.EstadoParticipacion == EstadoParticipacion.Activo)
            .OrderBy(p => p.Apellidos)
            .ThenBy(p => p.Nombres)
            .ToListAsync();
    }
}
