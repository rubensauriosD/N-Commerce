namespace Presentacion.Core.Usuario
{
    using System;
    using System.Windows.Forms;
    using Aplicacion.Constantes;
    using IServicio.Seguridad;

    public partial class Login : Form
    {
        private readonly ISeguridadServicio _servicio;

        public bool PuedeAccedearAlSistema { get; private set; }
        public bool DeseaSalirDelSistema { get; private set; }
        private readonly Validar Validar;

        public Login(ISeguridadServicio servicio)
        {
            InitializeComponent();

            Validar = new Validar();
            _servicio = servicio;
            PuedeAccedearAlSistema = false;
            DeseaSalirDelSistema = false;
            AutoValidate = AutoValidate.EnableAllowFocusChange;
        }

        private void Login_Load(object sender, EventArgs e)
        {
            // Cargar imagenes
            picLogo.Image = Imagen.Logo;
            picUsuario.Image = Imagen.Usuario;
            picPassword.Image = Imagen.Bloquear;

            // Setear Controles
            Validar.ComoAlfanumerico(txtUsuario, true);
            txtUsuario.MaxLength = 30;

            Validar.ComoPassword(txtPassword, true);
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!PuedeAccedearAlSistema && !DeseaSalirDelSistema)
                e.Cancel = true;
        }

        public void ResetearCampos() {
            txtPassword.Clear();
            txtUsuario.Clear();
            txtUsuario.Focus();
            PuedeAccedearAlSistema = false;
        }

        // --- Acciones de controles
        private void btnIngresar_Click(object sender, EventArgs e)
        {
            if (!ValidateChildren())
            {
                Mjs.Alerta($"Algunos de los datos ingresados no son corretos.");
                return;
            }

            // Loguear Usuario Admin
            //if (txtUsuario.Text == UsuarioAdmin.Usuario
            //    && txtPassword.Text == UsuarioAdmin.Password)
            //{
            //    Identidad.UsuarioId = 0;
            //    Identidad.Usuario = "Admin";
            //    Identidad.Nombre = "Super";
            //    Identidad.Apellido = "Usuario";
            //    Identidad.Foto = Imagen.Usuario;
            //
            //    PuedeAccedearAlSistema = true;
            //    txtPassword.Clear();
            //    txtUsuario.Clear();
            //    txtUsuario.Focus();
            //    Close();
            //    return;
            //}

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
    }
}
