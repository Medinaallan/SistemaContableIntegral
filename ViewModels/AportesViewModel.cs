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

public partial class AportesViewModel : ViewModelBase
{
    private readonly IAporteService _aporteService;
    private readonly IPersonaService _personaService;

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

    public AportesViewModel(IAporteService aporteService, IPersonaService personaService)
    {
        _aporteService = aporteService;
        _personaService = personaService;
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
}
