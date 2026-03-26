using System;
using System.Text.RegularExpressions;

namespace ValpoControl.Core.Common.Validators;

/// <summary>
/// Validador de RUT chileno según normas SII.
/// Valida formato y dígito verificador.
/// </summary>
public static class RutValidator
{
    /// <summary>
    /// Valida un RUT chileno incluyendo dígito verificador.
    /// Formato aceptados: "12345678-K" o "12345678K" o "12.345.678-K"
    /// </summary>
    public static (bool IsValid, string Message) ValidarRut(string rut)
    {
        if (string.IsNullOrWhiteSpace(rut))
            return (false, "RUT no puede estar vacío");

        // Limpiar RUT: remover espacios y puntos
        var rutLimpio = rut.Trim()
            .Replace(".", "")
            .Replace(" ", "")
            .ToUpper();

        // Validar formato básico (número-dígito o solo número+dígito)
        if (!Regex.IsMatch(rutLimpio, @"^\d{1,8}[-]?[0-9K]$"))
            return (false, "Formato de RUT inválido. Use: 12345678-K o 12345678K");

        // Separar número y dígito verificador
        var partes = rutLimpio.Split('-');
        var numeroRut = partes.Length > 1 ? partes[0] : rutLimpio[..^1];
        var dvIngresado = partes.Length > 1 ? partes[1] : rutLimpio[^1].ToString();

        // Validar número
        if (!long.TryParse(numeroRut, out var numeroRutLong))
            return (false, "RUT debe contener solo números (excepto dígito verificador)");

        if (numeroRutLong < 1000000 || numeroRutLong > 99999999)
            return (false, "RUT debe estar entre 1.000.000 y 99.999.999");

        // Calcular dígito verificador
        var dvCalculado = CalcularDigitoVerificador(numeroRutLong);

        if (dvCalculado.ToString() != dvIngresado)
            return (false, $"Dígito verificador inválido. Debería ser: {dvCalculado}");

        return (true, "RUT válido");
    }

    /// <summary>
    /// Calcula el dígito verificador de un RUT chileno.
    /// Algoritmo: módulo 11 con multiplicadores 2-7.
    /// </summary>
    public static char CalcularDigitoVerificador(long numeroRut)
    {
        int[] multiplicadores = { 2, 3, 4, 5, 6, 7 };
        var rutString = numeroRut.ToString();
        var suma = 0;
        var posicion = 0;

        // Recorrer de derecha a izquierda
        for (int i = rutString.Length - 1; i >= 0; i--)
        {
            suma += int.Parse(rutString[i].ToString()) * multiplicadores[posicion % 6];
            posicion++;
        }

        var residuo = 11 - (suma % 11);

        return residuo switch
        {
            11 => '0',
            10 => 'K',
            _ => char.Parse(residuo.ToString())
        };
    }

    /// <summary>
    /// Formatea un RUT para presentación: "12.345.678-K"
    /// </summary>
    public static string FormatearRut(string rut)
    {
        var rutLimpio = rut.Trim()
            .Replace(".", "")
            .Replace("-", "")
            .ToUpper();

        if (rutLimpio.Length < 2)
            return rut;

        var numero = rutLimpio[..^1];
        var dv = rutLimpio[^1];

        // Agregar puntos cada 3 dígitos desde la derecha
        var numeroFormato = numero.PadLeft((numero.Length / 3 + 1) * 3, '0');
        var numeroPuntos = "";
        for (int i = 0; i < numeroFormato.Length; i += 3)
        {
            if (i > 0) numeroPuntos += ".";
            numeroPuntos += numeroFormato.Substring(i, 3);
        }

        return $"{numeroPuntos.TrimStart('0')}-{dv}";
    }
}
