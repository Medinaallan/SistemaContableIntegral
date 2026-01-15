using SistemaComunidad.Business.Interfaces;
using SistemaComunidad.Data.Interfaces;

namespace SistemaComunidad.Business.Services;

/// <summary>
/// Servicio base que proporciona acceso a Unit of Work
/// </summary>
public abstract class ServicioBase : IServicioBase
{
    public IUnitOfWork UnitOfWork { get; }

    protected ServicioBase(IUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }
}
