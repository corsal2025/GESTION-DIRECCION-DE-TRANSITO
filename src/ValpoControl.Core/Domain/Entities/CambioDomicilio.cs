using System;
using System.ComponentModel.DataAnnotations;

namespace ValpoControl.Core.Domain.Entities;

/// <summary>
/// Módulo CambioDomicilio: Actualización de domicilio del titular.
/// </summary>
public class CambioDomicilio : BaseTramite
{
    [Required(ErrorMessage = "Dirección anterior es obligatoria")]
    [StringLength(300)]
    public required string DireccionAnterior { get; set; }

    [Required(ErrorMessage = "Dirección nueva es obligatoria")]
    [StringLength(300)]
    public required string DireccionNueva { get; set; }

    [StringLength(100)]
    public string? ComunaAnterior { get; set; }

    [StringLength(100)]
    public string? ComunaNueva { get; set; }

    public DateTime FechaSolicitud { get; set; } = DateTime.UtcNow;

    public DateTime? FechaActualizacion { get; set; }

    [StringLength(100)]
    public string? ComprobanteDomicilio { get; set; }

    public bool DomicilioVerificado { get; set; } = false;

    public override (bool IsValid, string Message) Validar()
    {
        if (string.IsNullOrWhiteSpace(DireccionAnterior))
            return (false, "Dirección anterior es requerida");

        if (string.IsNullOrWhiteSpace(DireccionNueva))
            return (false, "Dirección nueva es requerida");

        if (DireccionAnterior == DireccionNueva)
            return (false, "La dirección nueva debe ser diferente a la anterior");

        return base.Validar();
    }
}
