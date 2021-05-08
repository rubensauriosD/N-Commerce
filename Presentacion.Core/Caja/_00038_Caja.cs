using System;
using System.Linq;
using System.Windows.Forms;
using Aplicacion.Constantes;
using IServicios.Caja;
using IServicios.Caja.DTOs;
using PresentacionBase.Formularios;
using StructureMap;

namespace Presentacion.Core.Caja
{
    public partial class _00038_Caja : FormBase
    {
        private readonly ICajaServicio servicio;
        private CajaDto cajaDto;

        public _00038_Caja(ICajaServicio servicio)
        {
            InitializeComponent();

            this.servicio = servicio;
            cajaDto = new CajaDto();
        }

        private void _00038_Caja_Load(object sender, EventArgs e)
        {
            dtpFechaDesde.Enabled = chkFiltraFecha.Checked;
            dtpFechaHasta.Enabled = chkFiltraFecha.Checked;
            dtpFechaDesde.Value = DateTime.Now;
            dtpFechaHasta.Value = DateTime.Now;
            dtpFechaHasta.MaxDate = DateTime.Now;

            ActualizarDatos(string.Empty);
        }

        public override void FormatearGrilla(DataGridView dgv)
        {
            base.FormatearGrilla(dgv);

            dgv.Columns["UsuarioApertura"].Visible = true;
            dgv.Columns["UsuarioApertura"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["UsuarioApertura"].HeaderText = "Usr. Apert.";
            dgv.Columns["UsuarioApertura"].DisplayIndex = 1;

            dgv.Columns["FechaAperturaStr"].Visible = true;
            dgv.Columns["FechaAperturaStr"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["FechaAperturaStr"].HeaderText = "F. Apert.";
            dgv.Columns["FechaAperturaStr"].DisplayIndex = 2;

            dgv.Columns["MontoAperturaStr"].Visible = true;
            dgv.Columns["MontoAperturaStr"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["MontoAperturaStr"].HeaderText = "Monto Apert.";
            dgv.Columns["MontoAperturaStr"].DisplayIndex = 3;

            // ====

            dgv.Columns["UsuarioCierre"].Visible = true;
            dgv.Columns["UsuarioCierre"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["UsuarioCierre"].HeaderText = "Usr. Cierre";
            dgv.Columns["UsuarioCierre"].DisplayIndex = 4;

            dgv.Columns["FechaCierreStr"].Visible = true;
            dgv.Columns["FechaCierreStr"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["FechaCierreStr"].HeaderText = "F. Cierre";
            dgv.Columns["FechaCierreStr"].DisplayIndex = 5;

            dgv.Columns["MontoCierreStr"].Visible = true;
            dgv.Columns["MontoCierreStr"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["MontoCierreStr"].HeaderText = "Monto Cierre";
            dgv.Columns["MontoCierreStr"].DisplayIndex = 6;

            // ====

            dgv.Columns["TotalIngresosStr"].Visible = true;
            dgv.Columns["TotalIngresosStr"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["TotalIngresosStr"].HeaderText = "Ingresos";
            dgv.Columns["TotalIngresosStr"].DisplayIndex = 7;

            dgv.Columns["TotalComprasStr"].Visible = true;
            dgv.Columns["TotalComprasStr"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["TotalComprasStr"].HeaderText = "Comrpas";
            dgv.Columns["TotalComprasStr"].DisplayIndex = 8;

            dgv.Columns["TotalGastosStr"].Visible = true;
            dgv.Columns["TotalGastosStr"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["TotalGastosStr"].HeaderText = "Gastos";
            dgv.Columns["TotalGastosStr"].DisplayIndex = 9;

        }

        private void ActualizarDatos(string txtBuscar, bool buscarFecha = false, DateTime? desde = null, DateTime? hasta = null)
        {
            dgvGrilla.DataSource = servicio.Obtener(txtBuscar, buscarFecha, desde ?? DateTime.Now, hasta ?? DateTime.Now);

            FormatearGrilla(dgvGrilla);
        }

        private void dgvGrilla_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            cajaDto = (CajaDto)dgvGrilla.Rows[e.RowIndex].DataBoundItem;
        }

        // Filtrar por fecha
        private void txtBuscar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Enter)
                return;

            if (chkFiltraFecha.Checked)
                ActualizarDatos(txtBuscar.Text, chkFiltraFecha.Checked, dtpFechaDesde.Value, dtpFechaHasta.Value);

            else if (txtBuscar.Text.Length > 2)
                ActualizarDatos(txtBuscar.Text);

            e.Handled = true;
        }

        private void chkFiltraFecha_CheckedChanged(object sender, EventArgs e)
        {
            dtpFechaDesde.Enabled = chkFiltraFecha.Checked;
            dtpFechaHasta.Enabled = chkFiltraFecha.Checked;
        }

        private void dtpFechaDesde_ValueChanged(object sender, EventArgs e)
        {
            dtpFechaHasta.MinDate = dtpFechaDesde.Value;
            dtpFechaHasta.Value = dtpFechaDesde.Value;
        }

        private void dtpFechaHasta_ValueChanged(object sender, EventArgs e)
        {
            dtpFechaDesde.MaxDate = dtpFechaHasta.Value;
        }

        private void dtpFechaHasta_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            ActualizarDatos(txtBuscar.Text, chkFiltraFecha.Checked, dtpFechaDesde.Value, dtpFechaHasta.Value);
            e.Handled = true;
        }

        // Acciones de botones
        private void btnAbrirCaja_Click(object sender, EventArgs e)
        {
            // Verificar que la caja no este abierta
            if (servicio.VerificarSiExisteCajaAbierta(Identidad.UsuarioId))
            {
                Mjs.Alerta("El ussuario ya tiene una caja abierta");
                return;
            }

            var fAbrirCaja = ObjectFactory.GetInstance<_00039_AperturaCaja>();
            fAbrirCaja.ShowDialog();

            ActualizarDatos(string.Empty);
        }

        private void btnCierreCaja_Click(object sender, EventArgs e)
        {
            // Verificar que la caja no este abierta
            if (!servicio.VerificarSiExisteCajaAbierta(Identidad.UsuarioId))
            {
                Mjs.Alerta("No hay una caja para cerrar.");
                return;
            }

            var fCerrarCaja = new _00040_CierreCaja(cajaDto);
            fCerrarCaja.ShowDialog();

            ActualizarDatos(string.Empty);
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            ActualizarDatos(txtBuscar.Text, chkFiltraFecha.Checked, dtpFechaDesde.Value, dtpFechaHasta.Value);
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
