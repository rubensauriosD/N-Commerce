namespace Presentacion.Core.FormaPago
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Aplicacion.Constantes;
    using IServicio.FormaPago;
    using IServicio.Persona;
    using IServicio.Persona.DTOs;
    using IServicios.FormaPago.DTOs;
    using Presentacion.Core.Cliente;
    using PresentacionBase.Formularios;
    using StructureMap;

    public partial class _00044_FormaPago : FormBase
    {
        private readonly IBancoServicio bancoServicio;
        private readonly ITarjetaServicio tarjetaServicio;
        private readonly IClienteServicio clienteServicio;

        private ClienteDto Cliente;
        public List<FormaPagoDto> FormasPago { get; private set; }
        public bool RealizoVenta { get; private set; } = false;
        private decimal ClienteSaldoCuentaCorriente;
        private decimal TotalAPagar { get; }
        private decimal PagoEnEfectivo =>
            TotalAPagar
            - nudMontoCtaCte.Value
            - nudMontoCheque.Value
            - nudMontoTarjeta.Value;

        // Facturas Pendientes
        public _00044_FormaPago(decimal totalAPagar, long clienteId)
        {
            InitializeComponent();

            bancoServicio = ObjectFactory.GetInstance<IBancoServicio>();
            tarjetaServicio = ObjectFactory.GetInstance<ITarjetaServicio>();
            clienteServicio = ObjectFactory.GetInstance<IClienteServicio>();

            TotalAPagar = totalAPagar;
            FormasPago = new List<FormaPagoDto>();
            Cliente = (ClienteDto)clienteServicio.Obtener(typeof(ClienteDto), clienteId);
            ClienteSaldoCuentaCorriente = clienteServicio.SaldoCuentaCorriente(Cliente.Id);

            SetearControles();
            CargarDatos();
        }

        private void SetearControles()
        {
            PoblarComboBox(
                cmbBanco,
                (List<BancoDto>)bancoServicio.Obtener(string.Empty, false),
                "Descripcion", "Id"
                );

            PoblarComboBox(
                cmbTarjeta,
                (List<TarjetaDto>)tarjetaServicio.Obtener(string.Empty, false),
                "Descripcion", "Id"
                );

            nudMontoCheque.Maximum = TotalAPagar;
            nudMontoTarjeta.Maximum = TotalAPagar;
            nudMontoCtaCte.Maximum = TotalAPagar;
        }

        private void CargarDatos()
        {
            lblTotalAPagar.Text = TotalAPagar.ToString("C2");
            lblTotalEfectivo.Text = TotalAPagar.ToString("C2");

            nudMontoCheque.Value = 0;
            txtNumeroCheque.Clear();
            dtpFechaVencimientoCheque.Value = DateTime.Now;

            CargarDatosCuentaCorriente();

            nudMontoTarjeta.Value = 0;
            txtNumeroTarjeta.Clear();
            txtCuponPago.Clear();
            nudCantidadCuotas.Value = 1;
        }

        private void CargarDatosCuentaCorriente()
        {
            nudMontoCtaCte.Value = 0;
            nudMontoCtaCte.Enabled = true;

            if (Cliente.ActivarCtaCte && Cliente.Dni != "99999999")
            {
                txtApellido.Text = Cliente.Apellido;
                txtNombre.Text = Cliente.Nombre;
                txtDni.Text = Cliente.Dni;
                
                lblSaldoCuentaCorriente.Text = ClienteSaldoCuentaCorriente.ToString("C2");
                lblLimiteCuentaCorriente.Text = 
                    Cliente.TieneLimiteCompra
                    ? Cliente.MontoMaximoCtaCteStr
                    : "---";

                nudMontoCtaCte.Maximum = Cliente.TieneLimiteCompra
                    ? (Cliente.MontoMaximoCtaCte - ClienteSaldoCuentaCorriente)
                    : 99999999;

                nudMontoCtaCte.Focus();
                nudMontoCtaCte.Select(0, nudMontoCtaCte.Value.ToString().Length);

                return;
            }

            nudMontoCtaCte.Enabled = false;
            txtApellido.Text = string.Empty;
            txtNombre.Text = string.Empty;
            txtDni.Text = string.Empty;
            lblSaldoCuentaCorriente.Text = "---";
            lblLimiteCuentaCorriente.Text = "---";
        }

        private void nudMontoTarjeta_ValueChanged(object sender, EventArgs e)
        {
            if ((nudMontoCheque.Value + nudMontoCtaCte.Value + nudMontoTarjeta.Value) > TotalAPagar)
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
            if ((nudMontoCheque.Value + nudMontoCtaCte.Value + nudMontoTarjeta.Value) > TotalAPagar)
            {
                Mjs.Error("Los pagos superan el monto a pagar.");
                nudMontoCheque.Value = PagoEnEfectivo  < 0 ? 0 : PagoEnEfectivo;
                ActualizarMontoEfectivo();
                return;
            }
            lblTotalCheque.Text = nudMontoCheque.Value.ToString("C2");
            ActualizarMontoEfectivo();
        }

        private void nudMontoCtaCte_ValueChanged(object sender, EventArgs e)
        {
            if ((nudMontoCheque.Value + nudMontoCtaCte.Value + nudMontoTarjeta.Value) > TotalAPagar)
            {
                Mjs.Error("Los pagos superan el monto a pagar.");
                nudMontoCtaCte.Value = PagoEnEfectivo < 0 ? 0 : PagoEnEfectivo;
                ActualizarMontoEfectivo();
                return;
            }

            bool MontoPagoCtaCteOk = Cliente.TieneLimiteCompra 
                ? (ClienteSaldoCuentaCorriente + nudMontoCtaCte.Value) < Cliente.MontoMaximoCtaCte
                : true;

            if (!MontoPagoCtaCteOk)
            {
                Mjs.Alerta("El momnto en cuenta corriente supera el límite establecido para el cliente");
                nudMontoCtaCte.Maximum = Cliente.MontoMaximoCtaCte;
                nudMontoCtaCte.Value = Cliente.MontoMaximoCtaCte;
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

        private void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            var fLookUpCliente = ObjectFactory.GetInstance<ClienteLookUp>();
            fLookUpCliente.ShowDialog();

            if (fLookUpCliente.EntidadSeleccionada == null)
                return;

            Cliente = (ClienteDto)fLookUpCliente.EntidadSeleccionada;

            if (!Cliente.ActivarCtaCte || Cliente.Dni == "99999999")
                Mjs.Alerta("El cliente seleccionado no tiene activa la cuenta corriente.");

            CargarDatosCuentaCorriente();
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
                        ClienteId = Cliente.Id,
                        FechaVencimiento = dtpFechaVencimientoCheque.Value,
                        Monto = nudMontoCheque.Value,
                        Numero = txtNumeroCheque.Text,
                        Eliminado = false
                    });

                if (nudMontoCtaCte.Value > 0)
                    FormasPago.Add(new FormaPagoCtaCteDto
                    {
                        TipoPago = TipoPago.CtaCte,
                        ClienteId = Cliente.Id,
                        Monto = nudMontoCtaCte.Value,
                        Eliminado = false,
                    });

                RealizoVenta = true;
                Close();
            }
            catch (Exception ex)
            {
                RealizoVenta = false;
                Mjs.Error(ex.Message);
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            RealizoVenta = false;
            Close();
        }

        private void nudPagaCon_ValueChanged(object sender, EventArgs e)
        {
            CalcularVuelto();
        }

        private void CalcularVuelto()
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

            if(fBanco.RealizoAlgunaOperacion)
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
