using System;
using System.Threading.Tasks;
using SistemaComunidad.Business.Interfaces;
using SistemaComunidad.Data.Entities;
using SistemaComunidad.Data.Interfaces;

namespace SistemaComunidad.Business.Services;

/// <summary>
/// Servicio de negocio para la gestión de datos de la empresa
/// </summary>
public class EmpresaService : ServicioBase, IEmpresaService
{
    public EmpresaService(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    /// <summary>
    /// Obtiene los datos de la empresa (el registro activo)
    /// </summary>
    public async Task<Empresa?> ObtenerDatosEmpresaAsync()
    {
        return await UnitOfWork.Empresas.ObtenerDatosEmpresaAsync();
    }

    /// <summary>
    /// Crea el registro inicial de la empresa
    /// </summary>
    public async Task<Empresa> CrearEmpresaAsync(Empresa empresa)
    {
        // Validar que no exista un registro previo
        var existeRegistro = await UnitOfWork.Empresas.ExisteRegistroAsync();
        if (existeRegistro)
        {
            throw new InvalidOperationException("Ya existe un registro de empresa. Use ActualizarEmpresaAsync para modificar los datos.");
        }

        // Validar datos requeridos
        ValidarDatosEmpresa(empresa);

        empresa.FechaCreacion = DateTime.Now;
        empresa.Activo = true;

        var empresaCreada = await UnitOfWork.Empresas.AgregarAsync(empresa);
        await UnitOfWork.CompletarAsync();

        return empresaCreada;
    }

    /// <summary>
    /// Actualiza los datos de la empresa existente
    /// </summary>
    public async Task<Empresa> ActualizarEmpresaAsync(Empresa empresa)
    {
        var empresaExistente = await UnitOfWork.Empresas.ObtenerPorIdAsync(empresa.Id);
        if (empresaExistente == null)
        {
            throw new InvalidOperationException("No se encontró el registro de empresa a actualizar.");
        }

        // Validar datos requeridos
        ValidarDatosEmpresa(empresa);

        // Actualizar propiedades
        empresaExistente.RazonSocial = empresa.RazonSocial;
        empresaExistente.NombreComercial = empresa.NombreComercial;
        empresaExistente.RTN = empresa.RTN;
        empresaExistente.NumeroTelefono = empresa.NumeroTelefono;
        empresaExistente.Direccion = empresa.Direccion;
        empresaExistente.CorreoElectronico = empresa.CorreoElectronico;
        empresaExistente.Representante = empresa.Representante;
        empresaExistente.TelefonoRepresentante = empresa.TelefonoRepresentante;
        empresaExistente.FechaModificacion = DateTime.Now;

        await UnitOfWork.Empresas.ActualizarAsync(empresaExistente);
        await UnitOfWork.CompletarAsync();

        return empresaExistente;
    }

    /// <summary>
    /// Verifica si ya existe un registro de empresa
    /// </summary>
    public async Task<bool> ExisteRegistroEmpresaAsync()
    {
        return await UnitOfWork.Empresas.ExisteRegistroAsync();
    }

    /// <summary>
    /// Valida que los datos obligatorios de la empresa estén completos
    /// </summary>
    private void ValidarDatosEmpresa(Empresa empresa)
    {
        if (string.IsNullOrWhiteSpace(empresa.RazonSocial))
            throw new ArgumentException("La razón social es obligatoria.");

        if (string.IsNullOrWhiteSpace(empresa.NombreComercial))
            throw new ArgumentException("El nombre comercial es obligatorio.");

        if (string.IsNullOrWhiteSpace(empresa.RTN))
            throw new ArgumentException("El RTN es obligatorio.");

        if (string.IsNullOrWhiteSpace(empresa.NumeroTelefono))
            throw new ArgumentException("El número de teléfono es obligatorio.");

        if (string.IsNullOrWhiteSpace(empresa.Direccion))
            throw new ArgumentException("La dirección es obligatoria.");

        if (string.IsNullOrWhiteSpace(empresa.CorreoElectronico))
            throw new ArgumentException("El correo electrónico es obligatorio.");

        // Validar formato de correo electrónico
        if (!System.Text.RegularExpressions.Regex.IsMatch(
            empresa.CorreoElectronico, 
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            throw new ArgumentException("El formato del correo electrónico no es válido.");
        }
    }
}
