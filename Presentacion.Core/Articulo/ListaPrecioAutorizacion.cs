using System;
using System.Windows.Forms;
using Aplicacion.Constantes;
using IServicio.Seguridad;
using PresentacionBase.Formularios;

namespace Presentacion.Core.Articulo
{
    public partial class ListaPrecioAutorizacion : FormBase
    {
        private readonly ISeguridadServicio _servicio;

        public bool PuedeAcceder { get; private set; }

        public ListaPrecioAutorizacion(ISeguridadServicio servicio)
        {
            InitializeComponent();

            _servicio = servicio;
            PuedeAcceder = false;
        }

        public void ResetearCampos()
        {
            txtPassword.Clear();
            txtUsuario.Clear();
            txtUsuario.Focus();
            PuedeAcceder = false;
        }

        private void form_Load(object sender, EventArgs e)
        {
            // Cargar imagenes
            picUsuario.Image = Imagen.Usuario;
            picPassword.Image = Imagen.Bloquear;

            // Setear Controles
            txtUsuario.MaxLength = 30;
            txtPassword.MaxLength = 30;
            txtPassword.UseSystemPasswordChar = true;
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            // Los campos no pueden estar vacios
            if (string.IsNullOrEmpty(txtUsuario.Text)
                || string.IsNullOrEmpty(txtPassword.Text))
            {
                Mjs.Error("Ingrese los campos obligatorios.");
                return;
            }

            // Loguear Usuario Admin
            if (txtUsuario.Text == UsuarioAdmin.Usuario
                && txtPassword.Text == UsuarioAdmin.Password)
            {
                PuedeAcceder = true;
                Close();
                return;
            }

            // loguear en el sistema
            if (_servicio.VerificarAcceso(txtUsuario.Text, txtPassword.Text))
            {
                PuedeAcceder = true;
                Close();
                return;
            }

            Mjs.Error("Usuario o contraseña incorrectos.");
            txtPassword.Clear();
            txtPassword.Focus();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void pnlUsuario_Enter(object sender, EventArgs e)
        {
            txtUsuario.Focus();
        }

        private void pnlPassword_Enter(object sender, EventArgs e)
        {
            txtPassword.Focus();
        }
    }
}
