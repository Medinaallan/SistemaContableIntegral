# Script PowerShell para crear las tablas de Cobros y Pagos

$servidor = "localhost\SQLEXPRESS"
$database = "SistemaComunidad"
$scriptPath = "d:\ChurchSystem\create_cobros_pagos_tables.sql"

Write-Host "==================================" -ForegroundColor Cyan
Write-Host "Creando Tablas de Cobros y Pagos" -ForegroundColor Cyan
Write-Host "==================================" -ForegroundColor Cyan
Write-Host ""

try {
    # Ejecutar con autenticación de Windows
    sqlcmd -S $servidor -d $database -E -i $scriptPath
    
    Write-Host "`n✓ Tablas creadas exitosamente" -ForegroundColor Green
    Write-Host "`nYa puedes usar el módulo de Cobros y Pagos" -ForegroundColor Yellow
}
catch {
    Write-Host "`n✗ Error: $_" -ForegroundColor Red
    Write-Host "`nIntenta ejecutarlo manualmente desde SQL Server Management Studio" -ForegroundColor Yellow
}

Write-Host "`nPresiona cualquier tecla para continuar..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
