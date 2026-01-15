using System;
using Microsoft.EntityFrameworkCore;
using SistemaComunidad.Data.Entities;

namespace SistemaComunidad.Data.Context;

/// <summary>
/// Contexto principal de la base de datos del sistema comunitario
/// </summary>
public class SistemaComunidadDbContext : DbContext
{
    // DbSets para todas las entidades
    public DbSet<Persona> Personas { get; set; }
    public DbSet<NucleoFamiliar> NucleosFamiliares { get; set; }
    public DbSet<Aporte> Aportes { get; set; }
    public DbSet<Ingreso> Ingresos { get; set; }
    public DbSet<Egreso> Egresos { get; set; }
    public DbSet<Actividad> Actividades { get; set; }
    public DbSet<ParticipacionActividad> ParticipacionesActividad { get; set; }
    public DbSet<Documento> Documentos { get; set; }
    public DbSet<Bien> Bienes { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<AuditoriaAccion> AuditoriasAccion { get; set; }
    public DbSet<Empresa> Empresas { get; set; }

    public SistemaComunidadDbContext(DbContextOptions<SistemaComunidadDbContext> options) 
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuraciones de entidades
        ConfigurarPersona(modelBuilder);
        ConfigurarNucleoFamiliar(modelBuilder);
        ConfigurarAporte(modelBuilder);
        ConfigurarIngreso(modelBuilder);
        ConfigurarEgreso(modelBuilder);
        ConfigurarActividad(modelBuilder);
        ConfigurarParticipacionActividad(modelBuilder);
        ConfigurarDocumento(modelBuilder);
        ConfigurarBien(modelBuilder);
        ConfigurarUsuario(modelBuilder);
        ConfigurarAuditoria(modelBuilder);
        ConfigurarEmpresa(modelBuilder);

        // Datos iniciales (seed)
        SeedDataInicial(modelBuilder);
    }

    private void ConfigurarPersona(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Persona>(entity =>
        {
            entity.ToTable("Personas");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombres).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Apellidos).IsRequired().HasMaxLength(100);
            entity.Property(e => e.IdentidadNacional).HasMaxLength(20);
            entity.Property(e => e.Telefono).HasMaxLength(20);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Direccion).HasMaxLength(500);
            
            entity.HasOne(e => e.NucleoFamiliar)
                  .WithMany(n => n.Miembros)
                  .HasForeignKey(e => e.NucleoFamiliarId)
                  .OnDelete(DeleteBehavior.SetNull);

