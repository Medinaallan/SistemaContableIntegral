using SistemaComunidad.Business.Interfaces;
using SistemaComunidad.Data.Entities;
using SistemaComunidad.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaComunidad.Business.Services;

/// <summary>
/// Servicio de negocio para gestionar servicios del patronato
/// </summary>
public class ServicioService : ServicioBase, IServicioService
{
    private readonly IServicioRepositorio _servicioRepositorio;

    public ServicioService(IServicioRepositorio servicioRepositorio, IUnitOfWork unitOfWork) 
        : base(unitOfWork)
    {
        _servicioRepositorio = servicioRepositorio;
    }

    /// <summary>
    /// Obtiene todos los servicios
    /// </summary>
    public async Task<IEnumerable<Servicio>> ObtenerTodosAsync()
    {
        return await _servicioRepositorio.ObtenerTodosAsync();
    }

    /// <summary>
    /// Obtiene un servicio por ID
    /// </summary>
    public async Task<Servicio?> ObtenerPorIdAsync(int id)
    {
        return await _servicioRepositorio.ObtenerPorIdAsync(id);
    }

    /// <summary>
    /// Crea un nuevo servicio con validación
    /// </summary>
    public async Task<Servicio> CrearAsync(Servicio servicio)
    {
        var (esValido, mensaje) = await ValidarServicioAsync(servicio);
        if (!esValido)
        {
            throw new InvalidOperationException(mensaje);
        }

        await _servicioRepositorio.AgregarAsync(servicio);
        await UnitOfWork.CompletarAsync();
        return servicio;
    }

    /// <summary>
    /// Actualiza un servicio con validación
    /// </summary>
    public async Task<Servicio> ActualizarAsync(Servicio servicio)
    {
        var (esValido, mensaje) = await ValidarServicioAsync(servicio);
        if (!esValido)
        {
            throw new InvalidOperationException(mensaje);
        }

        await _servicioRepositorio.ActualizarAsync(servicio);
        await UnitOfWork.CompletarAsync();
        return servicio;
    }

    /// <summary>
    /// Elimina un servicio (eliminación lógica)
    /// </summary>
    public async Task EliminarAsync(int id)
    {
        var servicio = await _servicioRepositorio.ObtenerPorIdAsync(id);
        if (servicio != null)
        {
            await _servicioRepositorio.EliminarAsync(id);
            await UnitOfWork.CompletarAsync();
        }
    }

    /// <summary>
    /// Activa un servicio
    /// </summary>
    public async Task ActivarAsync(int id)
    {
        var servicio = await _servicioRepositorio.ObtenerPorIdAsync(id);
        if (servicio != null)
        {
            servicio.Activo = true;
            await _servicioRepositorio.ActualizarAsync(servicio);
            await UnitOfWork.CompletarAsync();
        }
    }

    /// <summary>
    /// Desactiva un servicio
    /// </summary>
    public async Task DesactivarAsync(int id)
    {
        var servicio = await _servicioRepositorio.ObtenerPorIdAsync(id);
        if (servicio != null)
        {
            servicio.Activo = false;
            await _servicioRepositorio.ActualizarAsync(servicio);
            await UnitOfWork.CompletarAsync();
        }
    }

    /// <summary>
    /// Obtiene todos los servicios activos
    /// </summary>
    public async Task<IEnumerable<Servicio>> ObtenerServiciosActivosAsync()
    {
        return await _servicioRepositorio.ObtenerServiciosActivosAsync();
    }

    /// <summary>
    /// Obtiene servicios por empresa/patronato
    /// </summary>
    public async Task<IEnumerable<Servicio>> ObtenerServiciosPorEmpresaAsync(int empresaId)
    {
        return await _servicioRepositorio.ObtenerServiciosPorEmpresaAsync(empresaId);
    }

    /// <summary>
    /// Obtiene servicios obligatorios
    /// </summary>
    public async Task<IEnumerable<Servicio>> ObtenerServiciosObligatoriosAsync()
    {
        return await _servicioRepositorio.ObtenerServiciosObligatoriosAsync();
    }

    /// <summary>
    /// Valida que un servicio sea correcto antes de guardarlo
    /// </summary>
    public async Task<(bool EsValido, string Mensaje)> ValidarServicioAsync(Servicio servicio)
    {
        if (string.IsNullOrWhiteSpace(servicio.Nombre))
        {
            return (false, "El nombre del servicio es requerido");
        }

        if (servicio.CostoMensual < 0)
        {
            return (false, "El costo mensual no puede ser negativo");
        }

        // Verificar que no exista otro servicio con el mismo nombre para la misma empresa
        var serviciosExistentes = await ObtenerTodosAsync();
        var servicioExistente = serviciosExistentes.FirstOrDefault(s => 
            s.Nombre.Equals(servicio.Nombre, StringComparison.OrdinalIgnoreCase) && 
            s.EmpresaId == servicio.EmpresaId && 
            s.Id != servicio.Id &&
            s.Activo);

        if (servicioExistente != null)
        {
            return (false, "Ya existe un servicio con ese nombre para esta empresa");
        }

        return (true, "Validación exitosa");
    }

}
