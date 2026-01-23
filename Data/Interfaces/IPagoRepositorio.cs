using SistemaComunidad.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaComunidad.Data.Interfaces;

/// <summary>
/// Interfaz para el repositorio de Pagos
/// </summary>
public interface IPagoRepositorio : IRepositorio<Pago>
{
    /// <summary>
    /// Obtiene todos los pagos de una persona
    /// </summary>
    Task<IEnumerable<Pago>> ObtenerPagosPorPersonaAsync(int personaId);

    /// <summary>
    /// Obtiene los pagos aplicados a un cobro específico
    /// </summary>
    Task<IEnumerable<Pago>> ObtenerPagosPorCobroAsync(int cobroId);

    /// <summary>
    /// Obtiene un pago por número de recibo
    /// </summary>
    Task<Pago?> ObtenerPorNumeroReciboAsync(string numeroRecibo);

    /// <summary>
    /// Obtiene pagos realizados en un rango de fechas
    /// </summary>
    Task<IEnumerable<Pago>> ObtenerPagosPorFechaAsync(DateTimeOffset fechaInicio, DateTimeOffset fechaFin);

    /// <summary>
    /// Obtiene el último número de recibo de pago
    /// </summary>
    Task<int> ObtenerUltimoNumeroReciboPagoAsync();

    /// <summary>
    /// Obtiene el total de pagos de una persona en un período
    /// </summary>
    Task<decimal> ObtenerTotalPagadoPorPersonaEnPeriodoAsync(int personaId, int periodo);
}
