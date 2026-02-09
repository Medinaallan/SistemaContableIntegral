$cfg = Get-Content 'd:\ChurchSystem\appsettings.json' | ConvertFrom-Json
$cs = $cfg.ConnectionStrings.DefaultConnection
Write-Host "Using connection: $cs"
$sql = @"
IF COL_LENGTH('dbo.Aportes','NumeroRecibo') IS NULL
BEGIN
    ALTER TABLE dbo.Aportes ADD NumeroRecibo NVARCHAR(100) NULL;
END
"@
$conn = New-Object System.Data.SqlClient.SqlConnection $cs
$conn.Open()
try {
    $cmd = $conn.CreateCommand()
    $cmd.CommandText = $sql
    $cmd.ExecuteNonQuery() | Out-Null
    Write-Host 'NumeroRecibo column ensured or already exists.'
}
catch {
    Write-Error $_.Exception.Message
    exit 1
}
finally { $conn.Close() }
