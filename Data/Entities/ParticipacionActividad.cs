namespace SistemaComunidad.Data.Entities;

/// <summary>
/// Representa la participación de una persona en una actividad
/// </summary>
public class ParticipacionActividad : BaseEntity
{
    public int ActividadId { get; set; }
    public Actividad Actividad { get; set; } = null!;
    
    public int PersonaId { get; set; }
    public Persona Persona { get; set; } = null!;
    
    public bool Asistio { get; set; } = true;
    public string? Notas { get; set; }
}
