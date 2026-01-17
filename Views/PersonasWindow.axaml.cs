using Avalonia.Controls;
using Avalonia.Interactivity;
using SistemaComunidad.ViewModels;
using SistemaComunidad.Data.Entities;
using System.Collections.Generic;

namespace SistemaComunidad.Views;

public partial class PersonasWindow : Window
{
    public PersonasWindow()
    {
        InitializeComponent();
    }

    public PersonasWindow(PersonasViewModel viewModel) : this()
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

// Helper para los valores del ComboBox
public static class EstadoParticipacionValues
{
    public static IEnumerable<EstadoParticipacion> Values => new[]
    {
        EstadoParticipacion.Activo,
        EstadoParticipacion.Inactivo,
        EstadoParticipacion.Visitante
    };
}
