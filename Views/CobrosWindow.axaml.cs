using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using SistemaComunidad.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaComunidad.Views;

public partial class CobrosWindow : Window
{
    public CobrosWindow()
    {
        InitializeComponent();
        
        // Suscribirse al evento de cierre del ViewModel
        this.Opened += (s, e) =>
        {
            if (DataContext is CobrosViewModel viewModel)
            {
                viewModel.CerrarSolicitado += (sender, args) => Close();
            }
        };
    }

    private void OnCerrarClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}
