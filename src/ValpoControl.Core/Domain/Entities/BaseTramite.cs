using System;
using System.ComponentModel.DataAnnotations;

namespace ValpoControl.Core.Domain.Entities;

/// <summary>
/// Clase base abstracta para todos los trámites municipales.
/// Implementa el patrón Table Per Hierarchy (TPH) con discriminador "TipoModulo".
/// </summary>
public abstract class BaseTramite
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "RUT es obligatorio")]
    [StringLength(12)]
    public required string RUT { get; set; }

    [Required(ErrorMessage = "Nombre es obligatorio")]
    [StringLength(150)]
    public required string Nombre { get; set; }

    [Required(ErrorMessage = "Email es obligatorio")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Teléfono es obligatorio")]
    [StringLength(15)]
    public required string Telefono { get; set; }

    [Required]
    public DateTime FechaIngreso { get; set; } = DateTime.UtcNow;

    [Required]
    public DateTime? FechaActualizacion { get; set; } = DateTime.UtcNow;

    [Required(ErrorMessage = "Estado es obligatorio")]
    [StringLength(50)]
    public required string Estado { get; set; } = "En Proceso";

    [StringLength(500)]
    public string? Observaciones { get; set; }

    [Required]
    [StringLength(100)]
    public required string UsuarioId { get; set; }

    [Required]
    public required string TipoModulo { get; set; }

    // Auditoría
    [StringLength(45)]
    public string? IpOrigen { get; set; }

    public bool Activo { get; set; } = true;

    public DateTime? FechaEliminacion { get; set; }

    /// <summary>
    /// Método virtual para validación personalizada por módulo.
    /// </summary>
    public virtual (bool IsValid, string Message) Validar()
    {
        return (true, "Validación base completada");
    }

    /// <summary>
    /// Registra cambio de estado en la auditoría.
    /// </summary>
    public void CambiarEstado(string nuevoEstado, string usuarioId, string? ipOrigen = null)
    {
        Estado = nuevoEstado;
        UsuarioId = usuarioId;
        FechaActualizacion = DateTime.UtcNow;
        IpOrigen = ipOrigen;
    }
}
