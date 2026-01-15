using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SistemaComunidad.Data.Context;
using SistemaComunidad.Data.Entities;
using SistemaComunidad.Data.Interfaces;

namespace SistemaComunidad.Data.Repositories;

/// <summary>
/// Implementación del repositorio para la entidad Empresa
/// </summary>
public class EmpresaRepositorio : Repositorio<Empresa>, IEmpresaRepositorio
{
    public EmpresaRepositorio(SistemaComunidadDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Obtiene los datos de la empresa (el primer registro activo)
    /// </summary>
    public async Task<Empresa?> ObtenerDatosEmpresaAsync()
    {
        return await _dbSet
            .Where(e => e.Activo)
            .OrderBy(e => e.Id)
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Verifica si ya existe al menos un registro de empresa
    /// </summary>
    public async Task<bool> ExisteRegistroAsync()
    {
        return await _dbSet.AnyAsync(e => e.Activo);
    }
}
