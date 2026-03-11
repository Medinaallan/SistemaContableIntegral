 -- =============================================
-- Migración: Agregar tabla MiembrosFamiliares
-- Fecha: 2026-03-09
-- Descripción: Tabla para gestionar integrantes de núcleos familiares.
--              Permite vincular a persona existente o registrar miembros independientes.
-- =============================================

-- Verificar si la tabla ya existe antes de crearla
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'MiembrosFamiliares')
BEGIN
    CREATE TABLE [dbo].[MiembrosFamiliares] (
        [Id]                INT IDENTITY(1,1) NOT NULL,
        [Nombres]           NVARCHAR(100)     NOT NULL,
        [Apellidos]         NVARCHAR(100)     NOT NULL,
        [Telefono]          NVARCHAR(20)      NULL,
        [Notas]             NVARCHAR(500)     NULL,
        [Rol]               INT               NOT NULL DEFAULT 14, -- RolFamiliar.Otro
        [NucleoFamiliarId]  INT               NOT NULL,
        [PersonaId]         INT               NULL,
        [FechaCreacion]     DATETIME2         NOT NULL DEFAULT GETDATE(),
        [FechaModificacion] DATETIME2         NULL,
        [Activo]            BIT               NOT NULL DEFAULT 1,
        CONSTRAINT [PK_MiembrosFamiliares] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [FK_MiembrosFamiliares_NucleosFamiliares] FOREIGN KEY ([NucleoFamiliarId])
            REFERENCES [dbo].[NucleosFamiliares]([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_MiembrosFamiliares_Personas] FOREIGN KEY ([PersonaId])
            REFERENCES [dbo].[Personas]([Id]) ON DELETE SET NULL
    );

    -- Índices para búsquedas eficientes
    CREATE NONCLUSTERED INDEX [IX_MiembrosFamiliares_NucleoFamiliarId]
        ON [dbo].[MiembrosFamiliares]([NucleoFamiliarId]);

    CREATE NONCLUSTERED INDEX [IX_MiembrosFamiliares_PersonaId]
        ON [dbo].[MiembrosFamiliares]([PersonaId]);

    PRINT 'Tabla MiembrosFamiliares creada exitosamente.';
END
ELSE
BEGIN
    PRINT 'La tabla MiembrosFamiliares ya existe. No se realizaron cambios.';
END
GO

-- Registrar la migración si existe tabla de migraciones
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = '__EFMigrationsHistory')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM [dbo].[__EFMigrationsHistory] WHERE [MigrationId] = '20260309000000_AgregarMiembrosFamiliares')
    BEGIN
        INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
        VALUES ('20260309000000_AgregarMiembrosFamiliares', '10.0.1');
        PRINT 'Migración registrada en __EFMigrationsHistory.';
    END
END
GO
