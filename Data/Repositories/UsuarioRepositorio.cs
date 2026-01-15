using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SistemaComunidad.Data.Context;
using SistemaComunidad.Data.Entities;
using SistemaComunidad.Data.Interfaces;

namespace SistemaComunidad.Data.Repositories;

public class UsuarioRepositorio : Repositorio<Usuario>, IUsuarioRepositorio
{
    public UsuarioRepositorio(SistemaComunidadDbContext context) : base(context)
    {
    }

    public async Task<Usuario?> ObtenerPorNombreUsuarioAsync(string nombreUsuario)
    {
        return await _dbSet
            .FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario && u.Activo);
    }

    public async Task<bool> ValidarCredencialesAsync(string nombreUsuario, string password)
    {
        var usuario = await ObtenerPorNombreUsuarioAsync(nombreUsuario);
        if (usuario == null) return false;

        // Aquí se implementaría la validación con BCrypt
        // Por ahora retornamos true para simplificar
        return BCrypt.Net.BCrypt.Verify(password, usuario.PasswordHash);
    }

    public async Task<bool> CambiarPasswordAsync(int usuarioId, string passwordAntiguo, string passwordNuevo)
    {
        var usuario = await ObtenerPorIdAsync(usuarioId);
        if (usuario == null) return false;

        // Verificar password antiguo
        if (!BCrypt.Net.BCrypt.Verify(passwordAntiguo, usuario.PasswordHash))
            return false;

        // Establecer nuevo password
        usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(passwordNuevo);
        usuario.CambiarPassword = false;
        usuario.FechaModificacion = DateTime.Now;
        
        await ActualizarAsync(usuario);
        return true;
    }
}
