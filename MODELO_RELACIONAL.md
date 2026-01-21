# Modelo Relacional - Sistema Comunitario Integral
## Base de Datos: SistemaComunidad

---

## 📊 DIAGRAMA RELACIONAL

```
┌─────────────────────────────────────────────────────────────────────┐
│                        EMPRESAS (Patronatos/Iglesias)               │
├─────────────────────────────────────────────────────────────────────┤
│ • Id (PK, int)                                                      │
│ • RazonSocial (nvarchar(200), NOT NULL)                            │
│ • NombreComercial (nvarchar(200), NOT NULL)                        │
│ • RTN (nvarchar(20), NOT NULL, UNIQUE)                             │
│ • NumeroTelefono (nvarchar(20), NOT NULL)                          │
│ • Direccion (nvarchar(500), NOT NULL)                              │
│ • CorreoElectronico (nvarchar(100), NOT NULL)                      │
│ • Representante (nvarchar(200))                                    │
│ • TelefonoRepresentante (nvarchar(20))                             │
│ • FechaCreacion, FechaModificacion, Activo                         │
└─────────────────────────────────────────────────────────────────────┘
                                    │
                                    │ 1:N
                                    ▼
┌─────────────────────────────────────────────────────────────────────┐
│                        SERVICIOS (Pagos mensuales)                  │
├─────────────────────────────────────────────────────────────────────┤
│ • Id (PK, int)                                                      │
│ • Nombre (nvarchar(200), NOT NULL)                                 │
│ • Descripcion (nvarchar(500))                                      │
│ • CostoMensual (decimal, NOT NULL)                                 │
│ • Periodicidad (int, NOT NULL) [Mensual/Bimestral/etc]            │
│ • EsObligatorio (bit, NOT NULL)                                    │
│ • Notas (nvarchar(500))                                            │
│ • EmpresaId (FK → Empresas.Id)                                     │
│ • FechaCreacion, FechaModificacion, Activo                         │
└─────────────────────────────────────────────────────────────────────┘


┌─────────────────────────────────────────────────────────────────────┐
│                     NUCLEOS FAMILIARES                              │
├─────────────────────────────────────────────────────────────────────┤
│ • Id (PK, int)                                                      │
│ • Nombre (nvarchar(200), NOT NULL)                                 │
│ • Direccion (nvarchar(500))                                        │
│ • Telefono (nvarchar(20))                                          │
│ • Notas (nvarchar(MAX))                                            │
│ • FechaCreacion, FechaModificacion, Activo                         │
└─────────────────────────────────────────────────────────────────────┘
                                    │
                                    │ 1:N
                                    ▼
┌─────────────────────────────────────────────────────────────────────┐
│                          PERSONAS (Miembros)                        │
├─────────────────────────────────────────────────────────────────────┤
│ • Id (PK, int)                                                      │
│ • Nombres (nvarchar(100), NOT NULL)                                │
│ • Apellidos (nvarchar(100), NOT NULL)                              │
│ • IdentidadNacional (nvarchar(20))                                 │
│ • FechaNacimiento (datetimeoffset)                                 │
│ • Telefono (nvarchar(20))                                          │
│ • Email (nvarchar(100))                                            │
│ • Direccion (nvarchar(500))                                        │
│ • EstadoParticipacion (int) [Activo/Inactivo/Visitante]           │
│ • Notas (nvarchar(MAX))                                            │
│ • NucleoFamiliarId (FK → NucleosFamiliares.Id)                    │
│ • FechaCreacion, FechaModificacion, Activo                         │
└─────────────────────────────────────────────────────────────────────┘
            │                                   │
            │ 1:N                               │ N:M
            ▼                                   ▼
┌──────────────────────────────┐   ┌─────────────────────────────────┐
│         APORTES              │   │  PARTICIPACIONES ACTIVIDAD      │
├──────────────────────────────┤   ├─────────────────────────────────┤
│ • Id (PK, int)               │   │ • Id (PK, int)                  │
│ • PersonaId (FK → Personas)  │   │ • ActividadId (FK → Actividades)│
│ • Monto (decimal, NOT NULL)  │   │ • PersonaId (FK → Personas)     │
│ • FechaAporte (datetime2)    │   │ • Asistio (bit, NOT NULL)       │
│ • TipoAporte (int)           │   │ • Notas (nvarchar(MAX))         │
│ • Concepto (nvarchar(500))   │   │ • FechaCreacion, Fecha...Activo │
│ • Notas (nvarchar(MAX))      │   └─────────────────────────────────┘
│ • UsuarioRegistroId (int)    │                     │
│ • FechaCreacion, Fecha...    │                     │
└──────────────────────────────┘                     │
                                                     │
                                                     ▼
                                       ┌─────────────────────────────┐
                                       │      ACTIVIDADES            │
                                       ├─────────────────────────────┤
                                       │ • Id (PK, int)              │
                                       │ • Nombre (nvarchar(200))    │
                                       │ • Descripcion (nvarchar)    │
                                       │ • FechaInicio (datetime2)   │
                                       │ • FechaFin (datetime2)      │
                                       │ • Lugar (nvarchar(300))     │
                                       │ • Responsable (nvarchar)    │
                                       │ • TipoActividad (int)       │
                                       │ • Observaciones (nvarchar)  │
                                       │ • FechaCreacion, Fecha...   │
                                       └─────────────────────────────┘


┌─────────────────────────────────────────────────────────────────────┐
│                           USUARIOS                                  │
├─────────────────────────────────────────────────────────────────────┤
│ • Id (PK, int)                                                      │
│ • NombreUsuario (nvarchar(50), NOT NULL, UNIQUE)                   │
│ • PasswordHash (nvarchar(500), NOT NULL)                           │
│ • NombreCompleto (nvarchar(150), NOT NULL)                         │
│ • Email (nvarchar(100))                                            │
│ • Rol (int, NOT NULL) [Admin/Usuario/etc]                         │
│ • UltimoAcceso (datetime2)                                         │
│ • CambiarPassword (bit, NOT NULL)                                  │
│ • FechaCreacion, FechaModificacion, Activo                         │
└─────────────────────────────────────────────────────────────────────┘


┌─────────────────────────────────────────────────────────────────────┐
│                      INGRESOS (Transacciones)                       │
├─────────────────────────────────────────────────────────────────────┤
│ • Id (PK, int)                                                      │
│ • Monto (decimal, NOT NULL)                                        │
│ • FechaIngreso (datetime2, NOT NULL)                               │
│ • TipoIngreso (int, NOT NULL)                                      │
│ • Concepto (nvarchar(500), NOT NULL)                               │
│ • Descripcion (nvarchar(MAX))                                      │
│ • NumeroRecibo (nvarchar(50))                                      │
│ • UsuarioRegistroId (int)                                          │
│ • FechaCreacion, FechaModificacion, Activo                         │
└─────────────────────────────────────────────────────────────────────┘


┌─────────────────────────────────────────────────────────────────────┐
│                      EGRESOS (Gastos)                               │
├─────────────────────────────────────────────────────────────────────┤
│ • Id (PK, int)                                                      │
│ • Monto (decimal, NOT NULL)                                        │
│ • FechaEgreso (datetime2, NOT NULL)                                │
│ • TipoEgreso (int, NOT NULL)                                       │
│ • Concepto (nvarchar(500), NOT NULL)                               │
│ • Descripcion (nvarchar(MAX))                                      │
│ • NumeroComprobante (nvarchar(50))                                 │
│ • Beneficiario (nvarchar(200))                                     │
│ • UsuarioRegistroId (int)                                          │
│ • FechaCreacion, FechaModificacion, Activo                         │
└─────────────────────────────────────────────────────────────────────┘


┌─────────────────────────────────────────────────────────────────────┐
│                      DOCUMENTOS                                     │
├─────────────────────────────────────────────────────────────────────┤
│ • Id (PK, int)                                                      │
│ • Titulo (nvarchar(300), NOT NULL)                                 │
│ • TipoDocumento (int, NOT NULL)                                    │
│ • FechaDocumento (datetime2, NOT NULL)                             │
│ • Descripcion (nvarchar(MAX))                                      │
│ • RutaArchivo (nvarchar(500))                                      │
│ • NombreArchivo (nvarchar(200))                                    │
│ • UsuarioRegistroId (int)                                          │
│ • FechaCreacion, FechaModificacion, Activo                         │
└─────────────────────────────────────────────────────────────────────┘


┌─────────────────────────────────────────────────────────────────────┐
│                      BIENES (Inventario)                            │
├─────────────────────────────────────────────────────────────────────┤
│ • Id (PK, int)                                                      │
│ • Nombre (nvarchar(200), NOT NULL)                                 │
│ • Descripcion (nvarchar(MAX))                                      │
│ • CodigoInventario (nvarchar(50))                                  │
│ • FechaAdquisicion (datetime2, NOT NULL)                           │
│ • ValorAdquisicion (decimal)                                       │
│ • EstadoBien (int, NOT NULL)                                       │
│ • Ubicacion (nvarchar(300))                                        │
│ • Responsable (nvarchar(100))                                      │
│ • Notas (nvarchar(MAX))                                            │
│ • FechaCreacion, FechaModificacion, Activo                         │
└─────────────────────────────────────────────────────────────────────┘


┌─────────────────────────────────────────────────────────────────────┐
│                   AUDITORIAS ACCION (Log)                           │
├─────────────────────────────────────────────────────────────────────┤
│ • Id (PK, int)                                                      │
│ • UsuarioId (int, NOT NULL)                                        │
│ • NombreUsuario (nvarchar(50), NOT NULL)                           │
│ • Accion (nvarchar(100), NOT NULL)                                 │
│ • Entidad (nvarchar(50))                                           │
│ • EntidadId (int)                                                  │
│ • Descripcion (nvarchar(MAX))                                      │
│ • FechaAccion (datetime2, NOT NULL)                                │
│ • DireccionIP (nvarchar(50))                                       │
│ • FechaCreacion, FechaModificacion, Activo                         │
└─────────────────────────────────────────────────────────────────────┘
```

