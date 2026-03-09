using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SistemaComunidad.Business.Interfaces;
using SistemaComunidad.Data.Entities;
using SistemaComunidad.Data.Interfaces;

namespace SistemaComunidad.Business.Services;

public class NucleoFamiliarService : ServicioBase, INucleoFamiliarService
{
    public NucleoFamiliarService(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task<IEnumerable<NucleoFamiliar>> ObtenerTodosAsync()
    {
        return await UnitOfWork.NucleosFamiliares.ObtenerTodosConMiembrosAsync();
    }

    public async Task<NucleoFamiliar?> ObtenerPorIdAsync(int id)
    {
        return await UnitOfWork.NucleosFamiliares.ObtenerConMiembrosAsync(id);
    }

    public async Task<IEnumerable<NucleoFamiliar>> BuscarAsync(string textoBusqueda)
    {
        if (string.IsNullOrWhiteSpace(textoBusqueda))
            return await ObtenerTodosAsync();

        return await UnitOfWork.NucleosFamiliares.BuscarPorNombreAsync(textoBusqueda);
    }

    public async Task<NucleoFamiliar> CrearAsync(NucleoFamiliar nucleo)
    {
        ValidarNucleoFamiliar(nucleo);

        nucleo.FechaCreacion = DateTime.Now;
        nucleo.Activo = true;

        var creado = await UnitOfWork.NucleosFamiliares.AgregarAsync(nucleo);
        await UnitOfWork.CompletarAsync();

        return creado;
    }

    public async Task<NucleoFamiliar> ActualizarAsync(NucleoFamiliar nucleo)
    {
        var existente = await UnitOfWork.NucleosFamiliares.ObtenerPorIdAsync(nucleo.Id);
        if (existente == null)
            throw new InvalidOperationException("No se encontró el núcleo familiar a actualizar.");

        ValidarNucleoFamiliar(nucleo);

        existente.Nombre = nucleo.Nombre;
        existente.Direccion = nucleo.Direccion;
        existente.Telefono = nucleo.Telefono;
        existente.Notas = nucleo.Notas;
        existente.FechaModificacion = DateTime.Now;

        await UnitOfWork.NucleosFamiliares.ActualizarAsync(existente);
        await UnitOfWork.CompletarAsync();

        return existente;
    }

    public async Task EliminarAsync(int id)
    {
        var nucleo = await UnitOfWork.NucleosFamiliares.ObtenerPorIdAsync(id);
        if (nucleo == null)
            throw new InvalidOperationException("No se encontró el núcleo familiar a eliminar.");

        nucleo.Activo = false;
        nucleo.FechaModificacion = DateTime.Now;

        await UnitOfWork.NucleosFamiliares.ActualizarAsync(nucleo);
        await UnitOfWork.CompletarAsync();
    }

    public async Task<int> ObtenerConteoAsync()
    {
        return await UnitOfWork.NucleosFamiliares.ContarAsync(n => n.Activo);
    }

    public async Task<MiembroFamiliar> AgregarMiembroAsync(MiembroFamiliar miembro)
    {
        ValidarMiembro(miembro);

        // Si se vincula a una persona, copiar sus datos
        if (miembro.PersonaId.HasValue)
        {
            var persona = await UnitOfWork.Personas.ObtenerPorIdAsync(miembro.PersonaId.Value);
            if (persona == null)
                throw new InvalidOperationException("La persona seleccionada no existe.");

            miembro.Nombres = persona.Nombres;
            miembro.Apellidos = persona.Apellidos;
            miembro.Telefono = persona.Telefono;
        }

        miembro.FechaCreacion = DateTime.Now;
        miembro.Activo = true;

        var creado = await UnitOfWork.MiembrosFamiliares.AgregarAsync(miembro);
        await UnitOfWork.CompletarAsync();

        return creado;
    }

    public async Task<MiembroFamiliar> ActualizarMiembroAsync(MiembroFamiliar miembro)
    {
        var existente = await UnitOfWork.MiembrosFamiliares.ObtenerPorIdAsync(miembro.Id);
        if (existente == null)
            throw new InvalidOperationException("No se encontró el miembro familiar a actualizar.");

        ValidarMiembro(miembro);

        existente.Nombres = miembro.Nombres;
        existente.Apellidos = miembro.Apellidos;
        existente.Telefono = miembro.Telefono;
        existente.Notas = miembro.Notas;
        existente.Rol = miembro.Rol;
        existente.PersonaId = miembro.PersonaId;
        existente.FechaModificacion = DateTime.Now;

        await UnitOfWork.MiembrosFamiliares.ActualizarAsync(existente);
        await UnitOfWork.CompletarAsync();

        return existente;
    }

    public async Task EliminarMiembroAsync(int miembroId)
    {
        var miembro = await UnitOfWork.MiembrosFamiliares.ObtenerPorIdAsync(miembroId);
        if (miembro == null)
            throw new InvalidOperationException("No se encontró el miembro familiar a eliminar.");

        miembro.Activo = false;
        miembro.FechaModificacion = DateTime.Now;

        await UnitOfWork.MiembrosFamiliares.ActualizarAsync(miembro);
        await UnitOfWork.CompletarAsync();
    }

    public async Task<IEnumerable<MiembroFamiliar>> ObtenerMiembrosPorFamiliaAsync(int nucleoFamiliarId)
    {
        var miembros = await UnitOfWork.MiembrosFamiliares.BuscarAsync(
            m => m.NucleoFamiliarId == nucleoFamiliarId && m.Activo);
        return miembros.OrderBy(m => m.Rol).ThenBy(m => m.Nombres);
    }

    public async Task<MiembroFamiliar> VincularPersonaAFamiliaAsync(int nucleoFamiliarId, int personaId, RolFamiliar rol)
    {
        var nucleo = await UnitOfWork.NucleosFamiliares.ObtenerPorIdAsync(nucleoFamiliarId);
        if (nucleo == null)
            throw new InvalidOperationException("No se encontró el núcleo familiar.");

        var persona = await UnitOfWork.Personas.ObtenerPorIdAsync(personaId);
        if (persona == null)
            throw new InvalidOperationException("No se encontró la persona.");

        // Verificar que no esté ya como miembro activo
        var miembrosExistentes = await UnitOfWork.MiembrosFamiliares.BuscarAsync(
            m => m.NucleoFamiliarId == nucleoFamiliarId && m.PersonaId == personaId && m.Activo);
        if (miembrosExistentes.Any())
            throw new InvalidOperationException("Esta persona ya es miembro de esta familia.");

        var miembro = new MiembroFamiliar
        {
            NucleoFamiliarId = nucleoFamiliarId,
            PersonaId = personaId,
            Nombres = persona.Nombres,
            Apellidos = persona.Apellidos,
            Telefono = persona.Telefono,
            Rol = rol,
            FechaCreacion = DateTime.Now,
            Activo = true
        };

        var creado = await UnitOfWork.MiembrosFamiliares.AgregarAsync(miembro);
        await UnitOfWork.CompletarAsync();

        // Actualizar la vinculación de la persona si no tiene familia asignada
        if (!persona.NucleoFamiliarId.HasValue)
        {
            persona.NucleoFamiliarId = nucleoFamiliarId;
            persona.FechaModificacion = DateTime.Now;
            await UnitOfWork.Personas.ActualizarAsync(persona);
            await UnitOfWork.CompletarAsync();
        }

        return creado;
    }

    private void ValidarNucleoFamiliar(NucleoFamiliar nucleo)
    {
        if (string.IsNullOrWhiteSpace(nucleo.Nombre))
            throw new ArgumentException("El nombre de la familia es obligatorio.");
    }

    private void ValidarMiembro(MiembroFamiliar miembro)
    {
        if (string.IsNullOrWhiteSpace(miembro.Nombres) && !miembro.PersonaId.HasValue)
            throw new ArgumentException("Debe indicar el nombre del miembro o vincularlo a una persona existente.");

        if (miembro.NucleoFamiliarId <= 0)
            throw new ArgumentException("Debe indicar la familia a la que pertenece el miembro.");
    }
}
