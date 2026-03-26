using System;
using System.ComponentModel.DataAnnotations;

namespace ValpoControl.Core.Domain.Entities;

/// <summary>
/// Módulo ArchivoPlano: Motor de exportación de datos agregados.
/// Genera archivos TXT y CSV para integración con otros sistemas.
/// </summary>
public class ArchivoPlano : BaseTramite
{
    [Required(ErrorMessage = "Nombre de archivo es obligatorio")]
    [StringLength(200)]
    public required string NombreArchivo { get; set; }

    [Required(ErrorMessage = "Formato es obligatorio")]
    [StringLength(10)]
    public required string Formato { get; set; } // TXT, CSV, JSON

    [StringLength(500)]
    public string? RutaAlmacenamiento { get; set; }

    public long TamanoArchivo { get; set; } = 0; // en bytes

    public DateTime FechaGeneracion { get; set; } = DateTime.UtcNow;

    public DateTime? FechaModificacion { get; set; }

    public int RegistrosIncluidos { get; set; } = 0;

    [StringLength(100)]
    public string? UsuarioGenerador { get; set; }

    public bool Procesado { get; set; } = false;

    [StringLength(500)]
    public string? UltimoDestino { get; set; }

    public override (bool IsValid, string Message) Validar()
    {
        if (string.IsNullOrWhiteSpace(NombreArchivo))
            return (false, "Nombre de archivo es requerido");

        if (!Formato.Equals("TXT", StringComparison.OrdinalIgnoreCase) &&
            !Formato.Equals("CSV", StringComparison.OrdinalIgnoreCase) &&
            !Formato.Equals("JSON", StringComparison.OrdinalIgnoreCase))
            return (false, "Formato debe ser TXT, CSV o JSON");

        if (RegistrosIncluidos < 0)
            return (false, "Registros incluidos no puede ser negativo");

        return base.Validar();
    }
}
