namespace Presentacion.Core.Proveedor
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
    using Presentacion.Core.Cliente;

    public partial class _00036_ProveedorCtaCte : FormBase
    {
        private readonly IProveedorServicio _proveedorServicios;

        private ProveedorDto proveedor;

        private MovimientoCuentaCorrienteProveedorDto movimientoCuentaCorriente;

        public _00036_ProveedorCtaCte(long proveedorId)
        {
            InitializeComponent();

            _proveedorServicios = ObjectFactory.GetInstance<IProveedorServicio>();

            proveedor = (ProveedorDto)_proveedorServicios.Obtener(proveedorId);
            movimientoCuentaCorriente = new MovimientoCuentaCorrienteProveedorDto();

            SetearControles();

            CargarDatos();
        }

        private void SetearControles()
        {
            dgvGrilla.DataSource = new List<MovimientoCuentaCorrienteProveedorDto>();

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
            lblCuit.Text = proveedor.CUIT;
            lblRazonSocial.Text = proveedor.RazonSocial;

            ActualizarGrilla();
        }

        private void ActualizarGrilla()
        {
            var lstMovimientosCuentaCorriente = _proveedorServicios.ObtenerMovimientosCuentaCorriente(proveedor.Id)
                .OrderByDescending(x => x.Fecha)
                .ToList();

            dgvGrilla.DataSource = lstMovimientosCuentaCorriente;
            lblSaldoCuentaCorriente.Text = lstMovimientosCuentaCorriente
                .Sum(x => x.Monto * (x.TipoMovimiento == TipoMovimiento.Ingreso ? 1 : -1))
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

            var fMontoPago = new _00035_PagoCuentaCorriente(proveedor.SaldoCuentaCorriente);
            fMontoPago.ShowDialog();

            if (!fMontoPago.RealizoOperacion)
                return;

            var pago = new MovimientoCuentaCorrienteProveedorDto() {
                CajaId = cajaActiva.Id,
                ProveedorId = proveedor.Id,
                Monto = fMontoPago.MontoPago,
                TipoMovimiento = TipoMovimiento.Ingreso,
                Descripcion = "Pago en cuenta corriente",
            };

            if (!_proveedorServicios.AgregarPagoCuentaCorriente(pago))
                Mjs.Alerta($@"No se pudo ingresar el pago.");

            ActualizarGrilla();
        }

        private void btnRebertirPago_Click(object sender, EventArgs e)
        {
            var cajaActiva = ObjectFactory.GetInstance<ICajaServicio>().ObtenerCajaAciva(Identidad.UsuarioId);

            if (cajaActiva == null)
            {
                Mjs.Alerta($@"No hay una caja abierta.{Environment.NewLine}Por favor abra una caja para poder revertir el pago.");
                return;
            }

            movimientoCuentaCorriente.CajaId = cajaActiva.Id;

            if (!_proveedorServicios.RevertirPagoCuentaCorriente(movimientoCuentaCorriente))
                Mjs.Alerta($@"No se pudo revertir el pago.");

            ActualizarGrilla();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            ActualizarGrilla();
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {

        }

        private void dgvGrilla_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvGrilla.RowCount < 1)
                return;

            movimientoCuentaCorriente = (MovimientoCuentaCorrienteProveedorDto)dgvGrilla.Rows[e.RowIndex].DataBoundItem;
        }

    }
}
