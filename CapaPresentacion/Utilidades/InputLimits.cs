using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CapaPresentacion.Utilidades
{
    /// <summary>
    /// Clase centralizada que define los límites de longitud (MaxLength) para todos los campos de entrada
    /// y métodos de validación reutilizables.
    /// </summary>
    public static class InputLimits
    {
        // ===== USUARIO =====
        public const int USUARIO_DOCUMENTO = 20;
        public const int USUARIO_NOMBRECOMPLETO = 100;
        public const int USUARIO_CORREO = 100;
        public const int USUARIO_CLAVE = 50;
        public const int USUARIO_CONFIRMAR_CLAVE = 50;

        // ===== CATEGORÍA =====
        public const int CATEGORIA_DESCRIPCION = 50;

        // ===== PRODUCTO =====
        public const int PRODUCTO_CODIGO = 20;
        public const int PRODUCTO_NOMBRE = 100;
        public const int PRODUCTO_DESCRIPCION = 200;

        // ===== CLIENTE =====
        public const int CLIENTE_DOCUMENTO = 20;
        public const int CLIENTE_NOMBRECOMPLETO = 100;
        public const int CLIENTE_CORREO = 100;
        public const int CLIENTE_TELEFONO = 15;

        // ===== PROVEEDOR =====
        public const int PROVEEDOR_DOCUMENTO = 20;
        public const int PROVEEDOR_RAZONSOCIAL = 100;
        public const int PROVEEDOR_CORREO = 100;
        public const int PROVEEDOR_TELEFONO = 15;

        // ===== NEGOCIO =====
        public const int NEGOCIO_NOMBRE = 100;
        public const int NEGOCIO_RUC = 20;
        public const int NEGOCIO_DIRECCION = 200;

        /// <summary>
        /// Valida el formato de una dirección de correo electrónico.
        /// Utiliza expresión regular compatible con RFC 5322 (simplificada).
        /// </summary>
        /// <param name="email">Dirección de correo a validar</param>
        /// <returns>true si el formato es válido; false en caso contrario</returns>
        public static bool IsValidEmail(string email)
        {
            // Si está vacío, es válido (campo opcional en algunos contextos)
            if (string.IsNullOrWhiteSpace(email))
                return true;

            try
            {
                // Expresión regular para validar formato de correo electrónico
                // Patrón: usuario@dominio.extensión
                string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
            }
            catch
            {
                // En caso de error en regex, retornar false
                return false;
            }
        }

        /// <summary>
        /// Valida que un precio sea mayor a cero.
        /// </summary>
        /// <param name="precio">Precio a validar</param>
        /// <returns>true si el precio es válido (> 0); false en caso contrario</returns>
        public static bool IsValidPrice(decimal precio)
        {
            return precio > 0;
        }

        /// <summary>
        /// Valida que el precio de venta sea mayor al precio de compra.
        /// </summary>
        /// <param name="precioCompra">Precio de compra</param>
        /// <param name="precioVenta">Precio de venta</param>
        /// <returns>true si precioVenta > precioCompra; false en caso contrario</returns>
        public static bool IsValidPriceRelation(decimal precioCompra, decimal precioVenta)
        {
            return precioVenta > precioCompra;
        }
    }
}