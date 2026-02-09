using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SistemaComunidad.Data.Entities;

namespace SistemaComunidad.Business.Services;

/// <summary>
/// Servicio para generar recibos de pago en PDF
/// </summary>
public class RecibosPdfService
{
    public RecibosPdfService()
    {
        // Configurar QuestPDF (licencia gratuita para uso comunitario)
        QuestPDF.Settings.License = LicenseType.Community;
    }

    /// <summary>
    /// Genera un PDF del recibo de pago y lo abre
    /// </summary>
    public string GenerarReciboPago(Pago pago, Cobro cobro, Persona persona, Empresa empresa)
    {
        // Crear carpeta Reportes si no existe
        var carpetaReportes = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reportes");
        Directory.CreateDirectory(carpetaReportes);

        // Nombre del archivo
        var nombreArchivo = $"Recibo_{pago.NumeroReciboPago}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
        var rutaCompleta = Path.Combine(carpetaReportes, nombreArchivo);

        // Determinar formato según configuración
        var esTicket = empresa.FormatoRecibo == "Ticket80mm";

        // Generar el PDF
        Document.Create(container =>
        {
            container.Page(page =>
            {
                // Configurar tamaño según formato
                if (esTicket)
                {
                    // Ticket de 80mm de ancho (convertir a puntos: 80mm = 226.77 puntos)
                    var anchoTicket = 80 * 2.83465f; // 80mm a puntos
                    page.Size(anchoTicket, 500); // Ancho fijo, alto ajustado para ticket
                    page.Margin(5);
                }
                else
                {
                    // Media carta: 8.5" x 5.5" (horizontal - landscape)
                    page.Size(new PageSize(8.5f, 5.5f, Unit.Inch));
                    page.Margin(20);
                }
                
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(esTicket ? 8 : 10).FontFamily("Arial"));

                page.Header().Element(c => Header(c, empresa, esTicket));
                
                // Para ticket, incluir footer en el contenido para evitar espacio excesivo
                if (esTicket)
                {
                    page.Content().Column(column =>
                    {
                        column.Item().Element(content => Content(content, pago, cobro, persona, empresa, esTicket));
                        column.Item().PaddingTop(5).Element(Footer);
                    });
                }
                else
                {
                    page.Content().Element(content => Content(content, pago, cobro, persona, empresa, esTicket));
                    page.Footer().Element(Footer);
                }
            });
        })
        .GeneratePdf(rutaCompleta);

