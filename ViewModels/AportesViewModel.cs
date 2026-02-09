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

public partial class AportesViewModel : ViewModelBase
{
    private readonly IAporteService _aporteService;
    private readonly IPersonaService _personaService;
    private readonly IEmpresaService _empresaService;
    private readonly RecibosPdfService _recibosPdfService;

    [ObservableProperty]
    private ObservableCollection<Persona> _personas = new();

    [ObservableProperty]
    private Persona? _personaSeleccionada;

    [ObservableProperty]
    private ObservableCollection<Aporte> _aportes = new();

    [ObservableProperty]
    private decimal _monto;

    [ObservableProperty]
    private string _concepto = string.Empty;

    [ObservableProperty]
    private TipoAporte _tipoAporte = TipoAporte.Periodico;

    public AportesViewModel(IAporteService aporteService, IPersonaService personaService, IEmpresaService empresaService, RecibosPdfService recibosPdfService)
    {
        _aporteService = aporteService;
        _personaService = personaService;
        _empresaService = empresaService;
        _recibosPdfService = recibosPdfService;
    }

    // Habilita/Deshabilita el botón Registrar
    public bool CanRegistrarAporte() => PersonaSeleccionada != null && Monto > 0;

    partial void OnPersonaSeleccionadaChanged(Persona? value)
    {
        try { RegistrarAporteCommand.NotifyCanExecuteChanged(); } catch { }
    }

    partial void OnMontoChanged(decimal value)
    {
        try { RegistrarAporteCommand.NotifyCanExecuteChanged(); } catch { }
    }

    [RelayCommand]
    public async Task ImprimirAporteAsync(Aporte aporte)
    {
        if (aporte == null) return;
        Persona persona = aporte.Persona ?? (await _personaService.ObtenerPorIdAsync(aporte.PersonaId))!;
        var empresa = await _empresaService.ObtenerDatosEmpresaAsync();
        var ruta = _recibosPdfService.GenerarReciboAporte(aporte, persona, empresa ?? new Data.Entities.Empresa { RazonSocial = "Empresa" });
        _recibosPdfService.AbrirPdf(ruta);
    }

    public IEnumerable<TipoAporte> TiposAporte => Enum.GetValues(typeof(TipoAporte)).Cast<TipoAporte>();

    [RelayCommand]
    public async Task InicializarAsync()
    {
        var personas = await _personaService.ObtenerTodasAsync();
        Personas = new ObservableCollection<Persona>(personas);
    }

    [RelayCommand]
    public async Task CargarAportesAsync()
    {
        if (PersonaSeleccionada == null) return;
        var lista = await _aporteService.ObtenerAportesPorPersonaAsync(PersonaSeleccionada.Id);
        Aportes = new ObservableCollection<Aporte>(lista);
    }

    [RelayCommand]
    public async Task RegistrarAporteAsync()
    {
        try
        {
            if (PersonaSeleccionada == null) return;
            var aporte = new Aporte
            {
                PersonaId = PersonaSeleccionada.Id,
                Monto = Monto,
                Concepto = Concepto,
                TipoAporte = TipoAporte,
                FechaAporte = DateTimeOffset.Now
            };

            var registrado = await _aporteService.RegistrarAporteAsync(aporte);
            // Refrescar lista
            await CargarAportesAsync();
            // Reset form
            Monto = 0;
            Concepto = string.Empty;
        }
        catch (Exception ex)
        {
            try
            {
                var logDir = System.IO.Path.Combine(AppContext.BaseDirectory ?? ".", "logs");
                System.IO.Directory.CreateDirectory(logDir);
                var path = System.IO.Path.Combine(logDir, "aporte_error.txt");
                System.IO.File.WriteAllText(path, ex.ToString());
            }
            catch { }
            // No rethrow: registrar fallido se registra en logs pero no cierra la aplicación
            return;
        }
    }
}
