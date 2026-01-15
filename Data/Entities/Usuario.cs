using System;

namespace SistemaComunidad.Data.Entities;

/// <summary>
/// Usuario del sistema con control de acceso
/// </summary>
public class Usuario : BaseEntity
{
    public string NombreUsuario { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public string? Email { get; set; }
    public RolUsuario Rol { get; set; } = RolUsuario.Usuario;
    public DateTime? UltimoAcceso { get; set; }
    public bool CambiarPassword { get; set; } = false;
}

public enum RolUsuario
{
    Administrador,
    Tesorero,
    Secretario,
    Usuario
}
