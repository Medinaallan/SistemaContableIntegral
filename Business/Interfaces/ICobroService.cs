using SistemaComunidad.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaComunidad.Business.Interfaces;

/// <summary>
/// Interfaz para el servicio de gestión de cobros y pagos
/// </summary>
public interface ICobroService
{
    // === GENERACIÓN DE COBROS ===
    
    /// <summary>
    /// Genera cobros automáticos para un período específico
    /// </summary>
    Task<IEnumerable<Cobro>> GenerarCobrosMensualesAsync(int periodo);
    
    /// <summary>
    /// Genera un cobro para una persona específica
    /// </summary>
    Task<Cobro> GenerarCobroParaPersonaAsync(int personaId, int periodo);
    
    // === CONSULTAS DE COBROS ===
    
    /// <summary>
    /// Obtiene todos los cobros de una persona
    /// </summary>
    Task<IEnumerable<Cobro>> ObtenerCobrosPorPersonaAsync(int personaId);
    
    /// <summary>
    /// Obtiene cobros pendientes de una persona
    /// </summary>
    Task<IEnumerable<Cobro>> ObtenerCobrosPendientesAsync(int personaId);
    
    /// <summary>
    /// Obtiene un cobro por ID
    /// </summary>
    Task<Cobro?> ObtenerCobroPorIdAsync(int cobroId);
    
    /// <summary>
    /// Obtiene un cobro por número de recibo
    /// </summary>
    Task<Cobro?> ObtenerCobroPorNumeroReciboAsync(string numeroRecibo);
    
    /// <summary>
    /// Obtiene cobros vencidos
    /// </summary>
    Task<IEnumerable<Cobro>> ObtenerCobrosVencidosAsync();
    
    // === REGISTRO DE PAGOS ===
    
    /// <summary>
    /// Registra un pago para un cobro
    /// </summary>
    Task<Pago> RegistrarPagoAsync(int cobroId, decimal monto, MetodoPago metodoPago, int? usuarioId = null, string? observaciones = null);
    
    /// <summary>
    /// Obtiene el historial de pagos de una persona
    /// </summary>
    Task<IEnumerable<Pago>> ObtenerHistorialPagosAsync(int personaId);
    
    /// <summary>
    /// Obtiene un pago por ID
    /// </summary>
    Task<Pago?> ObtenerPagoPorIdAsync(int pagoId);
    
    // === REPORTES Y ESTADÍSTICAS ===
    
    /// <summary>
    /// Obtiene el total pendiente de cobro de una persona
    /// </summary>
    Task<decimal> ObtenerTotalPendienteAsync(int personaId);
    
    /// <summary>
    /// Obtiene el total cobrado en un período
    /// </summary>
    Task<decimal> ObtenerTotalCobradoEnPeriodoAsync(int periodo);
    
    /// <summary>
    /// Marca un recibo como impreso
    /// </summary>
    Task MarcarReciboImpresAsync(int pagoId);
}
