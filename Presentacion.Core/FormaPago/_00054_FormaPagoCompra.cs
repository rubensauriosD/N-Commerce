namespace Presentacion.Core.FormaPago
{
    using System;
    using System.Collections.Generic;
    using Aplicacion.Constantes;
    using IServicio.FormaPago;
    using IServicio.Persona;
    using IServicio.Persona.DTOs;
    using IServicios.FormaPago.DTOs;
    using Presentacion.Core.Cliente;
    using PresentacionBase.Formularios;
    using StructureMap;

    public partial class _00054_FormaPagoCompra : FormBase
    {
        private readonly IBancoServicio bancoServicio;
        private readonly ITarjetaServicio tarjetaServicio;
        private readonly IClienteServicio clienteServicio;

        public List<FormaPagoDto> FormasPago { get; private set; }
        public bool RealizoOperaciones { get; private set; } = false;
        private decimal TotalAPagar { get; }
        private decimal PagoEnEfectivo =>
            TotalAPagar
            - nudMontoCtaCte.Value
            - nudMontoCheque.Value
            - nudMontoTarjeta.Value;

        public _00054_FormaPagoCompra(decimal totalAPagar)
        {
            InitializeComponent();

            bancoServicio = ObjectFactory.GetInstance<IBancoServicio>();
            tarjetaServicio = ObjectFactory.GetInstance<ITarjetaServicio>();
            clienteServicio = ObjectFactory.GetInstance<IClienteServicio>();

            TotalAPagar = totalAPagar;
            FormasPago = new List<FormaPagoDto>();
        }

        public void HabilitarPagoCuentaCorriente(string nombre, string cuit, decimal saldo = 0, decimal limite = 0)
        {
            tabPageCuentaCorriente.Visible = true;
            lblEtiquetaTotalCuentaCorriente.Visible = true;
            lblTotalCuentaCorriente.Visible = false;

            lblCuitProveedor.Text = cuit;
            lblNombreSujeto.Text = nombre;

            lblSaldoCuentaCorriente.Text = saldo > 0 ? saldo.ToString("C2") : "---";
            lblLimiteCuentaCorriente.Text = limite > 0 ? limite.ToString("C2") : "---";

            if (limite > 0)
                nudMontoCtaCte.Maximum = limite - saldo;
        }

        private void _00054_FormaPagoCompra_Load(object sender, EventArgs e)
        {
            lblTotalAPagar.Text = TotalAPagar.ToString("C2");
            lblTotalEfectivo.Text = TotalAPagar.ToString("C2");

            nudMontoCheque.Value = 0;
            txtNumeroCheque.Clear();
            dtpFechaVencimientoCheque.Value = DateTime.Now;

            nudMontoTarjeta.Value = 0;
            txtNumeroTarjeta.Clear();
            txtCuponPago.Clear();
            nudCantidadCuotas.Value = 1;
        }

        private void nudMontoTarjeta_ValueChanged(object sender, EventArgs e)
        {
            var totalPagos = nudMontoCheque.Value + nudMontoCtaCte.Value + nudMontoTarjeta.Value;

            if (totalPagos > TotalAPagar)
            {
                Mjs.Error("Los pagos superan el monto a pagar.");
                nudMontoTarjeta.Value = PagoEnEfectivo < 0 ? 0 : PagoEnEfectivo;
                ActualizarMontoEfectivo();
                return;
            }

            lblTotalTarjeta.Text = nudMontoTarjeta.Value.ToString("C2");
            ActualizarMontoEfectivo();
        }

        private void nudMontoCheque_ValueChanged(object sender, EventArgs e)
        {
            var totalPagos = nudMontoCheque.Value + nudMontoCtaCte.Value + nudMontoTarjeta.Value;

            if (totalPagos > TotalAPagar)
            {
                Mjs.Error("Los pagos superan el monto a pagar.");
                nudMontoCheque.Value = PagoEnEfectivo < 0 ? 0 : PagoEnEfectivo;
                ActualizarMontoEfectivo();
                return;
            }

            lblTotalCheque.Text = nudMontoCheque.Value.ToString("C2");
            ActualizarMontoEfectivo();
        }

        private void nudMontoCtaCte_ValueChanged(object sender, EventArgs e)
        {
            var totalPagos = nudMontoCheque.Value + nudMontoCtaCte.Value + nudMontoTarjeta.Value;

            if (totalPagos > TotalAPagar)
            {
                Mjs.Error("Los pagos superan el monto a pagar.");
                nudMontoCtaCte.Value = PagoEnEfectivo < 0 ? 0 : PagoEnEfectivo;
                ActualizarMontoEfectivo();
                return;
            }

            lblTotalCuentaCorriente.Text = nudMontoCtaCte.Value.ToString("C2");
            ActualizarMontoEfectivo();
        }

        private void ActualizarMontoEfectivo()
        {
            lblTotalEfectivo.Text = PagoEnEfectivo.ToString("C2");
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                if (PagoEnEfectivo > 0)
                    FormasPago.Add(new FormaPagoDto
                    {
                        TipoPago = TipoPago.Efectivo,
                        Monto = PagoEnEfectivo,
                        Eliminado = false
                    });

                if (nudMontoTarjeta.Value > 0)
                    FormasPago.Add(new FormaPagoTarjetaDto
                    {
                        TipoPago = TipoPago.Tarjeta,
                        CantidadCuotas = (int)nudCantidadCuotas.Value,
                        CuponPago = txtCuponPago.Text,
                        Monto = nudMontoTarjeta.Value,
                        NumeroTarjeta = txtNumeroTarjeta.Text,
                        TarjetaId = (long)cmbTarjeta.SelectedValue,
                        Eliminado = false
                    });

                if (nudMontoCheque.Value > 0)
                    FormasPago.Add(new FormaPagoChequeDto
                    {
                        TipoPago = TipoPago.Cheque,
                        BancoId = (long)cmbBanco.SelectedValue,
                        FechaVencimiento = dtpFechaVencimientoCheque.Value,
                        Monto = nudMontoCheque.Value,
                        Numero = txtNumeroCheque.Text,
                        Eliminado = false
                    });

                if (nudMontoCtaCte.Value > 0)
                    FormasPago.Add(new FormaPagoCtaCteDto
                    {
                        TipoPago = TipoPago.CtaCte,
                        ClienteId = 0,
                        Monto = nudMontoCtaCte.Value,
                        Eliminado = false,
                    });

                RealizoOperaciones = true;
                Close();
            }
            catch (Exception ex)
            {
                RealizoOperaciones = false;
                Mjs.Error(ex.Message);
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            RealizoOperaciones = false;
            Close();
        }

        private void nudPagaCon_ValueChanged(object sender, EventArgs e)
        {
            lblVuelto.Text = (nudPagaCon.Value - PagoEnEfectivo).ToString("C2");

            lblVuelto.ForeColor = PagoEnEfectivo < nudPagaCon.Value
                ? System.Drawing.Color.SeaGreen
                : System.Drawing.Color.Coral;
        }

        private void btnNuevaTarjeta_Click(object sender, EventArgs e)
        {
            var fBanco = new _00048_Abm_Banco(TipoOperacion.Nuevo);
            fBanco.ShowDialog();

            if (fBanco.RealizoAlgunaOperacion)
                PoblarComboBox(
                    cmbBanco,
                    bancoServicio.Obtener(string.Empty, false),
                    "Descipcion", "Id"
                    );

        }

        private void btnNuevoBanco_Click(object sender, EventArgs e)
        {
            var fTarjeta = new _00046_Abm_tarjeta(TipoOperacion.Nuevo);
            fTarjeta.ShowDialog();

            if (fTarjeta.RealizoAlgunaOperacion)

                PoblarComboBox(
                    cmbTarjeta,
                    tarjetaServicio.Obtener(string.Empty, false),
                    "Descipcion", "Id"
                    );
        }
    }
}
