namespace Presentacion.Core.Articulo
{
    using System.Windows.Forms;
    using Aplicacion.Constantes;
    using IServicio.ListaPrecio;
    using IServicio.ListaPrecio.DTOs;
    using PresentacionBase.Formularios;
    using StructureMap;

    public partial class _00033_Abm_ListaPrecio : FormAbm
    {
        private readonly IListaPrecioServicio _servicio;
        private readonly Validar Validar;

        public _00033_Abm_ListaPrecio(TipoOperacion tipoOperacion, long? entidadId = null)
            : base(tipoOperacion, entidadId)
        {
            InitializeComponent();

            _servicio = ObjectFactory.GetInstance<IListaPrecioServicio>();
        }

        private void _00033_Abm_ListaPrecio_Load(object sender, System.EventArgs e)
        {
            Validar.ComoAlfanumerico(txtDescripcion);
            txtDescripcion.MaxLength = 250;
        }

        public override void CargarDatos(long? entidadId)
        {
            base.CargarDatos(entidadId);

            if (entidadId.HasValue)
            {
                var resultado = (ListaPrecioDto)_servicio.Obtener(entidadId.Value);

                if (resultado == null)
                {
                    MessageBox.Show("Ocurrio un error al obtener el registro seleccionado.");
                    Close();
                }

                txtDescripcion.Text = resultado.Descripcion;
                nudPorcentaje.Value = resultado.PorcentajeGanancia;
                chkPedirAutorizacion.Checked = resultado.NecesitaAutorizacion;

                if (TipoOperacion == TipoOperacion.Eliminar)
                    DesactivarControles(this);
            }
        }

        public override bool VerificarDatosObligatorios()
        {
            return !string.IsNullOrEmpty(txtDescripcion.Text);
        }

        public override bool VerificarSiExiste(long? id = null)
        {
            return _servicio.VerificarSiExiste(txtDescripcion.Text, id);
        }

        //
        // Acciones de botones
        //
        public override void EjecutarComandoNuevo()
        {
            var nuevoRegistro = new ListaPrecioDto();
            nuevoRegistro.Descripcion = txtDescripcion.Text;
            nuevoRegistro.PorcentajeGanancia = nudPorcentaje.Value;
            nuevoRegistro.NecesitaAutorizacion = chkPedirAutorizacion.Checked;
            nuevoRegistro.Eliminado = false;

            _servicio.Insertar(nuevoRegistro);
        }

        public override void EjecutarComandoModificar()
        {
            var modificarRegistro = new ListaPrecioDto();
            modificarRegistro.Id = EntidadId.Value;
            modificarRegistro.Descripcion = txtDescripcion.Text;
            modificarRegistro.PorcentajeGanancia = nudPorcentaje.Value;
            modificarRegistro.NecesitaAutorizacion = chkPedirAutorizacion.Checked;
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
