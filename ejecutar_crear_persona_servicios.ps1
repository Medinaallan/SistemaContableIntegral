# Script PowerShell para ejecutar el SQL de creación de tabla PersonaServicios
# Asegúrate de tener sqlcmd instalado

$servidor = "localhost\SQLEXPRESS"  # Cambia si es necesario
$database = "SistemaComunidad"
$scriptPath = "d:\ChurchSystem\create_persona_servicios_table.sql"

Write-Host "Ejecutando script SQL..." -ForegroundColor Cyan

try {
    # Ejecutar con autenticación de Windows
    sqlcmd -S $servidor -d $database -E -i $scriptPath
    
    Write-Host "`n✓ Script ejecutado exitosamente" -ForegroundColor Green
    Write-Host "`nAhora puedes usar la funcionalidad de PersonaServicios en la aplicación" -ForegroundColor Yellow
}
catch {
    Write-Host "`n✗ Error al ejecutar el script: $_" -ForegroundColor Red
    Write-Host "`nIntenta ejecutarlo manualmente desde SQL Server Management Studio" -ForegroundColor Yellow
}
