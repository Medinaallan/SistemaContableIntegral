-- Agregar columna FormatoRecibo a la tabla Empresas
USE SistemaComunidad;
GO

-- Verificar si la columna ya existe
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Empresas]') AND name = 'FormatoRecibo')
BEGIN
    ALTER TABLE [dbo].[Empresas]
    ADD [FormatoRecibo] nvarchar(50) NOT NULL DEFAULT 'MediaCarta';
    
    PRINT 'Columna FormatoRecibo agregada exitosamente a la tabla Empresas';
END
ELSE
BEGIN
    PRINT 'La columna FormatoRecibo ya existe en la tabla Empresas';
END
GO
