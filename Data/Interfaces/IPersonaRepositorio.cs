using System.Collections.Generic;
using System.Threading.Tasks;
using SistemaComunidad.Data.Entities;

namespace SistemaComunidad.Data.Interfaces;

public interface IPersonaRepositorio : IRepositorio<Persona>
{
    Task<IEnumerable<Persona>> ObtenerPorNucleoFamiliarAsync(int nucleoFamiliarId);
    Task<IEnumerable<Persona>> BuscarPorNombreAsync(string nombre);
    Task<Persona?> ObtenerPorIdentidadAsync(string identidad);
    Task<IEnumerable<Persona>> ObtenerActivosAsync();
}
