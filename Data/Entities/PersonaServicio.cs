using System;

namespace SistemaComunidad.Data.Entities;

/// <summary>
/// Entidad de relación entre Persona y Servicio
/// Representa la asignación de un servicio a una persona con información adicional
/// </summary>
public class PersonaServicio : BaseEntity
{
    /// <summary>
    /// ID de la persona a la que se asigna el servicio
    /// </summary>
    public int PersonaId { get; set; }

    /// <summary>
    /// Relación con la persona
    /// </summary>
    public Persona Persona { get; set; } = null!;

    /// <summary>
    /// ID del servicio asignado
    /// </summary>
    public int ServicioId { get; set; }

    /// <summary>
    /// Relación con el servicio
    /// </summary>
    public Servicio Servicio { get; set; } = null!;

    /// <summary>
    /// Fecha desde la cual la persona está suscrita al servicio
    /// </summary>
    public DateTimeOffset FechaInicio { get; set; }

    /// <summary>
    /// Fecha en la que terminó el servicio (null si está activo)
    /// </summary>
    public DateTimeOffset? FechaFin { get; set; }

    /// <summary>
    /// Indica si el servicio está activo para esta persona
    /// </summary>
    public bool EstaActivo { get; set; } = true;

    /// <summary>
    /// Costo personalizado del servicio para esta persona (si es diferente al costo estándar)
    /// Si es null, se usa el CostoMensual del servicio
    /// </summary>
    public decimal? CostoPersonalizado { get; set; }

    /// <summary>
    /// Notas específicas sobre esta asignación de servicio
    /// </summary>
    public string? Notas { get; set; }

    /// <summary>
    /// Último mes/año que se generó el cobro para este servicio
    /// Formato: YYYYMM (ejemplo: 202601 para enero 2026)
    /// </summary>
    public int? UltimoPeriodoCobrado { get; set; }

    /// <summary>
    /// Obtiene el costo efectivo del servicio (personalizado o estándar)
    /// </summary>
    public decimal ObtenerCostoEfectivo()
    {
        return CostoPersonalizado ?? Servicio?.CostoMensual ?? 0;
    }

    /// <summary>
    /// Verifica si el servicio debe ser cobrado en el período actual
    /// </summary>
    public bool DebeCobrarseEn(int periodo)
    {
        if (!EstaActivo) return false;
        if (UltimoPeriodoCobrado >= periodo) return false;
        
        // Verificar si la fecha de inicio es anterior o igual al período
        var anio = periodo / 100;
        var mes = periodo % 100;
        var fechaPeriodo = new DateTimeOffset(anio, mes, 1, 0, 0, 0, TimeSpan.Zero);
        
        return FechaInicio <= fechaPeriodo && (FechaFin == null || FechaFin >= fechaPeriodo);
    }
}
