using System;
using System.Collections.Generic;

namespace SistemaComunidad.Data.Entities;

/// <summary>
/// Entidad que representa un cobro mensual generado para una persona
/// Similar a una factura o recibo de cobro
/// </summary>
public class Cobro : BaseEntity
{
    /// <summary>
    /// Número de recibo único (formato: YYYYMM-XXXX)
    /// </summary>
    public string NumeroRecibo { get; set; } = string.Empty;

    /// <summary>
    /// ID de la persona a la que se le cobra
    /// </summary>
    public int PersonaId { get; set; }

    /// <summary>
    /// Relación con la persona
    /// </summary>
    public Persona Persona { get; set; } = null!;

    /// <summary>
    /// Período del cobro en formato YYYYMM (ejemplo: 202601 para enero 2026)
    /// </summary>
    public int Periodo { get; set; }

    /// <summary>
    /// Fecha de emisión del cobro
    /// </summary>
    public DateTimeOffset FechaEmision { get; set; }

    /// <summary>
    /// Fecha límite de pago
    /// </summary>
    public DateTimeOffset FechaLimitePago { get; set; }

    /// <summary>
    /// Monto total a pagar
    /// </summary>
    public decimal MontoTotal { get; set; }

    /// <summary>
    /// Monto ya pagado
    /// </summary>
    public decimal MontoPagado { get; set; }

    /// <summary>
    /// Saldo pendiente
    /// </summary>
    public decimal SaldoPendiente => MontoTotal - MontoPagado;

    /// <summary>
    /// Estado del cobro
    /// </summary>
    public EstadoCobro Estado { get; set; } = EstadoCobro.Pendiente;

    /// <summary>
    /// Observaciones o notas
    /// </summary>
    public string? Observaciones { get; set; }

    /// <summary>
    /// Indica si el cobro fue generado automáticamente
    /// </summary>
    public bool EsAutomatico { get; set; } = true;

    /// <summary>
    /// Detalles del cobro (servicios incluidos)
    /// </summary>
    public ICollection<CobroDetalle> Detalles { get; set; } = new List<CobroDetalle>();

    /// <summary>
    /// Pagos aplicados a este cobro
    /// </summary>
    public ICollection<Pago> Pagos { get; set; } = new List<Pago>();

    /// <summary>
    /// Verifica si el cobro está vencido
    /// </summary>
    public bool EstaVencido()
    {
        return FechaLimitePago < DateTimeOffset.Now && Estado != EstadoCobro.Pagado;
    }

    /// <summary>
    /// Verifica si el cobro está completamente pagado
    /// </summary>
    public bool EstaPagado()
    {
        return Estado == EstadoCobro.Pagado || SaldoPendiente <= 0;
    }
}

/// <summary>
/// Detalle de cada servicio incluido en el cobro
/// </summary>
public class CobroDetalle : BaseEntity
{
    /// <summary>
    /// ID del cobro al que pertenece
    /// </summary>
    public int CobroId { get; set; }

    /// <summary>
    /// Relación con el cobro
    /// </summary>
    public Cobro Cobro { get; set; } = null!;

    /// <summary>
    /// ID del servicio cobrado
    /// </summary>
    public int ServicioId { get; set; }

    /// <summary>
    /// Relación con el servicio
    /// </summary>
    public Servicio Servicio { get; set; } = null!;

    /// <summary>
    /// ID de PersonaServicio (la asignación específica)
    /// </summary>
    public int PersonaServicioId { get; set; }

    /// <summary>
    /// Relación con PersonaServicio
    /// </summary>
    public PersonaServicio PersonaServicio { get; set; } = null!;

    /// <summary>
    /// Descripción del concepto
    /// </summary>
    public string Concepto { get; set; } = string.Empty;

    /// <summary>
    /// Cantidad (normalmente 1 para servicios mensuales)
    /// </summary>
    public decimal Cantidad { get; set; } = 1;

    /// <summary>
    /// Precio unitario del servicio
    /// </summary>
    public decimal PrecioUnitario { get; set; }

    /// <summary>
    /// Subtotal (Cantidad * PrecioUnitario)
    /// </summary>
    public decimal Subtotal => Cantidad * PrecioUnitario;
}

/// <summary>
/// Estados posibles de un cobro
/// </summary>
public enum EstadoCobro
{
    Pendiente,
    PagoParcial,
    Pagado,
    Vencido,
    Cancelado
}
