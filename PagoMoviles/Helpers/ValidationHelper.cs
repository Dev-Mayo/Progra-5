using System.Text.RegularExpressions;

namespace PagoMoviles.Helpers
{
    /// <summary>
    /// Utilidades de validación reutilizables en los servicios.
    /// </summary>
    public static class ValidationHelper
    {
        /// <summary>
        /// Valida que un número de teléfono sea válido.
        /// Acepta formatos: 8 dígitos (Costa Rica) con o sin código de país.
        /// Ejemplos válidos: 88887777, +50688887777, 50688887777
        /// </summary>
        public static bool EsTelefonoValido(string? telefono)
        {
            if (string.IsNullOrWhiteSpace(telefono))
                return false;

            // Formato CR: 8 dígitos, opcionalmente con +506 o 506
            var regex = new Regex(@"^(\+?506)?[2-8]\d{7}$");
            return regex.IsMatch(telefono.Trim());
        }

        /// <summary>
        /// Valida que un string no sea nulo, vacío o solo espacios en blanco.
        /// </summary>
        public static bool EsTextoValido(string? texto)
        {
            return !string.IsNullOrWhiteSpace(texto);
        }

        /// <summary>
        /// Normaliza un número de teléfono quitando el código de país.
        /// Deja solo los 8 dígitos.
        /// </summary>
        public static string NormalizarTelefono(string telefono)
        {
            var limpio = telefono.Trim().Replace("+", "");
            if (limpio.StartsWith("506") && limpio.Length > 8)
                limpio = limpio.Substring(3);
            return limpio;
        }
    }
}
