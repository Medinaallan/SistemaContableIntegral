using SistemaComunidad.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaComunidad.Data.Interfaces;

/// <summary>
/// Interfaz para el repositorio de Cobros
/// </summary>
public interface ICobroRepositorio : IRepositorio<Cobro>
{
    /// <summary>
    /// Obtiene todos los cobros de una persona
    /// </summary>
    Task<IEnumerable<Cobro>> ObtenerCobrosPorPersonaAsync(int personaId);

    /// <summary>
    /// Obtiene cobros pendientes de una persona
    /// </summary>
    Task<IEnumerable<Cobro>> ObtenerCobrosPendientesPorPersonaAsync(int personaId);

    /// <summary>
    /// Obtiene cobros de un período específico
    /// </summary>
    Task<IEnumerable<Cobro>> ObtenerCobrosPorPeriodoAsync(int periodo);

    /// <summary>
    /// Obtiene un cobro por número de recibo
    /// </summary>
    Task<Cobro?> ObtenerPorNumeroReciboAsync(string numeroRecibo);

    /// <summary>
    /// Obtiene cobros vencidos
    /// </summary>
    Task<IEnumerable<Cobro>> ObtenerCobrosVencidosAsync();

    /// <summary>
    /// Verifica si existe un cobro para una persona en un período
    /// </summary>
    Task<bool> ExisteCobroEnPeriodoAsync(int personaId, int periodo);

    /// <summary>
    /// Obtiene el último número de recibo del período
    /// </summary>
    Task<int> ObtenerUltimoNumeroReciboAsync(int periodo);
}
