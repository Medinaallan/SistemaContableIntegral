using System;

namespace SistemaComunidad.Data.Entities;

/// <summary>
/// Clase base para todas las entidades del sistema
/// </summary>
public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime FechaCreacion { get; set; } = DateTime.Now;
    public DateTime? FechaModificacion { get; set; }
    public bool Activo { get; set; } = true;
}
