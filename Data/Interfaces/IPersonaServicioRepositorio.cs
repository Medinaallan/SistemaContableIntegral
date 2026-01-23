using SistemaComunidad.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaComunidad.Data.Interfaces;

/// <summary>
/// Interfaz para el repositorio de PersonaServicio
/// </summary>
public interface IPersonaServicioRepositorio : IRepositorio<PersonaServicio>
{
    /// <summary>
    /// Obtiene todos los servicios asignados a una persona
    /// </summary>
    Task<IEnumerable<PersonaServicio>> ObtenerServiciosPorPersonaAsync(int personaId);

    /// <summary>
    /// Obtiene todos los servicios activos de una persona
    /// </summary>
    Task<IEnumerable<PersonaServicio>> ObtenerServiciosActivosPorPersonaAsync(int personaId);

    /// <summary>
    /// Obtiene todas las personas que tienen asignado un servicio específico
    /// </summary>
    Task<IEnumerable<PersonaServicio>> ObtenerPersonasPorServicioAsync(int servicioId);

    /// <summary>
    /// Obtiene todas las personas activas con un servicio específico
    /// </summary>
    Task<IEnumerable<PersonaServicio>> ObtenerPersonasActivasPorServicioAsync(int servicioId);

    /// <summary>
    /// Verifica si una persona tiene asignado un servicio específico
    /// </summary>
    Task<bool> PersonaTieneServicioAsync(int personaId, int servicioId);

    /// <summary>
    /// Obtiene una asignación específica de servicio a persona
    /// </summary>
    Task<PersonaServicio?> ObtenerAsignacionAsync(int personaId, int servicioId);

    /// <summary>
    /// Obtiene todos los servicios que deben cobrarse en un período específico
    /// </summary>
    /// <param name="periodo">Período en formato YYYYMM (ejemplo: 202601)</param>
    Task<IEnumerable<PersonaServicio>> ObtenerServiciosPorCobrarEnPeriodoAsync(int periodo);

    /// <summary>
    /// Marca un servicio como cobrado en un período específico
    /// </summary>
    Task MarcarComoCobradoAsync(int personaServicioId, int periodo);
}
