-- Script para registrar las migraciones existentes en la base de datos
-- Ejecutar este script si las tablas ya existen pero las migraciones no están registradas

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

IF NOT EXISTS (SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = '20260119164710_EliminarFechasDeServicios')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES ('20260119164710_EliminarFechasDeServicios', '9.0.0');
    PRINT 'Migración EliminarFechasDeServicios registrada'
END

-- Esta migración NO se registra porque la vamos a aplicar
-- IF NOT EXISTS (SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = '20260122164438_AgregarTablaPersonaServicios')
-- BEGIN
--     INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
--     VALUES ('20260122164438_AgregarTablaPersonaServicios', '9.0.0');
--     PRINT 'Migración AgregarTablaPersonaServicios registrada'
-- END

PRINT 'Todas las migraciones existentes han sido registradas exitosamente'
PRINT 'Ahora ejecute: dotnet ef database update'
GO
