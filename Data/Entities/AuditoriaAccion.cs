using System;

namespace SistemaComunidad.Data.Entities;

/// <summary>
/// Registro de auditoría de acciones del sistema
/// </summary>
public class AuditoriaAccion : BaseEntity
{
    public int UsuarioId { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public string Accion { get; set; } = string.Empty;
    public string? Entidad { get; set; }
    public int? EntidadId { get; set; }
    public string? Descripcion { get; set; }
    public DateTime FechaAccion { get; set; } = DateTime.Now;
    public string? DireccionIP { get; set; }
}
