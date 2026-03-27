using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Usuario
    {
        private CD_Usuario objcd_usuario = new CD_Usuario();
        
        public List<Usuario> Listar()
        {
            return objcd_usuario.Listar();
        }

        /// <summary>
        /// Autentica un usuario validando documento, contraseña y estado.
        /// </summary>
        /// <param name="documento">Documento del usuario</param>
        /// <param name="clave">Contraseña del usuario</param>
        /// <param name="estado">Indica si el usuario está activo (out)</param>
        /// <returns>Usuario autenticado si credenciales son válidas y está activo; null en caso contrario</returns>
        public Usuario Autenticar(string documento, string clave, out bool usuarioExisteInactivo)
        {
            usuarioExisteInactivo = false;
            
            // Busca usuario por documento y contraseña
            Usuario usuario = objcd_usuario.Listar().FirstOrDefault(u => 
                u.Documento == documento && u.Clave == clave);
            
            if (usuario != null)
            {
                // Usuario encontrado con credenciales válidas
                if (usuario.Estado == true)
                {
                    // Usuario activo: autenticación exitosa
                    return usuario;
                }
                else
                {
                    // Usuario inactivo: retorna null pero marca flag
                    usuarioExisteInactivo = true;
                    return null;
                }
            }
            
            // Credenciales inválidas (usuario no existe o contraseña incorrecta)
            return null;
        }

        /// <summary>
        /// Valida que el documento, nombre completo y contraseña no estén vacíos.
        /// Adicionalmente, valida que la contraseña tenga una longitud mínima.
        /// </summary>
        /// <param name="obj">Objeto Usuario a validar</param>
        /// <param name="mensaje">Mensaje de error acumulativo</param>
        /// <returns>true si todas las validaciones pasan; false en caso contrario</returns>
        private bool ValidarDatosUsuario(Usuario obj, out string mensaje)
        {
            mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(obj.Documento))
            {
                mensaje += "Es necesario el documento del usuario.\n";
            }
            if (string.IsNullOrWhiteSpace(obj.NombreCompleto))
            {
                mensaje += "Es necesario el nombre completo del usuario.\n";
            }
            if (string.IsNullOrWhiteSpace(obj.Clave))
            {
                mensaje += "Es necesario la contraseña del usuario.\n";
            }
            else if (obj.Clave.Length < 4)
            {
                // Validación adicional: contraseña mínimo 4 caracteres
                mensaje += "La contraseña debe tener al menos 4 caracteres.\n";
            }

            return string.IsNullOrEmpty(mensaje);
        }

        public int Registrar(Usuario obj, out string Mensaje)
        {
            // Usar validación centralizada
            if (!ValidarDatosUsuario(obj, out Mensaje))
            {
                return 0;
            }

            return objcd_usuario.Registrar(obj, out Mensaje);
        }

        public bool Editar(Usuario obj, out string Mensaje)
        {
            // Usar validación centralizada
            if (!ValidarDatosUsuario(obj, out Mensaje))
            {
                return false;
            }

            return objcd_usuario.Editar(obj, out Mensaje);
        }

        public bool Eliminar(Usuario obj, out string Mensaje)
        {
            return objcd_usuario.Eliminar(obj, out Mensaje);
        }
    }
}
