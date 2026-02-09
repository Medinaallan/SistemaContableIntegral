$cfg = Get-Content 'd:\ChurchSystem\appsettings.json' | ConvertFrom-Json
$cs = $cfg.ConnectionStrings.DefaultConnection
Write-Host "Using connection: $cs"
$conn = New-Object System.Data.SqlClient.SqlConnection $cs
$conn.Open()
$sql = 'ALTER TABLE dbo.Aportes ALTER COLUMN FechaAporte datetimeoffset NOT NULL'
try {
    $cmd = $conn.CreateCommand()
    $cmd.CommandText = $sql
    $cmd.ExecuteNonQuery() | Out-Null
    Write-Host 'Column altered to datetimeoffset.'
}
catch {
    Write-Error $_.Exception.Message
    exit 1
}
finally { $conn.Close() }
