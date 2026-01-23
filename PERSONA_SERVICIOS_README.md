# Módulo de Asignación de Servicios a Personas

## Descripción General

Este módulo permite enlazar servicios del patronato (como agua potable, mantenimiento, seguridad, etc.) con personas específicas de la comunidad para posteriormente generar y gestionar cobros mensuales.

## Arquitectura

### Entidades

#### PersonaServicio
Entidad de relación que conecta Personas con Servicios. Incluye:
- **PersonaId**: ID de la persona
- **ServicioId**: ID del servicio asignado
- **FechaInicio**: Fecha desde la cual la persona tiene el servicio
- **FechaFin**: Fecha en que terminó el servicio (null si está activo)
- **EstaActivo**: Indica si el servicio está activo para esta persona
- **CostoPersonalizado**: Costo específico para esta persona (si es diferente al estándar)
- **UltimoPeriodoCobrado**: Último mes/año que se generó cobro (formato YYYYMM)
- **Notas**: Notas adicionales sobre la asignación

### Repositorio

**PersonaServicioRepositorio** (`Data/Repositories/PersonaServicioRepositorio.cs`)

Métodos principales:
- `ObtenerServiciosPorPersonaAsync(personaId)`: Obtiene todos los servicios de una persona
- `ObtenerServiciosActivosPorPersonaAsync(personaId)`: Servicios activos de una persona
- `ObtenerPersonasPorServicioAsync(servicioId)`: Personas con un servicio específico
- `ObtenerServiciosPorCobrarEnPeriodoAsync(periodo)`: Servicios pendientes de cobro en un período
- `MarcarComoCobradoAsync(personaServicioId, periodo)`: Marca un servicio como cobrado

### Servicio de Negocio

**PersonaServicioService** (`Business/Services/PersonaServicioService.cs`)

Métodos principales:
- `AsignarServicioAPersonaAsync(personaId, servicioId, costoPersonalizado?, notas?)`: Asigna un servicio a una persona
- `DesasignarServicioDePersonaAsync(personaId, servicioId)`: Desactiva un servicio para una persona
- `ReactivarServicioAsync(personaServicioId)`: Reactiva un servicio previamente desactivado
- `ActualizarCostoPersonalizadoAsync(personaServicioId, costoPersonalizado)`: Actualiza el costo específico
- `ObtenerTotalMensualPorPersonaAsync(personaId)`: Calcula el total mensual a pagar
- `GenerarCobrosPendientesPorPeriodoAsync(periodo)`: Genera lista de cobros pendientes
- `MarcarComoCobradoAsync(personaServicioId, periodo)`: Marca como cobrado

## Flujo de Uso

### 1. Asignar un Servicio a una Persona

```csharp
var personaServicio = await _personaServicioService.AsignarServicioAPersonaAsync(
    personaId: 1,
    servicioId: 5,
    costoPersonalizado: 150.00m, // Opcional: si es diferente al costo estándar
    notas: "Servicio con descuento especial" // Opcional
);
```

### 2. Consultar Servicios de una Persona

```csharp
// Obtener todos los servicios (activos e inactivos)
var todosLosServicios = await _personaServicioService.ObtenerServiciosPorPersonaAsync(personaId);

// Obtener solo servicios activos
var serviciosActivos = await _personaServicioService.ObtenerServiciosActivosPorPersonaAsync(personaId);

// Calcular total mensual
var totalMensual = await _personaServicioService.ObtenerTotalMensualPorPersonaAsync(personaId);
```

### 3. Gestionar Asignaciones

```csharp
// Desactivar un servicio
await _personaServicioService.DesasignarServicioDePersonaAsync(personaId, servicioId);

// Reactivar un servicio
await _personaServicioService.ReactivarServicioAsync(personaServicioId);

// Actualizar costo personalizado
await _personaServicioService.ActualizarCostoPersonalizadoAsync(personaServicioId, 200.00m);
```

### 4. Generar Cobros Mensuales

```csharp
// Generar cobros para enero 2026
int periodo = 202601; // Formato YYYYMM
var cobrosPendientes = await _personaServicioService.GenerarCobrosPendientesPorPeriodoAsync(periodo);

foreach (var cobro in cobrosPendientes)
{
    var persona = cobro.Persona;
    var servicio = cobro.Servicio;
    var monto = cobro.ObtenerCostoEfectivo();
    
    // Aquí se generaría el registro de cobro/factura
    // ...
    
    // Marcar como cobrado
    await _personaServicioService.MarcarComoCobradoAsync(cobro.Id, periodo);
}
```

