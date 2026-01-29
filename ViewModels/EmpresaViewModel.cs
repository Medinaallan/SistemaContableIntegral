using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SistemaComunidad.Business.Interfaces;
using SistemaComunidad.Data.Entities;

namespace SistemaComunidad.ViewModels;

/// <summary>
/// ViewModel para el módulo Mi Empresa
/// </summary>
public partial class EmpresaViewModel : ViewModelBase
{
    private readonly IEmpresaService _empresaService;

    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    private string _razonSocial = string.Empty;

    [ObservableProperty]
    private string _nombreComercial = string.Empty;

    [ObservableProperty]
    private string _rtn = string.Empty;

    [ObservableProperty]
    private string _numeroTelefono = string.Empty;

    [ObservableProperty]
    private string _direccion = string.Empty;

    [ObservableProperty]
    private string _correoElectronico = string.Empty;

    [ObservableProperty]
    private string? _representante;

    [ObservableProperty]
    private string? _telefonoRepresentante;

    [ObservableProperty]
    private string _formatoRecibo = "MediaCarta";

    public int FormatoReciboIndex
    {
        get => FormatoRecibo == "Ticket80mm" ? 1 : 0;
        set
        {
            var nuevoFormato = value == 1 ? "Ticket80mm" : "MediaCarta";
            if (FormatoRecibo != nuevoFormato)
            {
                FormatoRecibo = nuevoFormato;
                OnPropertyChanged(nameof(FormatoReciboIndex));
            }
        }
    }

    [ObservableProperty]
    private bool _esNuevoRegistro;

    [ObservableProperty]
    private bool _estaCargando;

    [ObservableProperty]
    private string _mensaje = string.Empty;

    [ObservableProperty]
    private bool _hayError;

    public EmpresaViewModel(IEmpresaService empresaService)
    {
        _empresaService = empresaService;
    }

    /// <summary>
    /// Inicializa el ViewModel cargando los datos de la empresa
    /// </summary>
    public async Task InicializarAsync()
    {
        try
        {
            EstaCargando = true;
            HayError = false;
            Mensaje = string.Empty;

            var existeRegistro = await _empresaService.ExisteRegistroEmpresaAsync();

            if (existeRegistro)
            {
                // Cargar datos existentes
                var empresa = await _empresaService.ObtenerDatosEmpresaAsync();
                if (empresa != null)
                {
                    CargarDatosEmpresa(empresa);
                    EsNuevoRegistro = false;
                    Mensaje = "Datos de la empresa cargados. Puede actualizar la información.";
                }
            }
            else
            {
                // Es el primer registro
                EsNuevoRegistro = true;
                LimpiarFormulario();
                Mensaje = "No hay datos de empresa registrados. Por favor ingrese la información.";
            }
        }
        catch (Exception ex)
        {
            HayError = true;
            Mensaje = $"Error al cargar datos: {ex.Message}";
        }
        finally
        {
            EstaCargando = false;
        }
    }

    /// <summary>
    /// Guarda o actualiza los datos de la empresa
    /// </summary>
    [RelayCommand]
    private async Task GuardarAsync()
    {
        try
        {
            EstaCargando = true;
            HayError = false;
            Mensaje = string.Empty;

            var empresa = new Empresa
            {
                Id = Id,
                RazonSocial = RazonSocial?.Trim() ?? string.Empty,
                NombreComercial = NombreComercial?.Trim() ?? string.Empty,
                RTN = Rtn?.Trim() ?? string.Empty,
                NumeroTelefono = NumeroTelefono?.Trim() ?? string.Empty,
                Direccion = Direccion?.Trim() ?? string.Empty,
                CorreoElectronico = CorreoElectronico?.Trim() ?? string.Empty,
                Representante = string.IsNullOrWhiteSpace(Representante) ? null : Representante.Trim(),
                TelefonoRepresentante = string.IsNullOrWhiteSpace(TelefonoRepresentante) ? null : TelefonoRepresentante.Trim(),
                FormatoRecibo = FormatoRecibo ?? "MediaCarta"
            };

            if (EsNuevoRegistro)
            {
                var empresaCreada = await _empresaService.CrearEmpresaAsync(empresa);
                CargarDatosEmpresa(empresaCreada);
                EsNuevoRegistro = false;
                Mensaje = "✓ Datos de la empresa guardados exitosamente.";
            }
            else
            {
                var empresaActualizada = await _empresaService.ActualizarEmpresaAsync(empresa);
                CargarDatosEmpresa(empresaActualizada);
                Mensaje = "✓ Datos de la empresa actualizados exitosamente.";
            }
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
    /// Carga los datos de una empresa en el formulario
    /// </summary>
    private void CargarDatosEmpresa(Empresa empresa)
    {
        Id = empresa.Id;
        RazonSocial = empresa.RazonSocial;
        NombreComercial = empresa.NombreComercial;
        Rtn = empresa.RTN;
        NumeroTelefono = empresa.NumeroTelefono;
        Direccion = empresa.Direccion;
        CorreoElectronico = empresa.CorreoElectronico;
        Representante = empresa.Representante;
        TelefonoRepresentante = empresa.TelefonoRepresentante;
        FormatoRecibo = empresa.FormatoRecibo ?? "MediaCarta";
        OnPropertyChanged(nameof(FormatoReciboIndex));
    }

    /// <summary>
    /// Limpia el formulario
    /// </summary>
    private void LimpiarFormulario()
    {
        Id = 0;
        RazonSocial = string.Empty;
        NombreComercial = string.Empty;
        Rtn = string.Empty;
        NumeroTelefono = string.Empty;
        Direccion = string.Empty;
        CorreoElectronico = string.Empty;
        Representante = null;
        TelefonoRepresentante = null;
        FormatoRecibo = "MediaCarta";
    }
}
