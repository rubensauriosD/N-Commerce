namespace Presentacion.Core.Cliente
{
    using PresentacionBase.Formularios;

    public partial class _00035_PagoCuentaCorriente : FormBase
    {
        public bool RealizoOperacion { get; private set; }
        public decimal MontoPago { get; private set; }
        private decimal MontoAdeudado { get; set; }

        public _00035_PagoCuentaCorriente(decimal MontoAdeudado)
        {
            InitializeComponent();

            RealizoOperacion = false;
            this.MontoAdeudado = MontoAdeudado;
            MontoPago = this.MontoAdeudado;

            lblMontoAdeudado.Text = this.MontoAdeudado.ToString("C2");
            nudMontoPago.Value = this.MontoAdeudado;
            nudMontoPago.Focus();
            nudMontoPago.Select(0, nudMontoPago.Value.ToString().Length);
        }

        private void btnPagar_Click(object sender, System.EventArgs e)
        {
            MontoPago = nudMontoPago.Value;
            RealizoOperacion = true;
            Close();
        }

        private void btnLimpiar_Click(object sender, System.EventArgs e)
        {
            MontoPago = MontoAdeudado;
            lblMontoAdeudado.Text = MontoAdeudado.ToString("C2");
        }
    }
}
