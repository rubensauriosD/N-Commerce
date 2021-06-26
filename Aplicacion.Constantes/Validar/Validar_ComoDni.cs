namespace Aplicacion.Constantes
{
    using System;
    using System.Linq;
    using System.Windows.Forms;

    public partial class Validar
    {
        private const int DniLength = 8;

        public void ComoDni(Control control, bool obligatorio = false)
        {
            if (control is TextBox)
                ConfigurarProiedadesDni(control as TextBox);

            Validador validador = (string txt, out string errMjs) =>
            {
                errMjs = "El campo debe contener 8 carácteres numéricos.";

                bool ok = txt.All(c => char.IsDigit(c) || char.IsWhiteSpace(c) || c == '\b');

                ok &= txt.Length == DniLength;

                if (obligatorio)
                {
                    errMjs += " El campo es obligatorio.";
                    ok &= obligatorio ? txt != "" : true;
                }

                errMjs += " Ej.: 20123456";

                return ok;
            };

            control.KeyPress += (object sender, KeyPressEventArgs e) =>
            {
                if (!validador(e.KeyChar.ToString(), out string errMjs))
                    e.Handled = true;
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

        private void ConfigurarProiedadesDni(TextBox textBox)
        {
            textBox.MaxLength = DniLength;
            textBox.TextAlign = HorizontalAlignment.Right;
        }
    }
}
