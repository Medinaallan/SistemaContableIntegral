using System;
using Avalonia;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using SistemaComunidad.Data.Context;
using SistemaComunidad.Data.Repositories;
using SistemaComunidad.Data.Interfaces;
using SistemaComunidad.Business.Services;
using SistemaComunidad.Business.Interfaces;

namespace SistemaComunidad;

class Program
{
    public static IServiceProvider? Services { get; private set; }

    [STAThread]
    public static void Main(string[] args)
    {
        try
        {
            ConfigurarServicios();
            InicializarBaseDatos();
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al iniciar la aplicación: {ex.Message}");
            throw;
        }
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();

    private static void ConfigurarServicios()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var services = new ServiceCollection();

        // DbContext
        services.AddDbContext<SistemaComunidadDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Repositorios
        services.AddScoped<IPersonaRepositorio, PersonaRepositorio>();
        services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
        services.AddScoped<IEmpresaRepositorio, EmpresaRepositorio>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Servicios
        services.AddScoped<IAutenticacionService, AutenticacionService>();
        services.AddScoped<IEmpresaService, EmpresaService>();

        Services = services.BuildServiceProvider();
    }

    private static void InicializarBaseDatos()
    {
        using var scope = Services!.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<SistemaComunidadDbContext>();
        
        try
        {
            // Aplicar migraciones pendientes
            context.Database.Migrate();
            Console.WriteLine("Base de datos inicializada correctamente");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al inicializar la base de datos: {ex.Message}");
            throw;
        }
    }
}
