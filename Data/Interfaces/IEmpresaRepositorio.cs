using SistemaComunidad.Data.Entities;
using System.Threading.Tasks;

namespace SistemaComunidad.Data.Interfaces;

/// <summary>
/// Repositorio específico para la entidad Empresa
/// </summary>
public interface IEmpresaRepositorio : IRepositorio<Empresa>
{
    /// <summary>
    /// Obtiene los datos de la empresa (normalmente solo existe un registro)
    /// </summary>
    Task<Empresa?> ObtenerDatosEmpresaAsync();
    
    /// <summary>
    /// Verifica si ya existe un registro de empresa
    /// </summary>
    Task<bool> ExisteRegistroAsync();
}
