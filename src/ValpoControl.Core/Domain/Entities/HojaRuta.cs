using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ValpoControl.Core.Domain.Entities;

/// <summary>
/// Módulo HojaRuta: Trazabilidad de pasos en el proceso.
/// Registra cada etapa completada del trámite.
/// </summary>
public class HojaRuta : BaseTramite
{
    [Required(ErrorMessage = "Etapa actual es obligatoria")]
    [StringLength(100)]
    public required string EtapaActual { get; set; }

    public int NumeroEtapas { get; set; } = 1;

    [StringLength(500)]
    public string? DescripcionEtapa { get; set; }

    public DateTime FechaEtapa { get; set; } = DateTime.UtcNow;

    [StringLength(100)]
    public string? ResponsableEtapa { get; set; }

    public int Progreso { get; set; } = 0; // Porcentaje 0-100

    public bool EtapaCompletada { get; set; } = false;

    [StringLength(500)]
    public string? ProximaPaso { get; set; }

    public override (bool IsValid, string Message) Validar()
    {
        if (string.IsNullOrWhiteSpace(EtapaActual))
            return (false, "Etapa actual es requerida");

        if (Progreso < 0 || Progreso > 100)
            return (false, "Progreso debe estar entre 0 y 100");

        return base.Validar();
    }
}
