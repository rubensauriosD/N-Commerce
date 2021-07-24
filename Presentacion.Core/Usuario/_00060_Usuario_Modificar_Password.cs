namespace Presentacion.Core.Usuario
{
    using System;
    using Aplicacion.Constantes;
    using IServicio.Seguridad;
    using PresentacionBase.Formularios;
    using StructureMap;

    public partial class _00060_Usuario_Modificar_Password : FormBase
    {
        private readonly ISeguridadServicio _seguridadServicio;
        private readonly long _usuarioId;
        private readonly Validar Validar;

        public _00060_Usuario_Modificar_Password()
        {
            InitializeComponent();

            _seguridadServicio = ObjectFactory.GetInstance<ISeguridadServicio>();
            _usuarioId = Identidad.UsuarioId;
            Validar = new Validar();
        }

        private void _00060_Usuario_Modificar_Password_Load(object sender, EventArgs e)
        {
            Validar.ComoPassword(txtNuevaPassword, true);
            Validar.ComoPassword(txtNuevaPasswordRepetida, true);
            Validar.ComoPassword(txtPasswordActual, true);
        }

        // --- Guardar Datos Ingresados
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!VerificarDatos())
                return;

            var nuevaPassword = txtNuevaPassword.Text;
            if (!_seguridadServicio.ModificarPassword(_usuarioId, nuevaPassword))
                return;

            Mjs.Info("Password fue modificada correctamente.");
            Close();
        }

        private bool VerificarDatos()
        {
            string pssActual = txtPasswordActual.Text;
            string pssNueva = txtNuevaPassword.Text;
            string pssRepetida = txtNuevaPasswordRepetida.Text;
            string nombreUsuario = Identidad.Usuario;

            if (!_seguridadServicio.VerificarAcceso(nombreUsuario, pssActual)) 
            {
                Mjs.Alerta("El password actual no es correcto.");
                txtPasswordActual.Clear();
                txtPasswordActual.Focus();
                return false;
            }

            if (pssNueva != pssRepetida)
            {
                Mjs.Alerta("Password repetida incorrecta.");
                txtNuevaPasswordRepetida.Clear();
                txtNuevaPasswordRepetida.Focus();
                return false;
            }

            return true;
        }
    }
}
