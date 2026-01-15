using System;

namespace SistemaComunidad.Data.Entities;

/// <summary>
/// Registro de egresos operativos
/// </summary>
public class Egreso : BaseEntity
{
    public decimal Monto { get; set; }
    public DateTime FechaEgreso { get; set; } = DateTime.Now;
    public TipoEgreso TipoEgreso { get; set; }
    public string Concepto { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string? NumeroComprobante { get; set; }
    public string? Beneficiario { get; set; }
    public int? UsuarioRegistroId { get; set; }
}

public enum TipoEgreso
{
    Mantenimiento,
    Servicios,
    Compras,
    Ayuda,
    Eventos,
    Administracion,
    Otro
}
