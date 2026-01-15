using System.Collections.Generic;

namespace SistemaComunidad.Data.Entities;

/// <summary>
/// Representa un núcleo familiar
/// </summary>
public class NucleoFamiliar : BaseEntity
{
    public string Nombre { get; set; } = string.Empty;
    public string? Direccion { get; set; }
    public string? Telefono { get; set; }
    public string? Notas { get; set; }
    
    // Relaciones
    public ICollection<Persona> Miembros { get; set; } = new List<Persona>();
}
