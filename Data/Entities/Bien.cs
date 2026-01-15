using System;

namespace SistemaComunidad.Data.Entities;

/// <summary>
/// Inventario de bienes comunitarios
/// </summary>
public class Bien : BaseEntity
{
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string? CodigoInventario { get; set; }
    public DateTime FechaAdquisicion { get; set; }
    public decimal? ValorAdquisicion { get; set; }
    public EstadoBien EstadoBien { get; set; } = EstadoBien.Bueno;
    public string? Ubicacion { get; set; }
    public string? Responsable { get; set; }
    public string? Notas { get; set; }
}

public enum EstadoBien
{
    Excelente,
    Bueno,
    Regular,
    Malo,
    DeBaja
}
