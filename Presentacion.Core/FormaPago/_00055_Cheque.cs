namespace Presentacion.Core.Cliente
{
    using System;
    using System.Linq;
    using System.Windows.Forms;
    using Aplicacion.Constantes;
    using IServicios.FormaPago;
    using IServicios.FormaPago.DTOs;
    using Presentacion.Core.FormaPago;
    using PresentacionBase.Formularios;

    public partial class _00055_Cheque : FormConsulta
    {
        private readonly IChequeServicio _chequeServicio;
        private ChequeDto cheque;
        private ToolStripButton btnDepositar;

        public _00055_Cheque(IChequeServicio chequeServicio)
        {
            InitializeComponent();

            _chequeServicio = chequeServicio;
            btnDepositar = new ToolStripButton();

            chkDepositados.CheckedChanged += AplicarFiltroAlListado;
            chkVencidos.CheckedChanged += AplicarFiltroAlListado;
            chkPorVencer.CheckedChanged += AplicarFiltroAlListado;
        }

        private void AplicarFiltroAlListado(object sender, EventArgs e)
        {
            btnActualizar.PerformClick();
        }

        private void _00055_Cheque_Load(object sender, EventArgs e)
        {
            btnModificar.Enabled = false;
            btnDepositar.Enabled = false;

            AgregarSeparadorAlMenu();
            
            btnDepositar.Text = "Depositar";
            btnDepositar.Image = Imagen.Pago;
            btnDepositar.Click += BtnDepositar_Click;

            AgregarBotonAlMenu(btnDepositar);
        }

        private void BtnDepositar_Click(object sender, EventArgs e)
        {
            if (dgvGrilla.RowCount < 1)
            {
                Mjs.Info("No hay elementos para depositar.");
                return;
            }

            var fDepositoCheque = new _00057_Cheque_Deposito(cheque.Banco, cheque.Cliente);
            fDepositoCheque.ShowDialog();

            if (!fDepositoCheque.RealizoOperacion)
                return;

            if (_chequeServicio.DepositarCheque(cheque.Id, fDepositoCheque.Fecha))
                Mjs.Info("Cheque depositado.");

            else
                Mjs.Alerta("Ocurrió un error al realizar el depósito.");

            btnActualizar.PerformClick();
        }

        public override void ActualizarDatos(DataGridView dgv, string cadenaBuscar)
        {
            var lstCheques = _chequeServicio.Obtener(cadenaBuscar)
                .Select(x => (ChequeDto)x)
                .ToList();

            if (!chkDepositados.Checked)
                lstCheques = lstCheques
                    .Where(x => x.Depositado == false)
                    .ToList();

            if (!chkVencidos.Checked)
                lstCheques = lstCheques
                    .Where(x => x.Vencido == false)
                    .ToList();

            if (!chkPorVencer.Checked)
                lstCheques = lstCheques
                    .Where(x => x.PorVencer == false)
                    .ToList();

            dgv.DataSource = lstCheques
                .OrderBy(x => x.Depositado)
                .ThenByDescending(x => x.FechaDeposito)
                .ThenBy(x => x.Cliente)
                .ToList();

            base.ActualizarDatos(dgv, cadenaBuscar);
        }

        public override void FormatearGrilla(DataGridView dgv)
        {
            base.FormatearGrilla(dgv);

            dgv.Columns["FechaVencimiento"].Visible = true;
            dgv.Columns["FechaVencimiento"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["FechaVencimiento"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns["FechaVencimiento"].HeaderText = "Vencimiento";
            dgv.Columns["FechaVencimiento"].DefaultCellStyle.Format = "d";
            dgv.Columns["FechaVencimiento"].DisplayIndex = 1;

            dgv.Columns["Cliente"].Visible = true;
            dgv.Columns["Cliente"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["Cliente"].HeaderText = "Cliente";
            dgv.Columns["Cliente"].DisplayIndex = 2;

            dgv.Columns["Banco"].Visible = true;
            dgv.Columns["Banco"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["Banco"].HeaderText = "Banco";
            dgv.Columns["Banco"].DisplayIndex = 3;

            dgv.Columns["DepositadoStr"].Visible = true;
            dgv.Columns["DepositadoStr"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["DepositadoStr"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns["DepositadoStr"].HeaderText = "Dep.";
            dgv.Columns["DepositadoStr"].DisplayIndex = 4;

            dgv.Columns["FechaDepositoStr"].Visible = true;
            dgv.Columns["FechaDepositoStr"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["FechaDepositoStr"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns["FechaDepositoStr"].HeaderText = "Fecha Dep.";
            dgv.Columns["FechaDepositoStr"].DisplayIndex = 5;
        }

        public override void dgvGrilla_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            base.dgvGrilla_RowEnter(sender, e);

            cheque = (ChequeDto)EntidadSeleccionada;

            if (cheque.Depositado)
            {
                btnModificar.Enabled = false;
                btnDepositar.Enabled = false;
            }
            else {
                btnModificar.Enabled = true;
                btnDepositar.Enabled = true;
            }

        }

        public override bool EjecutarComando(TipoOperacion tipoOperacion, long? id = null)
        {
            var form = new _00056_Cheque_Abm(tipoOperacion, id);
            form.ShowDialog();
            return form.RealizoAlgunaOperacion;
        }
    }
}
