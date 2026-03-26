using System;
using System.ComponentModel.DataAnnotations;

namespace ValpoControl.Core.Domain.Entities;

/// <summary>
/// Módulo 12: Gestión administrativa con folio y derivación.
/// </summary>
public class Modulo12 : BaseTramite
{
    [Required(ErrorMessage = "Folio es obligatorio")]
    [StringLength(50)]
    public required string Folio { get; set; }

    [Required]
    [StringLength(200)]
    public required string Ubicacion { get; set; }

    [StringLength(200)]
    public string? Derivacion { get; set; }

    [StringLength(500)]
    public string? Descripcion { get; set; }

    public DateTime? FechaVencimiento { get; set; }

    public override (bool IsValid, string Message) Validar()
    {
        if (string.IsNullOrWhiteSpace(Folio))
            return (false, "Folio es requerido");

        if (string.IsNullOrWhiteSpace(Ubicacion))
            return (false, "Ubicación es requerida");

        return base.Validar();
    }
}
