using Avalonia.Controls;
using SistemaComunidad.ViewModels;
using System.Threading.Tasks;

namespace SistemaComunidad.Views;

public partial class AportesWindow : Window
{
    public AportesWindow()
    {
        InitializeComponent();
        this.Opened += async (_, __) => await OnOpened();
    }

    private async Task OnOpened()
    {
        if (DataContext is AportesViewModel vm)
        {
            await vm.InicializarAsync();
        }
    }
}
