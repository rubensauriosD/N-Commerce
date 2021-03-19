namespace Presentacion.Core.Caja
{
    using System;
    using Aplicacion.Constantes;
    using IServicios.Caja.DTOs;
    using PresentacionBase.Formularios;

    public partial class _00040_CierreCaja : FormBase
    {
        private CajaDto caja;

        public _00040_CierreCaja(CajaDto caja)
        {
            InitializeComponent();
            this.caja = caja;
            CargarDatos();
        }

        private void CargarDatos()
        {
            lblMontoInicial.Text = caja.MontoAperturaStr;
            lblMontoCierre.Text = caja.MontoCierreStr;

            lblIngresoEfectivo.Text = caja.TotalEntradaEfectivoStr;
            lblIngresoCheque.Text = caja.TotalEntradaChequeStr;
            lblIngresoCtaCte.Text = caja.TotalEntradaCtaCteStr;
            lblIngresoTarjeta.Text = caja.TotalEntradaTarjetaStr;

            // lblEgresosCompras.Text = ;
            // lblEgresosGastos.Text = ;

            lblEgresoEfectivo.Text = caja.TotalSalidaEfectivoStr;
            lblEgresoCheque.Text = caja.TotalSalidaChequeStr;
            lblEgresoCtaCte.Text = caja.TotalSalidaCtaCteStr;
            lblEgresoTarjeta.Text = caja.TotalSalidaTarjetaStr;

        }

        // Acciones de botones
        private void btnEjecutar_Click(object sender, EventArgs e)
        {

        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarControles(this);
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnVerDetalleVenta_Click(object sender, EventArgs e)
        {
            Mjs.Info("Funcionalidad no implementada.");
        }

        private void btnVerDetalleCompra_Click(object sender, EventArgs e)
        {
            Mjs.Info("Funcionalidad no implementada.");
        }

        private void btnVerDetalleGastos_Click(object sender, EventArgs e)
        {
            Mjs.Info("Funcionalidad no implementada.");
        }
    }
}
