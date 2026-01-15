using Avalonia.Controls;
using SistemaComunidad.ViewModels;

namespace SistemaComunidad.Views;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
        
        var viewModel = new LoginViewModel();
        DataContext = viewModel;
        
        viewModel.LoginExitoso += (sender, usuario) =>
        {
            var dashboard = new MainWindow();
            dashboard.Show();
            this.Close();
        };
    }
}
