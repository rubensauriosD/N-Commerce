namespace Presentacion.Core.Cliente
{
    using System;
    using System.Linq;
    using System.Windows.Forms;
    using System.Collections.Generic;
    using StructureMap;
    using Aplicacion.Constantes;
    using IServicio.Persona;
    using IServicio.Persona.DTOs;
    using IServicios.Persona.DTOs;
    using PresentacionBase.Formularios;
    using IServicios.Caja;

    public partial class _00034_ClienteCtaCte : FormBase
    {
        private readonly IClienteServicio _clienteServicios;

        private ClienteDto cliente;

        private MovimientoCuentaCorrienteClienteDto movimientoCuentaCorriente;

        public _00034_ClienteCtaCte(long clienteId)
        {
            InitializeComponent();

            _clienteServicios = ObjectFactory.GetInstance<IClienteServicio>();

            cliente = (ClienteDto)_clienteServicios.Obtener(typeof(ClienteDto),clienteId);
            movimientoCuentaCorriente = new MovimientoCuentaCorrienteClienteDto();

            SetearControles();

            CargarDatos();
        }

        private void SetearControles()
        {
            dgvGrilla.DataSource = new List<MovimientoCuentaCorrienteClienteDto>();

            // Formatear Grilla
            for (int i = 0; i < dgvGrilla.ColumnCount; i++)
                dgvGrilla.Columns[i].Visible = false;

            dgvGrilla.Columns["Fecha"].Visible = true;
            dgvGrilla.Columns["Fecha"].DisplayIndex = 0;
            dgvGrilla.Columns["Fecha"].HeaderText = "Fecha";
            dgvGrilla.Columns["Fecha"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvGrilla.Columns["Fecha"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvGrilla.Columns["Fecha"].DefaultCellStyle.Format = "d";

            dgvGrilla.Columns["Descripcion"].Visible = true;
            dgvGrilla.Columns["Descripcion"].DisplayIndex = 1;
            dgvGrilla.Columns["Descripcion"].HeaderText = "Descripción";
            dgvGrilla.Columns["Descripcion"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvGrilla.Columns["Descripcion"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dgvGrilla.Columns["MontoSignado"].Visible = true;
            dgvGrilla.Columns["MontoSignado"].DisplayIndex = 2;
            dgvGrilla.Columns["MontoSignado"].HeaderText = "Monto";
            dgvGrilla.Columns["MontoSignado"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvGrilla.Columns["MontoSignado"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvGrilla.Columns["MontoSignado"].DefaultCellStyle.Format = "c2";
        }

        private void CargarDatos()
        {
            lblCuit.Text = cliente.Dni;
            lblRazonSocial.Text = cliente.ApyNom;

            var lstMovimientosCuentaCorriente = _clienteServicios.ObtenerMovimientosCuentaCorriente(cliente.Id)
                .OrderByDescending(x => x.Fecha)
                .ToList();

            dgvGrilla.DataSource = lstMovimientosCuentaCorriente;
            lblSaldoCuentaCorriente.Text = lstMovimientosCuentaCorriente
                .Sum(x => x.Monto *(x.TipoMovimiento == TipoMovimiento.Ingreso ? 1 : -1))
                .ToString("C2");
        }

        // ACCIONES DE CONTROLES
        private void btnRealizarPago_Click(object sender, EventArgs e)
        {
            var cajaActiva = ObjectFactory.GetInstance<ICajaServicio>().ObtenerCajaAciva(Identidad.UsuarioId);

            if (cajaActiva == null)
            {
                Mjs.Alerta($@"No hay una caja abierta.{Environment.NewLine}Por favor abra una caja para poder realizar el pago.");
                return;
            }

            var saldoCuentaCorriente = _clienteServicios.ObtenerMovimientosCuentaCorriente(cliente.Id)
                .Sum(x => x.Monto *(x.TipoMovimiento == TipoMovimiento.Ingreso ? 1 : -1));

            var fMontoPago = new _00035_PagoCuentaCorriente(saldoCuentaCorriente);
            fMontoPago.ShowDialog();

            if (!fMontoPago.RealizoOperacion)
                return;

            var pago = new MovimientoCuentaCorrienteClienteDto() {
                CajaId = cajaActiva.Id,
                ClienteId = cliente.Id,
                Monto = fMontoPago.MontoPago,
                TipoMovimiento = TipoMovimiento.Ingreso,
                Descripcion = "Pago en cuenta corriente",
            };

            if (!_clienteServicios.AgregarPagoCuentaCorriente(pago))
                Mjs.Alerta($@"No se pudo ingresar el pago.");

            btnActualizar.PerformClick();
        }

        private void btnCancelarPago_Click(object sender, EventArgs e)
        {

        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {

        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {

        }

        private void dgvGrilla_RowEnter(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            if (dgvGrilla.RowCount < 1)
                return;

            movimientoCuentaCorriente = (MovimientoCuentaCorrienteClienteDto)dgvGrilla.Rows[e.RowIndex].DataBoundItem;
        }

    }
}