---

## 🔗 RELACIONES (FOREIGN KEYS)

1. **Personas** → **NucleosFamiliares**
   - `Personas.NucleoFamiliarId` → `NucleosFamiliares.Id`
   - Una familia puede tener muchos miembros

2. **Aportes** → **Personas**
   - `Aportes.PersonaId` → `Personas.Id`
   - Una persona puede hacer muchos aportes

3. **ParticipacionesActividad** → **Actividades**
   - `ParticipacionesActividad.ActividadId` → `Actividades.Id`
   - Una actividad puede tener muchas participaciones

4. **ParticipacionesActividad** → **Personas**
   - `ParticipacionesActividad.PersonaId` → `Personas.Id`
   - Una persona puede participar en muchas actividades

5. **Servicios** → **Empresas**
   - `Servicios.EmpresaId` → `Empresas.Id`
   - Una empresa/patronato puede tener muchos servicios

---

## 📋 ÍNDICES PRINCIPALES

### Personas
- `IX_Personas_IdentidadNacional`
- `IX_Personas_NucleoFamiliarId`

### Servicios
- `IX_Servicios_Nombre`
- `IX_Servicios_EmpresaId`

### Empresas
- `UQ_Empresas_RTN` (UNIQUE)

---

## 📊 RESUMEN DE ENTIDADES

