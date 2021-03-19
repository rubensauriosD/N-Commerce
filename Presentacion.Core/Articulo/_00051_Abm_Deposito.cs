using System.Windows.Forms;
using IServicio.Deposito;
using IServicio.Deposito.DTOs;
using PresentacionBase.Formularios;
using StructureMap;

namespace Presentacion.Core.Articulo
{
    public partial class _00051_Abm_Deposito : FormAbm
    {
        private readonly IDepositoSevicio _servicio;

        public _00051_Abm_Deposito(TipoOperacion tipoOperacion, long? entidadId = null)
            : base(tipoOperacion, entidadId)
        {
            InitializeComponent();

            _servicio = ObjectFactory.GetInstance<IDepositoSevicio>();
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
            return !string.IsNullOrEmpty(txtDescripcion.Text)
                && !string.IsNullOrEmpty(txtUbicacion.Text);
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
            _servicio.Eliminar(EntidadId.Value);
        }

        public override void LimpiarControles(object obj, bool tieneValorAsociado = false)
        {
            base.LimpiarControles(obj);

            txtDescripcion.Focus();
        }

    }
}
