namespace Presentacion.Core.Caja
{
    using System;
    using System.Linq;
    using System.Windows.Forms;
    using Aplicacion.Constantes;
    using IServicio.Caja;
    using IServicio.Caja.DTOs;
    using IServicios.Caja;
    using PresentacionBase.Formularios;
    using StructureMap;

    public partial class _00044_Abm_Gastos : FormAbm
    {
        private readonly IGastoServicio _servicio;
        private readonly IConceptoGastoServicio _servicioConceptoGasto;
        private readonly ICajaServicio _servicioCaja;
        private readonly Validar Validar;
        private long cajaActivaId;

        public _00044_Abm_Gastos(TipoOperacion tipoOperacion, long? entidadId = null)
            : base(tipoOperacion, entidadId)
        {
            InitializeComponent();

            _servicio = ObjectFactory.GetInstance<IGastoServicio>();
            _servicioConceptoGasto = ObjectFactory.GetInstance<IConceptoGastoServicio>();
            _servicioCaja = ObjectFactory.GetInstance<ICajaServicio>();
            Validar = new Validar();
        }

        private void _00044_Abm_Gastos_Load(object sender, System.EventArgs e)
        {
            Validar.ComoAlfanumerico(txtDescripcion, true);

            var cajaId = _servicioCaja.ObtenerIdCajaAciva(Identidad.UsuarioId);

            if (cajaId == null)
            {
                Mjs.Alerta("No se puede continuar con la operación porqué no hay una caja abierta.");
                Close();
            }

            cajaActivaId = (long)cajaId;

            dtpFecha.MaxDate = DateTime.Today;
            dtpFecha.MinDate = DateTime.Today.AddYears(-1);

            CargarComboConceptoGasto();

            if (EntidadId.HasValue)
            {
                var gasto = (GastoDto)_servicio.Obtener(EntidadId.Value);

                if (gasto == null)
                {
                    MessageBox.Show("Ocurrio un error al obtener el registro seleccionado.");
                    Close();
                }

                txtDescripcion.Text = gasto.Descripcion;
                dtpFecha.Value = gasto.Fecha;
                nudMontoPagar.Value = gasto.Monto;

                CargarComboConceptoGasto(gasto.ConceptoGastoId);
                cmbConcepto.SelectedValue = gasto.ConceptoGastoId;
            }
        }

        private void CargarComboConceptoGasto(long id = 0)
        {
            var lstConceptos = _servicioConceptoGasto.Obtener(string.Empty, false)
                .Select(x => (ConceptoGastoDto)x)
                .ToList();

            if (id != 0)
            {
                var conceptoGasto = (ConceptoGastoDto)_servicioConceptoGasto.Obtener(id);

                if (conceptoGasto.Eliminado)
                    lstConceptos.Add(conceptoGasto);
            }

            PoblarComboBox(cmbConcepto, lstConceptos, "Descripcion", "Id");
        }

        public override bool VerificarDatosObligatorios()
        {
            bool ok = true;

            if (nudMontoPagar.Value <= 0)
            {
                Validar.SetErrorProvider(nudMontoPagar, "El monto de gasto debe ser superior a 0.");
                ok = false;
            }
            else Validar.ClearErrorProvider(nudMontoPagar);

            if (cmbConcepto.Items.Count < 1 || cmbConcepto.SelectedValue == null)
            {
                Validar.SetErrorProvider(cmbConcepto, "Seleccione un concepto de gasto.");
                ok = false;
            }
            else Validar.ClearErrorProvider(cmbConcepto);

            ok &= ValidateChildren();

            return ok;
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
            var nuevoRegistro = new GastoDto();
            nuevoRegistro.Descripcion = txtDescripcion.Text;
            nuevoRegistro.Fecha = dtpFecha.Value;
            nuevoRegistro.ConceptoGastoId = (long)cmbConcepto.SelectedValue;
            nuevoRegistro.Monto = nudMontoPagar.Value;
            nuevoRegistro.Eliminado = false;
            nuevoRegistro.CajaId = cajaActivaId;

            _servicio.Insertar(nuevoRegistro);
        }

        public override void EjecutarComandoModificar()
        {
            var modificarRegistro = new GastoDto();
            modificarRegistro.Id = EntidadId.Value;
            modificarRegistro.Descripcion = txtDescripcion.Text;
            modificarRegistro.Fecha = dtpFecha.Value;
            modificarRegistro.ConceptoGastoId = (long)cmbConcepto.SelectedValue;
            modificarRegistro.Monto = nudMontoPagar.Value;
            modificarRegistro.Eliminado = false;
            modificarRegistro.CajaId = cajaActivaId;

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

        private void btnNuevoConcepto_Click(object sender, System.EventArgs e)
        {
            var form = new _00042_Abm_ConceptoGastos(TipoOperacion.Nuevo);
            form.ShowDialog();

            if(form.RealizoAlgunaOperacion)
                
                PoblarComboBox(
                    cmbConcepto,
                    _servicioConceptoGasto.Obtener(string.Empty, false),
                    "Descripcion",
                    "Id"
                    );
        }

    }
}
