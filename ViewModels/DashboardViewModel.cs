using SistemaComunidad.Data.Entities;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using SistemaComunidad.Business.Interfaces;
using SistemaComunidad.Views;
using System.Threading.Tasks;
using Avalonia.Controls.ApplicationLifetimes;

namespace SistemaComunidad.ViewModels;

public partial class DashboardViewModel : ViewModelBase
{
    public Usuario UsuarioActual { get; }
    public string NombreUsuario => UsuarioActual.NombreCompleto;
    public string RolUsuario => UsuarioActual.Rol.ToString();

    public DashboardViewModel(Usuario usuario)
    {
        UsuarioActual = usuario;
    }

    [RelayCommand]
    private async Task AbrirMiEmpresaAsync()
    {
        var empresaService = Program.Services?.GetRequiredService<IEmpresaService>();
        if (empresaService != null)
        {
            var viewModel = new EmpresaViewModel(empresaService);
            var window = new EmpresaWindow(viewModel);
            
            // Obtener la ventana principal
            if (Avalonia.Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                await window.ShowDialog(desktop.MainWindow!);
            }
        }
    }
}
