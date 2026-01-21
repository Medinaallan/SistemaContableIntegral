using SistemaComunidad.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaComunidad.Business.Interfaces;

/// <summary>
/// Interfaz para el servicio de negocio de Servicios
/// </summary>
public interface IServicioService : IServicioBase
{
    /// <summary>
    /// Obtiene todos los servicios
    /// </summary>
    Task<IEnumerable<Servicio>> ObtenerTodosAsync();

    /// <summary>
    /// Obtiene un servicio por ID
    /// </summary>
    Task<Servicio?> ObtenerPorIdAsync(int id);

    /// <summary>
    /// Crea un nuevo servicio
    /// </summary>
    Task<Servicio> CrearAsync(Servicio servicio);

    /// <summary>
    /// Actualiza un servicio existente
    /// </summary>
    Task<Servicio> ActualizarAsync(Servicio servicio);

    /// <summary>
    /// Elimina un servicio
    /// </summary>
    Task EliminarAsync(int id);

    /// <summary>
    /// Activa un servicio
    /// </summary>
    Task ActivarAsync(int id);

    /// <summary>
    /// Desactiva un servicio
    /// </summary>
    Task DesactivarAsync(int id);

    /// <summary>
    /// Obtiene todos los servicios activos
    /// </summary>
    Task<IEnumerable<Servicio>> ObtenerServiciosActivosAsync();

    /// <summary>
    /// Obtiene servicios por empresa/patronato
    /// </summary>
    Task<IEnumerable<Servicio>> ObtenerServiciosPorEmpresaAsync(int empresaId);

    /// <summary>
    /// Obtiene servicios obligatorios
    /// </summary>
    Task<IEnumerable<Servicio>> ObtenerServiciosObligatoriosAsync();

    /// <summary>
    /// Valida que un servicio sea correcto antes de guardarlo
    /// </summary>
    Task<(bool EsValido, string Mensaje)> ValidarServicioAsync(Servicio servicio);
}
