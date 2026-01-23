using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using SistemaComunidad.ViewModels;
using System.Collections.Generic;

namespace SistemaComunidad.Views;

public partial class CobrosWindow : Window
{
    public CobrosWindow()
    {
        InitializeComponent();
    }

    private void OnCerrarClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}

// Helper para valores del ComboBox de métodos de pago
public static class MetodoPagoValues
{
    public static List<string> Values { get; } = new List<string>
    {
        "Efectivo",
        "Tarjeta",
        "Transferencia",
        "Cheque"
    };
}
