using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Compra
    {
        private CD_Compra objcd_compra = new CD_Compra();
        
        public int ObtenerCorrelativo()
        {
            return objcd_compra.ObtenerCorrelativo();
        }

        /// <summary>
        /// Valida que en el detalle de compra, el precio de venta sea siempre mayor al precio de compra.
        /// </summary>
        /// <param name="detalleCompra">DataTable con los detalles de la compra</param>
        /// <param name="mensaje">Mensaje de error si la validación falla</param>
        /// <returns>true si todas las filas cumplen la validación; false en caso contrario</returns>
        private bool ValidarPrecioVentaMayorCompra(DataTable detalleCompra, out string mensaje)
        {
            mensaje = string.Empty;
            CultureInfo culturaInvariante = CultureInfo.InvariantCulture;

            if (detalleCompra == null || detalleCompra.Rows.Count == 0)
            {
                mensaje = "El detalle de compra no puede estar vacío.";
                return false;
            }

            foreach (DataRow row in detalleCompra.Rows)
            {
                decimal precioCompra = 0;
                decimal precioVenta = 0;

                // Parsear con cultura invariante para evitar problemas de localización
                if (!decimal.TryParse(row["PrecioCompra"].ToString(), NumberStyles.Any, culturaInvariante, out precioCompra))
                {
                    mensaje = "Precio de compra inválido en una de las filas.";
                    return false;
                }

                if (!decimal.TryParse(row["PrecioVenta"].ToString(), NumberStyles.Any, culturaInvariante, out precioVenta))
                {
                    mensaje = "Precio de venta inválido en una de las filas.";
                    return false;
                }

                // Validar: Precio de venta debe ser MAYOR a precio de compra
                if (precioVenta <= precioCompra)
                {
                    mensaje = "El precio de venta debe ser mayor al precio de compra.";
                    return false;
                }
            }

            return true;
        }

        public bool Registrar(Compra obj, DataTable DetalleCompra, out string Mensaje)
        {
            // Validar precios antes de intentar registrar
            if (!ValidarPrecioVentaMayorCompra(DetalleCompra, out Mensaje))
            {
                return false;
            }

            return objcd_compra.Registrar(obj, DetalleCompra, out Mensaje);
        }

        public Compra ObtenerCompra(string numero)
        {
            Compra oCompra = objcd_compra.ObtenerCompra(numero);
            if(oCompra.IdCompra != 0)
            {
                List<Detalle_Compra> oDetalleCompra = objcd_compra.ObtenerDetalleCompra(oCompra.IdCompra);
                oCompra.oDetalleCompra = oDetalleCompra;
            }
            return oCompra;
        }
    }
}
