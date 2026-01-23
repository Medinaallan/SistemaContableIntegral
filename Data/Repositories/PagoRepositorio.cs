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
/// Repositorio para gestionar pagos
/// </summary>
public class PagoRepositorio : Repositorio<Pago>, IPagoRepositorio
{
    public PagoRepositorio(SistemaComunidadDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Pago>> ObtenerPagosPorPersonaAsync(int personaId)
    {
        return await _context.Set<Pago>()
            .Include(p => p.Cobro)
                .ThenInclude(c => c.Detalles)
            .Include(p => p.Usuario)
            .Where(p => p.PersonaId == personaId && p.Activo)
            .OrderByDescending(p => p.FechaPago)
            .ToListAsync();
    }

    public async Task<IEnumerable<Pago>> ObtenerPagosPorCobroAsync(int cobroId)
    {
        return await _context.Set<Pago>()
            .Include(p => p.Usuario)
            .Where(p => p.CobroId == cobroId && p.Activo)
            .OrderBy(p => p.FechaPago)
            .ToListAsync();
    }

    public async Task<Pago?> ObtenerPorNumeroReciboAsync(string numeroRecibo)
    {
        return await _context.Set<Pago>()
            .Include(p => p.Cobro)
                .ThenInclude(c => c.Detalles)
            .Include(p => p.Persona)
            .Include(p => p.Usuario)
            .FirstOrDefaultAsync(p => p.NumeroReciboPago == numeroRecibo && p.Activo);
    }

    public async Task<IEnumerable<Pago>> ObtenerPagosPorFechaAsync(DateTimeOffset fechaInicio, DateTimeOffset fechaFin)
    {
        return await _context.Set<Pago>()
            .Include(p => p.Persona)
            .Include(p => p.Cobro)
            .Include(p => p.Usuario)
            .Where(p => p.FechaPago >= fechaInicio && p.FechaPago <= fechaFin && p.Activo)
            .OrderBy(p => p.FechaPago)
            .ToListAsync();
    }

    public async Task<int> ObtenerUltimoNumeroReciboPagoAsync()
    {
        var ultimoPago = await _context.Set<Pago>()
            .OrderByDescending(p => p.Id)
            .FirstOrDefaultAsync();

        if (ultimoPago == null) return 0;

        var partes = ultimoPago.NumeroReciboPago.Split('-');
        if (partes.Length >= 1)
        {
            var ultimaParte = partes[partes.Length - 1];
            if (int.TryParse(ultimaParte, out int numero))
            {
                return numero;
            }
        }

        return 0;
    }

    public async Task<decimal> ObtenerTotalPagadoPorPersonaEnPeriodoAsync(int personaId, int periodo)
    {
        var total = await _context.Set<Pago>()
            .Where(p => p.PersonaId == personaId && 
                       p.Cobro.Periodo == periodo && 
                       p.Activo)
            .SumAsync(p => p.Monto);

        return total;
    }
}
