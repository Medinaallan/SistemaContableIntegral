using System;

namespace SistemaComunidad.Data.Entities;

/// <summary>
/// Entidad que representa un pago realizado por una persona
/// </summary>
public class Pago : BaseEntity
{
    /// <summary>
    /// Número de recibo de pago único
    /// </summary>
    public string NumeroReciboPago { get; set; } = string.Empty;

    /// <summary>
    /// ID del cobro al que se aplica este pago
    /// </summary>
    public int CobroId { get; set; }

    /// <summary>
    /// Relación con el cobro
    /// </summary>
    public Cobro Cobro { get; set; } = null!;

    /// <summary>
    /// ID de la persona que realiza el pago
    /// </summary>
    public int PersonaId { get; set; }

    /// <summary>
    /// Relación con la persona
    /// </summary>
    public Persona Persona { get; set; } = null!;

    /// <summary>
    /// Fecha en que se realizó el pago
    /// </summary>
    public DateTimeOffset FechaPago { get; set; }

    /// <summary>
    /// Monto pagado
    /// </summary>
    public decimal Monto { get; set; }

    /// <summary>
    /// Método de pago utilizado
    /// </summary>
    public MetodoPago MetodoPago { get; set; } = MetodoPago.Efectivo;

    /// <summary>
    /// Número de referencia (para pagos con tarjeta, transferencia, etc.)
    /// </summary>
    public string? NumeroReferencia { get; set; }

    /// <summary>
    /// Observaciones del pago
    /// </summary>
    public string? Observaciones { get; set; }

    /// <summary>
    /// Usuario que registró el pago
    /// </summary>
    public int? UsuarioId { get; set; }

    /// <summary>
    /// Relación con el usuario
    /// </summary>
    public Usuario? Usuario { get; set; }

    /// <summary>
    /// Indica si el recibo fue impreso
    /// </summary>
    public bool ReciboImpreso { get; set; }

    /// <summary>
    /// Fecha en que se imprimió el recibo
    /// </summary>
    public DateTimeOffset? FechaImpresion { get; set; }
}

/// <summary>
/// Métodos de pago disponibles
/// </summary>
public enum MetodoPago
{
    Efectivo,
    Tarjeta,
    Transferencia,
    Cheque,
    Deposito,
    Otro
}
