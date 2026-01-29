using SistemaComunidad.Business.Interfaces;
using SistemaComunidad.Data.Entities;
using SistemaComunidad.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaComunidad.Business.Services;

/// <summary>
/// Servicio de negocio para gestionar cobros y pagos mensuales
/// </summary>
public class CobroService : ServicioBase, ICobroService
{
    private readonly ICobroRepositorio _cobroRepositorio;
    private readonly IPagoRepositorio _pagoRepositorio;
    private readonly IPersonaServicioRepositorio _personaServicioRepositorio;

    public CobroService(
        ICobroRepositorio cobroRepositorio,
        IPagoRepositorio pagoRepositorio,
        IPersonaServicioRepositorio personaServicioRepositorio,
        IUnitOfWork unitOfWork) : base(unitOfWork)
    {
        _cobroRepositorio = cobroRepositorio;
        _pagoRepositorio = pagoRepositorio;
        _personaServicioRepositorio = personaServicioRepositorio;
    }

    /// <summary>
    /// Genera cobros mensuales para todas las personas con servicios activos
    /// </summary>
    public async Task<IEnumerable<Cobro>> GenerarCobrosMensualesAsync(int periodo)
    {
        var cobrosGenerados = new List<Cobro>();
        
        // Obtener todos los servicios pendientes de cobro en el período
        var serviciosPorCobrar = await _personaServicioRepositorio.ObtenerServiciosPorCobrarEnPeriodoAsync(periodo);
        
        // Agrupar por persona
        var serviciosPorPersona = serviciosPorCobrar.GroupBy(ps => ps.PersonaId);

        foreach (var grupo in serviciosPorPersona)
        {
            var personaId = grupo.Key;
            
            // Verificar si ya existe un cobro para esta persona en este período
            var existeCobro = await _cobroRepositorio.ExisteCobroEnPeriodoAsync(personaId, periodo);
            if (existeCobro)
                continue;

            // Generar el cobro
            var cobro = await GenerarCobroParaPersonaAsync(personaId, periodo);
            cobrosGenerados.Add(cobro);
        }

        return cobrosGenerados;
    }

    /// <summary>
    /// Genera un cobro individual para una persona en un período
    /// </summary>
    public async Task<Cobro> GenerarCobroParaPersonaAsync(int personaId, int periodo)
    {
        // Obtener servicios activos de la persona
        var servicios = await _personaServicioRepositorio.ObtenerServiciosActivosPorPersonaAsync(personaId);
        
        // TEMPORAL: Para pruebas, generar cobro con todos los servicios activos sin validar período
        var serviciosPorCobrar = servicios.ToList();
        // var serviciosPorCobrar = servicios.Where(ps => ps.DebeCobrarseEn(periodo)).ToList();

        if (!serviciosPorCobrar.Any())
        {
            throw new InvalidOperationException("La persona no tiene servicios activos");
        }

        // Generar número de recibo único
        var numeroRecibo = await GenerarNumeroReciboAsync(periodo);

        // Calcular fecha límite (15 del mes siguiente)
        var anio = periodo / 100;
        var mes = periodo % 100;
        var fechaEmision = DateTimeOffset.Now;
        var fechaLimite = new DateTimeOffset(anio, mes, 15, 23, 59, 59, TimeSpan.Zero);

        // Crear el cobro
        var cobro = new Cobro
        {
            NumeroRecibo = numeroRecibo,
            PersonaId = personaId,
            Periodo = periodo,
            FechaEmision = fechaEmision,
            FechaLimitePago = fechaLimite,
            MontoTotal = 0,
            MontoPagado = 0,
            Estado = EstadoCobro.Pendiente,
            EsAutomatico = true
        };

        // Agregar detalles por cada servicio
        decimal montoTotal = 0;
        foreach (var ps in serviciosPorCobrar)
        {
            var costo = ps.ObtenerCostoEfectivo();
            montoTotal += costo;

            var detalle = new CobroDetalle
            {
                Cobro = cobro,
                ServicioId = ps.ServicioId,
                PersonaServicioId = ps.Id,
                Concepto = ps.Servicio?.Nombre ?? "Servicio",
                Cantidad = 1,
                PrecioUnitario = costo
            };

            cobro.Detalles.Add(detalle);
        }

        cobro.MontoTotal = montoTotal;

        // Guardar en la base de datos
        await _cobroRepositorio.AgregarAsync(cobro);
        await UnitOfWork.CompletarAsync();

        // Marcar los servicios como cobrados
        foreach (var ps in serviciosPorCobrar)
        {
            await _personaServicioRepositorio.MarcarComoCobradoAsync(ps.Id, periodo);
        }
        await UnitOfWork.CompletarAsync();

        return cobro;
    }

