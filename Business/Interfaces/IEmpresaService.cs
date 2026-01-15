using SistemaComunidad.Data.Entities;
using System.Threading.Tasks;

namespace SistemaComunidad.Business.Interfaces;

/// <summary>
/// Interfaz del servicio de negocio para la entidad Empresa
/// </summary>
public interface IEmpresaService : IServicioBase
{
    /// <summary>
    /// Obtiene los datos de la empresa
    /// </summary>
    Task<Empresa?> ObtenerDatosEmpresaAsync();
    
    /// <summary>
    /// Crea el registro inicial de la empresa
    /// </summary>
    Task<Empresa> CrearEmpresaAsync(Empresa empresa);
    
    /// <summary>
    /// Actualiza los datos de la empresa
    /// </summary>
    Task<Empresa> ActualizarEmpresaAsync(Empresa empresa);
    
    /// <summary>
    /// Verifica si ya existe un registro de empresa
    /// </summary>
    Task<bool> ExisteRegistroEmpresaAsync();
}
