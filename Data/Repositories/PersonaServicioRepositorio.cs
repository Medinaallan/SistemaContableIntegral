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
/// Repositorio para gestionar la relación entre Personas y Servicios
/// </summary>
public class PersonaServicioRepositorio : Repositorio<PersonaServicio>, IPersonaServicioRepositorio
{
    public PersonaServicioRepositorio(SistemaComunidadDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Obtiene todos los servicios asignados a una persona
    /// </summary>
    public async Task<IEnumerable<PersonaServicio>> ObtenerServiciosPorPersonaAsync(int personaId)
    {
        return await _context.Set<PersonaServicio>()
            .Include(ps => ps.Servicio)
                .ThenInclude(s => s.Empresa)
            .Include(ps => ps.Persona)
            .Where(ps => ps.PersonaId == personaId && ps.Activo)
            .OrderByDescending(ps => ps.EstaActivo)
            .ThenBy(ps => ps.Servicio.Nombre)
            .ToListAsync();
    }

    /// <summary>
    /// Obtiene todos los servicios activos de una persona
    /// </summary>
    public async Task<IEnumerable<PersonaServicio>> ObtenerServiciosActivosPorPersonaAsync(int personaId)
    {
        return await _context.Set<PersonaServicio>()
            .Include(ps => ps.Servicio)
                .ThenInclude(s => s.Empresa)
            .Include(ps => ps.Persona)
            .Where(ps => ps.PersonaId == personaId && ps.EstaActivo && ps.Activo)
            .OrderBy(ps => ps.Servicio.Nombre)
            .ToListAsync();
    }

    /// <summary>
    /// Obtiene todas las personas que tienen asignado un servicio específico
    /// </summary>
    public async Task<IEnumerable<PersonaServicio>> ObtenerPersonasPorServicioAsync(int servicioId)
    {
        return await _context.Set<PersonaServicio>()
            .Include(ps => ps.Persona)
                .ThenInclude(p => p.NucleoFamiliar)
            .Include(ps => ps.Servicio)
            .Where(ps => ps.ServicioId == servicioId && ps.Activo)
            .OrderByDescending(ps => ps.EstaActivo)
            .ThenBy(ps => ps.Persona.Apellidos)
            .ThenBy(ps => ps.Persona.Nombres)
            .ToListAsync();
    }

    /// <summary>
    /// Obtiene todas las personas activas con un servicio específico
    /// </summary>
    public async Task<IEnumerable<PersonaServicio>> ObtenerPersonasActivasPorServicioAsync(int servicioId)
    {
        return await _context.Set<PersonaServicio>()
            .Include(ps => ps.Persona)
                .ThenInclude(p => p.NucleoFamiliar)
            .Include(ps => ps.Servicio)
            .Where(ps => ps.ServicioId == servicioId && ps.EstaActivo && ps.Activo)
            .OrderBy(ps => ps.Persona.Apellidos)
            .ThenBy(ps => ps.Persona.Nombres)
            .ToListAsync();
    }

    /// <summary>
    /// Verifica si una persona tiene asignado un servicio específico
    /// </summary>
    public async Task<bool> PersonaTieneServicioAsync(int personaId, int servicioId)
    {
        return await _context.Set<PersonaServicio>()
            .AnyAsync(ps => ps.PersonaId == personaId && 
                           ps.ServicioId == servicioId && 
                           ps.EstaActivo && 
                           ps.Activo);
    }

    /// <summary>
    /// Obtiene una asignación específica de servicio a persona
    /// </summary>
    public async Task<PersonaServicio?> ObtenerAsignacionAsync(int personaId, int servicioId)
    {
        return await _context.Set<PersonaServicio>()
            .Include(ps => ps.Servicio)
                .ThenInclude(s => s.Empresa)
            .Include(ps => ps.Persona)
            .FirstOrDefaultAsync(ps => ps.PersonaId == personaId && 
                                      ps.ServicioId == servicioId && 
                                      ps.Activo);
    }

    /// <summary>
    /// Obtiene todos los servicios que deben cobrarse en un período específico
    /// </summary>
    public async Task<IEnumerable<PersonaServicio>> ObtenerServiciosPorCobrarEnPeriodoAsync(int periodo)
    {
        var anio = periodo / 100;
        var mes = periodo % 100;
        var fechaPeriodo = new DateTimeOffset(anio, mes, 1, 0, 0, 0, TimeSpan.Zero);

        return await _context.Set<PersonaServicio>()
            .Include(ps => ps.Servicio)
                .ThenInclude(s => s.Empresa)
            .Include(ps => ps.Persona)
            .Where(ps => ps.EstaActivo && 
                        ps.Activo &&
                        ps.FechaInicio <= fechaPeriodo &&
                        (ps.FechaFin == null || ps.FechaFin >= fechaPeriodo) &&
                        (ps.UltimoPeriodoCobrado == null || ps.UltimoPeriodoCobrado < periodo))
            .OrderBy(ps => ps.Persona.Apellidos)
            .ThenBy(ps => ps.Persona.Nombres)
            .ThenBy(ps => ps.Servicio.Nombre)
            .ToListAsync();
    }

    /// <summary>
    /// Marca un servicio como cobrado en un período específico
    /// </summary>
    public async Task MarcarComoCobradoAsync(int personaServicioId, int periodo)
    {
        var personaServicio = await _context.Set<PersonaServicio>()
            .FirstOrDefaultAsync(ps => ps.Id == personaServicioId);

        if (personaServicio != null)
        {
            personaServicio.UltimoPeriodoCobrado = periodo;
            await _context.SaveChangesAsync();
        }
    }
}
