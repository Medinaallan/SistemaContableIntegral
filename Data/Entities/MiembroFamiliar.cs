using System;

namespace SistemaComunidad.Data.Entities;

/// <summary>
/// Representa un integrante dentro de un núcleo familiar.
/// Puede estar vinculado a una Persona existente o ser un registro independiente.
/// </summary>
public class MiembroFamiliar : BaseEntity
{
    // Datos del miembro (usados cuando no hay Persona vinculada)
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public string? Notas { get; set; }

    public RolFamiliar Rol { get; set; } = RolFamiliar.Otro;

    // FK al núcleo familiar (obligatorio)
    public int NucleoFamiliarId { get; set; }
    public NucleoFamiliar NucleoFamiliar { get; set; } = null!;

    // FK opcional a Persona existente
    public int? PersonaId { get; set; }
    public Persona? Persona { get; set; }

    /// <summary>
    /// Nombre para mostrar: si está vinculado a Persona, usa sus datos; de lo contrario, usa los propios.
    /// </summary>
    public string NombreCompleto =>
        Persona != null
            ? $"{Persona.Nombres} {Persona.Apellidos}"
            : $"{Nombres} {Apellidos}";
}

public enum RolFamiliar
{
    Padre,
    Madre,
    Hijo,
    Hija,
    Abuelo,
    Abuela,
    Tio,
    Tia,
    Primo,
    Prima,
    Sobrino,
    Sobrina,
    Esposo,
    Esposa,
    Otro
}
