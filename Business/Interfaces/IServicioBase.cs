using SistemaComunidad.Data.Interfaces;

namespace SistemaComunidad.Business.Interfaces;

/// <summary>
/// Servicio base con acceso a Unit of Work
/// </summary>
public interface IServicioBase
{
    IUnitOfWork UnitOfWork { get; }
}
