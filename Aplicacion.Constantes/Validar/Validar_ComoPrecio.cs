namespace Aplicacion.Constantes
{
    using System;
    using System.Windows.Forms;

    public partial class Validar
    {
        private readonly decimal precioMin = 1/100;
        private readonly decimal precioMax = 99999;

        public void ComoPrecio(Control control, bool obligatorio = false)
        {
            if (control is TextBox)
                ConfigurarProiedadesPrecio(control as NumericUpDown);

            Validador validador = (string txt, out string errMjs) =>
            {
                errMjs = $"El precio debe estar entre {precioMin} y {precioMax}.";

                if (!decimal.TryParse(txt, out decimal precio))
                    return false;

                bool ok = precioMin <= precio && precio <= precioMax;

                if (obligatorio)
                {
                    errMjs += " El campo es obligatorio.";
                    ok &= obligatorio ? txt != "" : true;
                }

                return ok;
            };

            control.Validating += (object sender, System.ComponentModel.CancelEventArgs e) =>
            {
                if (!validador(control.Text, out string errorMsg))
                {
                    e.Cancel = true;
                    errorProvider.SetError(control, errorMsg);
                }
            };

            control.Validated += (object sender, EventArgs e)
                => errorProvider.SetError((Control)sender, "");
        }

        private void ConfigurarProiedadesPrecio(NumericUpDown nud)
        {
            nud.TextAlign = HorizontalAlignment.Right;
            nud.DecimalPlaces = 2;
            nud.Value = precioMin;
            nud.Maximum = precioMax;
            nud.Minimum = precioMin;
        }
    }
}
