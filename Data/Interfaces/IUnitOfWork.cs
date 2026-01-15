using System;
using System.Threading.Tasks;

namespace SistemaComunidad.Data.Interfaces;

/// <summary>
/// Unidad de trabajo para gestionar transacciones
/// </summary>
public interface IUnitOfWork : IDisposable
{
    IPersonaRepositorio Personas { get; }
    IUsuarioRepositorio Usuarios { get; }
    IRepositorio<SistemaComunidad.Data.Entities.NucleoFamiliar> NucleosFamiliares { get; }
    IRepositorio<SistemaComunidad.Data.Entities.Aporte> Aportes { get; }
    IRepositorio<SistemaComunidad.Data.Entities.Ingreso> Ingresos { get; }
    IRepositorio<SistemaComunidad.Data.Entities.Egreso> Egresos { get; }
    IRepositorio<SistemaComunidad.Data.Entities.Actividad> Actividades { get; }
    IRepositorio<SistemaComunidad.Data.Entities.Documento> Documentos { get; }
    IRepositorio<SistemaComunidad.Data.Entities.Bien> Bienes { get; }
    IRepositorio<SistemaComunidad.Data.Entities.AuditoriaAccion> Auditorias { get; }
    IEmpresaRepositorio Empresas { get; }

    Task<int> CompletarAsync();
    Task IniciarTransaccionAsync();
    Task ConfirmarTransaccionAsync();
    Task RevertirTransaccionAsync();
}
