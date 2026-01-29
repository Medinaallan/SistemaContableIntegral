using System;

namespace SistemaComunidad.Data.Entities;

/// <summary>
/// Entidad que representa los datos de la empresa/organización
/// </summary>
public class Empresa : BaseEntity
{
    public string RazonSocial { get; set; } = string.Empty;
    public string NombreComercial { get; set; } = string.Empty;
    public string RTN { get; set; } = string.Empty;
    public string NumeroTelefono { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string CorreoElectronico { get; set; } = string.Empty;
    public string? Representante { get; set; }
    public string? TelefonoRepresentante { get; set; }
    
    /// <summary>
    /// Formato de impresión de recibos: "MediaCarta", "Ticket80mm"
    /// </summary>
    public string FormatoRecibo { get; set; } = "MediaCarta";
}
