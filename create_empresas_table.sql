-- Verificar y marcar migración inicial como aplicada
IF NOT EXISTS (SELECT * FROM __EFMigrationsHistory WHERE MigrationId = '20260113145335_InitialCreate')
BEGIN
    INSERT INTO __EFMigrationsHistory (MigrationId, ProductVersion) 
    VALUES ('20260113145335_InitialCreate', '9.0.0');
END
GO

-- Crear tabla Empresas si no existe
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Empresas')
BEGIN
    CREATE TABLE [Empresas] (
        [Id] int NOT NULL IDENTITY(1,1),
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
    
    CREATE UNIQUE INDEX [IX_Empresas_RTN] ON [Empresas] ([RTN]);
    
    PRINT 'Tabla Empresas creada exitosamente';
END
ELSE
BEGIN
    PRINT 'La tabla Empresas ya existe';
END
GO

-- Registrar la migración de Empresa
IF NOT EXISTS (SELECT * FROM __EFMigrationsHistory WHERE MigrationId = '20260114174009_AgregarTablaEmpresa')
BEGIN
    INSERT INTO __EFMigrationsHistory (MigrationId, ProductVersion) 
    VALUES ('20260114174009_AgregarTablaEmpresa', '9.0.0');
    PRINT 'Migración AgregarTablaEmpresa registrada';
END
GO
