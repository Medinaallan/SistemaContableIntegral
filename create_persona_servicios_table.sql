-- Script para crear la tabla PersonaServicios
-- Ejecutar este script en SQL Server Management Studio o Azure Data Studio

USE [SistemaComunidad]
GO

-- Verificar si la tabla ya existe
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PersonaServicios')
BEGIN
    PRINT 'Creando tabla PersonaServicios...'
    
    CREATE TABLE [dbo].[PersonaServicios](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [PersonaId] [int] NOT NULL,
        [ServicioId] [int] NOT NULL,
        [FechaInicio] [datetimeoffset](7) NOT NULL,
        [FechaFin] [datetimeoffset](7) NULL,
        [EstaActivo] [bit] NOT NULL,
        [CostoPersonalizado] [decimal](18, 2) NULL,
        [Notas] [nvarchar](500) NULL,
        [UltimoPeriodoCobrado] [int] NULL,
        [FechaCreacion] [datetime2](7) NOT NULL,
        [FechaModificacion] [datetime2](7) NULL,
        [Activo] [bit] NOT NULL,
        CONSTRAINT [PK_PersonaServicios] PRIMARY KEY CLUSTERED ([Id] ASC)
    )
    
    PRINT 'Tabla PersonaServicios creada exitosamente'
    
    -- Crear Foreign Keys
    ALTER TABLE [dbo].[PersonaServicios] WITH CHECK 
    ADD CONSTRAINT [FK_PersonaServicios_Personas_PersonaId] 
    FOREIGN KEY([PersonaId]) REFERENCES [dbo].[Personas] ([Id])
    ON DELETE CASCADE
    
    ALTER TABLE [dbo].[PersonaServicios] WITH CHECK 
    ADD CONSTRAINT [FK_PersonaServicios_Servicios_ServicioId] 
    FOREIGN KEY([ServicioId]) REFERENCES [dbo].[Servicios] ([Id])
    ON DELETE CASCADE
    
    PRINT 'Foreign Keys creadas'
    
    -- Crear Índices
    CREATE NONCLUSTERED INDEX [IX_PersonaServicios_PersonaId] 
    ON [dbo].[PersonaServicios]([PersonaId] ASC)
    
    CREATE NONCLUSTERED INDEX [IX_PersonaServicios_ServicioId] 
    ON [dbo].[PersonaServicios]([ServicioId] ASC)
    
    CREATE NONCLUSTERED INDEX [IX_PersonaServicios_EstaActivo] 
    ON [dbo].[PersonaServicios]([EstaActivo] ASC)
    
    CREATE NONCLUSTERED INDEX [IX_PersonaServicios_PersonaId_ServicioId] 
    ON [dbo].[PersonaServicios]([PersonaId] ASC, [ServicioId] ASC)
    
    PRINT 'Índices creados'
    
    -- Registrar la migración
    IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '__EFMigrationsHistory')
    BEGIN
        IF NOT EXISTS (SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = '20260122164438_AgregarTablaPersonaServicios')
        BEGIN
            INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
            VALUES ('20260122164438_AgregarTablaPersonaServicios', '9.0.0')
            PRINT 'Migración registrada en historial'
        END
    END
    
    PRINT '¡Tabla PersonaServicios creada y configurada exitosamente!'
END
ELSE
BEGIN
    PRINT 'La tabla PersonaServicios ya existe'
END
GO
