using System;

namespace SistemaComunidad.Data.Entities;

/// <summary>
/// Representa un aporte o contribución de una persona
/// </summary>
public class Aporte : BaseEntity
{
    public int PersonaId { get; set; }
    public Persona Persona { get; set; } = null!;
    
    public decimal Monto { get; set; }
    public DateTime FechaAporte { get; set; } = DateTime.Now;
    public TipoAporte TipoAporte { get; set; }
    public string? Concepto { get; set; }
    public string? Notas { get; set; }
    public int? UsuarioRegistroId { get; set; }
}

public enum TipoAporte
{
    Periodico,
    Extraordinario,
    Ofrenda,
    Diezmo,
    Donacion
}
