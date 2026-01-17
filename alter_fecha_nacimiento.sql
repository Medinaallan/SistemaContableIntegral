-- Cambiar el tipo de columna FechaNacimiento de datetime2 a datetimeoffset
ALTER TABLE Personas
ALTER COLUMN FechaNacimiento datetimeoffset NULL;

-- Insertar o actualizar el registro en __EFMigrationsHistory para marcar InitialCreate como aplicada
IF NOT EXISTS (SELECT 1 FROM __EFMigrationsHistory WHERE MigrationId = '20260113145335_InitialCreate')
BEGIN
    INSERT INTO __EFMigrationsHistory (MigrationId, ProductVersion)
    VALUES ('20260113145335_InitialCreate', '10.0.0');
END
GO
