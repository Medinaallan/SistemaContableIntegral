-- Script para crear las tablas de Cobros y Pagos
-- Ejecutar en SQL Server Management Studio o Azure Data Studio

USE [SistemaComunidad]
GO

-- Crear tabla Cobros
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Cobros')
BEGIN
    CREATE TABLE [dbo].[Cobros](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [NumeroRecibo] [nvarchar](50) NOT NULL,
        [PersonaId] [int] NOT NULL,
        [Periodo] [int] NOT NULL,
        [FechaEmision] [datetimeoffset](7) NOT NULL,
        [FechaLimitePago] [datetimeoffset](7) NOT NULL,
        [MontoTotal] [decimal](18, 2) NOT NULL,
        [MontoPagado] [decimal](18, 2) NOT NULL DEFAULT(0),
        [Estado] [int] NOT NULL DEFAULT(0),
        [Observaciones] [nvarchar](500) NULL,
        [EsAutomatico] [bit] NOT NULL DEFAULT(1),
        [FechaCreacion] [datetime2](7) NOT NULL,
        [FechaModificacion] [datetime2](7) NULL,
        [Activo] [bit] NOT NULL DEFAULT(1),
        CONSTRAINT [PK_Cobros] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_Cobros_Personas] FOREIGN KEY([PersonaId]) 
            REFERENCES [dbo].[Personas]([Id])
    )
    
    CREATE UNIQUE NONCLUSTERED INDEX [IX_Cobros_NumeroRecibo] ON [dbo].[Cobros]([NumeroRecibo] ASC)
    CREATE NONCLUSTERED INDEX [IX_Cobros_PersonaId] ON [dbo].[Cobros]([PersonaId] ASC)
    CREATE NONCLUSTERED INDEX [IX_Cobros_Periodo] ON [dbo].[Cobros]([Periodo] ASC)
    CREATE NONCLUSTERED INDEX [IX_Cobros_Estado] ON [dbo].[Cobros]([Estado] ASC)
    CREATE NONCLUSTERED INDEX [IX_Cobros_FechaLimitePago] ON [dbo].[Cobros]([FechaLimitePago] ASC)
    
    PRINT 'Tabla Cobros creada'
END

-- Crear tabla CobroDetalles
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'CobroDetalles')
BEGIN
    CREATE TABLE [dbo].[CobroDetalles](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [CobroId] [int] NOT NULL,
        [ServicioId] [int] NOT NULL,
        [PersonaServicioId] [int] NOT NULL,
        [Concepto] [nvarchar](200) NOT NULL,
        [Cantidad] [decimal](18, 2) NOT NULL DEFAULT(1),
        [PrecioUnitario] [decimal](18, 2) NOT NULL,
        [FechaCreacion] [datetime2](7) NOT NULL,
        [FechaModificacion] [datetime2](7) NULL,
        [Activo] [bit] NOT NULL DEFAULT(1),
        CONSTRAINT [PK_CobroDetalles] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_CobroDetalles_Cobros] FOREIGN KEY([CobroId]) 
            REFERENCES [dbo].[Cobros]([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_CobroDetalles_Servicios] FOREIGN KEY([ServicioId]) 
            REFERENCES [dbo].[Servicios]([Id]),
        CONSTRAINT [FK_CobroDetalles_PersonaServicios] FOREIGN KEY([PersonaServicioId]) 
            REFERENCES [dbo].[PersonaServicios]([Id])
    )
    
    CREATE NONCLUSTERED INDEX [IX_CobroDetalles_CobroId] ON [dbo].[CobroDetalles]([CobroId] ASC)
    CREATE NONCLUSTERED INDEX [IX_CobroDetalles_ServicioId] ON [dbo].[CobroDetalles]([ServicioId] ASC)
    
    PRINT 'Tabla CobroDetalles creada'
END

-- Crear tabla Pagos
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Pagos')
BEGIN
    CREATE TABLE [dbo].[Pagos](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [NumeroReciboPago] [nvarchar](50) NOT NULL,
        [CobroId] [int] NOT NULL,
        [PersonaId] [int] NOT NULL,
        [FechaPago] [datetimeoffset](7) NOT NULL,
        [Monto] [decimal](18, 2) NOT NULL,
        [MetodoPago] [int] NOT NULL DEFAULT(0),
        [NumeroReferencia] [nvarchar](100) NULL,
        [Observaciones] [nvarchar](500) NULL,
        [UsuarioId] [int] NULL,
        [ReciboImpreso] [bit] NOT NULL DEFAULT(0),
        [FechaImpresion] [datetimeoffset](7) NULL,
        [FechaCreacion] [datetime2](7) NOT NULL,
        [FechaModificacion] [datetime2](7) NULL,
        [Activo] [bit] NOT NULL DEFAULT(1),
        CONSTRAINT [PK_Pagos] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_Pagos_Cobros] FOREIGN KEY([CobroId]) 
            REFERENCES [dbo].[Cobros]([Id]),
        CONSTRAINT [FK_Pagos_Personas] FOREIGN KEY([PersonaId]) 
            REFERENCES [dbo].[Personas]([Id]),
        CONSTRAINT [FK_Pagos_Usuarios] FOREIGN KEY([UsuarioId]) 
            REFERENCES [dbo].[Usuarios]([Id]) ON DELETE SET NULL
    )
    
    CREATE UNIQUE NONCLUSTERED INDEX [IX_Pagos_NumeroReciboPago] ON [dbo].[Pagos]([NumeroReciboPago] ASC)
    CREATE NONCLUSTERED INDEX [IX_Pagos_CobroId] ON [dbo].[Pagos]([CobroId] ASC)
    CREATE NONCLUSTERED INDEX [IX_Pagos_PersonaId] ON [dbo].[Pagos]([PersonaId] ASC)
    CREATE NONCLUSTERED INDEX [IX_Pagos_FechaPago] ON [dbo].[Pagos]([FechaPago] ASC)
    
    PRINT 'Tabla Pagos creada'
END

-- Registrar migración
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '__EFMigrationsHistory')
BEGIN
    IF NOT EXISTS (SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] LIKE '%_AgregarCobrosYPagos')
    BEGIN
        DECLARE @MigrationId NVARCHAR(150)
        SELECT TOP 1 @MigrationId = [MigrationId] 
        FROM [__EFMigrationsHistory] 
        ORDER BY [MigrationId] DESC
        
        -- Buscar la nueva migración en el sistema de archivos
        DECLARE @NewMigrationId NVARCHAR(150) = (
            SELECT TOP 1 name 
            FROM sys.dm_io_virtual_file_stats(NULL, NULL)
            WHERE name LIKE '%AgregarCobrosYPagos%'
        )
        
        IF @NewMigrationId IS NOT NULL
        BEGIN
            INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
            VALUES (@NewMigrationId, '9.0.0')
            PRINT 'Migración AgregarCobrosYPagos registrada'
        END
    END
END

PRINT '¡Tablas de Cobros y Pagos creadas exitosamente!'
GO
