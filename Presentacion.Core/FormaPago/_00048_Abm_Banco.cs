namespace Presentacion.Core.FormaPago
{
    using Aplicacion.Constantes;
    using IServicio.FormaPago;
    using IServicios.FormaPago.DTOs;
    using PresentacionBase.Formularios;
    using StructureMap;

    public partial class _00048_Abm_Banco : FormAbm
    {
        private readonly IBancoServicio _servicio;
        private readonly Validar Validar;

        public _00048_Abm_Banco(TipoOperacion tipoOperacion, long? entidadId = null)
            : base(tipoOperacion, entidadId)
        {
            InitializeComponent();

            _servicio = ObjectFactory.GetInstance<IBancoServicio>();
            Validar = new Validar();
        }

        private void _00048_Abm_Banco_Load(object sender, System.EventArgs e)
        {
            Validar.ComoTexto(txtDescripcion, true);
        }

        public override void CargarDatos(long? entidadId)
        {
            base.CargarDatos(entidadId);

            if (entidadId.HasValue)
            {
                var resultado = (BancoDto)_servicio.Obtener(entidadId.Value);

                if (resultado == null)
                {
                    Mjs.Error("Ocurrio un error al obtener el registro seleccionado.");
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
            var nuevoRegistro = new BancoDto();
            nuevoRegistro.Descripcion = txtDescripcion.Text;
            nuevoRegistro.Eliminado = false;

            _servicio.Insertar(nuevoRegistro);
        }

        public override void EjecutarComandoModificar()
        {
            var modificarRegistro = new BancoDto();
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
