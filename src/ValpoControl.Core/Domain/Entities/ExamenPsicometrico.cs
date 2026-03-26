using System;
using System.ComponentModel.DataAnnotations;

namespace ValpoControl.Core.Domain.Entities;

/// <summary>
/// Módulo ExamenPsicométrico: Evaluación psicológica para licencias.
/// </summary>
public class ExamenPsicometrico : BaseTramite
{
    [Required]
    public int Puntaje { get; set; }

    [Required]
    [StringLength(50)]
    public required string Resultado { get; set; } // Aprobado, Rechazado, Pendiente

    [StringLength(500)]
    public string? Observaciones { get; set; }

    public DateTime FechaExamen { get; set; } = DateTime.UtcNow;

    [StringLength(100)]
    public string? PsicologoResponsable { get; set; }

    public int IntentosRealizados { get; set; } = 1;

    public DateTime? FechaProximoIntento { get; set; }

    public bool Apto { get; set; } = false;

    public override (bool IsValid, string Message) Validar()
    {
        if (Puntaje < 0 || Puntaje > 100)
            return (false, "Puntaje debe estar entre 0 y 100");

        if (string.IsNullOrWhiteSpace(Resultado))
            return (false, "Resultado es requerido");

        if (IntentosRealizados < 1)
            return (false, "Intentos realizados debe ser al menos 1");

        return base.Validar();
    }
}
