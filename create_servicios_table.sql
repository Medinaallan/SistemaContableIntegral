-- Script para crear la tabla Servicios
-- Ejecutar este script si las tablas base ya existen en la base de datos

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Servicios')
BEGIN
    CREATE TABLE [dbo].[Servicios] (
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

    CREATE INDEX [IX_Servicios_Nombre] ON [Servicios] ([Nombre]);
    CREATE INDEX [IX_Servicios_EmpresaId] ON [Servicios] ([EmpresaId]);

    PRINT 'Tabla Servicios creada exitosamente'
END
ELSE
BEGIN
    PRINT 'La tabla Servicios ya existe'
END
GO
