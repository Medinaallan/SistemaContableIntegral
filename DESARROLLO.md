# Guía de Desarrollo - Sistema Comunitario Integral

## 📋 Estado del Proyecto

✅ **Completado:**
- Estructura base con Avalonia UI
- Entity Framework Core configurado
- 12 entidades de dominio completas
- Repositorios y Unit of Work implementados
- Sistema de autenticación básico
- Arquitectura en capas profesional

🚧 **Pendiente:**
- Vistas de gestión (CRUD)
- Formularios de entrada
- Dashboard administrativo
- Sistema de reportes
- Módulo de respaldos

## 🎓 Conceptos Clave

### Arquitectura en Capas

El proyecto sigue una arquitectura de 3 capas:

```
┌─────────────────────────────┐
│  Presentación (Views/VM)    │ ← Interfaz de usuario
├─────────────────────────────┤
│  Negocio (Services)         │ ← Lógica de negocio
├─────────────────────────────┤
│  Datos (Repositories)       │ ← Acceso a datos
└─────────────────────────────┘
```

### Patrones Implementados

#### 1. Repository Pattern
Abstrae el acceso a datos:

```csharp
// Uso básico
var personas = await _unitOfWork.Personas.ObtenerTodosAsync();
var persona = await _unitOfWork.Personas.ObtenerPorIdAsync(1);
```

#### 2. Unit of Work
Gestiona transacciones:

```csharp
await _unitOfWork.IniciarTransaccionAsync();
try
{
    await _unitOfWork.Personas.AgregarAsync(nuevaPersona);
    await _unitOfWork.Ingresos.AgregarAsync(nuevoIngreso);
    await _unitOfWork.CompletarAsync();
    await _unitOfWork.ConfirmarTransaccionAsync();
}
catch
{
    await _unitOfWork.RevertirTransaccionAsync();
    throw;
}
```

#### 3. MVVM (Model-View-ViewModel)
Separación entre UI y lógica:

```
View (XAML) ←→ ViewModel ←→ Model/Services
```

## 🔧 Tareas Comunes

### Crear una Nueva Entidad

1. **Crear la clase en `Data/Entities/`:**

```csharp
public class MiEntidad : BaseEntity
{
    public string Nombre { get; set; } = string.Empty;
    // Propiedades...
}
```

2. **Agregar al DbContext:**

```csharp
public DbSet<MiEntidad> MisEntidades { get; set; }
```

3. **Configurar en `OnModelCreating`:**

```csharp
modelBuilder.Entity<MiEntidad>(entity =>
{
    entity.ToTable("MisEntidades");
    entity.HasKey(e => e.Id);
    entity.Property(e => e.Nombre).IsRequired().HasMaxLength(200);
});
```

4. **Crear migración:**

```bash
dotnet ef migrations add AgregarMiEntidad
dotnet ef database update
```

### Agregar un Nuevo Repositorio

1. **Crear interfaz en `Data/Interfaces/`:**

```csharp
public interface IMiRepositorio : IRepositorio<MiEntidad>
{
    Task<IEnumerable<MiEntidad>> MetodoEspecifico();
}
```

2. **Implementar en `Data/Repositories/`:**

```csharp
public class MiRepositorio : Repositorio<MiEntidad>, IMiRepositorio
{
    public MiRepositorio(SistemaComunidadDbContext context) : base(context) { }
    
    public async Task<IEnumerable<MiEntidad>> MetodoEspecifico()
    {
        return await _dbSet.Where(x => x.Activo).ToListAsync();
    }
}
```

3. **Registrar en `Program.cs`:**

```csharp
services.AddScoped<IMiRepositorio, MiRepositorio>();
```

### Crear un Nuevo Servicio

1. **Definir interfaz en `Business/Interfaces/`:**

```csharp
public interface IMiServicio
{
    Task<bool> RealizarOperacionAsync(int id);
}
```

2. **Implementar en `Business/Services/`:**

```csharp
public class MiServicio : ServicioBase, IMiServicio
{
    public MiServicio(IUnitOfWork unitOfWork) : base(unitOfWork) { }
    
    public async Task<bool> RealizarOperacionAsync(int id)
    {
        var entidad = await UnitOfWork.MisEntidades.ObtenerPorIdAsync(id);
        if (entidad == null) return false;
        
        // Lógica de negocio...
        await UnitOfWork.CompletarAsync();
        return true;
    }
}
```

3. **Registrar en `Program.cs`:**

```csharp
services.AddScoped<IMiServicio, MiServicio>();
```

### Crear una Nueva Vista

1. **Crear ViewModel en `ViewModels/`:**

```csharp
public partial class MiViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _titulo = "Mi Vista";
    
    [RelayCommand]
    private async Task GuardarAsync()
    {
        // Lógica de guardado
    }
}
```

2. **Crear Vista en `Views/`:**

```xml
<UserControl xmlns="https://github.com/avaloniaui"
             x:Class="SistemaComunidad.Views.MiVista">
    <StackPanel>
        <TextBlock Text="{Binding Titulo}"/>
        <Button Content="Guardar" Command="{Binding GuardarCommand}"/>
    </StackPanel>
</UserControl>
```

