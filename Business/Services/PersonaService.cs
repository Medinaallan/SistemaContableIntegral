using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SistemaComunidad.Business.Interfaces;
using SistemaComunidad.Data.Entities;
using SistemaComunidad.Data.Interfaces;

namespace SistemaComunidad.Business.Services;

/// <summary>
/// Servicio de negocio para la gestión de personas
/// </summary>
public class PersonaService : ServicioBase, IPersonaService
{
    public PersonaService(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task<IEnumerable<Persona>> ObtenerTodasAsync()
    {
        var personas = await UnitOfWork.Personas.ObtenerTodosAsync();
        return personas.Where(p => p.Activo).OrderBy(p => p.Apellidos).ThenBy(p => p.Nombres);
    }

    public async Task<Persona?> ObtenerPorIdAsync(int id)
    {
        return await UnitOfWork.Personas.ObtenerPorIdAsync(id);
    }

    public async Task<IEnumerable<Persona>> BuscarAsync(string textoBusqueda)
    {
        if (string.IsNullOrWhiteSpace(textoBusqueda))
            return await ObtenerTodasAsync();

        var todasLasPersonas = await ObtenerTodasAsync();
        var textoLower = textoBusqueda.ToLower();

        return todasLasPersonas.Where(p =>
            p.Nombres.ToLower().Contains(textoLower) ||
            p.Apellidos.ToLower().Contains(textoLower) ||
            (!string.IsNullOrEmpty(p.IdentidadNacional) && p.IdentidadNacional.Contains(textoLower)) ||
            (!string.IsNullOrEmpty(p.Telefono) && p.Telefono.Contains(textoLower))
        );
    }

    public async Task<Persona> CrearAsync(Persona persona)
    {
        ValidarPersona(persona);
        
        persona.FechaCreacion = DateTime.Now;
        persona.Activo = true;

        var personaCreada = await UnitOfWork.Personas.AgregarAsync(persona);
        await UnitOfWork.CompletarAsync();

        return personaCreada;
    }

    public async Task<Persona> ActualizarAsync(Persona persona)
    {
        var personaExistente = await UnitOfWork.Personas.ObtenerPorIdAsync(persona.Id);
        if (personaExistente == null)
        {
            throw new InvalidOperationException("No se encontró la persona a actualizar.");
        }

        ValidarPersona(persona);

        personaExistente.Nombres = persona.Nombres;
        personaExistente.Apellidos = persona.Apellidos;
        personaExistente.IdentidadNacional = persona.IdentidadNacional;
        personaExistente.FechaNacimiento = persona.FechaNacimiento;
        personaExistente.Telefono = persona.Telefono;
        personaExistente.Email = persona.Email;
        personaExistente.Direccion = persona.Direccion;
        personaExistente.EstadoParticipacion = persona.EstadoParticipacion;
        personaExistente.Notas = persona.Notas;
        personaExistente.NucleoFamiliarId = persona.NucleoFamiliarId;
        personaExistente.FechaModificacion = DateTime.Now;

        await UnitOfWork.Personas.ActualizarAsync(personaExistente);
        await UnitOfWork.CompletarAsync();

        return personaExistente;
    }

    public async Task EliminarAsync(int id)
    {
        var persona = await UnitOfWork.Personas.ObtenerPorIdAsync(id);
        if (persona == null)
        {
            throw new InvalidOperationException("No se encontró la persona a eliminar.");
        }

        persona.Activo = false;
        persona.FechaModificacion = DateTime.Now;

        await UnitOfWork.Personas.ActualizarAsync(persona);
        await UnitOfWork.CompletarAsync();
    }

    public async Task<int> ObtenerConteoAsync()
    {
        return await UnitOfWork.Personas.ContarAsync(p => p.Activo);
    }

    private void ValidarPersona(Persona persona)
    {
        if (string.IsNullOrWhiteSpace(persona.Nombres))
            throw new ArgumentException("El nombre es obligatorio.");

        if (string.IsNullOrWhiteSpace(persona.Apellidos))
            throw new ArgumentException("Los apellidos son obligatorios.");

        if (!string.IsNullOrWhiteSpace(persona.Email) && 
            !System.Text.RegularExpressions.Regex.IsMatch(persona.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            throw new ArgumentException("El formato del correo electrónico no es válido.");
        }
    }
}
