using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SistemaComunidad.Business.Interfaces;
using SistemaComunidad.Data.Entities;

namespace SistemaComunidad.ViewModels;

public partial class FamiliasViewModel : ViewModelBase
{
    private readonly INucleoFamiliarService _nucleoFamiliarService;
    private readonly IPersonaService _personaService;

    // === Colecciones ===
    [ObservableProperty]
    private ObservableCollection<NucleoFamiliar> _familias = new();

    [ObservableProperty]
    private NucleoFamiliar? _familiaSeleccionada;

    [ObservableProperty]
    private ObservableCollection<MiembroFamiliar> _miembros = new();

    [ObservableProperty]
    private ObservableCollection<Persona> _personasDisponibles = new();

    [ObservableProperty]
    private ObservableCollection<RolFamiliar> _rolesDisponibles = new(
        Enum.GetValues<RolFamiliar>());

    // === Estado UI ===
    [ObservableProperty]
    private string _textoBusqueda = string.Empty;

    [ObservableProperty]
    private bool _estaCargando;

    [ObservableProperty]
    private bool _modoEdicionFamilia;

    [ObservableProperty]
    private bool _modoEdicionMiembro;

    [ObservableProperty]
    private bool _mostrarMiembros;

    [ObservableProperty]
    private bool _mostrarVincularPersona;

    [ObservableProperty]
    private string _mensaje = string.Empty;

    [ObservableProperty]
    private bool _hayError;

    // === Campos formulario Familia ===
    [ObservableProperty]
    private int _familiaId;

    [ObservableProperty]
    private string _nombreFamilia = string.Empty;

    [ObservableProperty]
    private string? _direccionFamilia;

    [ObservableProperty]
    private string? _telefonoFamilia;

    [ObservableProperty]
    private string? _notasFamilia;

    // === Campos formulario Miembro ===
    [ObservableProperty]
    private int _miembroId;

    [ObservableProperty]
    private string _nombresMiembro = string.Empty;

    [ObservableProperty]
    private string _apellidosMiembro = string.Empty;

    [ObservableProperty]
    private string? _telefonoMiembro;

    [ObservableProperty]
    private string? _notasMiembro;

    [ObservableProperty]
    private RolFamiliar _rolMiembro = RolFamiliar.Otro;

    [ObservableProperty]
    private int? _personaVinculadaId;

    // === Vincular persona ===
    [ObservableProperty]
    private string _busquedaPersona = string.Empty;

    [ObservableProperty]
    private Persona? _personaSeleccionadaVincular;

    [ObservableProperty]
    private RolFamiliar _rolVincular = RolFamiliar.Otro;

    public FamiliasViewModel(INucleoFamiliarService nucleoFamiliarService, IPersonaService personaService)
    {
        _nucleoFamiliarService = nucleoFamiliarService;
        _personaService = personaService;
    }

    public async Task InicializarAsync()
    {
        await CargarFamiliasAsync();
    }

