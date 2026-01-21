using System;
using System.IO;
using System.Linq;
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
        var logFile = Path.Combine(AppContext.BaseDirectory, "error_log.txt");
        try
        {
            File.WriteAllText(logFile, "Iniciando aplicación...\n");
            ConfigurarServicios();
            File.AppendAllText(logFile, "Servicios configurados\n");
            InicializarBaseDatos();
            File.AppendAllText(logFile, "Base de datos inicializada\n");
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }
        catch (Exception ex)
        {
            var errorMsg = $"Error: {ex.Message}\nStackTrace: {ex.StackTrace}\n";
            if (ex.InnerException != null)
            {
                errorMsg += $"Inner Exception: {ex.InnerException.Message}\n";
            }
            File.WriteAllText(logFile, errorMsg);
            Console.WriteLine(errorMsg);
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
        services.AddScoped<IServicioRepositorio, ServicioRepositorio>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Servicios
        services.AddScoped<IAutenticacionService, AutenticacionService>();
        services.AddScoped<IEmpresaService, EmpresaService>();
        services.AddScoped<IPersonaService, PersonaService>();
        services.AddScoped<IServicioService, ServicioService>();

        Services = services.BuildServiceProvider();
    }

    private static void InicializarBaseDatos()
    {
        using var scope = Services!.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<SistemaComunidadDbContext>();
        
        try
        {
            // Verificar si la base de datos existe y puede conectarse
            if (context.Database.CanConnect())
            {
                Console.WriteLine("Conexión a la base de datos establecida correctamente");
                
                // Intentar aplicar migraciones pendientes solo si es necesario
                var pendingMigrations = context.Database.GetPendingMigrations();
                if (pendingMigrations.Any())
                {
                    Console.WriteLine($"Hay {pendingMigrations.Count()} migraciones pendientes. Intentando aplicarlas...");
                    context.Database.Migrate();
                    Console.WriteLine("Migraciones aplicadas correctamente");
                }
                else
                {
                    Console.WriteLine("No hay migraciones pendientes. Base de datos actualizada.");
                }
            }
            else
            {
                Console.WriteLine("Creando base de datos...");
                context.Database.Migrate();
                Console.WriteLine("Base de datos creada correctamente");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Advertencia al inicializar la base de datos: {ex.Message}");
            Console.WriteLine("La aplicación continuará ejecutándose. Si hay problemas, verifique la conexión a la base de datos.");
            // No lanzar la excepción para permitir que la aplicación continúe
        }
    }
}
