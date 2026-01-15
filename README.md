# Sistema Comunitario Integral

Sistema de gestión comunitaria para patronatos e iglesias en Honduras.

## 🎯 Descripción

Aplicación de escritorio desarrollada en C# con Avalonia UI para la gestión administrativa y financiera de organizaciones comunitarias sin fines de lucro.

## 🛠️ Stack Tecnológico

- **Framework**: .NET 10.0
- **UI**: Avalonia UI 11.3
- **Base de Datos**: SQL Server Express / LocalDB
- **ORM**: Entity Framework Core 10.0
- **Seguridad**: BCrypt.Net para contraseñas
- **Patrón**: MVVM (Model-View-ViewModel)
- **Arquitectura**: Capas (Presentación, Negocio, Datos)

## 📦 Estructura del Proyecto

```
SistemaComunidad/
├── Data/                          # Capa de Datos
│   ├── Context/                   # DbContext
│   ├── Entities/                  # Entidades del dominio
│   ├── Interfaces/                # Interfaces de repositorios
│   └── Repositories/              # Implementación de repositorios
├── Business/                      # Capa de Negocio
│   ├── Interfaces/                # Interfaces de servicios
│   └── Services/                  # Implementación de servicios
├── ViewModels/                    # ViewModels (MVVM)
├── Views/                         # Vistas (UI)
├── Models/                        # Modelos de vista
└── Assets/                        # Recursos estáticos
```

## 📊 Entidades Principales

1. **Persona** - Registro de personas y miembros
2. **NucleoFamiliar** - Grupos familiares
3. **Aporte** - Contribuciones individuales
4. **Ingreso** - Ingresos comunitarios
5. **Egreso** - Gastos operativos
6. **Actividad** - Eventos y reuniones
7. **Documento** - Gestión documental
8. **Bien** - Inventario de bienes
9. **Usuario** - Control de acceso
10. **AuditoriaAccion** - Registro de acciones

## 🚀 Requisitos Previos

- .NET SDK 10.0 o superior
- SQL Server Express o LocalDB
- VS Code con extensión C# Dev Kit (opcional)
- Windows 10/11 (recomendado)

## ⚙️ Configuración

### 1. Cadena de Conexión

Editar `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SistemaComunidad;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
}
```

### 2. Crear Base de Datos

La base de datos se crea automáticamente al ejecutar la aplicación por primera vez.

Si prefieres usar migraciones:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 3. Usuario Predeterminado

- **Usuario**: admin
- **Contraseña**: admin123
- ⚠️ **Importante**: Cambiar contraseña en el primer inicio

## 🏃‍♂️ Ejecutar el Proyecto

```bash
cd SistemaComunidad
dotnet restore
dotnet build
dotnet run
```

## 📝 Características Implementadas

✅ Arquitectura en capas profesional
✅ Entity Framework Core con SQL Server
✅ 12 entidades de dominio completas
✅ Patrón repositorio genérico
✅ Unit of Work para transacciones
✅ Sistema de autenticación con BCrypt
✅ Auditoría de acciones del sistema
✅ Inyección de dependencias
✅ Configuración centralizada

## 🚧 En Desarrollo

- [ ] Interfaz de inicio de sesión
- [ ] Vistas CRUD para todas las entidades
- [ ] Dashboard con estadísticas
- [ ] Reportes financieros (RDLC)
- [ ] Sistema de respaldos automáticos
- [ ] Exportación a Excel/PDF
- [ ] Control de permisos por rol
- [ ] Gestión de documentos con archivos adjuntos

## 🔐 Seguridad

- Contraseñas hasheadas con BCrypt
- Control de acceso basado en roles
- Auditoría completa de acciones
- Validación de datos en todas las capas

## 📖 Documentación Adicional

### Roles de Usuario

- **Administrador**: Acceso completo al sistema
- **Tesorero**: Gestión financiera
- **Secretario**: Documentación y actas
- **Usuario**: Acceso limitado de consulta

### Buenas Prácticas

- Usar Unit of Work para operaciones complejas
- Validar datos antes de guardar
- Registrar acciones importantes en auditoría
- Mantener respaldos periódicos
- Cambiar contraseñas predeterminadas

## 🤝 Contribuir

Este proyecto está diseñado para ser adaptable a diferentes organizaciones comunitarias en Honduras.

## 📄 Licencia

Proyecto educativo y comunitario.

## 👨‍💻 Desarrollo

Desarrollado siguiendo las mejores prácticas de:
- Clean Architecture
- SOLID Principles
- Repository Pattern
- Unit of Work Pattern
- Dependency Injection

---

**Nota**: Este es un sistema base que puede ser ampliado según las necesidades específicas de cada organización.
