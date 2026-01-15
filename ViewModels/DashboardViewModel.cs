using SistemaComunidad.Data.Entities;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using SistemaComunidad.Business.Interfaces;
using SistemaComunidad.Views;

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
    private void AbrirMiEmpresa()
    {
        var empresaService = Program.Services?.GetRequiredService<IEmpresaService>();
        if (empresaService != null)
        {
            var viewModel = new EmpresaViewModel(empresaService);
            var window = new EmpresaWindow(viewModel);
            window.Show();
        }
    }
}
