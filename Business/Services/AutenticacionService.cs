using System;
using System.Threading.Tasks;
using SistemaComunidad.Business.Interfaces;
using SistemaComunidad.Data.Entities;
using SistemaComunidad.Data.Interfaces;

namespace SistemaComunidad.Business.Services;

public class AutenticacionService : ServicioBase, IAutenticacionService
{
    public AutenticacionService(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task<Usuario?> IniciarSesionAsync(string nombreUsuario, string password)
    {
        Console.WriteLine($"[DEBUG] Intentando login para: {nombreUsuario}");
        
        var usuario = await UnitOfWork.Usuarios.ObtenerPorNombreUsuarioAsync(nombreUsuario);
        
        if (usuario == null)
        {
            Console.WriteLine("[DEBUG] Usuario no encontrado");
            return null;
        }
        
        if (!usuario.Activo)
        {
            Console.WriteLine("[DEBUG] Usuario inactivo");
            return null;
        }
        
        Console.WriteLine($"[DEBUG] Usuario encontrado. Hash: {usuario.PasswordHash?.Substring(0, 20)}...");
        Console.WriteLine($"[DEBUG] Password proporcionado: {password}");

        var esValido = await UnitOfWork.Usuarios.ValidarCredencialesAsync(nombreUsuario, password);
        
        Console.WriteLine($"[DEBUG] Validación resultado: {esValido}");
        
        if (!esValido)
            return null;

        // Actualizar último acceso
        usuario.UltimoAcceso = DateTime.Now;
        await UnitOfWork.Usuarios.ActualizarAsync(usuario);
        await UnitOfWork.CompletarAsync();

        await RegistrarAccesoAsync(usuario.Id, usuario.NombreUsuario);

        return usuario;
    }

    public async Task<bool> CambiarPasswordAsync(int usuarioId, string passwordAntiguo, string passwordNuevo)
    {
        var resultado = await UnitOfWork.Usuarios.CambiarPasswordAsync(usuarioId, passwordAntiguo, passwordNuevo);
        
        if (resultado)
        {
            await UnitOfWork.CompletarAsync();
            
            var usuario = await UnitOfWork.Usuarios.ObtenerPorIdAsync(usuarioId);
            if (usuario != null)
            {
                await RegistrarAuditoriaAsync(usuarioId, usuario.NombreUsuario, "Cambio de contraseña", "Usuario");
            }
        }

        return resultado;
    }

    public async Task RegistrarAccesoAsync(int usuarioId, string nombreUsuario)
    {
        var auditoria = new AuditoriaAccion
        {
            UsuarioId = usuarioId,
            NombreUsuario = nombreUsuario,
            Accion = "Inicio de sesión",
            Descripcion = "Usuario inició sesión en el sistema",
            FechaAccion = DateTime.Now
        };

        await UnitOfWork.Auditorias.AgregarAsync(auditoria);
        await UnitOfWork.CompletarAsync();
    }

    private async Task RegistrarAuditoriaAsync(int usuarioId, string nombreUsuario, string accion, string entidad)
    {
        var auditoria = new AuditoriaAccion
        {
            UsuarioId = usuarioId,
            NombreUsuario = nombreUsuario,
            Accion = accion,
            Entidad = entidad,
            FechaAccion = DateTime.Now
        };

        await UnitOfWork.Auditorias.AgregarAsync(auditoria);
        await UnitOfWork.CompletarAsync();
    }
}
