using System;
using System.Windows.Forms;
using Aplicacion.Constantes;
using IServicio.Seguridad;

namespace Presentacion.Core.Usuario
{
    public partial class Login : Form
    {
        private readonly ISeguridadServicio _servicio;

        public bool PuedeAccedearAlSistema { get; private set; }
        public bool DeseaSalirDelSistema { get; private set; }

        public Login(ISeguridadServicio servicio)
        {
            InitializeComponent();

            _servicio = servicio;
            PuedeAccedearAlSistema = false;
            DeseaSalirDelSistema = false;
        }

        public void ResetearCampos() {
            txtPassword.Clear();
            txtUsuario.Clear();
            txtUsuario.Focus();
            PuedeAccedearAlSistema = false;
        }


        private void Login_Load(object sender, EventArgs e)
        {
            // Cargar imagenes
            picLogo.Image = Imagen.Logo;
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
                Identidad.UsuarioId = 0;
                Identidad.Usuario = "Admin";
                Identidad.Nombre = "Super";
                Identidad.Apellido = "Usuario";
                Identidad.Foto = Imagen.Usuario;

                PuedeAccedearAlSistema = true;
                txtPassword.Clear();
                txtUsuario.Clear();
                txtUsuario.Focus();
                Close();
                return;
            }

            // Usuario y Password deben ser validos


            // loguear en el sistema
            if (_servicio.VerificarAcceso(txtUsuario.Text, txtPassword.Text))
            {
                // Cargar los datos de la identidad del usuario
                var usr = _servicio.ObtenerUsuarioLogin(txtUsuario.Text);

                Identidad.UsuarioId = usr.Id;
                Identidad.EmpleadoId = usr.EmpleadoId;
                Identidad.Usuario = usr.NombreUsuario;
                Identidad.Nombre = usr.NombreEmpleado;
                Identidad.Apellido = usr.ApellidoEmpleado;
                Identidad.Foto = Imagen.ConvertirImagen(usr.FotoEmpleado);
                        
                PuedeAccedearAlSistema = true;
                        txtPassword.Clear();
                        txtUsuario.Clear();
                        Close();
                        return;
            }

            Mjs.Error("Usuario o contrasña incorrectos.");
            txtPassword.Clear();
            txtPassword.Focus();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            DeseaSalirDelSistema = true;
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

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!PuedeAccedearAlSistema && !DeseaSalirDelSistema)
                e.Cancel = true;
        }
    }
}