            entity.HasIndex(e => e.IdentidadNacional);
        });
    }

    private void ConfigurarNucleoFamiliar(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NucleoFamiliar>(entity =>
        {
            entity.ToTable("NucleosFamiliares");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Direccion).HasMaxLength(500);
            entity.Property(e => e.Telefono).HasMaxLength(20);
        });
    }

    private void ConfigurarAporte(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Aporte>(entity =>
        {
            entity.ToTable("Aportes");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Monto).HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(e => e.Concepto).HasMaxLength(500);
            
            entity.HasOne(e => e.Persona)
                  .WithMany(p => p.Aportes)
                  .HasForeignKey(e => e.PersonaId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.FechaAporte);
            entity.HasIndex(e => e.PersonaId);
        });
    }

    private void ConfigurarIngreso(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ingreso>(entity =>
        {
            entity.ToTable("Ingresos");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Monto).HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(e => e.Concepto).IsRequired().HasMaxLength(500);
            entity.Property(e => e.NumeroRecibo).HasMaxLength(50);
            
            entity.HasIndex(e => e.FechaIngreso);
        });
    }

    private void ConfigurarEgreso(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Egreso>(entity =>
        {
            entity.ToTable("Egresos");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Monto).HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(e => e.Concepto).IsRequired().HasMaxLength(500);
            entity.Property(e => e.NumeroComprobante).HasMaxLength(50);
            entity.Property(e => e.Beneficiario).HasMaxLength(200);
            
            entity.HasIndex(e => e.FechaEgreso);
        });
    }

    private void ConfigurarActividad(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Actividad>(entity =>
        {
            entity.ToTable("Actividades");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Lugar).HasMaxLength(300);
            entity.Property(e => e.Responsable).HasMaxLength(100);
            
            entity.HasIndex(e => e.FechaInicio);
        });
    }

    private void ConfigurarParticipacionActividad(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ParticipacionActividad>(entity =>
        {
            entity.ToTable("ParticipacionesActividad");
            entity.HasKey(e => e.Id);
            
            entity.HasOne(e => e.Actividad)
                  .WithMany(a => a.Participaciones)
                  .HasForeignKey(e => e.ActividadId)
                  .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(e => e.Persona)
                  .WithMany(p => p.Participaciones)
                  .HasForeignKey(e => e.PersonaId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => new { e.ActividadId, e.PersonaId }).IsUnique();
        });
    }

    private void ConfigurarDocumento(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Documento>(entity =>
        {
            entity.ToTable("Documentos");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Titulo).IsRequired().HasMaxLength(300);
            entity.Property(e => e.RutaArchivo).HasMaxLength(500);
            entity.Property(e => e.NombreArchivo).HasMaxLength(200);
            
            entity.HasIndex(e => e.FechaDocumento);
            entity.HasIndex(e => e.TipoDocumento);
        });
    }

    private void ConfigurarBien(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bien>(entity =>
        {
            entity.ToTable("Bienes");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(200);
            entity.Property(e => e.CodigoInventario).HasMaxLength(50);
            entity.Property(e => e.ValorAdquisicion).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Ubicacion).HasMaxLength(300);
            entity.Property(e => e.Responsable).HasMaxLength(100);
            
            entity.HasIndex(e => e.CodigoInventario).IsUnique();
        });
    }

    private void ConfigurarUsuario(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("Usuarios");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.NombreUsuario).IsRequired().HasMaxLength(50);
            entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(500);
            entity.Property(e => e.NombreCompleto).IsRequired().HasMaxLength(150);
            entity.Property(e => e.Email).HasMaxLength(100);
            
            entity.HasIndex(e => e.NombreUsuario).IsUnique();
        });
    }

    private void ConfigurarAuditoria(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuditoriaAccion>(entity =>
        {
            entity.ToTable("AuditoriasAccion");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.NombreUsuario).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Accion).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Entidad).HasMaxLength(50);
            entity.Property(e => e.DireccionIP).HasMaxLength(50);
            
            entity.HasIndex(e => e.FechaAccion);
            entity.HasIndex(e => e.UsuarioId);
        });
    }

    private void ConfigurarEmpresa(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Empresa>(entity =>
        {
            entity.ToTable("Empresas");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.RazonSocial).IsRequired().HasMaxLength(200);
            entity.Property(e => e.NombreComercial).IsRequired().HasMaxLength(200);
            entity.Property(e => e.RTN).IsRequired().HasMaxLength(20);
            entity.Property(e => e.NumeroTelefono).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Direccion).IsRequired().HasMaxLength(500);
            entity.Property(e => e.CorreoElectronico).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Representante).HasMaxLength(200);
            entity.Property(e => e.TelefonoRepresentante).HasMaxLength(20);
            
            entity.HasIndex(e => e.RTN).IsUnique();
        });
    }

    private void SeedDataInicial(ModelBuilder modelBuilder)
    {
        // Usuario administrador por defecto
        // Password: admin123 (debe cambiarse en el primer inicio)
        // Hash generado con BCrypt para "admin123"
        modelBuilder.Entity<Usuario>().HasData(
            new Usuario
            {
                Id = 1,
                NombreUsuario = "admin",
                PasswordHash = "$2a$11$N4HqjRHBZIhHzVH5bYzNnO6SZfs7EKjKx5r7XZQlQu5KjKvVmJYJO", // Hash de admin123
                NombreCompleto = "Administrador del Sistema",
                Email = "admin@sistemacomunidad.local",
                Rol = RolUsuario.Administrador,
                CambiarPassword = true,
                FechaCreacion = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                Activo = true
            }
        );
    }
}