    [RelayCommand]
    private async Task CargarFamiliasAsync()
    {
        try
        {
            EstaCargando = true;
            HayError = false;
            Mensaje = string.Empty;

            var familias = await _nucleoFamiliarService.ObtenerTodosAsync();
            Familias = new ObservableCollection<NucleoFamiliar>(familias);
        }
        catch (Exception ex)
        {
            HayError = true;
            Mensaje = $"Error al cargar familias: {ex.Message}";
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

            var familias = await _nucleoFamiliarService.BuscarAsync(TextoBusqueda);
            Familias = new ObservableCollection<NucleoFamiliar>(familias);
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

    // =============================================
    // CRUD Familia
    // =============================================

    [RelayCommand]
    private void NuevaFamilia()
    {
        LimpiarFormularioFamilia();
        ModoEdicionFamilia = true;
        MostrarMiembros = false;
        ModoEdicionMiembro = false;
        MostrarVincularPersona = false;
        Mensaje = "Ingrese los datos de la nueva familia";
        HayError = false;
    }

    [RelayCommand]
    private void EditarFamilia(NucleoFamiliar? familia)
    {
        if (familia == null) return;

        FamiliaSeleccionada = familia;
        CargarDatosFormularioFamilia(familia);
        ModoEdicionFamilia = true;
        MostrarMiembros = false;
        ModoEdicionMiembro = false;
        MostrarVincularPersona = false;
        Mensaje = "Modifique los datos de la familia";
        HayError = false;
    }

    [RelayCommand]
    private async Task GuardarFamiliaAsync()
    {
        try
        {
            EstaCargando = true;
            HayError = false;

            var familia = new NucleoFamiliar
            {
                Id = FamiliaId,
                Nombre = NombreFamilia?.Trim() ?? string.Empty,
                Direccion = string.IsNullOrWhiteSpace(DireccionFamilia) ? null : DireccionFamilia.Trim(),
                Telefono = string.IsNullOrWhiteSpace(TelefonoFamilia) ? null : TelefonoFamilia.Trim(),
                Notas = string.IsNullOrWhiteSpace(NotasFamilia) ? null : NotasFamilia.Trim()
            };

            if (FamiliaId == 0)
            {
                var creada = await _nucleoFamiliarService.CrearAsync(familia);
                Mensaje = "✓ Familia registrada exitosamente";
                // Ir a gestionar miembros de la nueva familia
                FamiliaSeleccionada = creada;
                FamiliaId = creada.Id;
                await VerMiembrosAsync(creada);
            }
            else
            {
                await _nucleoFamiliarService.ActualizarAsync(familia);
                Mensaje = "✓ Familia actualizada exitosamente";
                ModoEdicionFamilia = false;
            }

            await CargarFamiliasAsync();
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
    private async Task EliminarFamiliaAsync(NucleoFamiliar? familia)
    {
        if (familia == null) return;

        try
        {
            EstaCargando = true;
            HayError = false;

            await _nucleoFamiliarService.EliminarAsync(familia.Id);
            Mensaje = "✓ Familia eliminada exitosamente";

            ModoEdicionFamilia = false;
            MostrarMiembros = false;
            FamiliaSeleccionada = null;

            await CargarFamiliasAsync();
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

    // =============================================
    // Gestión de Miembros
    // =============================================

    [RelayCommand]
    private async Task VerMiembrosAsync(NucleoFamiliar? familia)
    {
        if (familia == null) return;

        try
        {
            EstaCargando = true;
            HayError = false;

            FamiliaSeleccionada = familia;
            CargarDatosFormularioFamilia(familia);

            var miembros = await _nucleoFamiliarService.ObtenerMiembrosPorFamiliaAsync(familia.Id);
            Miembros = new ObservableCollection<MiembroFamiliar>(miembros);

            ModoEdicionFamilia = false;
            ModoEdicionMiembro = false;
            MostrarMiembros = true;
            MostrarVincularPersona = false;
            Mensaje = $"Miembros de {familia.Nombre}";
        }
        catch (Exception ex)
        {
            HayError = true;
            Mensaje = $"Error al cargar miembros: {ex.Message}";
        }
        finally
        {
            EstaCargando = false;
        }
    }

    [RelayCommand]
    private void NuevoMiembro()
    {
        LimpiarFormularioMiembro();
        ModoEdicionMiembro = true;
        MostrarVincularPersona = false;
        HayError = false;
        Mensaje = "Registre un nuevo integrante de la familia";
    }

    [RelayCommand]
    private void EditarMiembro(MiembroFamiliar? miembro)
    {
        if (miembro == null) return;

        CargarDatosFormularioMiembro(miembro);
        ModoEdicionMiembro = true;
        MostrarVincularPersona = false;
        HayError = false;
        Mensaje = "Modifique los datos del integrante";
    }

    [RelayCommand]
    private async Task GuardarMiembroAsync()
    {
        if (FamiliaSeleccionada == null) return;

        try
        {
            EstaCargando = true;
            HayError = false;

            var miembro = new MiembroFamiliar
            {
                Id = MiembroId,
                NucleoFamiliarId = FamiliaSeleccionada.Id,
                Nombres = NombresMiembro?.Trim() ?? string.Empty,
                Apellidos = ApellidosMiembro?.Trim() ?? string.Empty,
                Telefono = string.IsNullOrWhiteSpace(TelefonoMiembro) ? null : TelefonoMiembro.Trim(),
                Notas = string.IsNullOrWhiteSpace(NotasMiembro) ? null : NotasMiembro.Trim(),
                Rol = RolMiembro,
                PersonaId = PersonaVinculadaId
            };

            if (MiembroId == 0)
            {
                await _nucleoFamiliarService.AgregarMiembroAsync(miembro);
                Mensaje = "✓ Integrante agregado exitosamente";
            }
            else
            {
                await _nucleoFamiliarService.ActualizarMiembroAsync(miembro);
                Mensaje = "✓ Integrante actualizado exitosamente";
            }

            ModoEdicionMiembro = false;
            await VerMiembrosAsync(FamiliaSeleccionada);
            await CargarFamiliasAsync();
        }
        catch (Exception ex)
        {
            HayError = true;
            Mensaje = $"Error al guardar miembro: {ex.Message}";
        }
        finally
        {
            EstaCargando = false;
        }
    }

    [RelayCommand]
    private async Task EliminarMiembroAsync(MiembroFamiliar? miembro)
    {
        if (miembro == null || FamiliaSeleccionada == null) return;

        try
        {
            EstaCargando = true;
            HayError = false;

            await _nucleoFamiliarService.EliminarMiembroAsync(miembro.Id);
            Mensaje = "✓ Integrante eliminado exitosamente";

            await VerMiembrosAsync(FamiliaSeleccionada);
            await CargarFamiliasAsync();
        }
        catch (Exception ex)
        {
            HayError = true;
            Mensaje = $"Error al eliminar miembro: {ex.Message}";
        }
        finally
        {
            EstaCargando = false;
        }
    }

    // =============================================
    // Vincular Persona Existente
    // =============================================

    [RelayCommand]
    private async Task MostrarVincularAsync()
    {
        try
        {
            EstaCargando = true;
            ModoEdicionMiembro = false;
            MostrarVincularPersona = true;
            HayError = false;
            BusquedaPersona = string.Empty;
            PersonaSeleccionadaVincular = null;
            RolVincular = RolFamiliar.Otro;

            var personas = await _personaService.ObtenerTodasAsync();
            PersonasDisponibles = new ObservableCollection<Persona>(personas);
            Mensaje = "Seleccione una persona existente para vincular a esta familia";
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
    private async Task BuscarPersonaAsync()
    {
        try
        {
            EstaCargando = true;
            HayError = false;

            var personas = await _personaService.BuscarAsync(BusquedaPersona);
            PersonasDisponibles = new ObservableCollection<Persona>(personas);
        }
        catch (Exception ex)
        {
            HayError = true;
            Mensaje = $"Error al buscar personas: {ex.Message}";
        }
        finally
        {
            EstaCargando = false;
        }
    }

    [RelayCommand]
    private async Task VincularPersonaAsync(Persona? persona)
    {
        if (persona == null || FamiliaSeleccionada == null) return;

        try
        {
            EstaCargando = true;
            HayError = false;

            await _nucleoFamiliarService.VincularPersonaAFamiliaAsync(
                FamiliaSeleccionada.Id, persona.Id, RolVincular);

            Mensaje = $"✓ {persona.Nombres} {persona.Apellidos} vinculado(a) a la familia";
            MostrarVincularPersona = false;

            await VerMiembrosAsync(FamiliaSeleccionada);
            await CargarFamiliasAsync();
        }
        catch (Exception ex)
        {
            HayError = true;
            Mensaje = $"Error al vincular: {ex.Message}";
        }
        finally
        {
            EstaCargando = false;
        }
    }

    // =============================================
    // Cancelar / Volver
    // =============================================

    [RelayCommand]
    private void CancelarEdicionFamilia()
    {
        ModoEdicionFamilia = false;
        MostrarMiembros = false;
        ModoEdicionMiembro = false;
        MostrarVincularPersona = false;
        LimpiarFormularioFamilia();
        FamiliaSeleccionada = null;
        Mensaje = string.Empty;
        HayError = false;
    }

    [RelayCommand]
    private void CancelarEdicionMiembro()
    {
        ModoEdicionMiembro = false;
        MostrarVincularPersona = false;
        LimpiarFormularioMiembro();
        HayError = false;
        if (FamiliaSeleccionada != null)
            Mensaje = $"Miembros de {FamiliaSeleccionada.Nombre}";
    }

    [RelayCommand]
    private void CancelarVincular()
    {
        MostrarVincularPersona = false;
        HayError = false;
        if (FamiliaSeleccionada != null)
            Mensaje = $"Miembros de {FamiliaSeleccionada.Nombre}";
    }

    // =============================================
    // Helpers
    // =============================================

    private void CargarDatosFormularioFamilia(NucleoFamiliar familia)
    {
        FamiliaId = familia.Id;
        NombreFamilia = familia.Nombre;
        DireccionFamilia = familia.Direccion;
        TelefonoFamilia = familia.Telefono;
        NotasFamilia = familia.Notas;
    }

    private void LimpiarFormularioFamilia()
    {
        FamiliaId = 0;
        NombreFamilia = string.Empty;
        DireccionFamilia = null;
        TelefonoFamilia = null;
        NotasFamilia = null;
    }

    private void CargarDatosFormularioMiembro(MiembroFamiliar miembro)
    {
        MiembroId = miembro.Id;
        NombresMiembro = miembro.Nombres;
        ApellidosMiembro = miembro.Apellidos;
        TelefonoMiembro = miembro.Telefono;
        NotasMiembro = miembro.Notas;
        RolMiembro = miembro.Rol;
        PersonaVinculadaId = miembro.PersonaId;
    }

    private void LimpiarFormularioMiembro()
    {
        MiembroId = 0;
        NombresMiembro = string.Empty;
        ApellidosMiembro = string.Empty;
        TelefonoMiembro = null;
        NotasMiembro = null;
        RolMiembro = RolFamiliar.Otro;
        PersonaVinculadaId = null;
    }
}
