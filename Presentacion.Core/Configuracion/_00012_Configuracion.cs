namespace Presentacion.Core.Configuracion
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;
    using Aplicacion.Constantes;
    using IServicio.Configuracion;
    using IServicio.Configuracion.DTOs;
    using IServicio.Departamento;
    using IServicio.Deposito;
    using IServicio.ListaPrecio;
    using IServicio.Localidad;
    using IServicio.Provincia;
    using Presentacion.Core.Articulo;
    using Presentacion.Core.Departamento;
    using Presentacion.Core.Localidad;
    using Presentacion.Core.Provincia;
    using PresentacionBase.Formularios;

    public partial class _00012_Configuracion : FormBase
    {
        private readonly IProvinciaServicio _provinciaServicio;
        private readonly IDepartamentoServicio _departamentoServicio;
        private readonly ILocalidadServicio _localidadServicio;
        private readonly IListaPrecioServicio _listaPreciosServicio;
        private readonly IDepositoSevicio _depositoServicio;
        private readonly IConfiguracionServicio _configuracionServicio;
        private readonly Validar validar;

        private ConfiguracionDto _configuracion;

        public _00012_Configuracion(
            IProvinciaServicio provinciaServicio,
            IDepartamentoServicio departamentoServicio,
            ILocalidadServicio localidadServicio,
            IListaPrecioServicio listaPreciosServicio,
            IDepositoSevicio depositoServicio,
            IConfiguracionServicio configuracionServicio
            )
        {
            InitializeComponent();

            AutoValidate = AutoValidate.EnableAllowFocusChange;

            _provinciaServicio = provinciaServicio;
            _departamentoServicio = departamentoServicio;
            _localidadServicio = localidadServicio;
            _listaPreciosServicio = listaPreciosServicio;
            _depositoServicio = depositoServicio;
            _configuracionServicio = configuracionServicio;
            validar = new Validar();

        }

        private void _00012_Configuracion_Load(object sender, EventArgs e)
        {
            // Asignar Eventos
            AsignarEvento_EnterLeave(this);

            PoblarComboBoxes();

            SetearValidaciones();

            _configuracion = _configuracionServicio.Obtener();

            if (_configuracion != null)
                CargarDatosDeConfiguracion();

            else
                PrepararParaCargaDeConfiguracion();
        }

        private void SetearValidaciones()
        {
            // Datos Empresa
            validar.ComoAlfanumerico(txtRazonSocial, true);
            validar.ComoAlfanumerico(txtNombreFantasia, true);
            validar.ComoCuit(txtCUIL, true);
            validar.ComoTelefono(txtTelefono);
            validar.ComoTelefono(txtCelular);
            validar.ComoDomicilio(txtDireccion);
            validar.ComoMail(txtEmail);

            // Ventas
            validar.ComoAlfanumerico(txtObservacionFactura);

            // Bascula
            validar.ComoNumero(txtDigitosComienzoCodigo);
        }

        private void PoblarComboBoxes()
        {
            PoblarComboBox(
                cmbProvincia,
                _provinciaServicio.Obtener(string.Empty, false),
                "Descripcion",
                "Id");

            if (cmbProvincia.Items.Count > 0)
                PoblarComboBox(
                    cmbDepartamento,
                    _departamentoServicio.ObtenerPorProvincia((long)cmbProvincia.SelectedValue),
                    "Descripcion",
                    "Id");

            if (cmbDepartamento.Items.Count > 0)
                PoblarComboBox(
                    cmbLocalidad,
                    _localidadServicio.ObtenerPorDepartamento((long)cmbDepartamento.SelectedValue),
                    "Descripcion",
                    "Id");

            PoblarComboBox(
                cmbListaPrecioDefecto,
                _listaPreciosServicio.Obtener(string.Empty, false),
                "Descripcion",
                "Id");

            PoblarComboBox(
                cmbDepositoPorDefectoStock,
                _depositoServicio.Obtener(string.Empty, false),
                "Descripcion",
                "Id");

            PoblarComboBox(
                cmbDepositoPorDefectoVenta,
                _depositoServicio.Obtener(string.Empty, false),
                "Descripcion",
                "Id");

            PoblarComboBox(
                cmbTipoPagoCompraPorDefecto,
                Enum.GetValues(typeof(TipoPago)));

            PoblarComboBox(
                cmbTipoPagoPorDefecto,
                Enum.GetValues(typeof(TipoPago)));
        }

        private void PrepararParaCargaDeConfiguracion()
        {
            _configuracion = new ConfiguracionDto();
            _configuracion.EsPrimeraVez = true;
        }

        private void CargarDatosDeConfiguracion()
        {
            _configuracion.EsPrimeraVez = false;

            //
            // Datos de la empresa
            //
            txtRazonSocial.Text = _configuracion.RazonSocial;
            txtNombreFantasia.Text = _configuracion.NombreFantasia;
            txtCUIL.Text = _configuracion.Cuit;
            txtTelefono.Text = _configuracion.Telefono;
            txtCelular.Text = _configuracion.Celular;
            txtDireccion.Text = _configuracion.Direccion;
            txtEmail.Text = _configuracion.Email;

            cmbProvincia.SelectedValue = _configuracion.ProvinciaId;
            CargarDepartamentosPorProvincia(_configuracion.ProvinciaId);
            cmbDepartamento.SelectedValue = _configuracion.DepartamentoId;
            CargarLocalidadesPorDepartamento(_configuracion.DepartamentoId);
            cmbLocalidad.SelectedValue = _configuracion.LocalidadId;

            //
            // Datos del Stock
            //
            chkFacturaDescuentaStock.Checked = _configuracion.FacturaDescuentaStock;
            chkPresupuestoDescuentaStock.Checked = _configuracion.PresupuestoDescuentaStock;
            chkRemitoDescuentaStock.Checked = _configuracion.RemitoDescuentaStock;
            chkActualizaCostoDesdeCompra.Checked = _configuracion.ActualizaCostoDesdeCompra;
            cmbTipoPagoCompraPorDefecto.SelectedItem = _configuracion.TipoFormaPagoPorDefectoCompra;
            cmbDepositoPorDefectoStock.SelectedValue = _configuracion.DepositoNuevoArticuloId;

            //
            // Datos de Ventas
            //
            txtObservacionFactura.Text = _configuracion.ObservacionEnPieFactura;
            chkRenglonesFactura.Checked = _configuracion.UnificarRenglonesIngresarMismoProducto;
            cmbListaPrecioDefecto.SelectedValue = _configuracion.ListaPrecioPorDefectoId;
            cmbTipoPagoPorDefecto.SelectedItem = _configuracion.TipoFormaPagoPorDefectoVenta;
            cmbDepositoPorDefectoVenta.SelectedValue = _configuracion.DepositoVentaId;

            //
            // Datos de Báscula
            //
            chkActibarBascula.Checked = _configuracion.ActivarBascula;
            txtDigitosComienzoCodigo.Text = _configuracion.CodigoBascula;
            rdbEtiquetaPorPeso.Checked = _configuracion.EtiquetaPorPeso;
            rdbEtiquetaPorPrecio.Checked = _configuracion.EtiquetaPorPrecio;

            //
            // Datos de la Caja
            //
            if (_configuracion.IngresoManualCajaInicial)
                rdbIngresoManualCaja.Checked = true;
            else
                rdbIngresoPorCierreDelDIaAnterior.Checked = true;

            chkPuestoSeparado.Checked = _configuracion.PuestoCajaSeparado;
            chkPermitirArqueoNegativo.Checked = _configuracion.PermitirArqueoNegativo;
        }

        private void CargarDepartamentosPorProvincia(long provinciaId)
        {
            var lstDepartamentos = _departamentoServicio.ObtenerPorProvincia(provinciaId).ToList();

            if (lstDepartamentos.Count < 1)
            {
                cmbLocalidad.Enabled = false;
                cmbLocalidad.DataSource = new List<object>();

                cmbDepartamento.Enabled = false;
                cmbDepartamento.DataSource = new List<object>();

                return;
            }

            cmbDepartamento.Enabled = true;
            PoblarComboBox(cmbDepartamento, lstDepartamentos, "Descripcion", "Id");
        }

        private void CargarLocalidadesPorDepartamento(long departamentoId)
        {
            var lstlocalidades = _localidadServicio.ObtenerPorDepartamento(departamentoId).ToList();

            if (lstlocalidades.Count < 1)
            {
                cmbLocalidad.DataSource = new List<object>();
                cmbLocalidad.Enabled = false;
                return;
            }

            cmbLocalidad.Enabled = true;
            PoblarComboBox(cmbLocalidad, lstlocalidades, "Descripcion", "Id");
        }

        private void ExtraerDatosCargados()
        {
            //
            // Datos de la empresa
            //
            _configuracion.RazonSocial = txtRazonSocial.Text;
            _configuracion.NombreFantasia = txtNombreFantasia.Text;
            _configuracion.Cuit = txtCUIL.Text;
            _configuracion.Telefono = txtTelefono.Text;
            _configuracion.Celular = txtCelular.Text;
            _configuracion.Direccion = txtDireccion.Text;
            _configuracion.Email = txtEmail.Text;

            _configuracion.ProvinciaId = (long)cmbProvincia.SelectedValue;
            _configuracion.DepartamentoId = (long)cmbProvincia.SelectedValue;
            _configuracion.LocalidadId = (long)cmbProvincia.SelectedValue;

            //
            // Datos del Stock
            //
            _configuracion.FacturaDescuentaStock = chkFacturaDescuentaStock.Checked;
            _configuracion.PresupuestoDescuentaStock = chkPresupuestoDescuentaStock.Checked;
            _configuracion.RemitoDescuentaStock = chkRemitoDescuentaStock.Checked;
            _configuracion.ActualizaCostoDesdeCompra = chkActualizaCostoDesdeCompra.Checked;
            _configuracion.TipoFormaPagoPorDefectoCompra = (TipoPago)cmbTipoPagoCompraPorDefecto.SelectedItem;
            _configuracion.DepositoNuevoArticuloId = (long)cmbDepositoPorDefectoStock.SelectedValue;

            //
            // Datos de Ventas
            //
            _configuracion.ObservacionEnPieFactura = txtObservacionFactura.Text;
            _configuracion.UnificarRenglonesIngresarMismoProducto = chkRenglonesFactura.Checked;
            _configuracion.TipoFormaPagoPorDefectoVenta = (TipoPago)cmbTipoPagoPorDefecto.SelectedItem;
            _configuracion.ListaPrecioPorDefectoId = (long)cmbListaPrecioDefecto.SelectedValue;
            _configuracion.DepositoVentaId = (long)cmbDepositoPorDefectoVenta.SelectedValue;

            //
            // Datos de Báscula
            //
            _configuracion.ActivarBascula = chkActibarBascula.Checked;
            _configuracion.CodigoBascula = txtDigitosComienzoCodigo.Text.PadLeft(4,'0');
            _configuracion.EtiquetaPorPeso = rdbEtiquetaPorPeso.Checked;
            _configuracion.EtiquetaPorPrecio = rdbEtiquetaPorPrecio.Checked;

            //
            // Datos de la Caja
            //
            _configuracion.IngresoManualCajaInicial = rdbIngresoManualCaja.Checked;

            _configuracion.PuestoCajaSeparado = chkPuestoSeparado.Checked;
            _configuracion.PermitirArqueoNegativo = chkPermitirArqueoNegativo.Checked;
        }

        private bool VerificarDatosIngresados()
        {
            bool ok = true;

            ok &= ValidateChildren();

            if (!validar.EsCuit(txtCUIL.Text, out string errMjs))
            {
                validar.SetErrorProvider(txtCUIL, errMjs);
                ok = false;
            }
            else
                validar.ClearErrorProvider(txtCUIL);

            if (_configuracion.ProvinciaId < 1)
            {
                validar.SetErrorProvider(cmbProvincia, "Debes seleccionar una provincia.");
                ok = false;
            }
            else
                validar.ClearErrorProvider(cmbProvincia);

            if(_configuracion.LocalidadId < 1)
            {
                validar.SetErrorProvider(cmbLocalidad, "Debes seleccionar una localidad.");
                ok = false;
            }
            else
                validar.ClearErrorProvider(cmbLocalidad);

            if (_configuracion.DepartamentoId < 1)
            {
                validar.SetErrorProvider(cmbDepartamento, "Debes seleccionar una departamento.");
                ok = false;
            }
            else
                validar.ClearErrorProvider(cmbDepartamento);

            if (!ok)
                Mjs.Alerta("Alguno de los datos ingresados son incorrectos.");

            return ok;
        }

        // --- Cascadeo de Provincia - Departamento - Localidad
        private void cmbProvincia_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (_configuracion.ProvinciaId == (long)cmbProvincia.SelectedValue)
                return;

            _configuracion.ProvinciaId = (long)cmbProvincia.SelectedValue;

            CargarDepartamentosPorProvincia(_configuracion.ProvinciaId);

        }

        private void cmbDepartamento_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (_configuracion.DepartamentoId == (long)cmbDepartamento.SelectedValue)
                return;

            _configuracion.DepartamentoId = 
                cmbDepartamento.Items.Count > 0
                ? (long)cmbDepartamento.SelectedValue
                : 0;

            if(_configuracion.DepartamentoId > 0)
                CargarLocalidadesPorDepartamento(_configuracion.DepartamentoId);

            _configuracion.LocalidadId =
                cmbLocalidad.Items.Count > 0
                ? (long)cmbLocalidad.SelectedValue
                : 0;
        }

        private void cmbLocalidad_SelectionChangeCommitted(object sender, EventArgs e)
        {
            _configuracion.LocalidadId =
                cmbLocalidad.Items.Count > 0
                ? (long)cmbLocalidad.SelectedValue
                : 0;
        }

        // --- Acciones de botones
        private void btnEjecutar_Click(object sender, EventArgs e)
        {
            if (!VerificarDatosIngresados())
                return;

            ExtraerDatosCargados();
            _configuracionServicio.Grabar(_configuracion);
            Mjs.Info("Los datos se guardaron correctamente.");
            Close();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            if (DialogResult.No == MessageBox.Show("¿Desea limpiar todos los campos?","Atención",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation))
                return;

            LimpiarControles(this);
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnNuevaProvincia_Click(object sender, EventArgs e)
        {
            var form = new _00002_Abm_Provincia(TipoOperacion.Nuevo);
            form.ShowDialog();

            if(form.RealizoAlgunaOperacion)
                PoblarComboBox(
                    cmbProvincia,
                    _provinciaServicio.Obtener(string.Empty, false),
                    "Descripcion",
                    "Id");
        }

        private void btnNuevoDepartamento_Click(object sender, EventArgs e)
        {
            var form = new _00004_Abm_Departamento(TipoOperacion.Nuevo);
            form.ShowDialog();

            if (form.RealizoAlgunaOperacion)
                PoblarComboBox(
                    cmbDepartamento,
                    _departamentoServicio.ObtenerPorProvincia((long)cmbProvincia.SelectedValue),
                    "Descripcion",
                    "Id");
        }

        private void btnNuevaLocalidad_Click(object sender, EventArgs e)
        {
            var form = new _00006_AbmLocalidad(TipoOperacion.Nuevo);
            form.ShowDialog();

            if (form.RealizoAlgunaOperacion)
                PoblarComboBox(
                    cmbLocalidad,
                    _localidadServicio.ObtenerPorDepartamento((long)cmbDepartamento.SelectedValue),
                    "Descripcion",
                    "Id");
        }

        private void btnNuevaListaPrecio_Click(object sender, EventArgs e)
        {
            var f = new _00033_Abm_ListaPrecio(TipoOperacion.Nuevo);
            f.ShowDialog();

            if(f.RealizoAlgunaOperacion)
                PoblarComboBox(
                    cmbListaPrecioDefecto,
                    _listaPreciosServicio.Obtener(string.Empty, false),
                    "Descripcion",
                    "Id");
        }

        private void btnNuevoDeposito_Click(object sender, EventArgs e)
        {
            var f = new _00051_Abm_Deposito(TipoOperacion.Nuevo);
            f.ShowDialog();

            if (!f.RealizoAlgunaOperacion)
                return;

            PoblarComboBox(
                    cmbDepositoPorDefectoStock,
                    _depositoServicio.Obtener(string.Empty, false),
                    "Descripcion",
                    "Id");
            PoblarComboBox(
                cmbDepositoPorDefectoVenta,
                _depositoServicio.Obtener(string.Empty, false),
                "Descripcion",
                "Id");
        }

        private void chkActibarBascula_CheckedChanged(object sender, EventArgs e)
        {
            pnlBasculaConfig.Enabled = chkActibarBascula.Checked;
        }
    }
}
