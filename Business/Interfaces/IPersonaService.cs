using SistemaComunidad.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaComunidad.Business.Interfaces;

/// <summary>
/// Interfaz del servicio de negocio para la entidad Persona
/// </summary>
public interface IPersonaService : IServicioBase
{
    /// <summary>
    /// Obtiene todas las personas activas
    /// </summary>
    Task<IEnumerable<Persona>> ObtenerTodasAsync();
    
    /// <summary>
    /// Obtiene una persona por su ID
    /// </summary>
    Task<Persona?> ObtenerPorIdAsync(int id);
    
    /// <summary>
    /// Busca personas por nombre o apellido
    /// </summary>
    Task<IEnumerable<Persona>> BuscarAsync(string textoBusqueda);
    
    /// <summary>
    /// Crea una nueva persona
    /// </summary>
    Task<Persona> CrearAsync(Persona persona);
    
    /// <summary>
    /// Actualiza una persona existente
    /// </summary>
    Task<Persona> ActualizarAsync(Persona persona);
    
    /// <summary>
    /// Elimina (desactiva) una persona
    /// </summary>
    Task EliminarAsync(int id);
    
    /// <summary>
    /// Obtiene el conteo total de personas activas
    /// </summary>
    Task<int> ObtenerConteoAsync();
}
