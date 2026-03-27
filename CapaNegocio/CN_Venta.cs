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
    public class CN_Venta
    {
        private CD_Venta objcd_venta = new CD_Venta();

        public bool RestarStock(int idproducto, int cantidad)
        {
            return objcd_venta.RestarStock(idproducto, cantidad);
        }

        public bool SumarStock(int idproducto, int cantidad)
        {
            return objcd_venta.SumarStock(idproducto, cantidad);
        }

        public int ObtenerCorrelativo()
        {
            return objcd_venta.ObtenerCorrelativo();
        }

        /// <summary>
        /// Valida que el monto de pago sea suficiente para cubrir el monto total de la venta.
        /// </summary>
        /// <param name="oVenta">Objeto Venta con MontoPago y MontoTotal</param>
        /// <param name="mensaje">Mensaje de error si la validación falla</param>
        /// <returns>true si el pago es válido; false en caso contrario</returns>
        private bool ValidarMontoPago(Venta oVenta, out string mensaje)
        {
            mensaje = string.Empty;

            if (oVenta == null)
            {
                mensaje = "Los datos de la venta no son válidos.";
                return false;
            }

            // Validar que MontoPago >= MontoTotal
            if (oVenta.MontoPago < oVenta.MontoTotal)
            {
                mensaje = "El monto de pago no puede ser menor al monto total.";
                return false;
            }

            // Validar que no haya cambio negativo
            decimal cambio = oVenta.MontoPago - oVenta.MontoTotal;
            if (cambio < 0)
            {
                mensaje = "No es posible calcular un cambio válido.";
                return false;
            }

            return true;
        }

        public bool Registrar(Venta obj, DataTable DetalleVenta, out string Mensaje)
        {
            // Validar montos antes de intentar registrar
            if (!ValidarMontoPago(obj, out Mensaje))
            {
                return false;
            }

            return objcd_venta.Registrar(obj, DetalleVenta, out Mensaje);
        }

        public Venta ObtenerVenta(string numero)
        {
            Venta oVenta = objcd_venta.ObtenerVenta(numero);

            if (oVenta.IdVenta != 0)
            {
                List<Detalle_Venta> oDetalleVenta = objcd_venta.ObtenerDetalleVenta(oVenta.IdVenta);
                oVenta.oDetalle_Venta = oDetalleVenta;
            }
            return oVenta;
        }
    }
}
