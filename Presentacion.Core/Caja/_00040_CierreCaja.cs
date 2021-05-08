namespace Presentacion.Core.Caja
{
    using System;
    using System.Linq;
    using System.Windows.Forms;
    using StructureMap;
    using IServicios.Caja;
    using IServicios.Caja.DTOs;
    using PresentacionBase.Formularios;
    using Aplicacion.Constantes;
    using IServicio.Configuracion.DTOs;
    using IServicio.Configuracion;

    public partial class _00040_CierreCaja : FormBase
    {
        private readonly ICajaServicio _cajaServicio;
        private CajaDto caja;
        private ConfiguracionDto config;
        private decimal totalArqueo;

        public _00040_CierreCaja(CajaDto caja)
        {
            InitializeComponent();

            _cajaServicio = ObjectFactory.GetInstance<ICajaServicio>();
            config = ObjectFactory.GetInstance<IConfiguracionServicio>().Obtener();
            
            this.caja = caja;
        }

        private void _00040_CierreCaja_Load(object sender, EventArgs e)
        {
            lblMontoInicial.Text = caja.MontoAperturaStr;
            lblMontoCierre.Text = caja.MontoCierreCalculadoStr;

            lblTotalIngresos.Text = caja.TotalIngresosStr;
            lblIngresoEfectivo.Text = caja.TotalIngresoEfectivoStr;
            lblIngresoCheque.Text = caja.TotalIngresoChequeStr;
            lblIngresoTarjeta.Text = caja.TotalIngresoTarjetaStr;

            lblEgresoGastos.Text = caja.TotalGastosStr;
            lblEgresoCompras.Text = caja.TotalComprasStr;
            lblEgresoEfectivo.Text = caja.TotalEgresoEfectivoStr;
            lblEgresoCheque.Text = caja.TotalEgresoChequeStr;
            lblEgresoTarjeta.Text = caja.TotalEgresoTarjetaStr;

            ActalizarTotalArqueo();
            nudArqueoEfectivo.Focus();
        }

        private void ActalizarTotalArqueo()
        {
            totalArqueo = nudArqueoCheque.Value + nudArqueoEfectivo.Value + nudArqueoTarjeta.Value;
            lblArqueoTotal.Text = totalArqueo.ToString("C2");

            if (totalArqueo == caja.MontoCierreCalculado)
                lblArqueoTotal.ForeColor = System.Drawing.Color.DarkGreen;
            else
                lblArqueoTotal.ForeColor = System.Drawing.Color.Tomato;
        }

        // Acciones de Controles
        private void btnCerrarCaja_Click(object sender, EventArgs e)
        {
            if (!config.PermitirArqueoNegativo && totalArqueo < caja.MontoCierreCalculado)
            {
                Mjs.Alerta($@"El arqueo negativo se encuentra deshabilitado.
                {Environment.NewLine}Ingrese un monto adecuado para poder continuar.");
                return;
            }

            var mjs = $@"El arqueo difiere del monto de cierre en {(totalArqueo - caja.MontoCierreCalculado).ToString("C2")}.
                {Environment.NewLine}¿Seguro que desea continuar?";

            if (totalArqueo != caja.MontoCierreCalculado && !Mjs.Preguntar(mjs))
                    return;

            _cajaServicio.Cerrar(Identidad.UsuarioId, totalArqueo);
            Close();
        }

        private void nudArqueo_ValueChanged(object sender, EventArgs e)
        {
            ActalizarTotalArqueo();
        }

        // ---
        private void btnDetalleGastos_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var fDetalleGastos = new FormBusquedaSeleccion(
                ConfiguracionDatosGrillaGastos,
                ConfiguracionFormatoGrillaGastos
                );
            fDetalleGastos.Titulo = "Gastos";

            fDetalleGastos.ShowDialog();
        }

        private void ConfiguracionDatosGrillaGastos(DataGridView dgv, string filtro)
            => dgv.DataSource = caja.Gastos.Where(x => x.Descripcion.Contains(filtro)).ToList();

        private void ConfiguracionFormatoGrillaGastos(DataGridView dgv)
        {
            dgv.Columns["Fecha"].Visible = true;
            dgv.Columns["Fecha"].DisplayIndex = 1;
            dgv.Columns["Fecha"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns["Fecha"].DefaultCellStyle.Format = "d";

            dgv.Columns["Descripcion"].Visible = true;
            dgv.Columns["Descripcion"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["Descripcion"].DisplayIndex = 2;

            dgv.Columns["Monto"].Visible = true;
            dgv.Columns["Monto"].DisplayIndex = 3;
            dgv.Columns["Monto"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["Monto"].DefaultCellStyle.Format = "C2";

        }

        // ---
        private void btnDetalleIngresos_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var fDetalleIngresos = new FormBusquedaSeleccion(
                ConfiguracionDatosGrillaIngresos,
                ConfiguracionFormatoGrillaIngresos
                );
            fDetalleIngresos.Titulo = "Ingresos";

            fDetalleIngresos.ShowDialog();
        }

        private void ConfiguracionDatosGrillaIngresos(DataGridView dgv, string filtro)
            => dgv.DataSource = caja.Detalle.Where(x => x.TipoMovimiento == Aplicacion.Constantes.TipoMovimiento.Ingreso).ToList();

        private void ConfiguracionFormatoGrillaIngresos(DataGridView dgv)
        {
            dgv.Columns["TipoPago"].Visible = true;
            dgv.Columns["TipoPago"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["TipoPago"].DisplayIndex = 1;

            dgv.Columns["Monto"].Visible = true;
            dgv.Columns["Monto"].DisplayIndex = 2;
            dgv.Columns["Monto"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["Monto"].DefaultCellStyle.Format = "C2";
        }

        // ---
        private void btnDetalleCompras_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var fDetalleCompras = new FormBusquedaSeleccion(
                ConfiguracionDatosGrillaCompras,
                ConfiguracionFormatoGrillaCompras
                );
            fDetalleCompras.Titulo = "Compras";

            fDetalleCompras.ShowDialog();
        }

        private void ConfiguracionDatosGrillaCompras(DataGridView dgv, string filtro)
            => dgv.DataSource = caja.Detalle.Where(x => x.TipoMovimiento == Aplicacion.Constantes.TipoMovimiento.Egreso).ToList();

        private void ConfiguracionFormatoGrillaCompras(DataGridView dgv)
        {
            dgv.Columns["TipoPago"].Visible = true;
            dgv.Columns["TipoPago"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["TipoPago"].DisplayIndex = 1;

            dgv.Columns["Monto"].Visible = true;
            dgv.Columns["Monto"].DisplayIndex = 2;
            dgv.Columns["Monto"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["Monto"].DefaultCellStyle.Format = "C2";
        }
    }
}
