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
    using IServicio.Departamento.DTOs;
    using IServicio.Deposito;
    using IServicio.Deposito.DTOs;
    using IServicio.ListaPrecio;
    using IServicio.ListaPrecio.DTOs;
    using IServicio.Localidad;
    using IServicio.Localidad.DTOs;
    using IServicio.Provincia;
    using IServicio.Provincia.DTOs;
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

            if (_configuracion == null)
               _configuracion = _configuracionServicio.ConfiguracionPorDefecto();

            else
                _configuracion.EsPrimeraVez = false;

            CargarDatosDeConfiguracion();
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
            txtDigitosComienzoCodigo.MaxLength = 4;
        }

        private void PoblarComboBoxes()
        {
            CargarComboProvincia();

            CargarComboDepartamento();

            CargarComboLocalidad();

            CargarComboListaPrecios();

            CargarComboDepositoStock();

            CargarComboDepositoVenta();

            PoblarComboBox(
                cmbTipoPagoCompraPorDefecto,
                Enum.GetValues(typeof(TipoPago)));

            PoblarComboBox(
                cmbTipoPagoPorDefecto,
                Enum.GetValues(typeof(TipoPago)));
        }

        private void CargarComboProvincia(long idElemento = 0)
        {
            var lstProvincias = _provinciaServicio.Obtener(string.Empty, false)
                .Select(x => (ProvinciaDto)x)
                .ToList();

            if (idElemento != 0)
            {
                var provincia = (ProvinciaDto)_provinciaServicio.Obtener(idElemento);
                if(provincia.Eliminado)
                    lstProvincias.Add(provincia);
            }

            PoblarComboBox(cmbProvincia, lstProvincias, "Descripcion", "id");
        }

        private void CargarComboDepartamento(long idElemento = 0)
        {
            var lstDepartamentos = new List<DepartamentoDto>();
            if (cmbProvincia.Items.Count > 0 || cmbProvincia.SelectedValue != null)
                lstDepartamentos = _departamentoServicio.ObtenerPorProvincia((long)cmbProvincia.SelectedValue)
                    .Select(x => (DepartamentoDto)x)
                    .ToList();

            if (idElemento != 0)
            {
                var departamento = (DepartamentoDto)_departamentoServicio.Obtener(idElemento);

                if(departamento.Eliminado)
                    lstDepartamentos.Add(departamento);
            }

            PoblarComboBox(cmbDepartamento, lstDepartamentos, "Descripcion", "Id");
        }

        private void CargarComboLocalidad(long idElemento = 0)
        {
            var lstLocalidades = new List<LocalidadDto>();

            if (cmbDepartamento.Items.Count > 0 || cmbDepartamento.SelectedValue != null)
                lstLocalidades = _localidadServicio.ObtenerPorDepartamento((long)cmbDepartamento.SelectedValue)
                    .Select(x => (LocalidadDto)x)
                    .ToList();

            if (idElemento != 0)
            {
                var localidad = (LocalidadDto)_localidadServicio.Obtener(idElemento);

                if(localidad.Eliminado)
                    lstLocalidades.Add(localidad);
            }

            PoblarComboBox(cmbLocalidad, lstLocalidades, "Descripcion", "id");
        }

        private void CargarComboListaPrecios(long idElemento = 0)
        {
            var lstListas = _listaPreciosServicio.Obtener(string.Empty, false)
                .Select(x => (ListaPrecioDto)x)
                .Where(x => !x.NecesitaAutorizacion)
                .ToList();

            if (idElemento != 0)
            {
                var listaPrecio = (ListaPrecioDto)_listaPreciosServicio.Obtener(idElemento);

                if(listaPrecio.Eliminado)
                    lstListas.Add(listaPrecio);
            }

            PoblarComboBox(cmbListaPrecioDefecto, lstListas, "Descripcion", "id");
        }

        private void CargarComboDepositoStock(long idElemento = 0)
        {
            var lstDeposito = _depositoServicio.Obtener(string.Empty, false)
                .Select(x => (DepositoDto)x)
                .ToList();

            if (idElemento != 0)
            {
                var listaPrecio = (DepositoDto)_depositoServicio.Obtener(idElemento);

                if(listaPrecio.Eliminado)
                    lstDeposito.Add(listaPrecio);
            }

            PoblarComboBox(cmbDepositoPorDefectoStock, lstDeposito, "Descripcion", "id");
        }

        private void CargarComboDepositoVenta(long idElemento = 0)
        {
            var lstDeposito = _depositoServicio.Obtener(string.Empty, false)
                    .Select(x => (DepositoDto)x)
                    .ToList();

            if (idElemento != 0)
            {
                var deposito = (DepositoDto)_depositoServicio.Obtener(idElemento);

                if(deposito.Eliminado)
                    lstDeposito.Add(deposito);
            }

            PoblarComboBox(cmbDepositoPorDefectoVenta, lstDeposito, "Descripcion", "id");
        }

        private void CargarDatosDeConfiguracion()
        {
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

            CargarComboProvincia(_configuracion.ProvinciaId);
            cmbProvincia.SelectedValue = _configuracion.ProvinciaId;

            CargarComboDepartamento(_configuracion.DepartamentoId);
            cmbDepartamento.SelectedValue = _configuracion.DepartamentoId;

            CargarComboLocalidad(_configuracion.LocalidadId);
            cmbLocalidad.SelectedValue = _configuracion.LocalidadId;

            //
            // Datos del Stock
            //
            chkFacturaDescuentaStock.Checked = _configuracion.FacturaDescuentaStock;
            chkPresupuestoDescuentaStock.Checked = _configuracion.PresupuestoDescuentaStock;
            chkActualizaCostoDesdeCompra.Checked = _configuracion.ActualizaCostoDesdeCompra;
            cmbTipoPagoCompraPorDefecto.SelectedItem = _configuracion.TipoFormaPagoPorDefectoCompra;

            CargarComboDepositoStock(_configuracion.DepositoNuevoArticuloId);
            cmbDepositoPorDefectoStock.SelectedValue = _configuracion.DepositoNuevoArticuloId;

            //
            // Datos de Ventas
            //
            txtObservacionFactura.Text = _configuracion.ObservacionEnPieFactura;
            chkRenglonesFactura.Checked = _configuracion.UnificarRenglonesIngresarMismoProducto;
            cmbTipoPagoPorDefecto.SelectedItem = _configuracion.TipoFormaPagoPorDefectoVenta;

            CargarComboListaPrecios(_configuracion.ListaPrecioPorDefectoId);
            cmbListaPrecioDefecto.SelectedValue = _configuracion.ListaPrecioPorDefectoId;

            CargarComboDepositoVenta(_configuracion.DepositoVentaId);
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

            //_configuracion.ProvinciaId = (long)cmbProvincia.SelectedValue;
            //_configuracion.DepartamentoId = (long)cmbProvincia.SelectedValue;
            _configuracion.LocalidadId = (long)cmbLocalidad.SelectedValue;

            //
            // Datos del Stock
            //
            _configuracion.FacturaDescuentaStock = chkFacturaDescuentaStock.Checked;
            _configuracion.PresupuestoDescuentaStock = chkPresupuestoDescuentaStock.Checked;
            _configuracion.RemitoDescuentaStock = false;
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
                tabControlConfig.SelectedIndex = 0;
                ok = false;
            }
            else
                validar.ClearErrorProvider(txtCUIL);

            if (cmbProvincia.SelectedValue == null || cmbProvincia.Items.Count < 1)
            {
                validar.SetErrorProvider(cmbProvincia, "Debes seleccionar una provincia.");
                ok = false;
            }
            else
                validar.ClearErrorProvider(cmbProvincia);

            if (cmbLocalidad.SelectedValue == null || cmbLocalidad.Items.Count < 1)
            {
                validar.SetErrorProvider(cmbLocalidad, "Debes seleccionar una localidad.");
                tabControlConfig.SelectedIndex = 0;
                ok = false;
            }
            else
                validar.ClearErrorProvider(cmbLocalidad);

            if (cmbDepartamento.SelectedValue == null || cmbDepartamento.Items.Count < 1)
            {
                validar.SetErrorProvider(cmbDepartamento, "Debes seleccionar una departamento.");
                tabControlConfig.SelectedIndex = 0;
                ok = false;
            }
            else
                validar.ClearErrorProvider(cmbDepartamento);

            if (chkActibarBascula.Checked && string.IsNullOrEmpty(txtDigitosComienzoCodigo.Text))
            {
                validar.SetErrorProvider(txtDigitosComienzoCodigo, "Ingrese un codigo.");
                tabControlConfig.SelectedIndex = 4;
                ok = false;
            }
            else
                validar.ClearErrorProvider(txtDigitosComienzoCodigo);

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

            CargarComboDepartamento();
            CargarComboLocalidad();
        }

        private void cmbDepartamento_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (_configuracion.DepartamentoId == (long)cmbDepartamento.SelectedValue)
                return;

            _configuracion.DepartamentoId = (long)cmbDepartamento.SelectedValue;

            CargarComboDepartamento(_configuracion.DepartamentoId);

            CargarComboLocalidad();
        }

        private void cmbLocalidad_SelectionChangeCommitted(object sender, EventArgs e)
        {
            _configuracion.LocalidadId = (long)cmbLocalidad.SelectedValue;
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

            if (!form.RealizoAlgunaOperacion)
                return;

            CargarComboProvincia();
            CargarComboDepartamento();
            CargarComboLocalidad();
        }

        private void btnNuevoDepartamento_Click(object sender, EventArgs e)
        {
            var form = new _00004_Abm_Departamento(TipoOperacion.Nuevo);
            form.ShowDialog();

            if (!form.RealizoAlgunaOperacion)
                return;

            CargarComboDepartamento();
            CargarComboLocalidad();
        }

        private void btnNuevaLocalidad_Click(object sender, EventArgs e)
        {
            var form = new _00006_AbmLocalidad(TipoOperacion.Nuevo);
            form.ShowDialog();

            if (!form.RealizoAlgunaOperacion)
                return;

            CargarComboLocalidad();
        }

        private void btnNuevaListaPrecio_Click(object sender, EventArgs e)
        {
            var f = new _00033_Abm_ListaPrecio(TipoOperacion.Nuevo);
            f.ShowDialog();

            if (f.RealizoAlgunaOperacion)
                return;

            CargarComboListaPrecios();
        }

        private void btnNuevoDeposito_Click(object sender, EventArgs e)
        {
            var f = new _00051_Abm_Deposito(TipoOperacion.Nuevo);
            f.ShowDialog();

            if (!f.RealizoAlgunaOperacion)
                return;

            CargarComboDepositoStock(_configuracion.DepositoNuevoArticuloId);
            CargarComboDepositoVenta(_configuracion.DepositoVentaId);
        }

        private void chkActibarBascula_CheckedChanged(object sender, EventArgs e)
        {
            pnlBasculaConfig.Enabled = chkActibarBascula.Checked;
        }
    }
}
