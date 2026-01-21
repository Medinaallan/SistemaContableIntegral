# Funcionalidad de Servicios para Patronatos

## Resumen
Se ha agregado una funcionalidad completa para gestionar servicios de patronatos que servirán para los pagos mensuales de los socios.

## Componentes Implementados

### 1. **Entidad de Datos**
- **Archivo**: `Data/Entities/Servicio.cs`
- **Características**:
  - Nombre del servicio
  - Descripción
  - Costo mensual (decimal)
  - Periodicidad (Mensual, Bimestral, Trimestral, Semestral, Anual)
  - Es obligatorio (booleano)
  - Fecha inicio y fin
  - Relación con Empresa/Patronato
  - Notas adicionales

### 2. **Capa de Datos**
- **Repositorio**: `Data/Repositories/ServicioRepositorio.cs`
- **Interfaz**: `Data/Interfaces/IServicioRepositorio.cs`
- **Métodos especializados**:
  - Obtener servicios activos
  - Obtener servicios por empresa
  - Obtener servicios obligatorios

### 3. **Capa de Negocio**
- **Servicio**: `Business/Services/ServicioService.cs`
- **Interfaz**: `Business/Interfaces/IServicioService.cs`
- **Validaciones**:
  - Nombre requerido
  - Costo no negativo
  - Fechas coherentes
  - Nombres únicos por empresa

### 4. **Interfaz de Usuario**
- **ViewModel**: `ViewModels/ServiciosViewModel.cs`
- **Vista**: `Views/ServiciosWindow.axaml` y `.axaml.cs`
- **Funcionalidades**:
  - Crear nuevo servicio
  - Editar servicio existente
  - Eliminar servicio
  - Listar servicios activos
  - Validación de formularios

### 5. **Integración**
- **Botón agregado** en la sección CONFIGURACIÓN del menú principal
- **DbContext** actualizado con la entidad Servicio
- **Inyección de dependencias** configurada en Program.cs
- **Migración** creada: `20260119_AgregarTablaServicios`

## Base de Datos

### Tabla: Servicios
```sql
- Id (int, PK, Identity)
- Nombre (nvarchar(200), NOT NULL)
- Descripcion (nvarchar(500), NULL)
- CostoMensual (decimal(18,2), NOT NULL)
- Periodicidad (int, NOT NULL)
- EsObligatorio (bit, NOT NULL)
- FechaInicio (datetime2, NOT NULL)
- FechaFin (datetime2, NULL)
- Notas (nvarchar(500), NULL)
- EmpresaId (int, NULL, FK a Empresas)
- FechaCreacion (datetime2, NOT NULL)
- FechaModificacion (datetime2, NULL)
- Activo (bit, NOT NULL)
```

### Índices
- IX_Servicios_Nombre
- IX_Servicios_EmpresaId

## Cómo Usar

1. **Acceder al módulo**: 
   - Desde el menú principal, ir a CONFIGURACIÓN → Servicios

2. **Crear un nuevo servicio**:
   - Completar el formulario del panel izquierdo
   - Especificar nombre, costo mensual, periodicidad
   - Marcar si es obligatorio
   - Hacer clic en "Guardar"

3. **Editar un servicio**:
   - Hacer clic en "Editar" en el servicio deseado (panel derecho)
   - Modificar los datos
   - Hacer clic en "Actualizar"

4. **Eliminar un servicio**:
   - Hacer clic en "Eliminar" en el servicio deseado
   - El servicio se marcará como inactivo (eliminación lógica)

## Script de Base de Datos

Si la base de datos ya existe y solo necesitas agregar la tabla de Servicios, ejecuta el script:
`create_servicios_table.sql`

Este script verificará si la tabla existe antes de crearla.

## Uso Futuro

Esta funcionalidad servirá como base para:
- Sistema de cobros mensuales
- Generación de recibos
- Seguimiento de pagos por socio
- Reportes de servicios y cobros
- Gestión de morosos

## Notas Técnicas

- La aplicación usa Avalonia UI para la interfaz
- Entity Framework Core para acceso a datos
- Patrón Repository y Unit of Work
- MVVM con CommunityToolkit.Mvvm
- Validaciones en capa de negocio
- Eliminación lógica (no física)
