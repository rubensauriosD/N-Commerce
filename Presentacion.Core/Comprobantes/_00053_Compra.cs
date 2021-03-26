namespace Presentacion.Core.Comprobantes
{
    using System;
    using System.Linq;
    using StructureMap;
    using IServicio.Persona;
    using IServicio.Persona.DTOs;
    using Presentacion.Core.Comprobantes.Clases;
    using Presentacion.Core.Proveedor;
    using PresentacionBase.Formularios;
    using System.Windows.Forms;
    using IServicio.Articulo;
    using Aplicacion.Constantes;
    using IServicios.Articulo.DTOs;
    using IServicios.Comprobante.DTOs;
    using IServicios.Comprobante;

    public partial class _00053_Compra : FormBase
    {
        private readonly IProveedorServicio _proveedorServicios;
        private readonly IArticuloServicio _artiuloServicios;
        private readonly ICompraServicio _compraServicios;

        private ArticuloCompraDto itemSeleccionado;
        private CompraView compra;

        public _00053_Compra()
        {
            InitializeComponent();

            _proveedorServicios = ObjectFactory.GetInstance<IProveedorServicio>();
            _artiuloServicios = ObjectFactory.GetInstance<IArticuloServicio>();
            _compraServicios = ObjectFactory.GetInstance<ICompraServicio>();

        }

        private void _00053_Compra_Load(object sender, EventArgs e)
        {
            dtpFecha.MaxDate = DateTime.Now;
            PoblarComboBox(cmbTipoComprobante, Enum.GetValues(typeof(TipoComprobante)));

            SetearControles();
        }

        private void SetearControles()
        {
            compra = new CompraView();
            itemSeleccionado = new ArticuloCompraDto();

            nudCantidad.Value = 1;
            nudCantidad.Enabled = false;

            nudPrecioUnitario.Value = 0;
            nudPrecioUnitario.Enabled = false;

            dtpFecha.Value = DateTime.Today;

            cmbTipoComprobante.SelectedItem = TipoComprobante.A;
            compra.Tipo = (TipoComprobante)cmbTipoComprobante.SelectedItem;
            
            compra.Proveedor = _proveedorServicios.ObtenerPorCuit("99999999999");
            CargarDatosProveedor();

            ActualizarGrilla();

            txtCuit.Focus();
            txtCuit.SelectAll();
        }

        private void CargarDatosProveedor()
        {
            txtCuit.Text = compra.Proveedor.CUIT;
            txtNombre.Text = compra.Proveedor.RazonSocial;
            txtDomicilio.Text = compra.Proveedor.Direccion;
            txtTelefono.Text = compra.Proveedor.Telefono;
            txtCondicionIva.Text = compra.Proveedor.CondicionIva;
        }

        private void ActualizarTotal()
        {
            lblTotal.Text = (compra.Items.Sum(x => x.SubTotal)
                    +nudImpuestoInterno.Value 
                    +nudIva105.Value 
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

        // EVENTO DE COTROLES
        private void nud_Leave(object sender, EventArgs e)
        {
            ActualizarTotal();
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
            // TODO: Chequear que solo haga la busqueda si el formato es de cuit
            //if (txtCuit.TextLength != 11)
            //    return;

            var proveedor = _proveedorServicios.ObtenerPorCuit(txtCuit.Text);

            if (proveedor == null)
                return;

            compra.Proveedor = proveedor;
            CargarDatosProveedor();
        }

        private void txtCodigo_KeyPress(object sender, KeyPressEventArgs e)
        {
            string codigo = txtCodigo.Text;

            if (codigo == string.Empty || e.KeyChar != (char)Keys.Enter)
            {
                nudCantidad.Value = 1;
                nudCantidad.Enabled = false;
                nudPrecioUnitario.Value = 0;
                nudPrecioUnitario.Enabled = false;
                return;
            }

            itemSeleccionado = _artiuloServicios.ObtenerPorCodigo(txtCodigo.Text);

            if (itemSeleccionado == null)
            {
                Mjs.Error("Código no encontrado.");
                txtCodigo.Clear();
                txtCodigo.Focus();
                txtDescripcion.Text = "";
                return;
            }

            txtDescripcion.Text = itemSeleccionado.Descripcion;
            nudCantidad.Enabled = true;
            nudPrecioUnitario.Enabled = true;
            nudPrecioUnitario.Value = 0;
            nudPrecioUnitario.Select(0, 4);
        }

        // EVENTO DE BOTONES

        private void btnAgregarItem_Click(object sender, EventArgs e)
        {
            itemSeleccionado.Precio = nudPrecioUnitario.Value;
            itemSeleccionado.Cantidad = nudCantidad.Value;
            compra.AgregarItem(itemSeleccionado);

            itemSeleccionado = new ArticuloCompraDto();
            nudCantidad.Value = 1;
            nudCantidad.Enabled = false;
            nudPrecioUnitario.Value = 0;
            nudPrecioUnitario.Enabled = false;
            txtDescripcion.Text = "";
            ActualizarGrilla();
            txtCodigo.Clear();
            txtCodigo.Focus();
        }

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

            // guardar los datos
            var nuevaCompra = new CompraDto()
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

            nuevaCompra.Id = _compraServicios.Insertar(nuevaCompra);

            // TODO: Ver que hacemos con el pago de la compra

            Mjs.Info("Comprobante guardado correctamente.");
            SetearControles();
        }
    }
}
