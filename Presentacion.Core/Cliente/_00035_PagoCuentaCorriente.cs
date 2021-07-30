namespace Presentacion.Core.Cliente
{
    using Aplicacion.Constantes;
    using PresentacionBase.Formularios;

    public partial class _00035_PagoCuentaCorriente : FormBase
    {
        private Validar Validar;

        public bool RealizoOperacion { get; private set; }
        public decimal MontoPago { get; private set; }
        private decimal MontoAdeudado { get; set; }

        public _00035_PagoCuentaCorriente(decimal montoAdeudado)
        {
            InitializeComponent();

            RealizoOperacion = false;
            Validar = new Validar();

            MontoAdeudado = montoAdeudado > 0 ? montoAdeudado : 0;
            MontoPago = MontoAdeudado;

            lblMontoAdeudado.Text = MontoAdeudado.ToString("C2");
            nudMontoPago.Value = MontoAdeudado;
            nudMontoPago.Focus();
            nudMontoPago.Select(0, nudMontoPago.Value.ToString().Length);
        }

        private void btnPagar_Click(object sender, System.EventArgs e)
        {
            MontoPago = nudMontoPago.Value;
            if (MontoPago == 0)
            {
                Validar.SetErrorProvider(nudMontoPago, "Ingrese un monto a pagar");
                return;
            }
            else Validar.ClearErrorProvider(nudMontoPago);

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
