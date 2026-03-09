using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using SistemaComunidad.Data.Context;
using SistemaComunidad.Data.Entities;
using SistemaComunidad.Data.Interfaces;

namespace SistemaComunidad.Data.Repositories;

/// <summary>
/// Unidad de trabajo para gestionar transacciones y repositorios
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly SistemaComunidadDbContext _context;
    private IDbContextTransaction? _transaction;

    public IPersonaRepositorio Personas { get; }
    public IUsuarioRepositorio Usuarios { get; }
    public INucleoFamiliarRepositorio NucleosFamiliares { get; }
    public IRepositorio<MiembroFamiliar> MiembrosFamiliares { get; }
    public IRepositorio<Aporte> Aportes { get; }
    public IRepositorio<Ingreso> Ingresos { get; }
    public IRepositorio<Egreso> Egresos { get; }
    public IRepositorio<Actividad> Actividades { get; }
    public IRepositorio<Documento> Documentos { get; }
    public IRepositorio<Bien> Bienes { get; }
    public IRepositorio<AuditoriaAccion> Auditorias { get; }
    public IEmpresaRepositorio Empresas { get; }

    public UnitOfWork(SistemaComunidadDbContext context)
    {
        _context = context;
        
        // Inicializar repositorios
        Personas = new PersonaRepositorio(_context);
        Usuarios = new UsuarioRepositorio(_context);
        NucleosFamiliares = new NucleoFamiliarRepositorio(_context);
        MiembrosFamiliares = new Repositorio<MiembroFamiliar>(_context);
        Aportes = new Repositorio<Aporte>(_context);
        Ingresos = new Repositorio<Ingreso>(_context);
        Egresos = new Repositorio<Egreso>(_context);
        Actividades = new Repositorio<Actividad>(_context);
        Documentos = new Repositorio<Documento>(_context);
        Bienes = new Repositorio<Bien>(_context);
        Auditorias = new Repositorio<AuditoriaAccion>(_context);
        Empresas = new EmpresaRepositorio(_context);
    }

    public async Task<int> CompletarAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task IniciarTransaccionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task ConfirmarTransaccionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RevertirTransaccionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
