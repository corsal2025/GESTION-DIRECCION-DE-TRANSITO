using System;
using System.ComponentModel.DataAnnotations;

namespace ValpoControl.Core.Domain.Entities;

/// <summary>
/// Módulo 16: Registro interno y trazabilidad.
/// </summary>
public class Modulo16 : BaseTramite
{
    [Required(ErrorMessage = "Número de registro es obligatorio")]
    public int NumeroRegistro { get; set; }

    [Required]
    [StringLength(500)]
    public required string DescripcionRegistro { get; set; }

    [StringLength(100)]
    public string? Clasificacion { get; set; }

    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    [StringLength(500)]
    public string? NotasInternas { get; set; }

    public override (bool IsValid, string Message) Validar()
    {
        if (NumeroRegistro <= 0)
            return (false, "Número de registro debe ser mayor a 0");

        if (string.IsNullOrWhiteSpace(DescripcionRegistro))
            return (false, "Descripción es requerida");

        return base.Validar();
    }
}