## 🗄️ Trabajar con la Base de Datos

### Migraciones de Entity Framework

```bash
# Crear una migración
dotnet ef migrations add NombreMigracion

# Aplicar migraciones
dotnet ef database update

# Revertir a una migración específica
dotnet ef database update MigracionAnterior

# Eliminar última migración
dotnet ef migrations remove

# Ver migraciones aplicadas
dotnet ef migrations list

# Generar script SQL
dotnet ef migrations script
```

### Consultas Comunes

```csharp
// Obtener todos
var todos = await _unitOfWork.Personas.ObtenerTodosAsync();

// Obtener por ID
var persona = await _unitOfWork.Personas.ObtenerPorIdAsync(1);

// Buscar con condición
var activos = await _unitOfWork.Personas.BuscarAsync(p => p.Activo);

// Agregar
var nueva = new Persona { Nombres = "Juan", Apellidos = "Pérez" };
await _unitOfWork.Personas.AgregarAsync(nueva);
await _unitOfWork.CompletarAsync();

// Actualizar
persona.Telefono = "99999999";
await _unitOfWork.Personas.ActualizarAsync(persona);
await _unitOfWork.CompletarAsync();

// Eliminar
await _unitOfWork.Personas.EliminarAsync(1);
await _unitOfWork.CompletarAsync();

// Contar
var total = await _unitOfWork.Personas.ContarAsync(p => p.Activo);
```

## 🔐 Sistema de Autenticación

### Uso Básico

```csharp
// Inyectar el servicio
private readonly IAutenticacionService _authService;

// Iniciar sesión
var usuario = await _authService.IniciarSesionAsync("admin", "admin123");

if (usuario != null)
{
    // Sesión iniciada correctamente
    Console.WriteLine($"Bienvenido {usuario.NombreCompleto}");
    Console.WriteLine($"Rol: {usuario.Rol}");
}

// Cambiar contraseña
var cambiado = await _authService.CambiarPasswordAsync(
    usuarioId: 1,
    passwordAntiguo: "admin123",
    passwordNuevo: "nuevaPassword123"
);
```

## 📊 Gestión de Auditoría

Las acciones importantes se registran automáticamente:

```csharp
var auditoria = new AuditoriaAccion
{
    UsuarioId = usuarioActual.Id,
    NombreUsuario = usuarioActual.NombreUsuario,
    Accion = "Crear Persona",
    Entidad = "Persona",
    EntidadId = nuevaPersona.Id,
    Descripcion = $"Se creó la persona {nuevaPersona.Nombres}",
    FechaAccion = DateTime.Now
};

await _unitOfWork.Auditorias.AgregarAsync(auditoria);
await _unitOfWork.CompletarAsync();
```

## 🧪 Testing (Futuro)

Estructura recomendada para pruebas:

```
Tests/
├── Unit/                      # Pruebas unitarias
│   ├── Repositories/
│   └── Services/
├── Integration/               # Pruebas de integración
└── UI/                        # Pruebas de interfaz
```

## 📱 Extensiones de VS Code Recomendadas

- **C# Dev Kit** - Soporte completo de C#
- **XAML** - Intellisense para Avalonia
- **GitLens** - Control de versiones mejorado
- **Error Lens** - Visualización de errores inline
- **Better Comments** - Comentarios con colores

## 🚀 Comandos Útiles

```bash
# Restaurar paquetes
dotnet restore

# Compilar
dotnet build

# Ejecutar
dotnet run

# Limpiar
dotnet clean

# Ver información del proyecto
dotnet list package

# Agregar paquete
dotnet add package NombrePaquete

# Eliminar paquete
dotnet remove package NombrePaquete
```

## 🐛 Depuración

### En VS Code

1. Presiona `F5` o ve a Run > Start Debugging
2. Los breakpoints se marcan con un clic en el margen izquierdo
3. Usa la consola de depuración para evaluar variables

### Logs

```csharp
// Agregar logging
Console.WriteLine($"Debug: {variable}");

// Para producción, usar ILogger
private readonly ILogger<MiClase> _logger;
_logger.LogInformation("Operación completada");
_logger.LogError(ex, "Error al procesar");
```

## 📚 Recursos Adicionales

- [Documentación de Avalonia](https://docs.avaloniaui.net/)
- [Entity Framework Core](https://learn.microsoft.com/ef/core/)
- [Patrones de diseño en C#](https://refactoring.guru/design-patterns/csharp)
- [.NET Documentation](https://learn.microsoft.com/dotnet/)

## 💡 Tips y Mejores Prácticas

1. **Siempre usa Unit of Work para transacciones complejas**
2. **Valida datos en múltiples capas**
3. **Registra acciones importantes en auditoría**
4. **Usa nombres descriptivos en español**
5. **Comenta código complejo**
6. **Haz commits frecuentes con mensajes claros**
7. **Prueba cada funcionalidad antes de commit**
8. **Mantén las entidades simples (no más lógica de la necesaria)**
9. **Separa responsabilidades (Single Responsibility Principle)**
10. **Usa async/await consistentemente**

---

¿Preguntas? Consulta el README.md principal o la documentación inline del código.
