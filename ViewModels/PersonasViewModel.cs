using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SistemaComunidad.Business.Interfaces;
using SistemaComunidad.Data.Entities;

namespace SistemaComunidad.ViewModels;

/// <summary>
/// ViewModel para el módulo de gestión de Personas
/// </summary>
public partial class PersonasViewModel : ViewModelBase
{
    private readonly IPersonaService _personaService;

    [ObservableProperty]
    private ObservableCollection<Persona> _personas = new();

    [ObservableProperty]
    private Persona? _personaSeleccionada;

    [ObservableProperty]
    private string _textoBusqueda = string.Empty;

    [ObservableProperty]
    private bool _estaCargando;

    [ObservableProperty]
    private bool _modoEdicion;

    [ObservableProperty]
    private string _mensaje = string.Empty;

    [ObservableProperty]
    private bool _hayError;

    // Campos del formulario
    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    private string _nombres = string.Empty;

    [ObservableProperty]
    private string _apellidos = string.Empty;

    [ObservableProperty]
    private string? _identidadNacional;

    [ObservableProperty]
    private DateTimeOffset? _fechaNacimiento;

    [ObservableProperty]
    private string? _telefono;

    [ObservableProperty]
    private string? _email;

    [ObservableProperty]
    private string? _direccion;

    [ObservableProperty]
    private EstadoParticipacion _estadoParticipacion = EstadoParticipacion.Activo;

    [ObservableProperty]
    private string? _notas;

    public PersonasViewModel(IPersonaService personaService)
    {
        _personaService = personaService;
    }

    public async Task InicializarAsync()
    {
        await CargarPersonasAsync();
    }

    [RelayCommand]
    private async Task CargarPersonasAsync()
    {
        try
        {
            EstaCargando = true;
            HayError = false;
            Mensaje = string.Empty;

            var personas = await _personaService.ObtenerTodasAsync();
            Personas = new ObservableCollection<Persona>(personas);
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
    private async Task BuscarAsync()
    {
        try
        {
            EstaCargando = true;
            HayError = false;

            var personas = await _personaService.BuscarAsync(TextoBusqueda);
            Personas = new ObservableCollection<Persona>(personas);
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

    [RelayCommand]
    private void NuevoRegistro()
    {
        LimpiarFormulario();
        ModoEdicion = true;
        Mensaje = "Ingrese los datos de la nueva persona";
    }

    [RelayCommand]
    private void EditarPersona(Persona? persona)
    {
        if (persona == null) return;

        PersonaSeleccionada = persona;
        CargarDatosFormulario(persona);
        ModoEdicion = true;
        Mensaje = "Modifique los datos necesarios";
    }

    [RelayCommand]
    private async Task GuardarAsync()
    {
        try
        {
            EstaCargando = true;
            HayError = false;
            Mensaje = string.Empty;

            var persona = new Persona
            {
                Id = Id,
                Nombres = Nombres?.Trim() ?? string.Empty,
                Apellidos = Apellidos?.Trim() ?? string.Empty,
                IdentidadNacional = string.IsNullOrWhiteSpace(IdentidadNacional) ? null : IdentidadNacional.Trim(),
                FechaNacimiento = FechaNacimiento,
                Telefono = string.IsNullOrWhiteSpace(Telefono) ? null : Telefono.Trim(),
                Email = string.IsNullOrWhiteSpace(Email) ? null : Email.Trim(),
                Direccion = string.IsNullOrWhiteSpace(Direccion) ? null : Direccion.Trim(),
                EstadoParticipacion = EstadoParticipacion,
                Notas = string.IsNullOrWhiteSpace(Notas) ? null : Notas.Trim()
            };

            if (Id == 0)
            {
                await _personaService.CrearAsync(persona);
                Mensaje = "✓ Persona registrada exitosamente";
            }
            else
            {
                await _personaService.ActualizarAsync(persona);
                Mensaje = "✓ Persona actualizada exitosamente";
            }

            await CargarPersonasAsync();
            CancelarEdicion();
        }
        catch (Exception ex)
        {
            HayError = true;
            Mensaje = $"Error al guardar: {ex.Message}";
        }
        finally
        {
            EstaCargando = false;
        }
    }

    [RelayCommand]
    private async Task EliminarAsync(Persona? persona)
    {
        if (persona == null) return;

        try
        {
            EstaCargando = true;
            await _personaService.EliminarAsync(persona.Id);
            Mensaje = "✓ Persona eliminada exitosamente";
            await CargarPersonasAsync();
        }
        catch (Exception ex)
        {
            HayError = true;
            Mensaje = $"Error al eliminar: {ex.Message}";
        }
        finally
        {
            EstaCargando = false;
        }
    }

    [RelayCommand]
    private void CancelarEdicion()
    {
        ModoEdicion = false;
        LimpiarFormulario();
        PersonaSeleccionada = null;
        Mensaje = string.Empty;
    }

    private void CargarDatosFormulario(Persona persona)
    {
        Id = persona.Id;
        Nombres = persona.Nombres;
        Apellidos = persona.Apellidos;
        IdentidadNacional = persona.IdentidadNacional;
        FechaNacimiento = persona.FechaNacimiento;
        Telefono = persona.Telefono;
        Email = persona.Email;
        Direccion = persona.Direccion;
        EstadoParticipacion = persona.EstadoParticipacion;
        Notas = persona.Notas;
    }

    private void LimpiarFormulario()
    {
        Id = 0;
        Nombres = string.Empty;
        Apellidos = string.Empty;
        IdentidadNacional = null;
        FechaNacimiento = null;
        Telefono = null;
        Email = null;
        Direccion = null;
        EstadoParticipacion = EstadoParticipacion.Activo;
        Notas = null;
    }
}
