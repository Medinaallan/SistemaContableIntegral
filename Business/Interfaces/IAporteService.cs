using SistemaComunidad.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaComunidad.Business.Interfaces;

public interface IAporteService
{
    Task<IEnumerable<Aporte>> ObtenerAportesPorPersonaAsync(int personaId);
    Task<Aporte?> ObtenerPorIdAsync(int id);
    Task<Aporte?> ObtenerPorNumeroReciboAsync(string numeroRecibo);
    Task<Aporte> RegistrarAporteAsync(Aporte aporte);
    Task<int> ObtenerUltimoNumeroReciboAsync();
}
