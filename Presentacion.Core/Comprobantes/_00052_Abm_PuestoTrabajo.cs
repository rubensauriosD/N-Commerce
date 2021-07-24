namespace Presentacion.Core.Comprobantes
{
    using System.Windows.Forms;
    using Aplicacion.Constantes;
    using IServicio.PuestoTrabajo;
    using IServicio.PuestoTrabajo.DTOs;
    using PresentacionBase.Formularios;
    using StructureMap;

    public partial class _00052_Abm_PuestoTrabajo : FormAbm
    {
        private readonly IPuestoTrabajoServicio _servicio;
        private readonly Validar Validar;

        public _00052_Abm_PuestoTrabajo(TipoOperacion tipoOperacion, long? entidadId = null)
            : base(tipoOperacion, entidadId)
        {
            InitializeComponent();

            _servicio = ObjectFactory.GetInstance<IPuestoTrabajoServicio>();
            Validar = new Validar();
        }

        private void _00052_Abm_PuestoTrabajo_Load(object sender, System.EventArgs e)
        {
            Validar.ComoAlfanumerico(txtDescripcion, true);
            txtCodigo.Text = _servicio.ProximoCodigo().ToString("0000");

            CargarDatos();
        }

        public void CargarDatos()
        {
            if (!EntidadId.HasValue)
                return;

            var resultado = (PuestoTrabajoDto)_servicio.Obtener(EntidadId.Value);

            if (resultado == null)
            {
                MessageBox.Show("Ocurrio un error al obtener el registro seleccionado.");
                Close();
            }

            txtDescripcion.Text = resultado.Descripcion;
            txtCodigo.Text = resultado.Codigo.ToString("0000");
        }

        public override bool VerificarDatosObligatorios()
        {
            return ValidateChildren()
                && int.TryParse(txtCodigo.Text, out int _);
        }

        public override bool VerificarSiExiste(long? id = null)
        {
            return _servicio.VerificarSiExiste(txtDescripcion.Text, id)
                && _servicio.VerificarSiExiste(txtCodigo.Text, id);
        }

        // --- Acciones de botones
        public override void EjecutarComandoNuevo()
        {
            if (!int.TryParse(txtCodigo.Text, out int _codigo))
            {
                MessageBox.Show("El código no es válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var nuevoRegistro = new PuestoTrabajoDto();
            nuevoRegistro.Descripcion = txtDescripcion.Text;
            nuevoRegistro.Codigo = _codigo;
            nuevoRegistro.Eliminado = false;

            _servicio.Insertar(nuevoRegistro);
        }

        public override void EjecutarComandoModificar()
        {
            if (!int.TryParse(txtCodigo.Text, out int _codigo))
            {
                MessageBox.Show("El código no es válido.","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }

            var modificarRegistro = new PuestoTrabajoDto();
            modificarRegistro.Id = EntidadId.Value;
            modificarRegistro.Descripcion = txtDescripcion.Text;
            modificarRegistro.Codigo = _codigo;
            modificarRegistro.Eliminado = false;

            _servicio.Modificar(modificarRegistro);
        }

        public override void EjecutarComandoEliminar()
        {
            _servicio.Eliminar(EntidadId.Value);
        }

    }
}
