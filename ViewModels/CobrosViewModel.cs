using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SistemaComunidad.Business.Interfaces;
using SistemaComunidad.Data.Entities;

namespace SistemaComunidad.ViewModels;

/// <summary>
/// ViewModel para el módulo de Cobros y Pagos
/// </summary>
public partial class CobrosViewModel : ViewModelBase
{
    private readonly ICobroService _cobroService;
    private readonly IPersonaService _personaService;

    [ObservableProperty]
    private ObservableCollection<Cobro> _cobrosPendientes = new();

    [ObservableProperty]
    private ObservableCollection<Pago> _pagosRecientes = new();

    [ObservableProperty]
    private Cobro? _cobroSeleccionado;

    [ObservableProperty]
    private bool _estaCargando;

    [ObservableProperty]
    private string _mensaje = string.Empty;

    [ObservableProperty]
    private bool _hayError;

    // Generación de cobros
    [ObservableProperty]
    private int _periodoGenerar;

    [ObservableProperty]
    private string _textoPeriodo = string.Empty;

    // Registro de pago
    [ObservableProperty]
    private decimal _montoPago;

    [ObservableProperty]
    private MetodoPago _metodoPago = MetodoPago.Efectivo;

    [ObservableProperty]
    private string? _observacionesPago;

    [ObservableProperty]
    private string _busquedaPersona = string.Empty;

    public CobrosViewModel(ICobroService cobroService, IPersonaService personaService)
    {
        _cobroService = cobroService;
        _personaService = personaService;
        
        // Establecer período actual
        var ahora = DateTimeOffset.Now;
        PeriodoGenerar = (ahora.Year * 100) + ahora.Month;
        TextoPeriodo = $"{ahora:MMMM yyyy}";
    }

    public async Task InicializarAsync()
    {
        await CargarCobrosPendientesAsync();
        await CargarPagosRecientesAsync();
    }

    [RelayCommand]
    private async Task CargarCobrosPendientesAsync()
    {
        try
        {
            EstaCargando = true;
            HayError = false;
            Mensaje = string.Empty;

            var cobrosVencidos = await _cobroService.ObtenerCobrosVencidosAsync();
            CobrosPendientes = new ObservableCollection<Cobro>(cobrosVencidos.Take(50));
        }
        catch (Exception ex)
        {
            HayError = true;
            Mensaje = $"Error al cargar cobros: {ex.Message}";
        }
        finally
        {
            EstaCargando = false;
        }
    }

    [RelayCommand]
    private async Task CargarPagosRecientesAsync()
    {
        try
        {
            EstaCargando = true;
            var ahora = DateTimeOffset.Now;
            var hace30Dias = ahora.AddDays(-30);
            
            // Por ahora mostramos mensaje
            Mensaje = "Módulo de pagos recientes en desarrollo";
        }
        catch (Exception ex)
        {
            HayError = true;
            Mensaje = $"Error: {ex.Message}";
        }
        finally
        {
            EstaCargando = false;
        }
    }

    [RelayCommand]
    private async Task GenerarCobrosMensualesAsync()
    {
        try
        {
            EstaCargando = true;
            HayError = false;

            var cobrosGenerados = await _cobroService.GenerarCobrosMensualesAsync(PeriodoGenerar);
            var cantidad = cobrosGenerados.Count();

            Mensaje = $"✓ Se generaron {cantidad} cobros para el período {TextoPeriodo}";
            
            await CargarCobrosPendientesAsync();
        }
        catch (Exception ex)
        {
            HayError = true;
            Mensaje = $"Error al generar cobros: {ex.Message}";
        }
        finally
        {
            EstaCargando = false;
        }
    }

    [RelayCommand]
    private void SeleccionarCobro(Cobro? cobro)
    {
        if (cobro == null) return;
        
        CobroSeleccionado = cobro;
        MontoPago = cobro.SaldoPendiente;
        ObservacionesPago = null;
    }

    [RelayCommand]
    private async Task RegistrarPagoAsync()
    {
        if (CobroSeleccionado == null)
        {
            HayError = true;
            Mensaje = "Debe seleccionar un cobro";
            return;
        }

        if (MontoPago <= 0)
        {
            HayError = true;
            Mensaje = "El monto debe ser mayor a cero";
            return;
        }

        try
        {
            EstaCargando = true;
            HayError = false;

            var pago = await _cobroService.RegistrarPagoAsync(
                CobroSeleccionado.Id,
                MontoPago,
                MetodoPago,
                usuarioId: 1, // TODO: Obtener del usuario actual
                observaciones: ObservacionesPago
            );

            Mensaje = $"✓ Pago registrado. Recibo: {pago.NumeroReciboPago}";
            
            // Limpiar formulario
            CobroSeleccionado = null;
            MontoPago = 0;
            ObservacionesPago = null;

            await CargarCobrosPendientesAsync();
        }
        catch (Exception ex)
        {
            HayError = true;
            Mensaje = $"Error al registrar pago: {ex.Message}";
        }
        finally
        {
            EstaCargando = false;
        }
    }

    [RelayCommand]
    private async Task BuscarPersonaAsync()
    {
        if (string.IsNullOrWhiteSpace(BusquedaPersona))
        {
            await CargarCobrosPendientesAsync();
            return;
        }

        try
        {
            EstaCargando = true;
            var personas = await _personaService.BuscarAsync(BusquedaPersona);
            
            if (!personas.Any())
            {
                Mensaje = "No se encontraron personas";
                CobrosPendientes.Clear();
                return;
            }

            var cobros = new List<Cobro>();
            foreach (var persona in personas)
            {
                var cobrosPen = await _cobroService.ObtenerCobrosPendientesAsync(persona.Id);
                cobros.AddRange(cobrosPen);
            }

            CobrosPendientes = new ObservableCollection<Cobro>(cobros);
            Mensaje = $"Se encontraron {cobros.Count} cobros pendientes";
        }
        catch (Exception ex)
        {
            HayError = true;
            Mensaje = $"Error en la búsqueda: {ex.Message}";
        }
        finally
        {
            EstaCargando = false;
        }
    }
}
