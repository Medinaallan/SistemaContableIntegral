using Microsoft.EntityFrameworkCore;
using SistemaComunidad.Data.Context;
using SistemaComunidad.Data.Entities;
using SistemaComunidad.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaComunidad.Data.Repositories;

/// <summary>
/// Repositorio para gestionar servicios del patronato
/// </summary>
public class ServicioRepositorio : Repositorio<Servicio>, IServicioRepositorio
{
    public ServicioRepositorio(SistemaComunidadDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Obtiene todos los servicios activos
    /// </summary>
    public async Task<IEnumerable<Servicio>> ObtenerServiciosActivosAsync()
    {
        return await _context.Set<Servicio>()
            .Include(s => s.Empresa)
            .Where(s => s.Activo)
            .OrderBy(s => s.Nombre)
            .ToListAsync();
    }

    /// <summary>
    /// Obtiene servicios por empresa/patronato
    /// </summary>
    public async Task<IEnumerable<Servicio>> ObtenerServiciosPorEmpresaAsync(int empresaId)
    {
        return await _context.Set<Servicio>()
            .Include(s => s.Empresa)
            .Where(s => s.EmpresaId == empresaId && s.Activo)
            .OrderBy(s => s.Nombre)
            .ToListAsync();
    }

    /// <summary>
    /// Obtiene servicios obligatorios
    /// </summary>
    public async Task<IEnumerable<Servicio>> ObtenerServiciosObligatoriosAsync()
    {
        return await _context.Set<Servicio>()
            .Include(s => s.Empresa)
            .Where(s => s.EsObligatorio && s.Activo)
            .OrderBy(s => s.Nombre)
            .ToListAsync();
    }
}
