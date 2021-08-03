namespace Presentacion.Core.Comprobantes
{
    using System;
    using System.Linq;
    using System.Windows.Forms;
    using StructureMap;
    using IServicio.Persona;
    using IServicio.Persona.DTOs;
    using Presentacion.Core.Comprobantes.Clases;
    using Presentacion.Core.Proveedor;
    using PresentacionBase.Formularios;
    using IServicio.Articulo;
    using Aplicacion.Constantes;
    using IServicios.Articulo.DTOs;
    using IServicios.Comprobante.DTOs;
    using IServicios.Comprobante;
    using IServicios.Persona.DTOs;
    using IServicios.Caja;
    using IServicios.Caja.DTOs;
    using System.Collections.Generic;
    using IServicio.Articulo.DTOs;

    public partial class _00053_Compra : FormBase
    {
        private readonly IProveedorServicio _proveedorServicios;
        private readonly IArticuloServicio _artiuloServicios;
        private readonly ICompraServicio _compraServicios;
        private readonly Validar Validar;

        private ArticuloCompraDto itemSeleccionado;
        private CompraView compra;
        private long cajaActivaId;


        public _00053_Compra()
        {
            InitializeComponent();

            _proveedorServicios = ObjectFactory.GetInstance<IProveedorServicio>();
            _artiuloServicios = ObjectFactory.GetInstance<IArticuloServicio>();
            _compraServicios = ObjectFactory.GetInstance<ICompraServicio>();
            Validar = new Validar();

        }

        private void _00053_Compra_Load(object sender, EventArgs e)
        {
            var cajaActivaId = ObjectFactory.GetInstance<ICajaServicio>().ObtenerIdCajaAciva(Identidad.UsuarioId);

            if (cajaActivaId == null)
            {
                Mjs.Alerta($@"No hay una caja abierta.{Environment.NewLine}Por favor abra una caja para poder realizar la operación.");
                Close();
                return;
            }

            this.cajaActivaId = (long)cajaActivaId;

            Validar.ComoCuit(txtCuit);
            Validar.ComoNumero(txtCodigo);
            Validar.ComoNumero(txtNroComprobante, true);

            dtpFecha.MaxDate = DateTime.Now;
            PoblarComboBox(cmbTipoComprobante, Enum.GetValues(typeof(TipoComprobante)));

            SetearControles();

            ActualizarGrilla();

            SetEstadoCargaCodigo();

            txtCuit.Focus();
            txtCuit.SelectAll();
        }

        private void SetearControles()
        {
            compra = new CompraView();
            itemSeleccionado = new ArticuloCompraDto();

            dtpFecha.Value = DateTime.Today;

            nudIva105.Value = 0;
            chkIva105.Checked = false;
            nudIva21.Value = 0;
            chkIva21.Checked = false;
            nudIva27.Value = 0;
            chkIva27.Checked = false;
            nudImpuestoInterno.Value = 0;
            chkImpuestoInterno.Checked = false;
            nudPercepcionIva.Value = 0;
            chkPercepcionIva.Checked = false;
            nudPercepcionTem.Value = 0;
            chkPercepcionTem.Checked = false;
            nudPercepcionPyP.Value = 0;
            chkPercepcionPyP.Checked = false;
            nudPercepcionIB.Value = 0;
            chkPercepcionIB.Checked = false;

            cmbTipoComprobante.SelectedItem = TipoComprobante.B;
            compra.Tipo = (TipoComprobante)cmbTipoComprobante.SelectedItem;
            txtNroComprobante.Text = "";

            compra.Proveedor = _proveedorServicios.ObtenerPorCuit("99999999999");
            CargarDatosProveedor();
            ActualizarGrilla();
        }

        private void CargarDatosProveedor()
        {
            grbFormasPago.Enabled = true;
            txtCuit.Text = compra.Proveedor.CUIT;
            txtNombre.Text = compra.Proveedor.RazonSocial;
            txtDomicilio.Text = compra.Proveedor.Direccion;
            txtTelefono.Text = compra.Proveedor.Telefono;
            txtCondicionIva.Text = compra.Proveedor.CondicionIva;

            if (compra.Proveedor.CUIT == "99999999999")
            {
                grbFormasPago.Enabled = false;
                chkEfectivo.Checked = true;
                chkCuentaCorriente.Checked = false;
            }
            else
            {
                grbFormasPago.Enabled = true;
                chkEfectivo.Checked = true;
                chkCuentaCorriente.Checked = false;
            }
        }

        private void ActualizarTotal()
        {
            lblTotal.Text = (compra.Items.Sum(x => x.SubTotal)
                    + nudImpuestoInterno.Value
                    + nudIva105.Value
                    + nudIva21.Value
                    + nudIva27.Value
                    + nudPercepcionIB.Value
                    + nudPercepcionIva.Value
                    + nudPercepcionPyP.Value
                    + nudPercepcionTem.Value
                ).ToString("C2");
        }

        private void ActualizarGrilla()
        {
            dgvGrilla.DataSource = compra.Items.OrderBy(x => x.Id).ToList();

            for (int i = 0; i < dgvGrilla.ColumnCount; i++)
                dgvGrilla.Columns[i].Visible = false;

            dgvGrilla.Columns["Cantidad"].Visible = true;
            dgvGrilla.Columns["Cantidad"].DisplayIndex = 1;
            dgvGrilla.Columns["Cantidad"].HeaderText = "Cant";
            dgvGrilla.Columns["Cantidad"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvGrilla.Columns["Cantidad"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvGrilla.Columns["Descripcion"].Visible = true;
            dgvGrilla.Columns["Descripcion"].DisplayIndex = 2;
            dgvGrilla.Columns["Descripcion"].HeaderText = "Descripcion";
            dgvGrilla.Columns["Descripcion"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvGrilla.Columns["Descripcion"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dgvGrilla.Columns["Precio"].Visible = true;
            dgvGrilla.Columns["Precio"].DisplayIndex = 3;
            dgvGrilla.Columns["Precio"].HeaderText = "Precio";
            dgvGrilla.Columns["Precio"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvGrilla.Columns["Precio"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvGrilla.Columns["Precio"].DefaultCellStyle.Format = "C2";

            dgvGrilla.Columns["Subtotal"].Visible = true;
            dgvGrilla.Columns["Subtotal"].DisplayIndex = 4;
            dgvGrilla.Columns["Subtotal"].HeaderText = "Subtotal";
            dgvGrilla.Columns["Subtotal"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvGrilla.Columns["Subtotal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvGrilla.Columns["Subtotal"].DefaultCellStyle.Format = "C2";

            ActualizarTotal();
        }

        // --- EVENTOS DE COTROLES
        private void nudPrecioUnitario_ValueChanged(object sender, EventArgs e)
        {
            lblSubTotalLinea.Text = (nudCantidad.Value * nudPrecioUnitario.Value).ToString("c");
        }

        private void nud_Leave(object sender, EventArgs e)
        {
            ActualizarTotal();
        }

        private void nudPrecioUnitario_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
                SetEstadoCargaCodigo();
        }

        private void chkIva27_CheckedChanged(object sender, EventArgs e)
        {
            nudIva27.Enabled = chkIva27.Checked;
            nudIva27.Value = 0;
        }

        private void chkIva21_CheckedChanged(object sender, EventArgs e)
        {
            nudIva21.Enabled = chkIva21.Checked;
            nudIva21.Value = 0;
        }

        private void chkIva105_CheckedChanged(object sender, EventArgs e)
        {
            nudIva105.Enabled = chkIva105.Checked;
            nudIva105.Value = 0;
        }

        private void chkImpuestoInterno_CheckedChanged(object sender, EventArgs e)
        {
            nudImpuestoInterno.Enabled = chkImpuestoInterno.Checked;
            nudImpuestoInterno.Value = 0;
        }

        private void chkPercepcionTemp_CheckedChanged(object sender, EventArgs e)
        {
            nudPercepcionTem.Enabled = chkPercepcionTem.Checked;
            nudPercepcionTem.Value = 0;
        }

        private void chkPercepcionPyP_CheckedChanged(object sender, EventArgs e)
        {
            nudPercepcionPyP.Enabled = chkPercepcionPyP.Checked;
            nudPercepcionPyP.Value = 0;
        }

        private void chkPercepcionIva_CheckedChanged(object sender, EventArgs e)
        {
            nudPercepcionIva.Enabled = chkPercepcionIva.Checked;
            nudPercepcionIva.Value = 0;
        }

        private void chkPercepcionIB_CheckedChanged(object sender, EventArgs e)
        {
            nudPercepcionIB.Enabled = chkPercepcionIB.Checked;
            nudPercepcionIB.Value = 0;
        }

        private void formaPagoEfectivo_CheckedChanged(object sender, EventArgs e)
        {
            chkCuentaCorriente.Checked = !chkEfectivo.Checked;
        }

        private void formaPagoCuentaCorriente_CheckedChanged(object sender, EventArgs e)
        {
            chkEfectivo.Checked = !chkCuentaCorriente.Checked;
        }

        private void cmbTipoComprobante_SelectionChangeCommitted(object sender, EventArgs e)
        {
            compra.Tipo = (TipoComprobante)cmbTipoComprobante.SelectedItem;
        }

        private void dgvGrilla_KeyDown(object sender, KeyEventArgs e)
        {
            if (dgvGrilla.RowCount < 1)
                return;

            if (e.KeyCode != Keys.Delete)
                return;

            compra.EliminarItem(itemSeleccionado.Id);
            ActualizarGrilla();
        }

        private void dgvGrilla_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvGrilla.RowCount < 1)
                return;

            itemSeleccionado = (ArticuloCompraDto)dgvGrilla.Rows[e.RowIndex].DataBoundItem;
        }

        private void txtCuit_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            if (!Validar.EsCuit(txtCuit.Text, out string errTxt))
            {
                Validar.SetErrorProvider(txtCuit, errTxt);
                return;
            }
            else Validar.ClearErrorProvider(txtCuit);

            var proveedor = _proveedorServicios.ObtenerPorCuit(txtCuit.Text);

            if (proveedor == null)
            {
                Validar.SetErrorProvider(txtCuit, "Proveedor no encontrado.");
                return;
            }
            else Validar.ClearErrorProvider(txtCuit);

            compra.Proveedor = proveedor;
            CargarDatosProveedor();
        }

        // --- PROCESO AGREGAR ITEM

        private void SetEstadoCargaCodigo()
        {
            btnAgregarItem.Enabled = false;
            nudCantidad.Enabled = false;
            nudPrecioUnitario.Enabled = false;
            txtCodigo.Enabled = true;

            nudCantidad.Value = 1;
            nudPrecioUnitario.Value = 1;
            lblSubTotalLinea.Text = "$0";
            txtDescripcion.Text = "";
            
            txtCodigo.Clear();
            txtCodigo.Focus();
        }

        private void SetEstadoConfirmarItem()
        {
            btnAgregarItem.Enabled = true;
            nudCantidad.Enabled = true;
            nudPrecioUnitario.Enabled = true;
            txtCodigo.Enabled = false;

            txtDescripcion.Text = itemSeleccionado.Descripcion;
            
            nudPrecioUnitario.Focus();
            nudPrecioUnitario.Select(1,4);
        }

        private void txtCodigo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Enter)
                return;

            if (txtCodigo.Text == "0")
                ActivarBusquedaYListadoDeArticulo();

            itemSeleccionado = _artiuloServicios.ObtenerPorCodigo(txtCodigo.Text);

            if (itemSeleccionado == null)
            {
                Mjs.Error("Código no encontrado.");
                txtCodigo.Focus();
                txtCodigo.SelectAll();
                return;
            }

            SetEstadoConfirmarItem();
        }

        private void btnAgregarItem_Click(object sender, EventArgs e)
        {
            if (nudPrecioUnitario.Value <= 0)
            {
                Mjs.Alerta("El precio del artículo no puede ser 0.");
                return;
            }

            itemSeleccionado.Precio = nudPrecioUnitario.Value;
            itemSeleccionado.Cantidad = nudCantidad.Value;
            compra.AgregarItem(itemSeleccionado);

            itemSeleccionado = new ArticuloCompraDto();
            ActualizarGrilla();
            SetEstadoCargaCodigo();
        }

        // --- BUSQUEDA Y LISTADO DE ARTICULOS

        private void ActivarBusquedaYListadoDeArticulo()
        {
            var fBusquedaSeleccionArticulos = new FormBusquedaSeleccion(setDatosBusquedaArticulos, setFormatoBusquedaArticulo);
            fBusquedaSeleccionArticulos.ShowDialog();

            if (!fBusquedaSeleccionArticulos.RealizoSeleccion)
                return;

            txtCodigo.Text = ((ArticuloDto)fBusquedaSeleccionArticulos.Seleccion).Codigo.ToString();
        }

        private void setFormatoBusquedaArticulo(DataGridView grilla)
        {
            grilla.Columns["Codigo"].Visible = true;
            grilla.Columns["Codigo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            grilla.Columns["Codigo"].HeaderText = @"Código";
            grilla.Columns["Codigo"].DisplayIndex = 1;

            grilla.Columns["Descripcion"].Visible = true;
            grilla.Columns["Descripcion"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            grilla.Columns["Descripcion"].HeaderText = @"Articulo";
            grilla.Columns["Descripcion"].DisplayIndex = 2;
        }

        private void setDatosBusquedaArticulos(DataGridView grilla, string cadenaBuscar)
        {
            grilla.DataSource = _artiuloServicios.Obtener(cadenaBuscar)
                .Select(x => (ArticuloDto)x)
                .OrderByDescending(x => x.Descripcion)
                .ToList();
        }

        // --- EVENTO DE BOTONES

        private void btnBuscarProveedor_Click(object sender, EventArgs e)
        {
            var fBusquedaProveedor = new ProveedorLookUp();
            fBusquedaProveedor.ShowDialog();

            if (!fBusquedaProveedor.RealizoSeleccion)
                return;

            compra.Proveedor = (ProveedorDto)fBusquedaProveedor.EntidadSeleccionada;
            CargarDatosProveedor();
        }

        private void btnGuardarCompra_Click(object sender, EventArgs e)
        {
            if (compra.Items.Count < 1)
            {
                Mjs.Alerta($@"No se puede guardar el comprobante.{Environment.NewLine} No tiene items agregados.");
                return;
            }

            CompraDto nuevaCompra = ExtraerDatosDeCompra();

            _compraServicios.Insertar(nuevaCompra);

            RealizarPagoDeLaCompra();

            Mjs.Info("Comprobante guardado correctamente.");

            SetearControles();
        }

        private CompraDto ExtraerDatosDeCompra()
        {
            // Extraer datos
            compra.ImpuestosInternos = nudImpuestoInterno.Value;
            compra.Iva105 = nudIva105.Value;
            compra.Iva21 = nudIva21.Value;
            compra.Iva27 = nudIva27.Value;
            compra.RetencionIB = nudPercepcionIB.Value;
            compra.RetencionIva = nudPercepcionIva.Value;
            compra.RetencionPyP = nudPercepcionPyP.Value;
            compra.RetencionTEM = nudPercepcionTem.Value;
            compra.Fecha = dtpFecha.Value;

            // Guardar los datos
            return new CompraDto()
            {
                ProveedorId = compra.Proveedor.Id,
                EmpleadoId = Identidad.EmpleadoId,
                UsuarioId = Identidad.UsuarioId,
                Fecha = compra.Fecha,
                Descuento = 0m,
                Iva21 = compra.Iva21,
                Iva105 = compra.Iva105,
                Iva27 = compra.Iva27,
                ImpuestosInternos = compra.ImpuestosInternos,
                PrecepcionIB = compra.RetencionIB,
                PrecepcionIva = compra.RetencionIva,
                PrecepcionPyP = compra.RetencionPyP,
                PrecepcionTem = compra.RetencionTEM,
                TipoComprobante = compra.Tipo,
                Items = compra.Items.Select(iv => new DetalleComprobanteDto()
                {
                    ArticuloId = iv.ProductoId,
                    Codigo = iv.Codigo,
                    Descripcion = iv.Descripcion,
                    Cantidad = iv.Cantidad,
                    Iva = 0m,
                    Precio = iv.Precio
                }).ToList()
            };
        }

        private void RealizarPagoDeLaCompra()
        {
            var totalCompra = compra.Items.Sum(x => x.SubTotal)
                    + nudImpuestoInterno.Value
                    + nudIva105.Value
                    + nudIva21.Value
                    + nudIva27.Value
                    + nudPercepcionIB.Value
                    + nudPercepcionIva.Value
                    + nudPercepcionPyP.Value
                    + nudPercepcionTem.Value;

            // Pago en efectivo
            if (chkEfectivo.Checked)
                _compraServicios.InsertarDetalleCaja(new CajaDetalleDto()
                {
                    CajaId = cajaActivaId,
                    TipoPago = TipoPago.Efectivo,
                    TipoMovimiento = TipoMovimiento.Egreso,
                    Monto = totalCompra
                });

            // Pago en cuenta corriente
            else
                _compraServicios.InsertarMovimientoCuentaCorriente(new MovimientoCuentaCorrienteProveedorDto()
                {
                    ProveedorId = compra.Proveedor.Id,
                    CajaId = cajaActivaId,
                    Fecha = compra.Fecha,
                    Descripcion = "Compra en cuenta corriente",
                    TipoMovimiento = TipoMovimiento.Ingreso,
                    Monto = totalCompra
                });
        }

    }
}
