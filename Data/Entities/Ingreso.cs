using System;

namespace SistemaComunidad.Data.Entities;

/// <summary>
/// Registro de ingresos comunitarios
/// </summary>
public class Ingreso : BaseEntity
{
    public decimal Monto { get; set; }
    public DateTime FechaIngreso { get; set; } = DateTime.Now;
    public TipoIngreso TipoIngreso { get; set; }
    public string Concepto { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string? NumeroRecibo { get; set; }
    public int? UsuarioRegistroId { get; set; }
}

public enum TipoIngreso
{
    Aporte,
    Donacion,
    Evento,
    Actividad,
    Otro
}
