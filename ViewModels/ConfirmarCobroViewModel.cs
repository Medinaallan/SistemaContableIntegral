using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SistemaComunidad.Data.Entities;

namespace SistemaComunidad.ViewModels;

/// <summary>
/// ViewModel para la ventana de confirmación de cobro
/// </summary>
public partial class ConfirmarCobroViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _nombrePersona = string.Empty;

    [ObservableProperty]
    private string _identidadPersona = string.Empty;

    [ObservableProperty]
    private string _periodoTexto = string.Empty;

    [ObservableProperty]
    private ObservableCollection<DetalleServicioPreview> _detalleServicios = new();

    [ObservableProperty]
    private decimal _montoTotal;

    [ObservableProperty]
    private string _numeroReciboProyectado = string.Empty;

    public int PersonaId { get; set; }
    public int Periodo { get; set; }
    public bool DebeImprimir { get; set; }

    public ConfirmarCobroViewModel()
    {
    }

    public void CargarDatos(Persona persona, List<PersonaServicio> servicios, int periodo, string numeroRecibo)
    {
        PersonaId = persona.Id;
        Periodo = periodo;
        NombrePersona = $"{persona.Nombres} {persona.Apellidos}";
        IdentidadPersona = persona.IdentidadNacional ?? "Sin documento";
        
        var fecha = DateTimeOffset.ParseExact(periodo.ToString(), "yyyyMM", null);
        PeriodoTexto = fecha.ToString("MMMM yyyy");
        
        NumeroReciboProyectado = numeroRecibo;

        var detalles = new List<DetalleServicioPreview>();
        decimal total = 0;

        foreach (var servicio in servicios)
        {
            var costo = servicio.ObtenerCostoEfectivo();
            detalles.Add(new DetalleServicioPreview
            {
                Concepto = servicio.Servicio?.Nombre ?? "Servicio",
                Cantidad = 1,
                PrecioUnitario = costo,
                Subtotal = costo
            });
            total += costo;
        }

        DetalleServicios = new ObservableCollection<DetalleServicioPreview>(detalles);
        MontoTotal = total;
    }
}

/// <summary>
/// Clase para mostrar previsualización de detalles de servicio
/// </summary>
public class DetalleServicioPreview
{
    public string Concepto { get; set; } = string.Empty;
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal { get; set; }
}
