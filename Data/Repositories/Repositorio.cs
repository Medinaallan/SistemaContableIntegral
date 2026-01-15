using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SistemaComunidad.Data.Context;
using SistemaComunidad.Data.Interfaces;

namespace SistemaComunidad.Data.Repositories;

/// <summary>
/// Implementación genérica del repositorio base
/// </summary>
public class Repositorio<T> : IRepositorio<T> where T : class
{
    protected readonly SistemaComunidadDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repositorio(SistemaComunidadDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<T?> ObtenerPorIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<IEnumerable<T>> ObtenerTodosAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task<IEnumerable<T>> BuscarAsync(Expression<Func<T, bool>> predicado)
    {
        return await _dbSet.Where(predicado).ToListAsync();
    }

    public virtual async Task<T> AgregarAsync(T entidad)
    {
        await _dbSet.AddAsync(entidad);
        return entidad;
    }

    public virtual async Task ActualizarAsync(T entidad)
    {
        _dbSet.Update(entidad);
        await Task.CompletedTask;
    }

    public virtual async Task EliminarAsync(int id)
    {
        var entidad = await ObtenerPorIdAsync(id);
        if (entidad != null)
        {
            _dbSet.Remove(entidad);
        }
    }

    public virtual async Task<bool> ExisteAsync(int id)
    {
        var entidad = await ObtenerPorIdAsync(id);
        return entidad != null;
    }

    public virtual async Task<int> ContarAsync(Expression<Func<T, bool>>? predicado = null)
    {
        if (predicado == null)
            return await _dbSet.CountAsync();
        
        return await _dbSet.CountAsync(predicado);
    }
}
