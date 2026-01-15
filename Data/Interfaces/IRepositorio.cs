using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace SistemaComunidad.Data.Interfaces;

/// <summary>
/// Repositorio genérico base para operaciones CRUD
/// </summary>
public interface IRepositorio<T> where T : class
{
    Task<T?> ObtenerPorIdAsync(int id);
    Task<IEnumerable<T>> ObtenerTodosAsync();
    Task<IEnumerable<T>> BuscarAsync(Expression<Func<T, bool>> predicado);
    Task<T> AgregarAsync(T entidad);
    Task ActualizarAsync(T entidad);
    Task EliminarAsync(int id);
    Task<bool> ExisteAsync(int id);
    Task<int> ContarAsync(Expression<Func<T, bool>>? predicado = null);
}
