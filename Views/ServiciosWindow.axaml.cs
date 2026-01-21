using Avalonia.Controls;
using SistemaComunidad.ViewModels;
using System;

namespace SistemaComunidad.Views;

public partial class ServiciosWindow : Window
{
    public ServiciosWindow()
    {
        InitializeComponent();
    }

    protected override async void OnOpened(EventArgs e)
    {
        base.OnOpened(e);

        if (DataContext is ServiciosViewModel viewModel)
        {
            await viewModel.InicializarAsync();
        }
    }
}
