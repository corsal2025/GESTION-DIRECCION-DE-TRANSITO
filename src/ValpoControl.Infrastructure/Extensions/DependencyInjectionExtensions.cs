using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ValpoControl.Core.Domain.Entities;
using ValpoControl.Core.Domain.Interfaces;
using ValpoControl.Infrastructure.Persistence;
using ValpoControl.Infrastructure.Services;

namespace ValpoControl.Infrastructure.Extensions;

/// <summary>
/// Extensiones para inyección de dependencias municipales.
/// Permite registrar todos los servicios con una sola línea: services.AddMunicipalidadModules()
/// </summary>
public static class DependencyInjectionExtensions
{
    /// <summary>
    /// Registra todos los servicios y módulos municipales en el contenedor de dependencias.
    /// </summary>
    public static IServiceCollection AddMunicipalidadModules(
        this IServiceCollection services,
        string connectionString,
        string rutaBaseServidor = @"\\Servidor\Gestion")
    {
        // Registrar DbContext
        services.AddDbContext<ValpoControlDbContext>(options =>
            options.UseSqlServer(connectionString)
                .EnableSensitiveDataLogging());

        // Registrar servicios específicos para cada módulo
        RegisterModuleServices(services, rutaBaseServidor);

        return services;
    }

    /// <summary>
    /// Registra servicios específicos para cada módulo municipal.
    /// </summary>
    private static void RegisterModuleServices(IServiceCollection services, string rutaBaseServidor)
    {
        // Registrar servicios con Factory Pattern
        services.AddScoped<ITramiteService<GestionF8>>(provider =>
            new GestionF8Service(
                provider.GetRequiredService<ValpoControlDbContext>(),
                provider.GetRequiredService<Microsoft.Extensions.Logging.ILogger<GestionF8Service>>(),
                rutaBaseServidor));

        services.AddScoped<ITramiteService<Modulo12>>(provider =>
            new Modulo12Service(
                provider.GetRequiredService<ValpoControlDbContext>(),
                provider.GetRequiredService<Microsoft.Extensions.Logging.ILogger<Modulo12Service>>(),
                rutaBaseServidor));

        services.AddScoped<ITramiteService<Modulo16>>(provider =>
            new Modulo16Service(
                provider.GetRequiredService<ValpoControlDbContext>(),
                provider.GetRequiredService<Microsoft.Extensions.Logging.ILogger<Modulo16Service>>(),
                rutaBaseServidor));

        services.AddScoped<ITramiteService<HojaRuta>>(provider =>
            new HojaRutaService(
                provider.GetRequiredService<ValpoControlDbContext>(),
                provider.GetRequiredService<Microsoft.Extensions.Logging.ILogger<HojaRutaService>>(),
                rutaBaseServidor));

        services.AddScoped<ITramiteService<ExamenPsicometrico>>(provider =>
            new ExamenPsicometricoService(
                provider.GetRequiredService<ValpoControlDbContext>(),
                provider.GetRequiredService<Microsoft.Extensions.Logging.ILogger<ExamenPsicometricoService>>(),
                rutaBaseServidor));

        services.AddScoped<ITramiteService<ExamenTeorico>>(provider =>
            new ExamenTeoricoService(
                provider.GetRequiredService<ValpoControlDbContext>(),
                provider.GetRequiredService<Microsoft.Extensions.Logging.ILogger<ExamenTeoricoService>>(),
                rutaBaseServidor));

        services.AddScoped<ITramiteService<CambioDomicilio>>(provider =>
            new CambioDomicilioService(
                provider.GetRequiredService<ValpoControlDbContext>(),
                provider.GetRequiredService<Microsoft.Extensions.Logging.ILogger<CambioDomicilioService>>(),
                rutaBaseServidor));

        services.AddScoped<ITramiteService<GestionLicencias>>(provider =>
            new GestionLicenciasService(
                provider.GetRequiredService<ValpoControlDbContext>(),
                provider.GetRequiredService<Microsoft.Extensions.Logging.ILogger<GestionLicenciasService>>(),
                rutaBaseServidor));

        services.AddScoped<ITramiteService<ArchivoPlano>>(provider =>
            new ArchivoPlanoService(
                provider.GetRequiredService<ValpoControlDbContext>(),
                provider.GetRequiredService<Microsoft.Extensions.Logging.ILogger<ArchivoPlanoService>>(),
                rutaBaseServidor));
    }

    /// <summary>
    /// Registra servicios con SQLite en lugar de SQL Server (para desarrollo local).
    /// </summary>
    public static IServiceCollection AddMunicipalidadModulesSqlite(
        this IServiceCollection services,
        string connectionString,
        string rutaBaseServidor = @".\Gestion")
    {
        services.AddDbContext<ValpoControlDbContext>(options =>
            options.UseSqlite(connectionString)
                .EnableSensitiveDataLogging());

        RegisterModuleServices(services, rutaBaseServidor);

        return services;
    }
}
