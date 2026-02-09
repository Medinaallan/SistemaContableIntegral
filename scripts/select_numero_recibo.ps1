$cfg = Get-Content 'd:\ChurchSystem\appsettings.json' | ConvertFrom-Json
$cs = $cfg.ConnectionStrings.DefaultConnection
Write-Host "Using connection: $cs"
$conn = New-Object System.Data.SqlClient.SqlConnection $cs
$conn.Open()
$cmd = $conn.CreateCommand()
$cmd.CommandText = 'SELECT TOP 1 NumeroRecibo FROM Aportes'
try {
    $val = $cmd.ExecuteScalar()
    Write-Host "Query succeeded. Value: $val"
} catch {
    Write-Host "Query failed: " + $_.Exception.Message
}
$conn.Close()
