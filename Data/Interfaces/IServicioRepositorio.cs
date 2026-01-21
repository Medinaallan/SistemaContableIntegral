using SistemaComunidad.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaComunidad.Data.Interfaces;

/// <summary>
/// Interfaz para el repositorio de Servicios
/// </summary>
public interface IServicioRepositorio : IRepositorio<Servicio>
{
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
}
