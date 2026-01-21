using System;

namespace SistemaComunidad.Data.Entities;

/// <summary>
/// Entidad que representa un servicio ofrecido por el patronato
/// (para pagos mensuales de los socios)
/// </summary>
public class Servicio : BaseEntity
{
    /// <summary>
    /// Nombre del servicio (ej: "Agua Potable", "Mantenimiento", "Seguridad")
    /// </summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Descripción detallada del servicio
    /// </summary>
    public string? Descripcion { get; set; }

    /// <summary>
    /// Costo mensual del servicio
    /// </summary>
    public decimal CostoMensual { get; set; }

    /// <summary>
    /// Periodicidad del servicio (Mensual, Bimestral, Trimestral, etc.)
    /// </summary>
    public PeriodicidadServicio Periodicidad { get; set; } = PeriodicidadServicio.Mensual;

    /// <summary>
    /// Indica si el servicio es obligatorio para todos los socios
    /// </summary>
    public bool EsObligatorio { get; set; } = true;

    /// <summary>
    /// Notas adicionales sobre el servicio
    /// </summary>
    public string? Notas { get; set; }

    /// <summary>
    /// ID de la empresa/patronato al que pertenece el servicio
    /// </summary>
    public int? EmpresaId { get; set; }

    /// <summary>
    /// Relación con la empresa/patronato
    /// </summary>
    public Empresa? Empresa { get; set; }
}

/// <summary>
/// Periodicidad con la que se cobra el servicio
/// </summary>
public enum PeriodicidadServicio
{
    Mensual,
    Bimestral,
    Trimestral,
    Semestral,
    Anual
}
