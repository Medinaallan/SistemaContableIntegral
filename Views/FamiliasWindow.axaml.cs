using Avalonia.Controls;
using Avalonia.Interactivity;
using SistemaComunidad.ViewModels;

namespace SistemaComunidad.Views;

public partial class FamiliasWindow : Window
{
    public FamiliasWindow()
    {
        InitializeComponent();
    }

    public FamiliasWindow(FamiliasViewModel viewModel) : this()
    {
        DataContext = viewModel;

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
