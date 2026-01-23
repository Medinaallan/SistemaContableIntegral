using SistemaComunidad.Business.Interfaces;
using SistemaComunidad.Data.Entities;
using SistemaComunidad.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaComunidad.Business.Services;

/// <summary>
/// Servicio de negocio para gestionar la asignación de servicios a personas
/// </summary>
public class PersonaServicioService : ServicioBase, IPersonaServicioService
{
    private readonly IPersonaServicioRepositorio _personaServicioRepositorio;
    private readonly IPersonaRepositorio _personaRepositorio;
    private readonly IServicioRepositorio _servicioRepositorio;

    public PersonaServicioService(
        IPersonaServicioRepositorio personaServicioRepositorio,
        IPersonaRepositorio personaRepositorio,
        IServicioRepositorio servicioRepositorio,
        IUnitOfWork unitOfWork) : base(unitOfWork)
    {
        _personaServicioRepositorio = personaServicioRepositorio;
        _personaRepositorio = personaRepositorio;
        _servicioRepositorio = servicioRepositorio;
    }

    /// <summary>
    /// Asigna un servicio a una persona
    /// </summary>
    public async Task<PersonaServicio> AsignarServicioAPersonaAsync(int personaId, int servicioId, decimal? costoPersonalizado = null, string? notas = null)
    {
        // Validar la asignación
        var (esValido, mensaje) = await ValidarAsignacionAsync(personaId, servicioId);
        if (!esValido)
        {
            throw new InvalidOperationException(mensaje);
        }

        // Verificar si ya existe una asignación previa
        var asignacionExistente = await _personaServicioRepositorio.ObtenerAsignacionAsync(personaId, servicioId);
        
        if (asignacionExistente != null)
        {
            // Si existe pero está inactiva, reactivarla
            if (!asignacionExistente.EstaActivo)
            {
                asignacionExistente.EstaActivo = true;
                asignacionExistente.FechaInicio = DateTimeOffset.UtcNow;
                asignacionExistente.FechaFin = null;
                asignacionExistente.CostoPersonalizado = costoPersonalizado;
                asignacionExistente.Notas = notas;
                
                await _personaServicioRepositorio.ActualizarAsync(asignacionExistente);
                await UnitOfWork.CompletarAsync();
                return asignacionExistente;
            }
            else
            {
                throw new InvalidOperationException("La persona ya tiene este servicio asignado y activo");
            }
        }

        // Crear nueva asignación
        var personaServicio = new PersonaServicio
        {
            PersonaId = personaId,
            ServicioId = servicioId,
            FechaInicio = DateTimeOffset.UtcNow,
            EstaActivo = true,
            CostoPersonalizado = costoPersonalizado,
            Notas = notas
        };

        await _personaServicioRepositorio.AgregarAsync(personaServicio);
        await UnitOfWork.CompletarAsync();

        // Recargar con las relaciones
        return (await _personaServicioRepositorio.ObtenerAsignacionAsync(personaId, servicioId))!;
    }

    /// <summary>
    /// Desasigna un servicio de una persona (lo marca como inactivo)
    /// </summary>
    public async Task DesasignarServicioDePersonaAsync(int personaId, int servicioId)
    {
        var asignacion = await _personaServicioRepositorio.ObtenerAsignacionAsync(personaId, servicioId);
        
        if (asignacion == null)
        {
            throw new InvalidOperationException("No se encontró la asignación del servicio a la persona");
        }

        if (!asignacion.EstaActivo)
        {
            throw new InvalidOperationException("El servicio ya está desactivado para esta persona");
        }

        asignacion.EstaActivo = false;
        asignacion.FechaFin = DateTimeOffset.UtcNow;

        await _personaServicioRepositorio.ActualizarAsync(asignacion);
        await UnitOfWork.CompletarAsync();
    }

    /// <summary>
    /// Reactiva un servicio para una persona
    /// </summary>
    public async Task ReactivarServicioAsync(int personaServicioId)
    {
        var asignacion = await _personaServicioRepositorio.ObtenerPorIdAsync(personaServicioId);
        
        if (asignacion == null)
        {
            throw new InvalidOperationException("No se encontró la asignación del servicio");
        }

        if (asignacion.EstaActivo)
        {
            throw new InvalidOperationException("El servicio ya está activo");
        }

        asignacion.EstaActivo = true;
        asignacion.FechaInicio = DateTimeOffset.UtcNow;
        asignacion.FechaFin = null;

        await _personaServicioRepositorio.ActualizarAsync(asignacion);
        await UnitOfWork.CompletarAsync();
    }

