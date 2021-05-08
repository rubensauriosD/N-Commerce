namespace Presentacion.Core.FormaPago
{
    using System;
    using PresentacionBase.Formularios;

    public partial class _00057_Cheque_Deposito : FormBase
    {
        public DateTime Fecha { get; private set; }
        public bool RealizoOperacion { get; private set; }

        private readonly string _nombreBanco;
        private readonly string _nombreCliente;


        public _00057_Cheque_Deposito(string nombreBanco, string nombreCliente)
        {
            InitializeComponent();

            Fecha = DateTime.Today;
            RealizoOperacion = false;
            _nombreBanco = nombreBanco;
            _nombreCliente = nombreCliente;
        }

        private void _00057_Cheque_Deposito_Load(object sender, EventArgs e)
        {
            dtpVencimiento.MaxDate = DateTime.Now;
            dtpVencimiento.Value = DateTime.Today;

            lblNombreCliente.Text = _nombreCliente;
            lblNombreBanco.Text = _nombreBanco;
        }

        private void dtpVencimiento_ValueChanged(object sender, EventArgs e)
        {
            Fecha = dtpVencimiento.Value;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            RealizoOperacion = true;
            Close();
        }
    }
}
