$cfg = Get-Content 'd:\ChurchSystem\appsettings.json' | ConvertFrom-Json
$cs = $cfg.ConnectionStrings.DefaultConnection
Write-Host "Using connection: $cs"
$conn = New-Object System.Data.SqlClient.SqlConnection $cs
$conn.Open()
$batch = @"
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Aportes_FechaAporte' AND object_id = OBJECT_ID('dbo.Aportes'))
BEGIN
    DROP INDEX IX_Aportes_FechaAporte ON dbo.Aportes;
END;

ALTER TABLE dbo.Aportes ALTER COLUMN FechaAporte datetimeoffset NOT NULL;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Aportes_FechaAporte' AND object_id = OBJECT_ID('dbo.Aportes'))
BEGIN
    CREATE INDEX IX_Aportes_FechaAporte ON dbo.Aportes(FechaAporte);
END;
"@
try {
    $cmd = $conn.CreateCommand()
    $cmd.CommandText = $batch
    $cmd.CommandTimeout = 600
    $cmd.ExecuteNonQuery() | Out-Null
    Write-Host 'Alter + index recreation executed successfully.'
}
catch {
    Write-Error $_.Exception.Message
    exit 1
}
finally { $conn.Close() }