    /// <summary>
    /// Obtiene todos los servicios asignados a una persona
    /// </summary>
    public async Task<IEnumerable<PersonaServicio>> ObtenerServiciosPorPersonaAsync(int personaId)
    {
        return await _personaServicioRepositorio.ObtenerServiciosPorPersonaAsync(personaId);
    }

    /// <summary>
    /// Obtiene todos los servicios activos de una persona
    /// </summary>
    public async Task<IEnumerable<PersonaServicio>> ObtenerServiciosActivosPorPersonaAsync(int personaId)
    {
        return await _personaServicioRepositorio.ObtenerServiciosActivosPorPersonaAsync(personaId);
    }

    /// <summary>
    /// Obtiene todas las personas que tienen un servicio específico
    /// </summary>
    public async Task<IEnumerable<PersonaServicio>> ObtenerPersonasPorServicioAsync(int servicioId)
    {
        return await _personaServicioRepositorio.ObtenerPersonasPorServicioAsync(servicioId);
    }

    /// <summary>
    /// Actualiza el costo personalizado de un servicio para una persona
    /// </summary>
    public async Task ActualizarCostoPersonalizadoAsync(int personaServicioId, decimal? costoPersonalizado)
    {
        var asignacion = await _personaServicioRepositorio.ObtenerPorIdAsync(personaServicioId);
        
        if (asignacion == null)
        {
            throw new InvalidOperationException("No se encontró la asignación del servicio");
        }

        if (costoPersonalizado.HasValue && costoPersonalizado.Value < 0)
        {
            throw new InvalidOperationException("El costo personalizado no puede ser negativo");
        }

        asignacion.CostoPersonalizado = costoPersonalizado;

        await _personaServicioRepositorio.ActualizarAsync(asignacion);
        await UnitOfWork.CompletarAsync();
    }

    /// <summary>
    /// Obtiene el total mensual que debe pagar una persona por todos sus servicios
    /// </summary>
    public async Task<decimal> ObtenerTotalMensualPorPersonaAsync(int personaId)
    {
        var serviciosActivos = await _personaServicioRepositorio.ObtenerServiciosActivosPorPersonaAsync(personaId);
        
        return serviciosActivos.Sum(ps => ps.ObtenerCostoEfectivo());
    }

    /// <summary>
    /// Genera los cobros pendientes para un período específico
    /// </summary>
    public async Task<IEnumerable<PersonaServicio>> GenerarCobrosPendientesPorPeriodoAsync(int periodo)
    {
        if (periodo < 202601 || periodo > 999912)
        {
            throw new InvalidOperationException("El período debe estar en formato YYYYMM (ejemplo: 202601)");
        }

        var mes = periodo % 100;
        if (mes < 1 || mes > 12)
        {
            throw new InvalidOperationException("El mes debe estar entre 01 y 12");
        }

        return await _personaServicioRepositorio.ObtenerServiciosPorCobrarEnPeriodoAsync(periodo);
    }

    /// <summary>
    /// Marca un servicio como cobrado en un período
    /// </summary>
    public async Task MarcarComoCobradoAsync(int personaServicioId, int periodo)
    {
        var asignacion = await _personaServicioRepositorio.ObtenerPorIdAsync(personaServicioId);
        
        if (asignacion == null)
        {
            throw new InvalidOperationException("No se encontró la asignación del servicio");
        }

        await _personaServicioRepositorio.MarcarComoCobradoAsync(personaServicioId, periodo);
        await UnitOfWork.CompletarAsync();
    }

    /// <summary>
    /// Valida que se puede asignar un servicio a una persona
    /// </summary>
    public async Task<(bool EsValido, string Mensaje)> ValidarAsignacionAsync(int personaId, int servicioId)
    {
        // Verificar que la persona existe
        var persona = await _personaRepositorio.ObtenerPorIdAsync(personaId);
        if (persona == null)
        {
            return (false, "La persona especificada no existe");
        }

        // Verificar que el servicio existe
        var servicio = await _servicioRepositorio.ObtenerPorIdAsync(servicioId);
        if (servicio == null)
        {
            return (false, "El servicio especificado no existe");
        }

        // Verificar que el servicio esté activo
        if (!servicio.Activo)
        {
            return (false, "El servicio no está activo");
        }

        // Verificar si ya tiene el servicio activo
        var tieneServicio = await _personaServicioRepositorio.PersonaTieneServicioAsync(personaId, servicioId);
        if (tieneServicio)
        {
            return (false, "La persona ya tiene este servicio asignado y activo");
        }

        return (true, "Validación exitosa");
    }
}
