$cfg = Get-Content 'd:\ChurchSystem\appsettings.json' | ConvertFrom-Json
$cs = $cfg.ConnectionStrings.DefaultConnection
Write-Host "Using connection: $cs"
$conn = New-Object System.Data.SqlClient.SqlConnection $cs
$conn.Open()
$cmd = $conn.CreateCommand()
$cmd.CommandText = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='Aportes' ORDER BY ORDINAL_POSITION"
$reader = $cmd.ExecuteReader()
Write-Host "Columns in Aportes:"
while ($reader.Read()) { Write-Host " - " + $reader.GetString(0) }
$reader.Close()
$conn.Close()
