using Avalonia.Controls;
using Avalonia.Interactivity;
using SistemaComunidad.ViewModels;

namespace SistemaComunidad.Views;

public partial class ConfirmarCobroWindow : Window
{
    public ConfirmarCobroWindow()
    {
        InitializeComponent();
    }

    private void OnConfirmarClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is ConfirmarCobroViewModel vm)
        {
            vm.DebeImprimir = false;
        }
        Close(true);
    }

    private void OnConfirmarEImprimirClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is ConfirmarCobroViewModel vm)
        {
            vm.DebeImprimir = true;
        }
        Close(true);
    }

    private void OnCancelarClick(object? sender, RoutedEventArgs e)
    {
        Close(false);
    }
}
