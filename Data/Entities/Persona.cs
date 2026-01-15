using System;
using System.Collections.Generic;

namespace SistemaComunidad.Data.Entities;

/// <summary>
/// Entidad que representa a una persona (miembro de la comunidad)
/// </summary>
public class Persona : BaseEntity
{
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string? IdentidadNacional { get; set; } // DNI/RTN en Honduras
    public DateTime? FechaNacimiento { get; set; }
    public string? Telefono { get; set; }
    public string? Email { get; set; }
    public string? Direccion { get; set; }
    public EstadoParticipacion EstadoParticipacion { get; set; } = EstadoParticipacion.Activo;
    public string? Notas { get; set; }
    
    // Relaciones
    public int? NucleoFamiliarId { get; set; }
    public NucleoFamiliar? NucleoFamiliar { get; set; }
    
    public ICollection<Aporte> Aportes { get; set; } = new List<Aporte>();
    public ICollection<ParticipacionActividad> Participaciones { get; set; } = new List<ParticipacionActividad>();
}

public enum EstadoParticipacion
{
    Activo,
    Inactivo,
    Visitante
}
