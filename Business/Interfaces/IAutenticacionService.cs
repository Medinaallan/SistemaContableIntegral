using System.Threading.Tasks;
using SistemaComunidad.Data.Entities;

namespace SistemaComunidad.Business.Interfaces;

public interface IAutenticacionService
{
    Task<Usuario?> IniciarSesionAsync(string nombreUsuario, string password);
    Task<bool> CambiarPasswordAsync(int usuarioId, string passwordAntiguo, string passwordNuevo);
    Task RegistrarAccesoAsync(int usuarioId, string nombreUsuario);
}
