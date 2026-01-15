using System;
using System.Collections.Generic;

namespace SistemaComunidad.Data.Entities;

/// <summary>
/// Representa una actividad o reunión comunitaria
/// </summary>
public class Actividad : BaseEntity
{
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public string? Lugar { get; set; }
    public string? Responsable { get; set; }
    public TipoActividad TipoActividad { get; set; }
    public string? Observaciones { get; set; }
    
    // Relaciones
    public ICollection<ParticipacionActividad> Participaciones { get; set; } = new List<ParticipacionActividad>();
}

public enum TipoActividad
{
    Reunion,
    Culto,
    Evento,
    Capacitacion,
    Asamblea,
    Ayuda,
    Otro
}
