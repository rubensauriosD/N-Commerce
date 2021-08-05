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
    using IServicios.Informes.DTOs;
    using Microsoft.Reporting.WinForms;

    public partial class _00034_ClienteCtaCte : FormBase
    {
        private readonly IClienteServicio _clienteServicios;

        private ClienteDto cliente;
        private MovimientoCuentaCorrienteClienteDto movimientoCuentaCorriente;
        private List<MovimientoCuentaCorrienteClienteDto> lstMovimientosCuentaCorriente;
        private bool _aplicarFiltros;

        public _00034_ClienteCtaCte(long clienteId)
        {
            InitializeComponent();

            _clienteServicios = ObjectFactory.GetInstance<IClienteServicio>();
            _aplicarFiltros = false;

            cliente = (ClienteDto)_clienteServicios.Obtener(typeof(ClienteDto),clienteId);
            movimientoCuentaCorriente = new MovimientoCuentaCorrienteClienteDto();
        }


        private void _00034_ClienteCtaCte_Load(object sender, EventArgs e)
        {
            CargarDatos();
        }

        private void FormatearGrilla()
        {
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

        private void ActualizarGrilla()
        {
            lstMovimientosCuentaCorriente = _clienteServicios.ObtenerMovimientosCuentaCorriente(cliente.Id)
                .OrderByDescending(x => x.Fecha)
                .ToList();

            // Filtros
            dtpDesde.MaxDate = lstMovimientosCuentaCorriente.Max(x => x.Fecha);
            dtpHasta.MaxDate = lstMovimientosCuentaCorriente.Max(x => x.Fecha);

            dtpDesde.MinDate = lstMovimientosCuentaCorriente.Min(x => x.Fecha);
            dtpHasta.MinDate = lstMovimientosCuentaCorriente.Min(x => x.Fecha);

            bool puedoAplicarElFilrto = dtpDesde.Value <= dtpHasta.Value;

            if (_aplicarFiltros && puedoAplicarElFilrto)
            {
                dgvGrilla.DataSource = lstMovimientosCuentaCorriente
                    .Where(x => dtpDesde.Value.Date <= x.Fecha.Date && x.Fecha.Date <= dtpHasta.Value.Date)
                    .ToList();

                _aplicarFiltros = false;
            }
            else
                dgvGrilla.DataSource = lstMovimientosCuentaCorriente;

            FormatearGrilla();

            lblSaldoCuentaCorriente.Text = lstMovimientosCuentaCorriente
                .Sum(x => x.Monto * (x.TipoMovimiento == TipoMovimiento.Ingreso ? 1 : -1))
                .ToString("C2");
        }

        // ACCIONES DE CONTROLES
        private void btnRealizarPago_Click(object sender, EventArgs e)
        {
            var cajaActivaId = ObjectFactory.GetInstance<ICajaServicio>().ObtenerIdCajaAciva(Identidad.UsuarioId);

            if (cajaActivaId == null)
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
                CajaId = (long)cajaActivaId,
                ClienteId = cliente.Id,
                Monto = fMontoPago.MontoPago,
                TipoMovimiento = TipoMovimiento.Egreso,
                Descripcion = "Pago en cuenta corriente",
            };

            if (!_clienteServicios.AgregarPagoCuentaCorriente(pago))
                Mjs.Alerta($@"No se pudo ingresar el pago.");

            ActualizarGrilla();
        }

        private void btnCancelarPago_Click(object sender, EventArgs e)
        {
            if (movimientoCuentaCorriente.TipoMovimiento == TipoMovimiento.Ingreso)
            {
                Mjs.Info("No se puede revertir el movimiento seleccionado.");
                return;
            }

            var cajaActivaId = ObjectFactory.GetInstance<ICajaServicio>().ObtenerIdCajaAciva(Identidad.UsuarioId);

            if (cajaActivaId == null)
            {
                Mjs.Alerta($@"No hay una caja abierta.{Environment.NewLine}Por favor abra una caja para poder realizar la reversión.");
                return;
            }

            if (!Mjs.Preguntar($@"Está por revertir un pago.{Environment.NewLine}¿Seguro que desea continuar?"))
                return;

            var pago = new MovimientoCuentaCorrienteClienteDto()
            {
                CajaId = (long)cajaActivaId,
                ClienteId = cliente.Id,
                Monto = movimientoCuentaCorriente.Monto,
                TipoMovimiento = TipoMovimiento.Ingreso,
                Descripcion = $@"Reversión pago: {movimientoCuentaCorriente.Descripcion}",
            };

            if (!_clienteServicios.RevertirPagoCuentaCorriente(pago))
                Mjs.Alerta($@"No se pudo revertir el pago.");

            ActualizarGrilla();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            ActualizarGrilla();
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            var lstMovimientosInforme = lstMovimientosCuentaCorriente
                .Select(x => new InformeMovimientoCuentaCorrienteDto()
                {
                    Fecha = x.Fecha.ToShortDateString(),
                    Descripcion = x.Descripcion,
                    Ingreso = x.TipoMovimiento == TipoMovimiento.Ingreso ? x.Monto.ToString("C2") : "",
                    Egreso = x.TipoMovimiento == TipoMovimiento.Egreso ? x.Monto.ToString("C2") : "",
                })
                .ToList();

            var saldoCuentaCorriente = lstMovimientosCuentaCorriente
                .Sum(x => x.Monto * (x.TipoMovimiento == TipoMovimiento.Ingreso ? 1 : -1))
                .ToString("C2");

            var parametros = new List<ReportParameter>() {
                new ReportParameter("nombreSujetoCuentaCorriente", cliente.ApyNom.ToUpper()),
                new ReportParameter("saldoCuentaCorriente", saldoCuentaCorriente)
            };

            var form = new FormBase();
            form.MostrarInforme(
                @"D:\Code\N-Commerce\Presentacion.Core\Informes\InformeMovimientoCuentaCorriente.rdlc",
                @"MovimientoCuentaCorriente",
                lstMovimientosInforme,
                parametros);

            form.ShowDialog();
        }

        private void dgvGrilla_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvGrilla.RowCount < 1)
                return;

            movimientoCuentaCorriente = (MovimientoCuentaCorrienteClienteDto)dgvGrilla.Rows[e.RowIndex].DataBoundItem;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            bool puedoAplicarElFilrto = dtpDesde.Value <= dtpHasta.Value;

            if (!puedoAplicarElFilrto)
            {
                Mjs.Alerta("Intervalo de tiempo no permitido.");
                return;
            }

            _aplicarFiltros = true;
            ActualizarGrilla();
        }
    }
}
