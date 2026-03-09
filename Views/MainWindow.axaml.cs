using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using Microsoft.Extensions.DependencyInjection;
using SistemaComunidad.Business.Interfaces;
using SistemaComunidad.Business.Services;
using SistemaComunidad.ViewModels;

namespace SistemaComunidad.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CerrarSesion_Click(object? sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private void Dashboard_Click(object? sender, RoutedEventArgs e)
        {
            // Por ahora solo mostramos mensaje
            ShowMessage("Dashboard", "Vista de Dashboard - En desarrollo");
        }

        private void Personas_Click(object? sender, RoutedEventArgs e)
        {
            try
            {
                var personaService = Program.Services?.GetRequiredService<IPersonaService>();
                var personaServicioService = Program.Services?.GetRequiredService<IPersonaServicioService>();
                var servicioService = Program.Services?.GetRequiredService<IServicioService>();
                
                if (personaService != null && personaServicioService != null && servicioService != null)
                {
                    var viewModel = new PersonasViewModel(personaService, personaServicioService, servicioService);
                    var window = new PersonasWindow(viewModel);
                    window.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error", $"No se pudo abrir Personas: {ex.Message}");
            }
        }

        private async void Familias_Click(object? sender, RoutedEventArgs e)
        {
            try
            {
                var nucleoFamiliarService = Program.Services?.GetRequiredService<INucleoFamiliarService>();
                var personaService = Program.Services?.GetRequiredService<IPersonaService>();

                if (nucleoFamiliarService != null && personaService != null)
                {
                    var viewModel = new FamiliasViewModel(nucleoFamiliarService, personaService);
                    var window = new FamiliasWindow(viewModel);
                    await window.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error", $"No se pudo abrir Familias: {ex.Message}");
            }
        }

        private async void Ingresos_Click(object? sender, RoutedEventArgs e)
        {
            try
            {
                var cobroService = Program.Services?.GetRequiredService<ICobroService>();
                var personaService = Program.Services?.GetRequiredService<IPersonaService>();
                var personaServicioService = Program.Services?.GetRequiredService<IPersonaServicioService>();
                var empresaService = Program.Services?.GetRequiredService<IEmpresaService>();
                var recibosPdfService = Program.Services?.GetRequiredService<RecibosPdfService>();
                
                if (cobroService != null && personaService != null && personaServicioService != null && empresaService != null && recibosPdfService != null)
                {
                    var viewModel = new CobrosViewModel(cobroService, personaService, personaServicioService, empresaService, recibosPdfService);
                    var window = new CobrosWindow
                    {
                        DataContext = viewModel
                    };
                    
                    // Inicializar datos
                    await viewModel.InicializarAsync();
                    
                    await window.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error", $"No se pudo abrir Cobros: {ex.Message}");
            }
        }

        private void Egresos_Click(object? sender, RoutedEventArgs e)
        {
            ShowMessage("Egresos", "Registro de Egresos - En desarrollo");
        }

        private async void Aportes_Click(object? sender, RoutedEventArgs e)
        {
            try
            {
                var aporteService = Program.Services?.GetRequiredService<IAporteService>();
                var personaService = Program.Services?.GetRequiredService<IPersonaService>();
                var empresaService = Program.Services?.GetRequiredService<IEmpresaService>();
                var recibosPdfService = Program.Services?.GetRequiredService<RecibosPdfService>();
                if (aporteService != null && personaService != null && empresaService != null && recibosPdfService != null)
                {
                    var viewModel = new AportesViewModel(aporteService, personaService, empresaService, recibosPdfService);
                    var window = new AportesWindow
                    {
                        DataContext = viewModel
                    };

                    // Inicializar datos y mostrar (await para capturar excepciones)
                    await viewModel.InicializarAsync();
                    await window.ShowDialog(this);
                }
                else
                {
                    ShowMessage("Aportes", "Servicios necesarios no disponibles");
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error", $"No se pudo abrir Aportes: {ex.Message}");
            }
        }

        private void Actividades_Click(object? sender, RoutedEventArgs e)
        {
            ShowMessage("Actividades", "Gestión de Actividades - En desarrollo");
        }

        private void Documentos_Click(object? sender, RoutedEventArgs e)
        {
            ShowMessage("Documentos", "Gestión de Documentos - En desarrollo");
        }

        private void Bienes_Click(object? sender, RoutedEventArgs e)
        {
            ShowMessage("Bienes", "Inventario de Bienes - En desarrollo");
        }

        private async void MiEmpresa_Click(object? sender, RoutedEventArgs e)
        {
            try
            {
                var empresaService = Program.Services?.GetRequiredService<IEmpresaService>();
                if (empresaService != null)
                {
                    var viewModel = new EmpresaViewModel(empresaService);
                    var window = new EmpresaWindow(viewModel);
                    await window.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error", $"No se pudo abrir Mi Empresa: {ex.Message}");
            }
        }

        private async void Servicios_Click(object? sender, RoutedEventArgs e)
        {
            try
            {
                var servicioService = Program.Services?.GetRequiredService<IServicioService>();
                var empresaService = Program.Services?.GetRequiredService<IEmpresaService>();
                if (servicioService != null && empresaService != null)
                {
                    var viewModel = new ServiciosViewModel(servicioService, empresaService);
                    var window = new ServiciosWindow
                    {
                        DataContext = viewModel
                    };
                    await window.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error", $"No se pudo abrir Servicios: {ex.Message}");
            }
        }

        private async void ShowMessage(string title, string message)
        {
            var dialog = new Window
            {
                Title = title,
                Width = 400,
                Height = 200,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                CanResize = false,
                Content = new StackPanel
                {
                    Margin = new Avalonia.Thickness(20),
                    Spacing = 20,
                    Children =
                    {
                        new TextBlock
                        {
                            Text = message,
                            FontSize = 14,
                            TextWrapping = Avalonia.Media.TextWrapping.Wrap
                        },
                        new Button
                        {
                            Content = "Aceptar",
                            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                            Padding = new Avalonia.Thickness(30, 10),
                            Background = Avalonia.Media.Brushes.DodgerBlue,
                            Foreground = Avalonia.Media.Brushes.White
                        }
                    }
                }
            };

            ((Button)((StackPanel)dialog.Content).Children[1]).Click += (s, e) => dialog.Close();
            await dialog.ShowDialog(this);
        }
    }
}