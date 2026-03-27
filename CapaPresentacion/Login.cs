using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CapaNegocio;
using CapaEntidad;
using CapaPresentacion.Utilidades;

namespace CapaPresentacion
{
    public partial class Login : Form
    {
        // Variable para contar intentos fallidos en esta ejecución
        private int _intentosFallidos = 0;
        private const int INTENTOS_MAXIMOS = 3;

        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            // Configurar MaxLength de TextBox
            txtdocumento.MaxLength = InputLimits.USUARIO_DOCUMENTO;
            txtclave.MaxLength = InputLimits.USUARIO_CLAVE;

            // Limpiar campos al abrir
            txtdocumento.Text = "";
            txtclave.Text = "";
            txtdocumento.Focus();
        }

        private void btncancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btningresar_Click(object sender, EventArgs e)
        {
            // Validar que los campos no estén vacíos
            if (string.IsNullOrWhiteSpace(txtdocumento.Text))
            {
                MessageBox.Show("Ingrese el número de documento.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtdocumento.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtclave.Text))
            {
                MessageBox.Show("Ingrese la contraseña.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtclave.Focus();
                return;
            }

            // Intentar autenticar al usuario
            bool usuarioExisteInactivo = false;
            Usuario ousuario = new CN_Usuario().Autenticar(txtdocumento.Text, txtclave.Text, out usuarioExisteInactivo);

            if (ousuario != null)
            {
                // Autenticación exitosa
                _intentosFallidos = 0; // Resetear contador
                Inicio form = new Inicio(ousuario);
                form.Show();
                this.Hide();
                form.FormClosing += frm_closing;
            }
            else
            {
                // Autenticación fallida
                if (usuarioExisteInactivo)
                {
                    // Usuario existe pero está inactivo: NO contar como intento fallido
                    MessageBox.Show("El usuario se encuentra inactivo. Contacte al administrador.", "Usuario Inactivo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    // Credenciales inválidas: contar como intento fallido
                    _intentosFallidos++;

                    if (_intentosFallidos < INTENTOS_MAXIMOS)
                    {
                        int intentosRestantes = INTENTOS_MAXIMOS - _intentosFallidos;
                        MessageBox.Show($"Documento o contraseña incorrectos.\nIntento {_intentosFallidos} de {INTENTOS_MAXIMOS}. Restan {intentosRestantes} intento(s).", 
                            "Autenticación Fallida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                    {
                        // Tercer intento fallido: bloquear
                        MessageBox.Show("Ha excedido el número máximo de intentos. Reinicie la aplicación o contacte al administrador.", 
                            "Sesión Bloqueada", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                        // Deshabilitar controles
                        txtdocumento.Enabled = false;
                        txtclave.Enabled = false;
                        btningresar.Enabled = false;
                    }
                }
            }

            // Limpiar campo de contraseña por seguridad
            txtclave.Text = "";
            txtclave.Focus();
        }

        private void frm_closing(object sender, FormClosingEventArgs e)
        {
            txtdocumento.Text = "";
            txtclave.Text = "";
            _intentosFallidos = 0; // Resetear contador al cerrar sesión
            this.Show();
        }
    }
}
