using System;
using System.ComponentModel.DataAnnotations;

namespace ValpoControl.Core.Domain.Entities;

/// <summary>
/// Módulo GestionLicencias: Otorgamiento y gestión de licencias de conducir.
/// </summary>
public class GestionLicencias : BaseTramite
{
    [Required(ErrorMessage = "Tipo de licencia es obligatorio")]
    [StringLength(50)]
    public required string TipoLicencia { get; set; } // A, B, C, D, E, F, etc.

    [Required]
    public DateTime FechaOtorgamiento { get; set; }

    public DateTime FechaVencimiento { get; set; }

    [StringLength(50)]
    public string? NumeroLicencia { get; set; }

    public int CategoriaLicencia { get; set; } = 1;

    public bool PermiteMoto { get; set; } = false;

    public bool PermiteCargo { get; set; } = false;

    public bool PermiteTransporte { get; set; } = false;

    [StringLength(500)]
    public string? RestriccionesEspeciales { get; set; }

    public bool LicenciaVigente { get; set; } = true;

    public int AniosValidez { get; set; } = 10;

    public override (bool IsValid, string Message) Validar()
    {
        if (string.IsNullOrWhiteSpace(TipoLicencia))
            return (false, "Tipo de licencia es requerido");

        if (FechaVencimiento <= FechaOtorgamiento)
            return (false, "Fecha de vencimiento debe ser posterior a otorgamiento");

        if (AniosValidez < 1 || AniosValidez > 50)
            return (false, "Años de validez debe estar entre 1 y 50");

        return base.Validar();
    }
}
