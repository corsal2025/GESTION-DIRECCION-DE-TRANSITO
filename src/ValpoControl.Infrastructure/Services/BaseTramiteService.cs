using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ValpoControl.Core.Common.Validators;
using ValpoControl.Core.Domain.Entities;
using ValpoControl.Core.Domain.Interfaces;
using ValpoControl.Infrastructure.Persistence;

namespace ValpoControl.Infrastructure.Services;

/// <summary>
/// Implementación genérica base para servicios de trámites.
/// Maneja automáticamente creación de carpetas en Windows Server.
/// </summary>
public abstract class BaseTramiteService<T> : ITramiteService<T> where T : BaseTramite
{
    protected readonly ValpoControlDbContext _context;
    protected readonly ILogger<BaseTramiteService<T>> _logger;
    protected readonly string _rutaBaseServidor;

    public BaseTramiteService(ValpoControlDbContext context, ILogger<BaseTramiteService<T>> logger, string rutaBaseServidor)
    {
        _context = context;
        _logger = logger;
        _rutaBaseServidor = rutaBaseServidor;
    }

    public virtual async Task<T?> ObtenerPorIdAsync(int id)
    {
        try
        {
            return await _context.Set<T>().FirstOrDefaultAsync(t => t.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener trámite por ID {Id}", id);
            throw;
        }
    }

    public virtual async Task<T?> ObtenerPorRutAsync(string rut)
    {
        try
        {
            var (isValid, _) = RutValidator.ValidarRut(rut);
            if (!isValid)
                return null;

            var rutLimpio = rut.Replace(".", "").Replace("-", "").ToUpper();
            return await _context.Set<T>().FirstOrDefaultAsync(t => t.RUT == rutLimpio);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener trámite por RUT");
            throw;
        }
    }

    public virtual async Task<IEnumerable<T>> ObtenerTodosAsync(int pagina = 1, int pageSize = 10)
    {
        try
        {
            return await _context.Set<T>()
                .Where(t => t.Activo)
                .Skip((pagina - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener trámites paginados");
            throw;
        }
    }

    public virtual async Task<T> CrearAsync(T tramite)
    {
        try
        {
            var (isValid, message) = ValidarTramite(tramite);
            if (!isValid)
                throw new InvalidOperationException($"Validación fallida: {message}");

            tramite.FechaIngreso = DateTime.UtcNow;
            tramite.FechaActualizacion = DateTime.UtcNow;
            tramite.TipoModulo = typeof(T).Name;

            // Crear carpeta física
            await GenerarArchivoFisicoAsync(tramite, _rutaBaseServidor);

            _context.Set<T>().Add(tramite);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Trámite creado: {TipoModulo} - RUT {RUT}", tramite.TipoModulo, tramite.RUT);
            return tramite;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear trámite");
            throw;
        }
    }

    public virtual async Task<T> ActualizarAsync(T tramite)
    {
        try
        {
            var (isValid, message) = ValidarTramite(tramite);
            if (!isValid)
                throw new InvalidOperationException($"Validación fallida: {message}");

            tramite.FechaActualizacion = DateTime.UtcNow;
            _context.Set<T>().Update(tramite);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Trámite actualizado: {Id}", tramite.Id);
            return tramite;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar trámite");
            throw;
        }
    }

    public virtual async Task<bool> EliminarAsync(int id)
    {
        try
        {
            var tramite = await ObtenerPorIdAsync(id);
            if (tramite == null)
                return false;

            tramite.Activo = false;
            tramite.FechaEliminacion = DateTime.UtcNow;

            _context.Set<T>().Update(tramite);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Trámite eliminado (soft delete): {Id}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar trámite");
            throw;
        }
    }

    public virtual async Task<IEnumerable<T>> BuscarPorEstadoAsync(string estado)
    {
        try
        {
            return await _context.Set<T>()
                .Where(t => t.Estado == estado && t.Activo)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al buscar por estado");
            throw;
        }
    }

    public virtual async Task<IEnumerable<T>> BuscarPorFechaAsync(DateTime fechaInicio, DateTime fechaFin)
    {
        try
        {
            return await _context.Set<T>()
                .Where(t => t.FechaIngreso >= fechaInicio && t.FechaIngreso <= fechaFin && t.Activo)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al buscar por fecha");
            throw;
        }
    }

    public virtual async Task<T> CambiarEstadoAsync(int id, string nuevoEstado, string usuarioId, string? ipOrigen = null)
    {
        try
        {
            var tramite = await ObtenerPorIdAsync(id);
            if (tramite == null)
                throw new InvalidOperationException("Trámite no encontrado");

            tramite.CambiarEstado(nuevoEstado, usuarioId, ipOrigen);
            tramite.FechaActualizacion = DateTime.UtcNow;

            _context.Set<T>().Update(tramite);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Estado cambiado: {Id} → {Nuevo Estado}", id, nuevoEstado);
            return tramite;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al cambiar estado");
            throw;
        }
    }

    public virtual (bool IsValid, string Message) ValidarTramite(T tramite)
    {
        // Validar RUT
        var (rutValido, rutMensaje) = RutValidator.ValidarRut(tramite.RUT);
        if (!rutValido)
            return (false, rutMensaje);

        // Validar propiedades base
        if (string.IsNullOrWhiteSpace(tramite.Nombre))
            return (false, "Nombre es requerido");

        if (string.IsNullOrWhiteSpace(tramite.Email))
            return (false, "Email es requerido");

        if (string.IsNullOrWhiteSpace(tramite.Telefono))
            return (false, "Teléfono es requerido");

        // Validación específica del módulo
        return tramite.Validar();
    }

    public virtual async Task<string> GenerarArchivoFisicoAsync(T tramite, string rutaBase)
    {
        try
        {
            var rutLimpio = tramite.RUT.Replace(".", "").Replace("-", "");
            var year = DateTime.UtcNow.Year.ToString();
            var moduloNombre = typeof(T).Name;

            // Patrón: \\Servidor\Gestion\{ModuloName}\{Year}\{RUT}\
            var rutaCompleta = Path.Combine(rutaBase, moduloNombre, year, rutLimpio);

            if (!Directory.Exists(rutaCompleta))
            {
                Directory.CreateDirectory(rutaCompleta);
                _logger.LogInformation("Carpeta creada: {Ruta}", rutaCompleta);
            }

            return rutaCompleta;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear carpeta física");
            throw;
        }
    }

    public virtual async Task<bool> ActualizarCarpetaFisicoAsync(int id, string nuevaRuta)
    {
        try
        {
            var tramite = await ObtenerPorIdAsync(id);
            if (tramite == null)
                return false;

            if (Directory.Exists(nuevaRuta))
            {
                _logger.LogInformation("Carpeta actualizada para trámite {Id}: {Ruta}", id, nuevaRuta);
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar carpeta física");
            throw;
        }
    }

    public virtual async Task<int> ObtenerTotalAsync()
    {
        try
        {
            return await _context.Set<T>().Where(t => t.Activo).CountAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener total");
            throw;
        }
    }

    public virtual async Task<Dictionary<string, int>> ObtenerEstadisticasPorEstadoAsync()
    {
        try
        {
            return await _context.Set<T>()
                .Where(t => t.Activo)
                .GroupBy(t => t.Estado)
                .Select(g => new { Estado = g.Key, Cantidad = g.Count() })
                .ToDictionaryAsync(x => x.Estado, x => x.Cantidad);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener estadísticas");
            throw;
        }
    }
}
