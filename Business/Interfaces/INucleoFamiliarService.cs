using SistemaComunidad.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaComunidad.Business.Interfaces;

public interface INucleoFamiliarService : IServicioBase
{
    Task<IEnumerable<NucleoFamiliar>> ObtenerTodosAsync();
    Task<NucleoFamiliar?> ObtenerPorIdAsync(int id);
    Task<IEnumerable<NucleoFamiliar>> BuscarAsync(string textoBusqueda);
    Task<NucleoFamiliar> CrearAsync(NucleoFamiliar nucleo);
    Task<NucleoFamiliar> ActualizarAsync(NucleoFamiliar nucleo);
    Task EliminarAsync(int id);
    Task<int> ObtenerConteoAsync();

    // Gestión de miembros
    Task<MiembroFamiliar> AgregarMiembroAsync(MiembroFamiliar miembro);
    Task<MiembroFamiliar> ActualizarMiembroAsync(MiembroFamiliar miembro);
    Task EliminarMiembroAsync(int miembroId);
    Task<IEnumerable<MiembroFamiliar>> ObtenerMiembrosPorFamiliaAsync(int nucleoFamiliarId);
    Task<MiembroFamiliar> VincularPersonaAFamiliaAsync(int nucleoFamiliarId, int personaId, RolFamiliar rol);
}
