namespace Presentacion.Core.Articulo
{
    using System.Windows.Forms;
    using Aplicacion.Constantes;
    using IServicio.Deposito;
    using IServicio.Deposito.DTOs;
    using PresentacionBase.Formularios;
    using StructureMap;

    public partial class _00051_Abm_Deposito : FormAbm
    {
        private readonly IDepositoSevicio _servicio;
        private readonly Validar Validar;

        public _00051_Abm_Deposito(TipoOperacion tipoOperacion, long? entidadId = null)
            : base(tipoOperacion, entidadId)
        {
            InitializeComponent();

            _servicio = ObjectFactory.GetInstance<IDepositoSevicio>();
            Validar = new Validar();
        }

        private void _00051_Abm_Deposito_Load(object sender, System.EventArgs e)
        {
            Validar.ComoAlfanumerico(txtDescripcion, true);
            Validar.ComoAlfanumerico(txtUbicacion);

            txtDescripcion.MaxLength = 250;
            txtUbicacion.MaxLength = 400;
        }

        public override void CargarDatos(long? entidadId)
        {
            base.CargarDatos(entidadId);

            if (entidadId.HasValue)
            {
                var resultado = (DepositoDto)_servicio.Obtener(entidadId.Value);

                if (resultado == null)
                {
                    MessageBox.Show("Ocurrio un error al obtener el registro seleccionado.");
                    Close();
                }

                txtDescripcion.Text = resultado.Descripcion;
                txtUbicacion.Text = resultado.Ubicacion;

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
            var nuevoRegistro = new DepositoDto();
            nuevoRegistro.Descripcion = txtDescripcion.Text;
            nuevoRegistro.Ubicacion = txtUbicacion.Text;
            nuevoRegistro.Eliminado = false;

            _servicio.Insertar(nuevoRegistro);
        }

        public override void EjecutarComandoModificar()
        {
            var modificarRegistro = new DepositoDto();
            modificarRegistro.Id = EntidadId.Value;
            modificarRegistro.Descripcion = txtDescripcion.Text;
            modificarRegistro.Ubicacion = txtUbicacion.Text;
            modificarRegistro.Eliminado = false;

            _servicio.Modificar(modificarRegistro);
        }

        public override void EjecutarComandoEliminar()
        {
            if (_servicio.TieneStokDeArticulos(EntidadId))
            {
                Mjs.Alerta("No se puede eliminar depósitos con stoks");
                return;
            }

            _servicio.Eliminar(EntidadId.Value);
        }

        public override void LimpiarControles(object obj, bool tieneValorAsociado = false)
        {
            base.LimpiarControles(obj);

            txtDescripcion.Focus();
        }
    }
}
