using System;
using System.ComponentModel.DataAnnotations;

namespace ValpoControl.Core.Domain.Entities;

/// <summary>
/// Módulo ExamenTeórico: Evaluación teórica de leyes de tránsito.
/// </summary>
public class ExamenTeorico : BaseTramite
{
    [Required]
    public int Puntaje { get; set; }

    [Required]
    public int Intentos { get; set; } = 1;

    public int MaximoIntentos { get; set; } = 3;

    public DateTime FechaExamen { get; set; } = DateTime.UtcNow;

    [StringLength(100)]
    public string? ExaminadorResponsable { get; set; }

    public int RespuestasCorrectas { get; set; }

    public int TotalPreguntas { get; set; } = 40;

    [StringLength(500)]
    public string? AreasFallidas { get; set; }

    public bool Aprobado { get; set; } = false;

    public override (bool IsValid, string Message) Validar()
    {
        if (Puntaje < 0 || Puntaje > 100)
            return (false, "Puntaje debe estar entre 0 y 100");

        if (Intentos < 1 || Intentos > MaximoIntentos)
            return (false, $"Intentos debe estar entre 1 y {MaximoIntentos}");

        if (RespuestasCorrectas < 0 || RespuestasCorrectas > TotalPreguntas)
            return (false, $"Respuestas correctas no puede exceder {TotalPreguntas}");

        return base.Validar();
    }
}
