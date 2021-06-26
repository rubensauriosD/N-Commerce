namespace Presentacion.Core.Provincia
{
    using System.Windows.Forms;
    using Aplicacion.Constantes;
    using IServicio.Provincia;
    using IServicio.Provincia.DTOs;
    using PresentacionBase.Formularios;
    using StructureMap;

    public partial class _00002_Abm_Provincia : FormAbm
    {
        private readonly IProvinciaServicio _provinciaServicio;
        private readonly Validar Validar;

        public _00002_Abm_Provincia(TipoOperacion tipoOperacion, long? entidadId = null)
            : base(tipoOperacion, entidadId)
        {
            InitializeComponent();

            _provinciaServicio = ObjectFactory.GetInstance<IProvinciaServicio>();
            Validar = new Validar();
        }

        public override void CargarDatos(long? entidadId)
        {
            base.CargarDatos(entidadId);

            if (entidadId.HasValue)
            {
                var resultado = (ProvinciaDto) _provinciaServicio.Obtener(entidadId.Value);

                if (resultado == null)
                {
                    MessageBox.Show("Ocurrio un error al obtener el registro seleccionado");
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

        private void _00002_Abm_Provincia_Load(object sender, System.EventArgs e)
        {
            Validar.ComoTexto(txtDescripcion, true);
        }

        public override bool VerificarSiExiste(long? id = null)
        {
            return _provinciaServicio.VerificarSiExiste(txtDescripcion.Text, id);
        }

        //
        // Acciones de botones
        //
        public override void EjecutarComandoNuevo()
        {
            var nuevoRegistro = new ProvinciaDto();
            nuevoRegistro.Descripcion = txtDescripcion.Text;
            nuevoRegistro.Eliminado = false;

            _provinciaServicio.Insertar(nuevoRegistro);
        }

        public override void EjecutarComandoModificar()
        {
            var modificarRegistro = new ProvinciaDto();
            modificarRegistro.Id = EntidadId.Value;
            modificarRegistro.Descripcion = txtDescripcion.Text;
            modificarRegistro.Eliminado = false;

            _provinciaServicio.Modificar(modificarRegistro);
        }

        public override void EjecutarComandoEliminar()
        {
            _provinciaServicio.Eliminar(EntidadId.Value);
        }

        public override void LimpiarControles(object obj, bool tieneValorAsociado = false)
        {
            base.LimpiarControles(obj);

            txtDescripcion.Focus();
        }
    }
}
