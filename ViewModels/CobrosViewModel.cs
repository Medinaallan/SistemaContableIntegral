using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SistemaComunidad.Business.Interfaces;
using SistemaComunidad.Business.Services;
using SistemaComunidad.Data.Entities;

namespace SistemaComunidad.ViewModels;

/// <summary>
/// ViewModel para el módulo de Cobros y Pagos
/// </summary>
public partial class CobrosViewModel : ViewModelBase
{
    private readonly ICobroService _cobroService;
    private readonly IPersonaService _personaService;
    private readonly IPersonaServicioService _personaServicioService;
    private readonly IEmpresaService _empresaService;
    private readonly RecibosPdfService _recibosPdfService;
    
    public event EventHandler? CerrarSolicitado;

    [ObservableProperty]
    private ObservableCollection<PersonaConServicios> _personasConServicios = new();

    [ObservableProperty]
    private PersonaConServicios? _personaSeleccionada;

    [ObservableProperty]
    private ObservableCollection<Cobro> _cobrosPersona = new();

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

    // Historial de pagos
    [ObservableProperty]
    private ObservableCollection<Pago> _historialPagos = new();

    // Reportes
    [ObservableProperty]
    private decimal _totalCobrado;

    [ObservableProperty]
    private decimal _totalPendiente;

    public CobrosViewModel(ICobroService cobroService, IPersonaService personaService, IPersonaServicioService personaServicioService, IEmpresaService empresaService, RecibosPdfService recibosPdfService)
    {
        _cobroService = cobroService;
        _personaService = personaService;
        _personaServicioService = personaServicioService;
        _empresaService = empresaService;
        _recibosPdfService = recibosPdfService;
        
        // Establecer período actual
        var ahora = DateTimeOffset.Now;
        PeriodoGenerar = (ahora.Year * 100) + ahora.Month;
        TextoPeriodo = $"{ahora:MMMM yyyy}";
    }

    public async Task InicializarAsync()
    {
        await CargarPersonasConServiciosAsync();
    }

    [RelayCommand]
    private async Task CargarPersonasConServiciosAsync()
    {
        try
        {
            EstaCargando = true;
            HayError = false;
            Mensaje = string.Empty;

            var personas = await _personaService.ObtenerTodasAsync();
            var personasData = new List<PersonaConServicios>();

            foreach (var persona in personas)
            {
                var servicios = await _personaServicioService.ObtenerServiciosActivosPorPersonaAsync(persona.Id);
                
                if (servicios.Any())
                {
                    var totalMensual = servicios.Sum(ps => ps.ObtenerCostoEfectivo());
                    
                    personasData.Add(new PersonaConServicios
                    {
                        Persona = persona,
                        Servicios = servicios.ToList(),
                        CantidadServicios = servicios.Count(),
                        TotalMensual = totalMensual
                    });
                }
            }

            PersonasConServicios = new ObservableCollection<PersonaConServicios>(
                personasData.OrderBy(p => $"{p.Persona.Nombres} {p.Persona.Apellidos}")
            );

            Mensaje = $"Se encontraron {PersonasConServicios.Count} personas con servicios activos";
        }
        catch (Exception ex)
        {
            HayError = true;
            Mensaje = $"Error al cargar personas: {ex.Message}";
        }
        finally
        {
            EstaCargando = false;
        }
    }

