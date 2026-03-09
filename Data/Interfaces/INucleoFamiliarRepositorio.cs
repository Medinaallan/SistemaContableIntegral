using System.Collections.Generic;
using System.Threading.Tasks;
using SistemaComunidad.Data.Entities;

namespace SistemaComunidad.Data.Interfaces;

public interface INucleoFamiliarRepositorio : IRepositorio<NucleoFamiliar>
{
    Task<NucleoFamiliar?> ObtenerConMiembrosAsync(int id);
    Task<IEnumerable<NucleoFamiliar>> ObtenerTodosConMiembrosAsync();
    Task<IEnumerable<NucleoFamiliar>> BuscarPorNombreAsync(string nombre);
}
