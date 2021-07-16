namespace Presentacion.Core.CondicionIva
{
    using System.Windows.Forms;
    using IServicio.Departamento;
    using IServicio.CondicionIva.DTOs;
    using PresentacionBase.Formularios;
    using StructureMap;
    using Aplicacion.Constantes;

    public partial class _00014_Abm_CondicionIva : FormAbm
    {
        private readonly ICondicionIvaServicio _servicio;
        private readonly Validar Validar;

        public _00014_Abm_CondicionIva(TipoOperacion tipoOperacion, long? entidadId = null)
            : base(tipoOperacion, entidadId)
        {
            InitializeComponent();

            _servicio = ObjectFactory.GetInstance<ICondicionIvaServicio>();
            Validar = new Validar();
        }

        private void _00014_Abm_CondicionIva_Load(object sender, System.EventArgs e)
        {
            Validar.ComoTexto(txtDescripcion, true);
        }

        public override void CargarDatos(long? entidadId)
        {
            base.CargarDatos(entidadId);

            if (entidadId.HasValue)
            {
                var resultado = (CondicionIvaDto)_servicio.Obtener(entidadId.Value);

                if (resultado == null)
                {
                    MessageBox.Show("Ocurrio un error al obtener el registro seleccionado.");
                    Close();
                }

                txtDescripcion.Text = resultado.Descripcion;

                if (TipoOperacion == TipoOperacion.Eliminar)
                    DesactivarControles(this);
            }
        }

        public override bool VerificarDatosObligatorios()
        {
            return ValidateChildren();
        }

        public override bool VerificarSiExiste(long? id = null)
        {
            return _servicio.VerificarSiExiste(txtDescripcion.Text, id);
        }

        // --- Acciones de botones
        public override void EjecutarComandoNuevo()
        {
            var nuevoRegistro = new CondicionIvaDto();
            nuevoRegistro.Descripcion = txtDescripcion.Text;
            nuevoRegistro.Eliminado = false;

            _servicio.Insertar(nuevoRegistro);
        }

        public override void EjecutarComandoModificar()
        {
            var modificarRegistro = new CondicionIvaDto();
            modificarRegistro.Id = EntidadId.Value;
            modificarRegistro.Descripcion = txtDescripcion.Text;
            modificarRegistro.Eliminado = false;

            _servicio.Modificar(modificarRegistro);
        }

        public override void EjecutarComandoEliminar()
        {
            _servicio.Eliminar(EntidadId.Value);
        }

        public override void LimpiarControles(object obj, bool tieneValorAsociado = false)
        {
            base.LimpiarControles(obj);

            txtDescripcion.Focus();
        }
    }
}
