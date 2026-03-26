using Microsoft.EntityFrameworkCore;
using ValpoControl.Core.Domain.Entities;

namespace ValpoControl.Infrastructure.Persistence;

/// <summary>
/// DbContext principal para ValpoControl.
/// Configura Table Per Hierarchy (TPH) con discriminador "TipoModulo".
/// </summary>
public class ValpoControlDbContext : DbContext
{
    public ValpoControlDbContext(DbContextOptions<ValpoControlDbContext> options) : base(options) { }

    // DbSets para cada módulo
    public DbSet<BaseTramite> Tramites { get; set; } = null!;
    public DbSet<GestionF8> GestionesF8 { get; set; } = null!;
    public DbSet<Modulo12> Modulos12 { get; set; } = null!;
    public DbSet<Modulo16> Modulos16 { get; set; } = null!;
    public DbSet<HojaRuta> HojasRuta { get; set; } = null!;
    public DbSet<ExamenPsicometrico> ExamenesPsicometricos { get; set; } = null!;
    public DbSet<ExamenTeorico> ExamenesTeoricos { get; set; } = null!;
    public DbSet<CambioDomicilio> CambiosDomicilio { get; set; } = null!;
    public DbSet<GestionLicencias> GestionesLicencias { get; set; } = null!;
    public DbSet<ArchivoPlano> ArchivosPlanos { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurar TPH (Table Per Hierarchy) con discriminador
        modelBuilder.Entity<BaseTramite>()
            .HasDiscriminator<string>("TipoModulo")
            .HasValue<BaseTramite>("BaseTramite")
            .HasValue<GestionF8>("GestionF8")
            .HasValue<Modulo12>("Modulo12")
            .HasValue<Modulo16>("Modulo16")
            .HasValue<HojaRuta>("HojaRuta")
            .HasValue<ExamenPsicometrico>("ExamenPsicometrico")
            .HasValue<ExamenTeorico>("ExamenTeorico")
            .HasValue<CambioDomicilio>("CambioDomicilio")
            .HasValue<GestionLicencias>("GestionLicencias")
            .HasValue<ArchivoPlano>("ArchivoPlano");

        // Índices para optimización de consultas
        modelBuilder.Entity<BaseTramite>()
            .HasIndex(t => t.RUT);

        modelBuilder.Entity<BaseTramite>()
            .HasIndex(t => t.Estado);

        modelBuilder.Entity<BaseTramite>()
            .HasIndex(t => t.FechaIngreso);

        modelBuilder.Entity<BaseTramite>()
            .HasIndex(t => t.UsuarioId);
    }
}