| Tabla | Propósito | Registros Típicos |
|-------|-----------|-------------------|
| **Empresas** | Datos del patronato/iglesia | 1 registro principal |
| **Servicios** | Servicios para cobro mensual | Agua, Luz, Mantenimiento, etc |
| **NucleosFamiliares** | Agrupación familiar | Una por familia |
| **Personas** | Socios/Miembros | Todos los miembros del patronato |
| **Aportes** | Donaciones de personas | Histórico de aportes |
| **Actividades** | Eventos y reuniones | Asambleas, eventos, etc |
| **ParticipacionesActividad** | Asistencia a eventos | Relación N:M entre personas y actividades |
| **Usuarios** | Acceso al sistema | Admin, usuarios operativos |
| **Ingresos** | Entradas de dinero | Transacciones financieras |
| **Egresos** | Salidas de dinero | Pagos y gastos |
| **Documentos** | Archivos y documentos | Actas, contratos, etc |
| **Bienes** | Inventario de activos | Propiedades y equipos |
| **AuditoriasAccion** | Log de actividad | Histórico de cambios |

---

## 🎯 CAMPOS COMUNES (Patrón Base Entity)

Todas las tablas heredan estos campos de `BaseEntity`:
- `Id` (int, PK, IDENTITY)
- `FechaCreacion` (datetime2, NOT NULL)
- `FechaModificacion` (datetime2, NULL)
- `Activo` (bit, NOT NULL) - Para eliminación lógica

---

## 🔮 PRÓXIMAS TABLAS SUGERIDAS

Para completar la funcionalidad de servicios y pagos mensuales:

1. **PersonaServicio** (Relación N:M)
   - PersonaId → Personas
   - ServicioId → Servicios
   - FechaInicio
   - FechaFin
   - Estado (Activo/Suspendido)

2. **Pagos** o **CobrosServicios**
   - PersonaServicioId
   - Monto
   - FechaPago
   - FechaVencimiento
   - Estado (Pendiente/Pagado/Vencido)
   - NumeroRecibo
   - MetodoPago

---

**Fecha de generación:** 19 de enero de 2026
**Base de datos:** SistemaComunidad
**Servidor:** localhost\SQLEXPRESS
**Total de tablas:** 14 (13 de negocio + 1 de migraciones)
