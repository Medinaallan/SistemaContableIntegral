-- Script para actualizar la tabla Servicios eliminando las columnas de fechas
-- Ejecutar este script para actualizar la estructura de la tabla

USE SistemaComunidad;
GO

-- Verificar y eliminar las columnas de fechas si existen
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Servicios' AND COLUMN_NAME = 'FechaInicio')
BEGIN
    ALTER TABLE [dbo].[Servicios] DROP COLUMN [FechaInicio];
    PRINT 'Columna FechaInicio eliminada'
END

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Servicios' AND COLUMN_NAME = 'FechaFin')
BEGIN
    ALTER TABLE [dbo].[Servicios] DROP COLUMN [FechaFin];
    PRINT 'Columna FechaFin eliminada'
END

PRINT 'Tabla Servicios actualizada correctamente - Columnas de fechas eliminadas'
GO
