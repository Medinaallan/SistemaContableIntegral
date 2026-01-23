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
/// Repositorio para gestionar cobros
/// </summary>
public class CobroRepositorio : Repositorio<Cobro>, ICobroRepositorio
{
    public CobroRepositorio(SistemaComunidadDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Cobro>> ObtenerCobrosPorPersonaAsync(int personaId)
    {
        return await _context.Set<Cobro>()
            .Include(c => c.Persona)
            .Include(c => c.Detalles)
                .ThenInclude(d => d.Servicio)
            .Include(c => c.Pagos)
            .Where(c => c.PersonaId == personaId && c.Activo)
            .OrderByDescending(c => c.Periodo)
            .ToListAsync();
    }

    public async Task<IEnumerable<Cobro>> ObtenerCobrosPendientesPorPersonaAsync(int personaId)
    {
        return await _context.Set<Cobro>()
            .Include(c => c.Persona)
            .Include(c => c.Detalles)
                .ThenInclude(d => d.Servicio)
            .Include(c => c.Pagos)
            .Where(c => c.PersonaId == personaId && 
                       c.Activo && 
                       c.Estado != EstadoCobro.Pagado && 
                       c.Estado != EstadoCobro.Cancelado)
            .OrderBy(c => c.FechaLimitePago)
            .ToListAsync();
    }

    public async Task<IEnumerable<Cobro>> ObtenerCobrosPorPeriodoAsync(int periodo)
    {
        return await _context.Set<Cobro>()
            .Include(c => c.Persona)
            .Include(c => c.Detalles)
                .ThenInclude(d => d.Servicio)
            .Include(c => c.Pagos)
            .Where(c => c.Periodo == periodo && c.Activo)
            .OrderBy(c => c.Persona.Apellidos)
            .ThenBy(c => c.Persona.Nombres)
            .ToListAsync();
    }

    public async Task<Cobro?> ObtenerPorNumeroReciboAsync(string numeroRecibo)
    {
        return await _context.Set<Cobro>()
            .Include(c => c.Persona)
            .Include(c => c.Detalles)
                .ThenInclude(d => d.Servicio)
            .Include(c => c.Pagos)
            .FirstOrDefaultAsync(c => c.NumeroRecibo == numeroRecibo && c.Activo);
    }

    public async Task<IEnumerable<Cobro>> ObtenerCobrosVencidosAsync()
    {
        var ahora = DateTimeOffset.Now;
        return await _context.Set<Cobro>()
            .Include(c => c.Persona)
            .Include(c => c.Detalles)
                .ThenInclude(d => d.Servicio)
            .Where(c => c.FechaLimitePago < ahora && 
                       c.Estado != EstadoCobro.Pagado && 
                       c.Estado != EstadoCobro.Cancelado &&
                       c.Activo)
            .OrderBy(c => c.FechaLimitePago)
            .ToListAsync();
    }

    public async Task<bool> ExisteCobroEnPeriodoAsync(int personaId, int periodo)
    {
        return await _context.Set<Cobro>()
            .AnyAsync(c => c.PersonaId == personaId && c.Periodo == periodo && c.Activo);
    }

    public async Task<int> ObtenerUltimoNumeroReciboAsync(int periodo)
    {
        var prefijo = periodo.ToString();
        var ultimoCobro = await _context.Set<Cobro>()
            .Where(c => c.NumeroRecibo.StartsWith(prefijo))
            .OrderByDescending(c => c.NumeroRecibo)
            .FirstOrDefaultAsync();

        if (ultimoCobro == null) return 0;

        var partes = ultimoCobro.NumeroRecibo.Split('-');
        if (partes.Length == 2 && int.TryParse(partes[1], out int numero))
        {
            return numero;
        }

        return 0;
    }
}
