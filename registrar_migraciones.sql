-- Script para registrar las migraciones en la base de datos
-- Ejecutar este script si las tablas ya existen pero las migraciones no están registradas

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '__EFMigrationsHistory')
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END
GO

-- Registrar las migraciones como aplicadas
IF NOT EXISTS (SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = '20260113145335_InitialCreate')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES ('20260113145335_InitialCreate', '9.0.0');
    PRINT 'Migración InitialCreate registrada'
END

IF NOT EXISTS (SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = '20260114174009_AgregarTablaEmpresa')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES ('20260114174009_AgregarTablaEmpresa', '9.0.0');
    PRINT 'Migración AgregarTablaEmpresa registrada'
END

IF NOT EXISTS (SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = '20260116173347_ActualizarModelo')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES ('20260116173347_ActualizarModelo', '9.0.0');
    PRINT 'Migración ActualizarModelo registrada'
END

IF NOT EXISTS (SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = '20260119155236_AgregarTablaServicios')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES ('20260119155236_AgregarTablaServicios', '9.0.0');
    PRINT 'Migración AgregarTablaServicios registrada'
END

PRINT 'Todas las migraciones han sido registradas exitosamente'
GO
