$cfg = Get-Content 'd:\ChurchSystem\appsettings.json' | ConvertFrom-Json
$cs = $cfg.ConnectionStrings.DefaultConnection
Write-Host "Using connection: $cs"
$conn = New-Object System.Data.SqlClient.SqlConnection $cs
$conn.Open()
$cmd = $conn.CreateCommand()
$cmd.CommandText = "SELECT DATA_TYPE, IS_NULLABLE, COLUMN_DEFAULT FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='Aportes' AND COLUMN_NAME='FechaAporte'"
$reader = $cmd.ExecuteReader()
if ($reader.Read()) {
    $dt = $reader.GetString(0)
    $nullable = $reader.GetString(1)
    if ($reader.IsDBNull(2)) { $default = '<no default>' } else { $default = $reader.GetValue(2) }
    Write-Host "FechaAporte: type=$dt nullable=$nullable default=$default"
}
$reader.Close()
$cmd.CommandText = "SELECT COUNT(*) FROM Aportes WHERE FechaAporte IS NULL"
$nulls = $cmd.ExecuteScalar()
Write-Host "Null count: $nulls"
$conn.Close()
