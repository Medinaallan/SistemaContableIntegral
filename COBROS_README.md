# Sistema de Cobros y Pagos Mensuales

## 🎯 Módulo Implementado

El sistema de cobros y pagos mensuales está listo con las siguientes funcionalidades:

###  📝 Entidades Creadas

1. **Cobro** - Representa una factura o recibo mensual
   - Número de recibo único
   - Período (YYYYMM)
   - Monto total y pagado
   - Estado (Pendiente, Pagado, Vencido, etc.)
   - Fecha límite de pago

2. **CobroDetalle** - Detalle de servicios en el cobro
   - Concepto (ej: Agua Potable)
   - Cantidad y precio
   - Subtotal

3. **Pago** - Registro de pagos realizados
   - Número de recibo de pago
   - Monto pagado
   - Método de pago (Efectivo, Tarjeta, etc.)
   - Fecha de pago

### 🔧 Repositorios Creados

- **CobroRepositorio**: Manejo de cobros en base de datos
- **PagoRepositorio**: Manejo de pagos en base de datos

### 📋 Próximos Pasos

Para completar el módulo, necesitas:

1. **Actualizar DbContext** - Agregar las nuevas entidades
2. **Crear migración** - Crear tablas en la base de datos
3. **Servicios de negocio** - Lógica para:
   - Generar cobros automáticos mensuales
   - Registrar pagos
   - Imprimir recibos
4. **Interfaz de usuario** - Ventanas para:
   - Ver cobros pendientes
   - Registrar pagos
   - Imprimir recibos
   - Generar cobros del mes

### 💡 Flujo del Sistema

```
1. Inicio del mes → Generar Cobros Automáticos
   ├─ Por cada persona con servicios activos
   ├─ Crear Cobro con número único
   ├─ Agregar CobroDetalle por cada servicio
   └─ Establecer fecha límite de pago

2. Persona viene a pagar → Registrar Pago
   ├─ Buscar cobro pendiente
   ├─ Registrar pago (efectivo, tarjeta, etc.)
   ├─ Actualizar estado del cobro
   └─ Generar e imprimir recibo

3. Imprimir Recibo
   ├─ Formato similar al de la foto
   ├─ Información del socio
   ├─ Detalle de servicios
   ├─ Total pagado
   └─ Fecha y número de recibo
```

### 🔄 Continuar Implementación

Ejecuta estos comandos para continuar:

1. Actualizar DbContext
2. Crear migración
3. Aplicar a base de datos
4. Crear servicios de negocio
5. Crear interfaz de usuario

¿Deseas que continúe con alguna parte específica?
