using System.Threading.Tasks;
using SistemaComunidad.Data.Entities;

namespace SistemaComunidad.Data.Interfaces;

public interface IUsuarioRepositorio : IRepositorio<Usuario>
{
    Task<Usuario?> ObtenerPorNombreUsuarioAsync(string nombreUsuario);
    Task<bool> ValidarCredencialesAsync(string nombreUsuario, string password);
    Task<bool> CambiarPasswordAsync(int usuarioId, string passwordAntiguo, string passwordNuevo);
}
