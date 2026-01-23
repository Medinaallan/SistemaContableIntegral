using SistemaComunidad.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaComunidad.Business.Interfaces;

/// <summary>
/// Interfaz para el servicio de gestión de servicios asignados a personas
/// </summary>
public interface IPersonaServicioService
{
    /// <summary>
    /// Asigna un servicio a una persona
    /// </summary>
    Task<PersonaServicio> AsignarServicioAPersonaAsync(int personaId, int servicioId, decimal? costoPersonalizado = null, string? notas = null);

    /// <summary>
    /// Desasigna un servicio de una persona (lo marca como inactivo)
    /// </summary>
    Task DesasignarServicioDePersonaAsync(int personaId, int servicioId);

    /// <summary>
    /// Reactiva un servicio para una persona
    /// </summary>
    Task ReactivarServicioAsync(int personaServicioId);

    /// <summary>
    /// Obtiene todos los servicios asignados a una persona
    /// </summary>
    Task<IEnumerable<PersonaServicio>> ObtenerServiciosPorPersonaAsync(int personaId);

    /// <summary>
    /// Obtiene todos los servicios activos de una persona
    /// </summary>
    Task<IEnumerable<PersonaServicio>> ObtenerServiciosActivosPorPersonaAsync(int personaId);

    /// <summary>
    /// Obtiene todas las personas que tienen un servicio específico
    /// </summary>
    Task<IEnumerable<PersonaServicio>> ObtenerPersonasPorServicioAsync(int servicioId);

    /// <summary>
    /// Actualiza el costo personalizado de un servicio para una persona
    /// </summary>
    Task ActualizarCostoPersonalizadoAsync(int personaServicioId, decimal? costoPersonalizado);

    /// <summary>
    /// Obtiene el total mensual que debe pagar una persona por todos sus servicios
    /// </summary>
    Task<decimal> ObtenerTotalMensualPorPersonaAsync(int personaId);

    /// <summary>
    /// Genera los cobros pendientes para un período específico
    /// </summary>
    /// <param name="periodo">Período en formato YYYYMM (ejemplo: 202601)</param>
    Task<IEnumerable<PersonaServicio>> GenerarCobrosPendientesPorPeriodoAsync(int periodo);

    /// <summary>
    /// Marca un servicio como cobrado en un período
    /// </summary>
    Task MarcarComoCobradoAsync(int personaServicioId, int periodo);

    /// <summary>
    /// Valida que se puede asignar un servicio a una persona
    /// </summary>
    Task<(bool EsValido, string Mensaje)> ValidarAsignacionAsync(int personaId, int servicioId);
}
