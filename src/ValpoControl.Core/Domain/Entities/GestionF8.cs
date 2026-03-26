using System;
using System.ComponentModel.DataAnnotations;

namespace ValpoControl.Core.Domain.Entities;

/// <summary>
/// Módulo F8: Gestión de documentos administrativos.
/// Incluye campos específicos para códigos y fechas de carpeta.
/// </summary>
public class GestionF8 : BaseTramite
{
    [Required(ErrorMessage = "Código F8 es obligatorio")]
    [StringLength(50)]
    public required string CodigoF8 { get; set; }

    [Required]
    public DateTime FechaCarpeta { get; set; } = DateTime.UtcNow;

    [StringLength(500)]
    public string? ClasificacionDocumentos { get; set; }

    public int? NumeroDocumentos { get; set; }

    [StringLength(500)]
    public string? UbicacionFisica { get; set; }

    public override (bool IsValid, string Message) Validar()
    {
        if (string.IsNullOrWhiteSpace(CodigoF8))
            return (false, "Código F8 es requerido");

        if (FechaCarpeta > DateTime.UtcNow)
            return (false, "Fecha de carpeta no puede ser futura");

        return base.Validar();
    }
}
