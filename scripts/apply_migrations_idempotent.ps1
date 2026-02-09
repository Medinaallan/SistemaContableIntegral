# Apply idempotent EF migrations script by splitting on GO and executing batches
$cfg = Get-Content 'd:\ChurchSystem\appsettings.json' | ConvertFrom-Json
$cs = $cfg.ConnectionStrings.DefaultConnection
Write-Host "Using connection: $cs"
$sql = Get-Content 'd:\ChurchSystem\migrations_idempotent.sql' -Raw
$batches = [System.Text.RegularExpressions.Regex]::Split($sql, '^(?:\s*GO\s*)$', [System.Text.RegularExpressions.RegexOptions]::Multiline -bor [System.Text.RegularExpressions.RegexOptions]::IgnoreCase)
$conn = New-Object System.Data.SqlClient.SqlConnection $cs
$conn.Open()
try {
    $i = 0
    foreach ($batch in $batches) {
        $i++
        $t = $batch.Trim()
        if ([string]::IsNullOrWhiteSpace($t)) { continue }
        Write-Host "Executing batch $i..."
        $cmd = $conn.CreateCommand()
        $cmd.CommandTimeout = 600
        $cmd.CommandText = $t
        $cmd.ExecuteNonQuery() | Out-Null
    }
    Write-Host 'All batches executed successfully.'
}
catch {
    Write-Error $_.Exception.Message
    exit 1
}
finally { $conn.Close() }
