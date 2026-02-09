-- Script: Agregar columna NumeroRecibo a la tabla Aportes
-- Ejecutar en la base de datos 'SistemaComunidad' (SQL Server)

IF COL_LENGTH('dbo.Aportes', 'NumeroRecibo') IS NULL
BEGIN
    ALTER TABLE dbo.Aportes
    ADD NumeroRecibo NVARCHAR(100) NULL;
END

-- Opcional: inicializar valores existentes con NULL o formato por defecto
-- UPDATE dbo.Aportes SET NumeroRecibo = NULL WHERE NumeroRecibo IS NULL;

PRINT 'Script ejecutado: columna NumeroRecibo agregada si no existía.';
