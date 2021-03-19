namespace Presentacion.Core.FormaPago
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Concurrency;
    using System.Reactive.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using IServicios.Comprobante;
    using IServicios.Comprobante.DTOs;
    using IServicios.FormaPago;
    using PresentacionBase.Formularios;

    public partial class _00049_CobroDiferido : FormBase
    {
        private delegate void NoArgDelefate();
        private readonly IFacturaServicio _facturaServicio;
        private readonly IFormaPagoServicios _formaPagoServicios;
        private ComprobantePendienteDto facturaSeleccionada;

        public _00049_CobroDiferido(
            IFacturaServicio facturaServicio,
            IFormaPagoServicios formaPagoServicios)
        {
            InitializeComponent();
            _facturaServicio = facturaServicio;
            _formaPagoServicios = formaPagoServicios;
            facturaSeleccionada = new ComprobantePendienteDto();

            // Libreria para que refresque cada 5 seg la grilla
            // con las facturas que estan pendientes de pago.
            Observable.Interval(TimeSpan.FromSeconds(5))
                .ObserveOn(DispatcherScheduler.Instance)
                .Subscribe(_ => { CargarDatos(); });
        }

        private void CargarDatos()
        {
            dgvGrillaPedientePago.DataSource = null;
            dgvGrillaPedientePago.DataSource = _facturaServicio.ObtenerPendientesPago();
            FormatearGrilla(dgvGrillaPedientePago);
            CargarDatosDetalleComprobante();
        }

        public override void FormatearGrilla(DataGridView dgv)
        {
            base.FormatearGrilla(dgv);

            dgv.Columns["NumeroComprobante"].Visible = true;
            dgv.Columns["NumeroComprobante"].DisplayIndex = 1;
            dgv.Columns["NumeroComprobante"].HeaderText = "Número";
            dgv.Columns["NumeroComprobante"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["NumeroComprobante"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dgv.Columns["Cliente"].Visible = true;
            dgv.Columns["Cliente"].DisplayIndex = 2;
            dgv.Columns["Cliente"].HeaderText = "Cliente";
            dgv.Columns["Cliente"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["Cliente"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dgv.Columns["MontoPagarStr"].Visible = true;
            dgv.Columns["MontoPagarStr"].DisplayIndex = 3;
            dgv.Columns["MontoPagarStr"].HeaderText = "Monto";
            dgv.Columns["MontoPagarStr"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["MontoPagarStr"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dgvGrillaPedientePago_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvGrillaPedientePago.RowCount < 1)
                return;

            facturaSeleccionada = (ComprobantePendienteDto)dgvGrillaPedientePago.Rows[e.RowIndex].DataBoundItem;

            if (facturaSeleccionada == null)
                return;

            CargarDatosDetalleComprobante();
        }

        private void CargarDatosDetalleComprobante()
        {
            dgvGrillaDetalleComprobante.DataSource = facturaSeleccionada.Items 
                ?? new List<ComprobantePendienteDetalleDto>();

            // Formatear grilla
            for (int i = 0; i < dgvGrillaDetalleComprobante.ColumnCount; i++)
                dgvGrillaDetalleComprobante.Columns[i].Visible = false;

            dgvGrillaDetalleComprobante.Columns["Descripcion"].Visible = true;
            dgvGrillaDetalleComprobante.Columns["Descripcion"].DisplayIndex = 1;
            dgvGrillaDetalleComprobante.Columns["Descripcion"].HeaderText = "Detalle";
            dgvGrillaDetalleComprobante.Columns["Descripcion"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvGrillaDetalleComprobante.Columns["Descripcion"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dgvGrillaDetalleComprobante.Columns["Cantidad"].Visible = true;
            dgvGrillaDetalleComprobante.Columns["Cantidad"].DisplayIndex = 2;
            dgvGrillaDetalleComprobante.Columns["Cantidad"].HeaderText = "Cant.";
            dgvGrillaDetalleComprobante.Columns["Cantidad"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvGrillaDetalleComprobante.Columns["Cantidad"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvGrillaDetalleComprobante.Columns["PrecioStr"].Visible = true;
            dgvGrillaDetalleComprobante.Columns["PrecioStr"].DisplayIndex = 3;
            dgvGrillaDetalleComprobante.Columns["PrecioStr"].HeaderText = "Precio";
            dgvGrillaDetalleComprobante.Columns["PrecioStr"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvGrillaDetalleComprobante.Columns["PrecioStr"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvGrillaDetalleComprobante.Columns["SubTotalStr"].Visible = true;
            dgvGrillaDetalleComprobante.Columns["SubTotalStr"].DisplayIndex = 4;
            dgvGrillaDetalleComprobante.Columns["SubTotalStr"].HeaderText = "Subtotal";
            dgvGrillaDetalleComprobante.Columns["SubTotalStr"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvGrillaDetalleComprobante.Columns["SubTotalStr"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            lblTotal.Text = facturaSeleccionada.Items.Sum(x => x.SubTotal).ToString("C2");
        }

        private void btnEjecutar_Click(object sender, EventArgs e)
        {
            if (facturaSeleccionada == null || facturaSeleccionada.ClienteId == 0)
                return;

            var fFormaPago = new _00044_FormaPago(facturaSeleccionada.MontoPagar, facturaSeleccionada.ClienteId);
            fFormaPago.ShowDialog();

            if (fFormaPago.RealizoVenta)
                _formaPagoServicios.Insertar(fFormaPago.FormasPago, facturaSeleccionada.Id);

            CargarDatos();
        }
    }
}
