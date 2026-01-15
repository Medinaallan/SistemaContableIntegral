using System;

namespace SistemaComunidad.Data.Entities;

/// <summary>
/// Gestión de documentos y registros formales
/// </summary>
public class Documento : BaseEntity
{
    public string Titulo { get; set; } = string.Empty;
    public TipoDocumento TipoDocumento { get; set; }
    public DateTime FechaDocumento { get; set; } = DateTime.Now;
    public string? Descripcion { get; set; }
    public string? RutaArchivo { get; set; }
    public string? NombreArchivo { get; set; }
    public int? UsuarioRegistroId { get; set; }
}

public enum TipoDocumento
{
    Acta,
    Comunicado,
    Carta,
    Oficio,
    Informe,
    Otro
}
