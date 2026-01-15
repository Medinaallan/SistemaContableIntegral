BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260114174009_AgregarTablaEmpresa'
)
BEGIN
    CREATE TABLE [Empresas] (
        [Id] int NOT NULL IDENTITY,
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
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260114174009_AgregarTablaEmpresa'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Empresas_RTN] ON [Empresas] ([RTN]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260114174009_AgregarTablaEmpresa'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260114174009_AgregarTablaEmpresa', N'10.0.1');
END;

COMMIT;
GO

