using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ValpoControl.Core.Domain.Entities;

namespace ValpoControl.Core.Domain.Interfaces;

/// <summary>
/// Interfaz genérica para servicios de trámites.
/// Define contrato estándar para CRUD y operaciones específicas.
/// </summary>
/// <typeparam name="T">Tipo de entidad que hereda de BaseTramite</typeparam>
public interface ITramiteService<T> where T : BaseTramite
{
    // CRUD Básico
    Task<T?> ObtenerPorIdAsync(int id);
    Task<T?> ObtenerPorRutAsync(string rut);
    Task<IEnumerable<T>> ObtenerTodosAsync(int pagina = 1, int pageSize = 10);
    Task<T> CrearAsync(T tramite);
    Task<T> ActualizarAsync(T tramite);
    Task<bool> EliminarAsync(int id);

    // Búsqueda
    Task<IEnumerable<T>> BuscarPorEstadoAsync(string estado);
    Task<IEnumerable<T>> BuscarPorFechaAsync(DateTime fechaInicio, DateTime fechaFin);

    // Operaciones de estado
    Task<T> CambiarEstadoAsync(int id, string nuevoEstado, string usuarioId, string? ipOrigen = null);

    // Validación
    (bool IsValid, string Message) ValidarTramite(T tramite);

    // Archivo
    Task<string> GenerarArchivoFisicoAsync(T tramite, string rutaBase);
    Task<bool> ActualizarCarpetaFisicoAsync(int id, string nuevaRuta);

    // Estadísticas
    Task<int> ObtenerTotalAsync();
    Task<Dictionary<string, int>> ObtenerEstadisticasPorEstadoAsync();
}
