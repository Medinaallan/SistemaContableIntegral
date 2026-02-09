$cfg = Get-Content 'd:\ChurchSystem\appsettings.json' | ConvertFrom-Json
$cs = $cfg.ConnectionStrings.DefaultConnection
Write-Host "Using connection: $cs"
$conn = New-Object System.Data.SqlClient.SqlConnection $cs
$conn.Open()
$cmd = $conn.CreateCommand()
$cmd.CommandText = "IF OBJECT_ID(N'__EFMigrationsHistory') IS NULL SELECT 0 AS Count ELSE SELECT COUNT(*) FROM __EFMigrationsHistory"
$count = $cmd.ExecuteScalar()
Write-Host "__EFMigrationsHistory rows: $count"

$cmd.CommandText = "IF OBJECT_ID(N'__EFMigrationsHistory') IS NOT NULL SELECT MigrationId FROM __EFMigrationsHistory ORDER BY MigrationId"
$reader = $cmd.ExecuteReader()
while ($reader.Read()) { Write-Host $reader.GetString(0) }
$reader.Close()
$conn.Close()
