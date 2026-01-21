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
/// ViewModel para gestionar los servicios del patronato
/// </summary>
public partial class ServiciosViewModel : ViewModelBase
{
    private readonly IServicioService _servicioService;
    private readonly IEmpresaService _empresaService;

    [ObservableProperty]
    private ObservableCollection<Servicio> _servicios = new();

    [ObservableProperty]
    private Servicio? _servicioSeleccionado;

    [ObservableProperty]
    private string _nombre = string.Empty;

    [ObservableProperty]
    private string _descripcion = string.Empty;

    [ObservableProperty]
    private decimal _costoMensual;

    [ObservableProperty]
    private PeriodicidadServicio _periodicidadSeleccionada = PeriodicidadServicio.Mensual;

    [ObservableProperty]
    private bool _esObligatorio = true;

    [ObservableProperty]
    private string _notas = string.Empty;

    [ObservableProperty]
    private bool _estaCargando;

    [ObservableProperty]
    private bool _esEdicion;

    [ObservableProperty]
    private string _mensaje = string.Empty;

    [ObservableProperty]
    private bool _hayError;

    [ObservableProperty]
    private int? _empresaId;

    public ObservableCollection<PeriodicidadServicio> Periodicidades { get; } = new()
    {
        PeriodicidadServicio.Mensual,
        PeriodicidadServicio.Bimestral,
        PeriodicidadServicio.Trimestral,
        PeriodicidadServicio.Semestral,
        PeriodicidadServicio.Anual
    };

    public ServiciosViewModel(IServicioService servicioService, IEmpresaService empresaService)
    {
        _servicioService = servicioService;
        _empresaService = empresaService;
    }

    /// <summary>
    /// Inicializa el ViewModel cargando los servicios
    /// </summary>
    public async Task InicializarAsync()
    {
        try
        {
            EstaCargando = true;
            HayError = false;

            // Obtener la empresa actual
            var empresa = await _empresaService.ObtenerDatosEmpresaAsync();
            EmpresaId = empresa?.Id;

            await CargarServiciosAsync();
            LimpiarFormulario();

            Mensaje = "Servicios cargados correctamente.";
        }
        catch (Exception ex)
        {
            HayError = true;
            Mensaje = $"Error al inicializar: {ex.Message}";
        }
        finally
        {
            EstaCargando = false;
        }
    }

    /// <summary>
    /// Carga todos los servicios activos
    /// </summary>
    private async Task CargarServiciosAsync()
    {
        var servicios = await _servicioService.ObtenerServiciosActivosAsync();
        Servicios.Clear();
        foreach (var servicio in servicios.OrderBy(s => s.Nombre))
        {
            Servicios.Add(servicio);
        }
    }

    /// <summary>
    /// Comando para crear un nuevo servicio
    /// </summary>
    [RelayCommand]
    private async Task GuardarAsync()
    {
        try
        {
            EstaCargando = true;
            HayError = false;
            Mensaje = string.Empty;

            var servicio = new Servicio
            {
                Nombre = Nombre,
                Descripcion = Descripcion,
                CostoMensual = CostoMensual,
                Periodicidad = PeriodicidadSeleccionada,
                EsObligatorio = EsObligatorio,
                Notas = Notas,
                EmpresaId = EmpresaId
            };

            if (EsEdicion && ServicioSeleccionado != null)
            {
                servicio.Id = ServicioSeleccionado.Id;
                await _servicioService.ActualizarAsync(servicio);
                Mensaje = "Servicio actualizado correctamente.";
            }
            else
            {
                await _servicioService.CrearAsync(servicio);
                Mensaje = "Servicio creado correctamente.";
            }

            await CargarServiciosAsync();
            LimpiarFormulario();
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

    /// <summary>
    /// Comando para editar un servicio seleccionado
    /// </summary>
    [RelayCommand]
    private void EditarServicio(Servicio? servicio)
    {
        if (servicio == null) return;

        ServicioSeleccionado = servicio;
        Nombre = servicio.Nombre;
        Descripcion = servicio.Descripcion ?? string.Empty;
        CostoMensual = servicio.CostoMensual;
        PeriodicidadSeleccionada = servicio.Periodicidad;
        EsObligatorio = servicio.EsObligatorio;
        Notas = servicio.Notas ?? string.Empty;
        EsEdicion = true;

        Mensaje = $"Editando servicio: {servicio.Nombre}";
    }

    /// <summary>
    /// Comando para eliminar un servicio
    /// </summary>
    [RelayCommand]
    private async Task EliminarAsync(Servicio? servicio)
    {
        if (servicio == null) return;

        try
        {
            EstaCargando = true;
            HayError = false;

            await _servicioService.EliminarAsync(servicio.Id);
            await CargarServiciosAsync();
            LimpiarFormulario();

            Mensaje = $"Servicio '{servicio.Nombre}' eliminado correctamente.";
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

    /// <summary>
    /// Comando para activar un servicio
    /// </summary>
    [RelayCommand]
    private async Task ActivarAsync(Servicio? servicio)
    {
        if (servicio == null) return;

        try
        {
            EstaCargando = true;
            HayError = false;

            await _servicioService.ActivarAsync(servicio.Id);
            await CargarServiciosAsync();

            Mensaje = $"Servicio '{servicio.Nombre}' activado correctamente.";
        }
        catch (Exception ex)
        {
            HayError = true;
            Mensaje = $"Error al activar: {ex.Message}";
        }
        finally
        {
            EstaCargando = false;
        }
    }

    /// <summary>
    /// Comando para desactivar un servicio
    /// </summary>
    [RelayCommand]
    private async Task DesactivarAsync(Servicio? servicio)
    {
        if (servicio == null) return;

        try
        {
            EstaCargando = true;
            HayError = false;

            await _servicioService.DesactivarAsync(servicio.Id);
            await CargarServiciosAsync();

            Mensaje = $"Servicio '{servicio.Nombre}' desactivado correctamente.";
        }
        catch (Exception ex)
        {
            HayError = true;
            Mensaje = $"Error al desactivar: {ex.Message}";
        }
        finally
        {
            EstaCargando = false;
        }
    }

    /// <summary>
    /// Comando para cancelar la edición
    /// </summary>
    [RelayCommand]
    private void Cancelar()
    {
        LimpiarFormulario();
        Mensaje = "Edición cancelada.";
    }

    /// <summary>
    /// Limpia el formulario
    /// </summary>
    private void LimpiarFormulario()
    {
        ServicioSeleccionado = null;
        Nombre = string.Empty;
        Descripcion = string.Empty;
        CostoMensual = 0;
        PeriodicidadSeleccionada = PeriodicidadServicio.Mensual;
        EsObligatorio = true;
        Notas = string.Empty;
        EsEdicion = false;
    }

    /// <summary>
    /// Valida que el formulario esté completo
    /// </summary>
    private bool ValidarFormulario()
    {
        if (string.IsNullOrWhiteSpace(Nombre))
        {
            Mensaje = "El nombre del servicio es requerido.";
            HayError = true;
            return false;
        }

        if (CostoMensual < 0)
        {
            Mensaje = "El costo mensual no puede ser negativo.";
            HayError = true;
            return false;
        }

        return true;
    }
}