    /// <summary>
    /// Registra un pago para un cobro
    /// </summary>
    public async Task<Pago> RegistrarPagoAsync(int cobroId, decimal monto, MetodoPago metodoPago, int? usuarioId = null, string? observaciones = null)
    {
        var cobro = await _cobroRepositorio.ObtenerPorIdAsync(cobroId);
        if (cobro == null)
        {
            throw new InvalidOperationException("No se encontró el cobro especificado");
        }

        if (monto <= 0)
        {
            throw new InvalidOperationException("El monto del pago debe ser mayor a cero");
        }

        if (monto > cobro.SaldoPendiente)
        {
            throw new InvalidOperationException($"El monto excede el saldo pendiente (L {cobro.SaldoPendiente:N2})");
        }

        // Generar número de recibo de pago
        var numeroReciboPago = await GenerarNumeroReciboPagoAsync();

        // Crear el pago
        var pago = new Pago
        {
            NumeroReciboPago = numeroReciboPago,
            CobroId = cobroId,
            PersonaId = cobro.PersonaId,
            FechaPago = DateTimeOffset.Now,
            Monto = monto,
            MetodoPago = metodoPago,
            Observaciones = observaciones,
            UsuarioId = usuarioId,
            ReciboImpreso = false
        };

        await _pagoRepositorio.AgregarAsync(pago);

        // Actualizar el cobro
        cobro.MontoPagado += monto;

        if (cobro.SaldoPendiente <= 0)
        {
            cobro.Estado = EstadoCobro.Pagado;
        }
        else if (cobro.MontoPagado > 0)
        {
            cobro.Estado = EstadoCobro.PagoParcial;
        }

        await _cobroRepositorio.ActualizarAsync(cobro);
        await UnitOfWork.CompletarAsync();

        return pago;
    }

    public async Task<IEnumerable<Cobro>> ObtenerCobrosPorPersonaAsync(int personaId)
    {
        return await _cobroRepositorio.ObtenerCobrosPorPersonaAsync(personaId);
    }

    public async Task<IEnumerable<Cobro>> ObtenerCobrosPendientesAsync(int personaId)
    {
        return await _cobroRepositorio.ObtenerCobrosPendientesPorPersonaAsync(personaId);
    }

    public async Task<Cobro?> ObtenerCobroPorIdAsync(int cobroId)
    {
        return await _cobroRepositorio.ObtenerPorIdAsync(cobroId);
    }

    public async Task<Cobro?> ObtenerCobroPorNumeroReciboAsync(string numeroRecibo)
    {
        return await _cobroRepositorio.ObtenerPorNumeroReciboAsync(numeroRecibo);
    }

    public async Task<IEnumerable<Cobro>> ObtenerCobrosVencidosAsync()
    {
        return await _cobroRepositorio.ObtenerCobrosVencidosAsync();
    }

    public async Task<IEnumerable<Pago>> ObtenerHistorialPagosAsync(int personaId)
    {
        return await _pagoRepositorio.ObtenerPagosPorPersonaAsync(personaId);
    }

    public async Task<Pago?> ObtenerPagoPorIdAsync(int pagoId)
    {
        return await _pagoRepositorio.ObtenerPorIdAsync(pagoId);
    }

    public async Task<decimal> ObtenerTotalPendienteAsync(int personaId)
    {
        var cobrosPendientes = await ObtenerCobrosPendientesAsync(personaId);
        return cobrosPendientes.Sum(c => c.SaldoPendiente);
    }

    public async Task<decimal> ObtenerTotalCobradoEnPeriodoAsync(int periodo)
    {
        var cobros = await _cobroRepositorio.ObtenerCobrosPorPeriodoAsync(periodo);
        return cobros.Sum(c => c.MontoPagado);
    }

    public async Task MarcarReciboImpresAsync(int pagoId)
    {
        var pago = await _pagoRepositorio.ObtenerPorIdAsync(pagoId);
        if (pago != null)
        {
            pago.ReciboImpreso = true;
            pago.FechaImpresion = DateTimeOffset.Now;
            await _pagoRepositorio.ActualizarAsync(pago);
            await UnitOfWork.CompletarAsync();
        }
    }

    /// <summary>
    /// Genera un número de recibo único para un cobro
    /// Formato: YYYYMM-0001
    /// </summary>
    private async Task<string> GenerarNumeroReciboAsync(int periodo)
    {
        var ultimoNumero = await _cobroRepositorio.ObtenerUltimoNumeroReciboAsync(periodo);
        var nuevoNumero = ultimoNumero + 1;
        return $"{periodo}-{nuevoNumero:D4}";
    }

    /// <summary>
    /// Genera un número de recibo de pago único
    /// Formato: 00001
    /// </summary>
    private async Task<string> GenerarNumeroReciboPagoAsync()
    {
        var ultimoNumero = await _pagoRepositorio.ObtenerUltimoNumeroReciboPagoAsync();
        var nuevoNumero = ultimoNumero + 1;
        return $"{nuevoNumero:D6}";
    }
}