        return rutaCompleta;
    }

    private void Header(IContainer container, Empresa empresa, bool esTicket)
    {
        container.Column(column =>
        {
            var padding = esTicket ? 5 : 10;
            var fontSizeTitulo = esTicket ? 9 : 11;
            var fontSizeRecibo = esTicket ? 12 : 16;
            
            column.Item().Border(esTicket ? 1 : 2).BorderColor(Colors.Black).Padding(padding).Column(innerColumn =>
            {
                // Título con nombre de la empresa
                var nombreEmpresa = empresa.RazonSocial.ToUpper();
                innerColumn.Item().AlignCenter().Text(nombreEmpresa)
                    .FontSize(fontSizeTitulo).Bold();
                
                innerColumn.Item().PaddingTop(esTicket ? 2 : 5).AlignCenter().Text("RECIBO")
                    .FontSize(fontSizeRecibo).Bold();
            });
        });
    }

    private void Content(IContainer container, Pago pago, Cobro cobro, Persona persona, Empresa empresa, bool esTicket)
    {
        var paddingTop = esTicket ? 8 : 20;
        var fontSize = esTicket ? 7 : 10;
        var fontSizeBold = esTicket ? 8 : 12;
        
        container.PaddingTop(paddingTop).Column(column =>
        {
            if (esTicket)
            {
                // Formato Ticket 80mm: Diseño estilo POS
                
                // Información de la empresa con bordes
                column.Item().Border(1).BorderColor(Colors.Black).Padding(5).Column(col =>
                {
                    col.Item().AlignCenter().Text("DOMICILIO").FontSize(9).Bold();
                    col.Item().AlignCenter().Text(empresa.Direccion ?? "").FontSize(8);
                    
                    col.Item().PaddingTop(3).AlignCenter().Text("PERSONERIA JURIDICA Nº").FontSize(9).Bold();
                    col.Item().AlignCenter().Text(empresa.RTN ?? "").FontSize(8);
                });

                // RECIBO Nº con línea separadora
                column.Item().PaddingTop(8).Column(col =>
                {
                    col.Item().LineHorizontal(1).LineColor(Colors.Black);
                    
                    col.Item().PaddingTop(5).AlignCenter().Text(text =>
                    {
                        text.Span("RECIBO Nº ").FontSize(10).Bold();
                        text.Span(pago.NumeroReciboPago).FontSize(11).Bold();
                    });
                    
                    col.Item().PaddingTop(3).LineHorizontal(1).LineColor(Colors.Black);
                });

                // Fecha
                column.Item().PaddingTop(6).Text(text =>
                {
                    text.Span("Fecha: ").Bold().FontSize(9);
                    text.Span(pago.FechaPago.ToString("dd/MM/yyyy HH:mm")).FontSize(9);
                });

                // Nombre de socio
                column.Item().PaddingTop(4).Text(text =>
                {
                    text.Span("Nombre de socio:").Bold().FontSize(9);
                });
                column.Item().Text($"{persona.Nombres} {persona.Apellidos}").FontSize(9);
                
                // ID
                column.Item().PaddingTop(2).Text(text =>
                {
                    text.Span("ID: ").Bold().FontSize(8);
                    text.Span(persona.IdentidadNacional ?? "N/A").FontSize(8);
                });

                // Línea separadora antes de servicios
                column.Item().PaddingTop(8).LineHorizontal(1).LineColor(Colors.Black);

                // Lista de servicios estilo POS (sin tabla)
                column.Item().PaddingTop(5).Column(col =>
                {
                    if (cobro.Detalles != null && cobro.Detalles.Any())
                    {
                        foreach (var detalle in cobro.Detalles)
                        {
                            // Obtener el periodo del cobro (Mes/Año) desde el formato YYYYMM
                            int mes = cobro.Periodo % 100;
                            int anio = cobro.Periodo / 100;
                            string periodo = $"{mes:00}/{anio}";

                            // Línea del servicio: "Servicio - Mensualidad - fecha"
                            col.Item().PaddingBottom(3).Row(row =>
                            {
                                row.RelativeItem().Column(leftCol =>
                                {
                                    leftCol.Item().Text($"{detalle.Concepto}").FontSize(9).Bold();
                                    leftCol.Item().Text($"Mensualidad: {periodo}").FontSize(8);
                                });
                                
                                row.AutoItem().AlignRight().Text($"L. {detalle.Subtotal:N2}").FontSize(9).Bold();
                            });
                            
                            // Línea separadora punteada
                            col.Item().PaddingBottom(3).LineHorizontal(0.5f).LineColor(Colors.Grey.Lighten2);
                        }
                    }
                });

                // TOTAL
                column.Item().PaddingTop(5).LineHorizontal(1).LineColor(Colors.Black);
                column.Item().PaddingTop(5).Row(row =>
                {
                    row.RelativeItem().Text("TOTAL:").Bold().FontSize(11);
                    row.AutoItem().Text($"L. {pago.Monto:N2}").FontSize(12).Bold();
                });
                column.Item().PaddingTop(3).LineHorizontal(1).LineColor(Colors.Black);

                // Valor en letras
                column.Item().PaddingTop(5).Column(col =>
                {
                    col.Item().Text("Son:").Bold().FontSize(8);
                    col.Item().PaddingLeft(5).Text(NumeroALetras(pago.Monto)).FontSize(8);
                });

                // Observaciones
                column.Item().PaddingTop(6).Text(text =>
                {
                    text.Span("Observaciones: ").Bold().FontSize(8);
                    text.Span(pago.Observaciones ?? "Ninguna").FontSize(8);
                });

                // Línea separadora
                column.Item().PaddingTop(8).LineHorizontal(1).LineColor(Colors.Black);

                // Mensaje de agradecimiento
                column.Item().PaddingTop(5).AlignCenter().Text("Gracias por su pago")
                    .FontSize(9).Italic();

                // Espacio para firma manual del tesorero
                column.Item().PaddingTop(90).AlignCenter().Column(col =>
                {
                    col.Item().Width(150).LineHorizontal(1).LineColor(Colors.Black);
                    col.Item().AlignCenter().Text("Sello/Firma Tesorero")
                        .FontSize(8).Italic();
                });
            }
            else
            {
                // Formato Media Carta: Diseño según imagen del usuario
                
                // Tabla superior con DOMICILIO y PERSONERIA JURIDICA
                column.Item().PaddingTop(5).Border(1).BorderColor(Colors.Black).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(80);
                        columns.RelativeColumn(2);
                        columns.ConstantColumn(150);
                        columns.RelativeColumn(1);
                    });
                    
                    table.Cell().Border(0.5f).BorderColor(Colors.Black)
                        .Padding(5).Text("DOMICILIO").FontSize(8).Bold();
                    table.Cell().Border(0.5f).BorderColor(Colors.Black)
                        .Padding(5).Text(empresa.Direccion ?? "").FontSize(8);
                    table.Cell().Border(0.5f).BorderColor(Colors.Black)
                        .Padding(5).Text("PERSONERIA JURIDICA Nº").FontSize(8).Bold();
                    table.Cell().Border(0.5f).BorderColor(Colors.Black)
                        .Padding(5).Text(empresa.RTN ?? "").FontSize(8);
                });

                // RECIBO Nº
                column.Item().PaddingTop(10).Row(row =>
                {
                    row.RelativeItem().Text(text =>
                    {
                        text.Span("RECIBO").Bold().FontSize(12);
                        text.Span("    Nº ").FontSize(10);
                        text.Span(pago.NumeroReciboPago).FontSize(12).Bold();
                    });
                });

                // Fecha
                column.Item().PaddingTop(8).Text(text =>
                {
                    text.Span("Fecha: ").Bold().FontSize(10);
                    text.Span(pago.FechaPago.ToString("dd/MM/yyyy")).FontSize(10);
                });

                // Nombre de socio
                column.Item().PaddingTop(8).Text(text =>
                {
                    text.Span("Nombre de socio: ").Bold().FontSize(10);
                    text.Span($"{persona.Nombres} {persona.Apellidos}").FontSize(10);
                });

                // Servicios a Pagar
                column.Item().PaddingTop(12).AlignCenter().Text("Servicios a Pagar")
                    .FontSize(11).Bold();

                // Tabla de servicios - Dinámico basado en los servicios del cobro
                column.Item().PaddingTop(10).Border(1).BorderColor(Colors.Black).Table(table =>
                {
                    // Obtener todos los servicios únicos del cobro
                    var serviciosEnCobro = cobro.Detalles?.Select(d => new { d.ServicioId, d.Concepto }).Distinct().OrderBy(x => x.ServicioId).ToList();
                    
                    if (serviciosEnCobro == null || !serviciosEnCobro.Any())
                    {
                        serviciosEnCobro = new List<dynamic>().Select(d => new { ServicioId = 0, Concepto = "" }).ToList();
                    }
                    
                    var numeroColumnas = serviciosEnCobro.Count + 2; // CONCEPTO + servicios + TOTAL A PAGAR

                    // Definir columnas dinámicamente
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(2); // Columna CONCEPTO (más ancha)
                        
                        // Una columna por cada servicio
                        foreach (var _ in serviciosEnCobro)
                        {
                            columns.RelativeColumn(1);
                        }
                        
                        columns.RelativeColumn(1); // Columna TOTAL A PAGAR
                    });

                    // Encabezado
                    table.Header(header =>
                    {
                        // Primera columna: CONCEPTO
                        header.Cell().Border(0.5f).BorderColor(Colors.Black)
                            .Padding(3).Text("CONCEPTO").Bold().FontSize(7);
                        
                        // Columnas de servicios
                        foreach (var servicio in serviciosEnCobro)
                        {
                            header.Cell().Border(0.5f).BorderColor(Colors.Black)
                                .Padding(3).Text(servicio.Concepto?.ToUpper() ?? "").Bold().FontSize(6).AlignCenter();
                        }
                        
                        // Última columna: TOTAL A PAGAR
                        header.Cell().Border(0.5f).BorderColor(Colors.Black)
                            .Padding(3).Text("TOTAL A PAGAR").Bold().FontSize(6).AlignCenter();
                    });

                    // Fila de datos - Una sola fila con los montos en las columnas correspondientes
                    table.Cell().Border(0.5f).BorderColor(Colors.Black)
                        .Padding(3).Text("Mensualidad").FontSize(8);
                    
                    // Para cada servicio, mostrar su monto
                    decimal totalFila = 0;
                    foreach (var servicio in serviciosEnCobro)
                    {
                        var detalle = cobro.Detalles?.FirstOrDefault(d => d.ServicioId == servicio.ServicioId);
                        if (detalle != null)
                        {
                            table.Cell().Border(0.5f).BorderColor(Colors.Black)
                                .Padding(3).AlignRight().Text($"{detalle.Subtotal:N2}").FontSize(8);
                            totalFila += detalle.Subtotal;
                        }
                        else
                        {
                            table.Cell().Border(0.5f).BorderColor(Colors.Black)
                                .Padding(3).Text("").FontSize(8);
                        }
                    }
                    
                    // Total de la fila
                    table.Cell().Border(0.5f).BorderColor(Colors.Black)
                        .Padding(3).AlignRight().Text($"{totalFila:N2}").FontSize(8).Bold();

                    // Fila de TOTAL A PAGAR
                    table.Cell().ColumnSpan((uint)(numeroColumnas - 1)).Border(0.5f).BorderColor(Colors.Black)
                        .Padding(3).AlignRight().Text("TOTAL A PAGAR:").Bold().FontSize(9);
                    table.Cell().Border(0.5f).BorderColor(Colors.Black).Background(Colors.Grey.Lighten3)
                        .Padding(3).AlignRight().Text($"L. {pago.Monto:N2}").Bold().FontSize(10);
                });

                // Observaciones
                column.Item().PaddingTop(12).Text(text =>
                {
                    text.Span("Observaciones: ").Bold().FontSize(9);
                    text.Span(pago.Observaciones ?? "").FontSize(9);
                });

                // Valor en letras
                column.Item().PaddingTop(10).Text(text =>
                {
                    text.Span("Valor en Letras: ").Bold();
                    text.Span(NumeroALetras(pago.Monto));
                });

                // Firma (centrada con línea más corta)
                column.Item().PaddingTop(20).AlignCenter().Width(300).Column(col =>
                {
                    col.Item().LineHorizontal(1).LineColor(Colors.Black);
                    col.Item().AlignCenter().Text("Sello/Firma Tesorero")
                        .FontSize(8).Italic();
                });
            }
        });
    }

    private void Footer(IContainer container)
    {
        container.PaddingTop(5).AlignCenter().Text(text =>
        {
            text.Span("*** Recibo generado automáticamente ***").FontSize(8).Italic();
        });
    }

    private string NumeroALetras(decimal numero)
    {
        if (numero == 0) return "CERO LEMPIRAS";

        string[] unidades = { "", "UN", "DOS", "TRES", "CUATRO", "CINCO", "SEIS", "SIETE", "OCHO", "NUEVE" };
        string[] decenas = { "", "DIEZ", "VEINTE", "TREINTA", "CUARENTA", "CINCUENTA", "SESENTA", "SETENTA", "OCHENTA", "NOVENTA" };
        string[] especiales = { "DIEZ", "ONCE", "DOCE", "TRECE", "CATORCE", "QUINCE", "DIECISEIS", "DIECISIETE", "DIECIOCHO", "DIECINUEVE" };
        string[] centenas = { "", "CIENTO", "DOSCIENTOS", "TRESCIENTOS", "CUATROCIENTOS", "QUINIENTOS", "SEISCIENTOS", "SETECIENTOS", "OCHOCIENTOS", "NOVECIENTOS" };

        int parteEntera = (int)numero;
        int centavos = (int)((numero - parteEntera) * 100);

        string resultado = "";

        if (parteEntera >= 1000)
        {
            int miles = parteEntera / 1000;
            resultado += miles == 1 ? "MIL " : $"{ConvertirCentenas(miles)} MIL ";
            parteEntera %= 1000;
        }

        if (parteEntera >= 100)
        {
            int centena = parteEntera / 100;
            if (parteEntera == 100)
            {
                return "CIEN LEMPIRAS EXACTOS";
            }
            resultado += centenas[centena] + " ";
            parteEntera %= 100;
        }

        if (parteEntera >= 20)
        {
            resultado += decenas[parteEntera / 10] + " ";
            parteEntera %= 10;
        }
        else if (parteEntera >= 10)
        {
            resultado += especiales[parteEntera - 10] + " ";
            parteEntera = 0;
        }

        if (parteEntera > 0)
        {
            resultado += unidades[parteEntera] + " ";
        }

        resultado += "LEMPIRAS";

        if (centavos > 0)
        {
            resultado += $" CON {centavos:00}/100";
        }

        return resultado.Trim();
    }

    private string ConvertirCentenas(int numero)
    {
        string[] centenas = { "", "CIENTO", "DOSCIENTOS", "TRESCIENTOS", "CUATROCIENTOS", "QUINIENTOS", "SEISCIENTOS", "SETECIENTOS", "OCHOCIENTOS", "NOVECIENTOS" };
        
        if (numero == 100) return "CIEN";
        if (numero < 100) return "";
        
        return centenas[numero];
    }

    /// <summary>
    /// Abre el PDF generado con el visor predeterminado
    /// </summary>
    public void AbrirPdf(string rutaArchivo)
    {
        try
        {
            var proceso = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = rutaArchivo,
                    UseShellExecute = true
                }
            };
            proceso.Start();
        }
        catch (Exception ex)
        {
            throw new Exception($"No se pudo abrir el PDF: {ex.Message}");
        }
    }

    /// <summary>
    /// Genera un comprobante PDF sencillo para un `Aporte`.
    /// </summary>
    public string GenerarReciboAporte(Data.Entities.Aporte aporte, Data.Entities.Persona persona, Data.Entities.Empresa empresa)
    {
        var carpetaReportes = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reportes");
        Directory.CreateDirectory(carpetaReportes);
        var nombreArchivo = $"ReciboAporte_{aporte.Id}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
        var rutaCompleta = Path.Combine(carpetaReportes, nombreArchivo);

        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.Letter);
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontSize(12).FontFamily("Arial"));

                page.Header().Text(empresa?.RazonSocial ?? "Recibo").SemiBold().FontSize(16).AlignCenter();
                page.Content().Column(col =>
                {
                    col.Item().PaddingTop(10).Text($"Recibo Nº: {aporte.NumeroRecibo ?? "(no asignado)"}");
                    col.Item().Text($"Fecha: {aporte.FechaAporte:dd/MM/yyyy HH:mm}");
                    col.Item().Text($"Persona: {persona.Nombres} {persona.Apellidos}");
                    col.Item().Text($"Monto: L. {aporte.Monto:N2}");
                    col.Item().PaddingTop(8).Text("Concepto:").Bold();
                    col.Item().Text(aporte.Concepto ?? "");
                });

                page.Footer().AlignCenter().Text("Comprobante generado automáticamente").FontSize(9);
            });
        })
        .GeneratePdf(rutaCompleta);

        return rutaCompleta;
    }
}