## Validaciones

El servicio incluye validaciones automáticas:
- Verifica que la persona existe
- Verifica que el servicio existe y está activo
- Previene asignaciones duplicadas de servicios activos
- Valida costos negativos
- Valida formato de período (YYYYMM)

## Migración de Base de Datos

### Aplicar la Migración

Si tu base de datos ya tiene las tablas existentes pero no tiene registradas las migraciones:

1. **Ejecutar el script de registro de migraciones:**
   ```sql
   -- Ejecutar en SQL Server Management Studio
   -- El archivo: registrar_migraciones_completo.sql
   ```

2. **Aplicar solo la nueva migración:**
   ```bash
   dotnet ef database update
   ```

### Si es una Base de Datos Nueva

```bash
dotnet ef database update
```

## Estructura de Tabla PersonaServicios

```sql
CREATE TABLE PersonaServicios (
    Id INT IDENTITY PRIMARY KEY,
    PersonaId INT NOT NULL,
    ServicioId INT NOT NULL,
    FechaInicio DATETIMEOFFSET NOT NULL,
    FechaFin DATETIMEOFFSET NULL,
    EstaActivo BIT NOT NULL,
    CostoPersonalizado DECIMAL(18,2) NULL,
    Notas NVARCHAR(500) NULL,
    UltimoPeriodoCobrado INT NULL,
    FechaCreacion DATETIME2 NOT NULL,
    FechaModificacion DATETIME2 NULL,
    Activo BIT NOT NULL,
    
    CONSTRAINT FK_PersonaServicios_Personas FOREIGN KEY (PersonaId) 
        REFERENCES Personas(Id) ON DELETE CASCADE,
    CONSTRAINT FK_PersonaServicios_Servicios FOREIGN KEY (ServicioId) 
        REFERENCES Servicios(Id) ON DELETE CASCADE
);
```

### Índices
- `IX_PersonaServicios_PersonaId`
- `IX_PersonaServicios_ServicioId`
- `IX_PersonaServicios_EstaActivo`
- `IX_PersonaServicios_PersonaId_ServicioId` (compuesto)

## Formato de Período

El período se almacena como entero en formato **YYYYMM**:
- Enero 2026: `202601`
- Febrero 2026: `202602`
- Diciembre 2026: `202612`

Este formato facilita comparaciones y ordenamiento cronológico.

## Ejemplos de Uso Avanzado

### Obtener Personas con un Servicio Específico

```csharp
var personasConAgua = await _personaServicioService.ObtenerPersonasPorServicioAsync(servicioId: 5);

foreach (var ps in personasConAgua)
{
    Console.WriteLine($"{ps.Persona.Nombres} {ps.Persona.Apellidos}");
    Console.WriteLine($"Costo: {ps.ObtenerCostoEfectivo()}");
    Console.WriteLine($"Estado: {(ps.EstaActivo ? "Activo" : "Inactivo")}");
}
```

### Calcular Cobros Totales de un Período

```csharp
var periodo = 202601;
var cobros = await _personaServicioService.GenerarCobrosPendientesPorPeriodoAsync(periodo);

var totalGeneral = cobros.Sum(c => c.ObtenerCostoEfectivo());
Console.WriteLine($"Total a cobrar en {periodo}: L {totalGeneral:N2}");
```

### Verificar si Debe Cobrarse en un Período

```csharp
var personaServicio = await _personaServicioRepositorio.ObtenerPorIdAsync(id);
bool debeCobrar = personaServicio.DebeCobrarseEn(202601);
```

## Notas Importantes

1. **Eliminación en Cascada**: Si se elimina una Persona o un Servicio, se eliminarán automáticamente sus relaciones en PersonaServicios.

2. **Costo Efectivo**: El método `ObtenerCostoEfectivo()` devuelve el costo personalizado si existe, o el costo estándar del servicio.

3. **Períodos de Cobro**: El sistema registra el último período cobrado para evitar cobros duplicados.

4. **Servicios Inactivos**: Los servicios desactivados mantienen su historial pero no se incluyen en los cobros futuros.

5. **Fechas con Zona Horaria**: Se usa `DateTimeOffset` para manejar correctamente las zonas horarias.

## Próximos Pasos

Para completar el sistema de cobros, considerar implementar:
- Módulo de Facturación que use PersonaServicio para generar facturas
- Módulo de Pagos para registrar los pagos recibidos
- Reportes de morosidad
- Notificaciones automáticas de cobros pendientes
- Historial de cambios en los servicios asignados
