IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260113145335_InitialCreate'
)
BEGIN
    CREATE TABLE [Actividades] (
        [Id] int NOT NULL IDENTITY,
        [Nombre] nvarchar(200) NOT NULL,
        [Descripcion] nvarchar(max) NULL,
        [FechaInicio] datetime2 NOT NULL,
        [FechaFin] datetime2 NULL,
        [Lugar] nvarchar(300) NULL,
        [Responsable] nvarchar(100) NULL,
        [TipoActividad] int NOT NULL,
        [Observaciones] nvarchar(max) NULL,
        [FechaCreacion] datetime2 NOT NULL,
        [FechaModificacion] datetime2 NULL,
        [Activo] bit NOT NULL,
        CONSTRAINT [PK_Actividades] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260113145335_InitialCreate'
)
BEGIN
    CREATE TABLE [AuditoriasAccion] (
        [Id] int NOT NULL IDENTITY,
        [UsuarioId] int NOT NULL,
        [NombreUsuario] nvarchar(50) NOT NULL,
        [Accion] nvarchar(100) NOT NULL,
        [Entidad] nvarchar(50) NULL,
        [EntidadId] int NULL,
        [Descripcion] nvarchar(max) NULL,
        [FechaAccion] datetime2 NOT NULL,
        [DireccionIP] nvarchar(50) NULL,
        [FechaCreacion] datetime2 NOT NULL,
        [FechaModificacion] datetime2 NULL,
        [Activo] bit NOT NULL,
        CONSTRAINT [PK_AuditoriasAccion] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260113145335_InitialCreate'
)
BEGIN
    CREATE TABLE [Bienes] (
        [Id] int NOT NULL IDENTITY,
        [Nombre] nvarchar(200) NOT NULL,
        [Descripcion] nvarchar(max) NULL,
        [CodigoInventario] nvarchar(50) NULL,
        [FechaAdquisicion] datetime2 NOT NULL,
        [ValorAdquisicion] decimal(18,2) NULL,
        [EstadoBien] int NOT NULL,
        [Ubicacion] nvarchar(300) NULL,
        [Responsable] nvarchar(100) NULL,
        [Notas] nvarchar(max) NULL,
        [FechaCreacion] datetime2 NOT NULL,
        [FechaModificacion] datetime2 NULL,
        [Activo] bit NOT NULL,
        CONSTRAINT [PK_Bienes] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260113145335_InitialCreate'
)
BEGIN
    CREATE TABLE [Documentos] (
        [Id] int NOT NULL IDENTITY,
        [Titulo] nvarchar(300) NOT NULL,
        [TipoDocumento] int NOT NULL,
        [FechaDocumento] datetime2 NOT NULL,
        [Descripcion] nvarchar(max) NULL,
        [RutaArchivo] nvarchar(500) NULL,
        [NombreArchivo] nvarchar(200) NULL,
        [UsuarioRegistroId] int NULL,
        [FechaCreacion] datetime2 NOT NULL,
        [FechaModificacion] datetime2 NULL,
        [Activo] bit NOT NULL,
        CONSTRAINT [PK_Documentos] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260113145335_InitialCreate'
)
BEGIN
    CREATE TABLE [Egresos] (
        [Id] int NOT NULL IDENTITY,
        [Monto] decimal(18,2) NOT NULL,
        [FechaEgreso] datetime2 NOT NULL,
        [TipoEgreso] int NOT NULL,
        [Concepto] nvarchar(500) NOT NULL,
        [Descripcion] nvarchar(max) NULL,
        [NumeroComprobante] nvarchar(50) NULL,
        [Beneficiario] nvarchar(200) NULL,
        [UsuarioRegistroId] int NULL,
        [FechaCreacion] datetime2 NOT NULL,
        [FechaModificacion] datetime2 NULL,
        [Activo] bit NOT NULL,
        CONSTRAINT [PK_Egresos] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260113145335_InitialCreate'
)
BEGIN
    CREATE TABLE [Ingresos] (
        [Id] int NOT NULL IDENTITY,
        [Monto] decimal(18,2) NOT NULL,
        [FechaIngreso] datetime2 NOT NULL,
        [TipoIngreso] int NOT NULL,
        [Concepto] nvarchar(500) NOT NULL,
        [Descripcion] nvarchar(max) NULL,
        [NumeroRecibo] nvarchar(50) NULL,
        [UsuarioRegistroId] int NULL,
        [FechaCreacion] datetime2 NOT NULL,
        [FechaModificacion] datetime2 NULL,
        [Activo] bit NOT NULL,
        CONSTRAINT [PK_Ingresos] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260113145335_InitialCreate'
)
BEGIN
    CREATE TABLE [NucleosFamiliares] (
        [Id] int NOT NULL IDENTITY,
        [Nombre] nvarchar(200) NOT NULL,
        [Direccion] nvarchar(500) NULL,
        [Telefono] nvarchar(20) NULL,
        [Notas] nvarchar(max) NULL,
        [FechaCreacion] datetime2 NOT NULL,
        [FechaModificacion] datetime2 NULL,
        [Activo] bit NOT NULL,
        CONSTRAINT [PK_NucleosFamiliares] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260113145335_InitialCreate'
)
BEGIN
    CREATE TABLE [Usuarios] (
        [Id] int NOT NULL IDENTITY,
        [NombreUsuario] nvarchar(50) NOT NULL,
        [PasswordHash] nvarchar(500) NOT NULL,
        [NombreCompleto] nvarchar(150) NOT NULL,
        [Email] nvarchar(100) NULL,
        [Rol] int NOT NULL,
        [UltimoAcceso] datetime2 NULL,
        [CambiarPassword] bit NOT NULL,
        [FechaCreacion] datetime2 NOT NULL,
        [FechaModificacion] datetime2 NULL,
        [Activo] bit NOT NULL,
        CONSTRAINT [PK_Usuarios] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260113145335_InitialCreate'
)
BEGIN
    CREATE TABLE [Personas] (
        [Id] int NOT NULL IDENTITY,
        [Nombres] nvarchar(100) NOT NULL,
        [Apellidos] nvarchar(100) NOT NULL,
        [IdentidadNacional] nvarchar(20) NULL,
        [FechaNacimiento] datetime2 NULL,
        [Telefono] nvarchar(20) NULL,
        [Email] nvarchar(100) NULL,
        [Direccion] nvarchar(500) NULL,
        [EstadoParticipacion] int NOT NULL,
        [Notas] nvarchar(max) NULL,
        [NucleoFamiliarId] int NULL,
        [FechaCreacion] datetime2 NOT NULL,
        [FechaModificacion] datetime2 NULL,
        [Activo] bit NOT NULL,
        CONSTRAINT [PK_Personas] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Personas_NucleosFamiliares_NucleoFamiliarId] FOREIGN KEY ([NucleoFamiliarId]) REFERENCES [NucleosFamiliares] ([Id]) ON DELETE SET NULL
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260113145335_InitialCreate'
)
BEGIN
    CREATE TABLE [Aportes] (
        [Id] int NOT NULL IDENTITY,
        [PersonaId] int NOT NULL,
        [Monto] decimal(18,2) NOT NULL,
        [FechaAporte] datetime2 NOT NULL,
        [TipoAporte] int NOT NULL,
        [Concepto] nvarchar(500) NULL,
        [Notas] nvarchar(max) NULL,
        [UsuarioRegistroId] int NULL,
        [FechaCreacion] datetime2 NOT NULL,
        [FechaModificacion] datetime2 NULL,
        [Activo] bit NOT NULL,
        CONSTRAINT [PK_Aportes] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Aportes_Personas_PersonaId] FOREIGN KEY ([PersonaId]) REFERENCES [Personas] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260113145335_InitialCreate'
)
BEGIN
    CREATE TABLE [ParticipacionesActividad] (
        [Id] int NOT NULL IDENTITY,
        [ActividadId] int NOT NULL,
        [PersonaId] int NOT NULL,
        [Asistio] bit NOT NULL,
        [Notas] nvarchar(max) NULL,
        [FechaCreacion] datetime2 NOT NULL,
        [FechaModificacion] datetime2 NULL,
        [Activo] bit NOT NULL,
        CONSTRAINT [PK_ParticipacionesActividad] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ParticipacionesActividad_Actividades_ActividadId] FOREIGN KEY ([ActividadId]) REFERENCES [Actividades] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_ParticipacionesActividad_Personas_PersonaId] FOREIGN KEY ([PersonaId]) REFERENCES [Personas] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260113145335_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Activo', N'CambiarPassword', N'Email', N'FechaCreacion', N'FechaModificacion', N'NombreCompleto', N'NombreUsuario', N'PasswordHash', N'Rol', N'UltimoAcceso') AND [object_id] = OBJECT_ID(N'[Usuarios]'))
        SET IDENTITY_INSERT [Usuarios] ON;
    EXEC(N'INSERT INTO [Usuarios] ([Id], [Activo], [CambiarPassword], [Email], [FechaCreacion], [FechaModificacion], [NombreCompleto], [NombreUsuario], [PasswordHash], [Rol], [UltimoAcceso])
    VALUES (1, CAST(1 AS bit), CAST(1 AS bit), N''admin@sistemacomunidad.local'', ''2026-01-01T00:00:00.0000000Z'', NULL, N''Administrador del Sistema'', N''admin'', N''$2a$11$N4HqjRHBZIhHzVH5bYzNnO6SZfs7EKjKx5r7XZQlQu5KjKvVmJYJO'', 0, NULL)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Activo', N'CambiarPassword', N'Email', N'FechaCreacion', N'FechaModificacion', N'NombreCompleto', N'NombreUsuario', N'PasswordHash', N'Rol', N'UltimoAcceso') AND [object_id] = OBJECT_ID(N'[Usuarios]'))
        SET IDENTITY_INSERT [Usuarios] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260113145335_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Actividades_FechaInicio] ON [Actividades] ([FechaInicio]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260113145335_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Aportes_FechaAporte] ON [Aportes] ([FechaAporte]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260113145335_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Aportes_PersonaId] ON [Aportes] ([PersonaId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260113145335_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_AuditoriasAccion_FechaAccion] ON [AuditoriasAccion] ([FechaAccion]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260113145335_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_AuditoriasAccion_UsuarioId] ON [AuditoriasAccion] ([UsuarioId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260113145335_InitialCreate'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Bienes_CodigoInventario] ON [Bienes] ([CodigoInventario]) WHERE [CodigoInventario] IS NOT NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260113145335_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Documentos_FechaDocumento] ON [Documentos] ([FechaDocumento]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260113145335_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Documentos_TipoDocumento] ON [Documentos] ([TipoDocumento]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260113145335_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Egresos_FechaEgreso] ON [Egresos] ([FechaEgreso]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260113145335_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Ingresos_FechaIngreso] ON [Ingresos] ([FechaIngreso]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260113145335_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_ParticipacionesActividad_ActividadId_PersonaId] ON [ParticipacionesActividad] ([ActividadId], [PersonaId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260113145335_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ParticipacionesActividad_PersonaId] ON [ParticipacionesActividad] ([PersonaId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260113145335_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Personas_IdentidadNacional] ON [Personas] ([IdentidadNacional]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260113145335_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Personas_NucleoFamiliarId] ON [Personas] ([NucleoFamiliarId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260113145335_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Usuarios_NombreUsuario] ON [Usuarios] ([NombreUsuario]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260113145335_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260113145335_InitialCreate', N'10.0.1');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260114174009_AgregarTablaEmpresa'
)
BEGIN
    CREATE TABLE [Empresas] (
        [Id] int NOT NULL IDENTITY,
        [RazonSocial] nvarchar(200) NOT NULL,
        [NombreComercial] nvarchar(200) NOT NULL,
        [RTN] nvarchar(20) NOT NULL,
        [NumeroTelefono] nvarchar(20) NOT NULL,
        [Direccion] nvarchar(500) NOT NULL,
        [CorreoElectronico] nvarchar(100) NOT NULL,
        [Representante] nvarchar(200) NULL,
        [TelefonoRepresentante] nvarchar(20) NULL,
        [FechaCreacion] datetime2 NOT NULL,
        [FechaModificacion] datetime2 NULL,
        [Activo] bit NOT NULL,
        CONSTRAINT [PK_Empresas] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260114174009_AgregarTablaEmpresa'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Empresas_RTN] ON [Empresas] ([RTN]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260114174009_AgregarTablaEmpresa'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260114174009_AgregarTablaEmpresa', N'10.0.1');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260116173347_ActualizarModelo'
)
BEGIN
    DECLARE @var nvarchar(max);
    SELECT @var = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Personas]') AND [c].[name] = N'FechaNacimiento');
    IF @var IS NOT NULL EXEC(N'ALTER TABLE [Personas] DROP CONSTRAINT ' + @var + ';');
    ALTER TABLE [Personas] ALTER COLUMN [FechaNacimiento] datetimeoffset NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260116173347_ActualizarModelo'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260116173347_ActualizarModelo', N'10.0.1');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260119155236_AgregarTablaServicios'
)
BEGIN
    CREATE TABLE [Servicios] (
        [Id] int NOT NULL IDENTITY,
        [Nombre] nvarchar(200) NOT NULL,
        [Descripcion] nvarchar(500) NULL,
        [CostoMensual] decimal(18,2) NOT NULL,
        [Periodicidad] int NOT NULL,
        [EsObligatorio] bit NOT NULL,
        [FechaInicio] datetime2 NOT NULL,
        [FechaFin] datetime2 NULL,
        [Notas] nvarchar(500) NULL,
        [EmpresaId] int NULL,
        [FechaCreacion] datetime2 NOT NULL,
        [FechaModificacion] datetime2 NULL,
        [Activo] bit NOT NULL,
        CONSTRAINT [PK_Servicios] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Servicios_Empresas_EmpresaId] FOREIGN KEY ([EmpresaId]) REFERENCES [Empresas] ([Id]) ON DELETE SET NULL
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260119155236_AgregarTablaServicios'
)
BEGIN
    CREATE INDEX [IX_Servicios_EmpresaId] ON [Servicios] ([EmpresaId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260119155236_AgregarTablaServicios'
)
BEGIN
    CREATE INDEX [IX_Servicios_Nombre] ON [Servicios] ([Nombre]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260119155236_AgregarTablaServicios'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260119155236_AgregarTablaServicios', N'10.0.1');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260119164710_EliminarFechasDeServicios'
)
BEGIN
    DECLARE @var1 nvarchar(max);
    SELECT @var1 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Servicios]') AND [c].[name] = N'FechaFin');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Servicios] DROP CONSTRAINT ' + @var1 + ';');
    ALTER TABLE [Servicios] DROP COLUMN [FechaFin];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260119164710_EliminarFechasDeServicios'
)
BEGIN
    DECLARE @var2 nvarchar(max);
    SELECT @var2 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Servicios]') AND [c].[name] = N'FechaInicio');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Servicios] DROP CONSTRAINT ' + @var2 + ';');
    ALTER TABLE [Servicios] DROP COLUMN [FechaInicio];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260119164710_EliminarFechasDeServicios'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260119164710_EliminarFechasDeServicios', N'10.0.1');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122164438_AgregarTablaPersonaServicios'
)
BEGIN
    CREATE TABLE [PersonaServicios] (
        [Id] int NOT NULL IDENTITY,
        [PersonaId] int NOT NULL,
        [ServicioId] int NOT NULL,
        [FechaInicio] datetimeoffset NOT NULL,
        [FechaFin] datetimeoffset NULL,
        [EstaActivo] bit NOT NULL,
        [CostoPersonalizado] decimal(18,2) NULL,
        [Notas] nvarchar(500) NULL,
        [UltimoPeriodoCobrado] int NULL,
        [FechaCreacion] datetime2 NOT NULL,
        [FechaModificacion] datetime2 NULL,
        [Activo] bit NOT NULL,
        CONSTRAINT [PK_PersonaServicios] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_PersonaServicios_Personas_PersonaId] FOREIGN KEY ([PersonaId]) REFERENCES [Personas] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_PersonaServicios_Servicios_ServicioId] FOREIGN KEY ([ServicioId]) REFERENCES [Servicios] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122164438_AgregarTablaPersonaServicios'
)
BEGIN
    CREATE INDEX [IX_PersonaServicios_EstaActivo] ON [PersonaServicios] ([EstaActivo]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122164438_AgregarTablaPersonaServicios'
)
BEGIN
    CREATE INDEX [IX_PersonaServicios_PersonaId] ON [PersonaServicios] ([PersonaId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122164438_AgregarTablaPersonaServicios'
)
BEGIN
    CREATE INDEX [IX_PersonaServicios_PersonaId_ServicioId] ON [PersonaServicios] ([PersonaId], [ServicioId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122164438_AgregarTablaPersonaServicios'
)
BEGIN
    CREATE INDEX [IX_PersonaServicios_ServicioId] ON [PersonaServicios] ([ServicioId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122164438_AgregarTablaPersonaServicios'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260122164438_AgregarTablaPersonaServicios', N'10.0.1');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122183014_AgregarCobrosYPagos'
)
BEGIN
    CREATE TABLE [Cobros] (
        [Id] int NOT NULL IDENTITY,
        [NumeroRecibo] nvarchar(50) NOT NULL,
        [PersonaId] int NOT NULL,
        [Periodo] int NOT NULL,
        [FechaEmision] datetimeoffset NOT NULL,
        [FechaLimitePago] datetimeoffset NOT NULL,
        [MontoTotal] decimal(18,2) NOT NULL,
        [MontoPagado] decimal(18,2) NOT NULL,
        [Estado] int NOT NULL,
        [Observaciones] nvarchar(500) NULL,
        [EsAutomatico] bit NOT NULL,
        [FechaCreacion] datetime2 NOT NULL,
        [FechaModificacion] datetime2 NULL,
        [Activo] bit NOT NULL,
        CONSTRAINT [PK_Cobros] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Cobros_Personas_PersonaId] FOREIGN KEY ([PersonaId]) REFERENCES [Personas] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122183014_AgregarCobrosYPagos'
)
BEGIN
    CREATE TABLE [CobroDetalles] (
        [Id] int NOT NULL IDENTITY,
        [CobroId] int NOT NULL,
        [ServicioId] int NOT NULL,
        [PersonaServicioId] int NOT NULL,
        [Concepto] nvarchar(200) NOT NULL,
        [Cantidad] decimal(18,2) NOT NULL,
        [PrecioUnitario] decimal(18,2) NOT NULL,
        [FechaCreacion] datetime2 NOT NULL,
        [FechaModificacion] datetime2 NULL,
        [Activo] bit NOT NULL,
        CONSTRAINT [PK_CobroDetalles] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_CobroDetalles_Cobros_CobroId] FOREIGN KEY ([CobroId]) REFERENCES [Cobros] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_CobroDetalles_PersonaServicios_PersonaServicioId] FOREIGN KEY ([PersonaServicioId]) REFERENCES [PersonaServicios] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_CobroDetalles_Servicios_ServicioId] FOREIGN KEY ([ServicioId]) REFERENCES [Servicios] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122183014_AgregarCobrosYPagos'
)
BEGIN
    CREATE TABLE [Pagos] (
        [Id] int NOT NULL IDENTITY,
        [NumeroReciboPago] nvarchar(50) NOT NULL,
        [CobroId] int NOT NULL,
        [PersonaId] int NOT NULL,
        [FechaPago] datetimeoffset NOT NULL,
        [Monto] decimal(18,2) NOT NULL,
        [MetodoPago] int NOT NULL,
        [NumeroReferencia] nvarchar(100) NULL,
        [Observaciones] nvarchar(500) NULL,
        [UsuarioId] int NULL,
        [ReciboImpreso] bit NOT NULL,
        [FechaImpresion] datetimeoffset NULL,
        [FechaCreacion] datetime2 NOT NULL,
        [FechaModificacion] datetime2 NULL,
        [Activo] bit NOT NULL,
        CONSTRAINT [PK_Pagos] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Pagos_Cobros_CobroId] FOREIGN KEY ([CobroId]) REFERENCES [Cobros] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Pagos_Personas_PersonaId] FOREIGN KEY ([PersonaId]) REFERENCES [Personas] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Pagos_Usuarios_UsuarioId] FOREIGN KEY ([UsuarioId]) REFERENCES [Usuarios] ([Id]) ON DELETE SET NULL
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122183014_AgregarCobrosYPagos'
)
BEGIN
    CREATE INDEX [IX_CobroDetalles_CobroId] ON [CobroDetalles] ([CobroId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122183014_AgregarCobrosYPagos'
)
BEGIN
    CREATE INDEX [IX_CobroDetalles_PersonaServicioId] ON [CobroDetalles] ([PersonaServicioId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122183014_AgregarCobrosYPagos'
)
BEGIN
    CREATE INDEX [IX_CobroDetalles_ServicioId] ON [CobroDetalles] ([ServicioId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122183014_AgregarCobrosYPagos'
)
BEGIN
    CREATE INDEX [IX_Cobros_Estado] ON [Cobros] ([Estado]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122183014_AgregarCobrosYPagos'
)
BEGIN
    CREATE INDEX [IX_Cobros_FechaLimitePago] ON [Cobros] ([FechaLimitePago]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122183014_AgregarCobrosYPagos'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Cobros_NumeroRecibo] ON [Cobros] ([NumeroRecibo]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122183014_AgregarCobrosYPagos'
)
BEGIN
    CREATE INDEX [IX_Cobros_Periodo] ON [Cobros] ([Periodo]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122183014_AgregarCobrosYPagos'
)
BEGIN
    CREATE INDEX [IX_Cobros_PersonaId] ON [Cobros] ([PersonaId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122183014_AgregarCobrosYPagos'
)
BEGIN
    CREATE INDEX [IX_Pagos_CobroId] ON [Pagos] ([CobroId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122183014_AgregarCobrosYPagos'
)
BEGIN
    CREATE INDEX [IX_Pagos_FechaPago] ON [Pagos] ([FechaPago]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122183014_AgregarCobrosYPagos'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Pagos_NumeroReciboPago] ON [Pagos] ([NumeroReciboPago]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122183014_AgregarCobrosYPagos'
)
BEGIN
    CREATE INDEX [IX_Pagos_PersonaId] ON [Pagos] ([PersonaId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122183014_AgregarCobrosYPagos'
)
BEGIN
    CREATE INDEX [IX_Pagos_UsuarioId] ON [Pagos] ([UsuarioId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122183014_AgregarCobrosYPagos'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260122183014_AgregarCobrosYPagos', N'10.0.1');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260124173245_AgregarFormatoReciboEmpresa'
)
BEGIN
    ALTER TABLE [Empresas] ADD [FormatoRecibo] nvarchar(max) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260124173245_AgregarFormatoReciboEmpresa'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260124173245_AgregarFormatoReciboEmpresa', N'10.0.1');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260207154226_AgregarNumeroReciboAportes'
)
BEGIN
    DROP INDEX [IX_Aportes_FechaAporte] ON [Aportes];
    DECLARE @var3 nvarchar(max);
    SELECT @var3 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Aportes]') AND [c].[name] = N'FechaAporte');
    IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Aportes] DROP CONSTRAINT ' + @var3 + ';');
    ALTER TABLE [Aportes] ALTER COLUMN [FechaAporte] datetimeoffset NOT NULL;
    CREATE INDEX [IX_Aportes_FechaAporte] ON [Aportes] ([FechaAporte]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260207154226_AgregarNumeroReciboAportes'
)
BEGIN
    ALTER TABLE [Aportes] ADD [NumeroRecibo] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260207154226_AgregarNumeroReciboAportes'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260207154226_AgregarNumeroReciboAportes', N'10.0.1');
END;

COMMIT;
GO

