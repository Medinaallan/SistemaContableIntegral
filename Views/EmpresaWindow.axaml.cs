using Avalonia.Controls;
using Avalonia.Interactivity;
using SistemaComunidad.ViewModels;

namespace SistemaComunidad.Views;

public partial class EmpresaWindow : Window
{
    public EmpresaWindow()
    {
        InitializeComponent();
    }

    public EmpresaWindow(EmpresaViewModel viewModel) : this()
    {
        DataContext = viewModel;
        
        // Inicializar el ViewModel cuando se carga la ventana
        Loaded += async (s, e) =>
        {
            await viewModel.InicializarAsync();
        };
    }

    private void OnCerrarClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}
