using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using SistemaComunidad.Business.Interfaces;
using SistemaComunidad.Data.Entities;

namespace SistemaComunidad.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    private readonly IAutenticacionService _autenticacionService;

    [ObservableProperty]
    private string _nombreUsuario = "admin";

    [ObservableProperty]
    private string _password = "";

    [ObservableProperty]
    private string _mensajeError = string.Empty;

    [ObservableProperty]
    private bool _estaAutenticando = false;

    public Usuario? UsuarioActual { get; private set; }

    public event EventHandler<Usuario>? LoginExitoso;

    public LoginViewModel()
    {
        _autenticacionService = Program.Services!.GetRequiredService<IAutenticacionService>();
    }

    [RelayCommand]
    private async Task IniciarSesionAsync()
    {
        MensajeError = string.Empty;

        if (string.IsNullOrWhiteSpace(NombreUsuario))
        {
            MensajeError = "Ingrese el nombre de usuario";
            return;
        }

        if (string.IsNullOrWhiteSpace(Password))
        {
            MensajeError = "Ingrese la contraseña";
            return;
        }

        EstaAutenticando = true;

        try
        {
            var usuario = await _autenticacionService.IniciarSesionAsync(NombreUsuario, Password);

            if (usuario != null)
            {
                UsuarioActual = usuario;
                LoginExitoso?.Invoke(this, usuario);
            }
            else
            {
                MensajeError = "Usuario o contraseña incorrectos";
            }
        }
        catch (Exception ex)
        {
            MensajeError = $"Error al iniciar sesión: {ex.Message}";
        }
        finally
        {
            EstaAutenticando = false;
        }
    }
}