    [RelayCommand]
    private async Task SeleccionarPersona(PersonaConServicios? personaConServicios)
    {
        if (personaConServicios == null)
        {
            Mensaje = "No se seleccionó ninguna persona";
            return;
        }

        try
        {
            EstaCargando = true;
            HayError = false;
            Mensaje = $"Cargando cobros de {personaConServicios.NombreCompleto}...";
            
            PersonaSeleccionada = personaConServicios;

            // Cargar cobros de esta persona - SOLO PENDIENTES Y VENCIDOS
            var todosCobros = await _cobroService.ObtenerCobrosPorPersonaAsync(personaConServicios.Persona.Id);
            var cobrosVencidosOPendientes = todosCobros
                .Where(c => c.Estado == EstadoCobro.Pendiente || c.Estado == EstadoCobro.Vencido)
                .Where(c => c.SaldoPendiente > 0) // Solo con saldo pendiente
                .OrderBy(c => c.FechaLimitePago) // Más antiguos primero
                .ToList();
            
            CobrosPersona = new ObservableCollection<Cobro>(cobrosVencidosOPendientes);
            
            if (CobrosPersona.Count > 0)
            {
                Mensaje = $"⚠️ {CobrosPersona.Count} cobro(s) pendiente(s) de pago";
            }
            else
            {
                Mensaje = $"✓ {personaConServicios.NombreCompleto} no tiene cobros pendientes";
            }
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
    private async Task BuscarAsync()
    {
        if (string.IsNullOrWhiteSpace(BusquedaPersona))
        {
            await CargarPersonasConServiciosAsync();
            return;
        }

        try
        {
            EstaCargando = true;
            var busqueda = BusquedaPersona.ToLower();
            
            var personasFiltradas = PersonasConServicios
                .Where(p => 
                    ($"{p.Persona.Nombres} {p.Persona.Apellidos}").ToLower().Contains(busqueda) ||
                    p.Persona.IdentidadNacional?.ToLower().Contains(busqueda) == true
                )
                .ToList();

            PersonasConServicios = new ObservableCollection<PersonaConServicios>(personasFiltradas);
        }
        catch (Exception ex)
        {
            HayError = true;
            Mensaje = $"Error en búsqueda: {ex.Message}";
        }
        finally
        {
            EstaCargando = false;
        }
    }

    [RelayCommand]
    private async Task MostrarTodosAsync()
    {
        BusquedaPersona = string.Empty;
        await CargarPersonasConServiciosAsync();
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
            HayError = false;
            
            // Recargar personas para mostrar nuevos cobros
            await CargarPersonasConServiciosAsync();
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
    private async Task GenerarCobroParaPersonaAsync()
    {
        if (PersonaSeleccionada == null)
        {
            HayError = true;
            Mensaje = "Debe seleccionar una persona";
            return;
        }

        try
        {
            EstaCargando = true;
            HayError = false;
            Mensaje = "Generando cobro...";

            var cobro = await _cobroService.GenerarCobroParaPersonaAsync(
                PersonaSeleccionada.Persona.Id, 
                PeriodoGenerar
            );

            if (cobro != null)
            {
                Mensaje = $"✓ Cobro {cobro.NumeroRecibo} generado correctamente. Monto: ${cobro.MontoTotal:N2}";
                HayError = false;
                
                // Recargar cobros de la persona
                await SeleccionarPersona(PersonaSeleccionada);
            }
            else
            {
                Mensaje = "Esta persona no tiene servicios activos para cobrar";
                HayError = true;
            }
        }
        catch (Exception ex)
        {
            HayError = true;
            Mensaje = $"❌ Error al generar cobro: {ex.Message}";
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

        if (MontoPago > CobroSeleccionado.SaldoPendiente)
        {
            HayError = true;
            Mensaje = $"El monto no puede ser mayor al saldo pendiente (${CobroSeleccionado.SaldoPendiente:N2})";
            return;
        }

        try
        {
            EstaCargando = true;
            HayError = false;
            Mensaje = "Registrando pago...";

            var pago = await _cobroService.RegistrarPagoAsync(
                CobroSeleccionado.Id,
                MontoPago,
                MetodoPago,
                usuarioId: 1, // TODO: Obtener del usuario actual
                observaciones: ObservacionesPago
            );

            Mensaje = $"✓ Pago registrado. Recibo: {pago.NumeroReciboPago} - L.{pago.Monto:N2}";
            HayError = false;
            
            // Generar PDF del recibo
            try
            {
                Mensaje = "Generando recibo en PDF...";
                
                // Obtener los datos de la empresa
                var empresa = await _empresaService.ObtenerDatosEmpresaAsync();
                if (empresa == null)
                {
                    throw new Exception("No se encontraron los datos de la empresa");
                }
                
                var rutaPdf = _recibosPdfService.GenerarReciboPago(pago, CobroSeleccionado, PersonaSeleccionada!.Persona, empresa);
                
                Mensaje = $"✓ Pago registrado y recibo generado: {pago.NumeroReciboPago}";
                
                // Abrir el PDF automáticamente para impresión
                _recibosPdfService.AbrirPdf(rutaPdf);
            }
            catch (Exception exPdf)
            {
                Mensaje = $"✓ Pago registrado, pero error al generar PDF: {exPdf.Message}";
            }
            
            // Recargar cobros de la persona seleccionada
            if (PersonaSeleccionada != null)
            {
                await SeleccionarPersona(PersonaSeleccionada);
            }
            
            // Limpiar formulario
            CobroSeleccionado = null;
            MontoPago = 0;
            ObservacionesPago = null;
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
    private void Cerrar()
    {
        CerrarSolicitado?.Invoke(this, EventArgs.Empty);
    }

    [RelayCommand]
    private async Task VerHistorialAsync()
    {
        if (PersonaSeleccionada == null)
        {
            Mensaje = "Debe seleccionar una persona primero";
            HayError = true;
            return;
        }

        await CargarHistorialAsync();
    }

    [RelayCommand]
    private async Task CargarHistorialAsync()
    {
        if (PersonaSeleccionada == null) return;

        try
        {
            EstaCargando = true;
            HayError = false;
            Mensaje = "Cargando historial de pagos...";

            var cobros = await _cobroService.ObtenerCobrosPorPersonaAsync(PersonaSeleccionada.Persona.Id);
            var todosPagos = new List<Pago>();

            foreach (var cobro in cobros)
            {
                if (cobro.Pagos != null && cobro.Pagos.Any())
                {
                    todosPagos.AddRange(cobro.Pagos);
                }
            }

            HistorialPagos = new ObservableCollection<Pago>(
                todosPagos.OrderByDescending(p => p.FechaPago)
            );

            Mensaje = $"Se encontraron {HistorialPagos.Count} pagos registrados";
        }
        catch (Exception ex)
        {
            HayError = true;
            Mensaje = $"Error al cargar historial: {ex.Message}";
        }
        finally
        {
            EstaCargando = false;
        }
    }

    [RelayCommand]
    private async Task ReimprimirRecibo(Pago? pago)
    {
        if (pago == null) return;

        try
        {
            EstaCargando = true;
            Mensaje = "Generando recibo...";

            // Obtener datos necesarios para el PDF
            var cobro = await _cobroService.ObtenerCobroPorIdAsync(pago.CobroId);
            if (cobro == null)
            {
                HayError = true;
                Mensaje = "No se encontró el cobro asociado al pago";
                return;
            }

            var persona = await _personaService.ObtenerPorIdAsync(cobro.PersonaId);
            if (persona == null)
            {
                HayError = true;
                Mensaje = "No se encontró la persona asociada al cobro";
                return;
            }

            var empresa = await _empresaService.ObtenerDatosEmpresaAsync();
            if (empresa == null)
            {
                HayError = true;
                Mensaje = "No se encontró la configuración de la empresa";
                return;
            }

            // Generar el PDF
            var rutaPdf = _recibosPdfService.GenerarReciboPago(pago, cobro, persona, empresa);

            // Abrir el PDF
            _recibosPdfService.AbrirPdf(rutaPdf);

            Mensaje = $"Recibo reimpreso: {pago.NumeroReciboPago}";
        }
        catch (Exception ex)
        {
            HayError = true;
            Mensaje = $"Error al reimprimir recibo: {ex.Message}";
        }
        finally
        {
            EstaCargando = false;
        }
    }

    [RelayCommand]
    private async Task CalcularTotalesAsync()
    {
        try
        {
            var todasPersonas = await _personaService.ObtenerTodasAsync();
            decimal totalCobradoCalc = 0;
            decimal totalPendienteCalc = 0;

            foreach (var persona in todasPersonas)
            {
                var cobros = await _cobroService.ObtenerCobrosPorPersonaAsync(persona.Id);
                foreach (var cobro in cobros.Where(c => c.Periodo == PeriodoGenerar))
                {
                    totalCobradoCalc += cobro.MontoPagado;
                    totalPendienteCalc += cobro.SaldoPendiente;
                }
            }

            TotalCobrado = totalCobradoCalc;
            TotalPendiente = totalPendienteCalc;
        }
        catch (Exception ex)
        {
            Mensaje = $"Error al calcular totales: {ex.Message}";
        }
    }

    [RelayCommand]
    private async Task GenerarReporteCobrosAsync()
    {
        try
        {
            EstaCargando = true;
            Mensaje = "Generando reporte de cobros pendientes...";
            
            await CalcularTotalesAsync();
            
            // TODO: Implementar generación de PDF
            Mensaje = "Funcionalidad de reporte PDF próximamente";
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
    private async Task GenerarReportePagosAsync()
    {
        try
        {
            EstaCargando = true;
            Mensaje = "Generando reporte de pagos...";
            
            await CalcularTotalesAsync();
            
            // TODO: Implementar generación de PDF
            Mensaje = "Funcionalidad de reporte PDF próximamente";
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
    private async Task GenerarReporteCompletoAsync()
    {
        try
        {
            EstaCargando = true;
            Mensaje = "Generando reporte completo del mes...";
            
            await CalcularTotalesAsync();
            
            // TODO: Implementar generación de PDF
            Mensaje = "Funcionalidad de reporte PDF próximamente";
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
    private async Task ExportarExcelAsync()
    {
        try
        {
            EstaCargando = true;
            Mensaje = "Exportando a Excel...";
            
            await CalcularTotalesAsync();
            
            // TODO: Implementar exportación a Excel
            Mensaje = "Funcionalidad de exportación a Excel próximamente";
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
}

/// <summary>
/// Clase auxiliar para mostrar persona con sus servicios
/// </summary>
public class PersonaConServicios
{
    public Persona Persona { get; set; } = null!;
    public List<PersonaServicio> Servicios { get; set; } = new();
    public int CantidadServicios { get; set; }
    public decimal TotalMensual { get; set; }
    
    public string NombreCompleto => $"{Persona.Nombres} {Persona.Apellidos}";
    public string Identidad => Persona.IdentidadNacional ?? "Sin documento";
    public string ResumenServicios => $"{CantidadServicios} servicio(s) - L.{TotalMensual:N2}/mes";
}
