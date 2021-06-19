namespace Aplicacion.Constantes
{
    using System;
    using System.Linq;
    using System.Windows.Forms;

    public partial class Validar
    {
        public void ComoCuit(Control control, bool obligatorio = false)
        {
            if (control is TextBox)
                ConfigurarProiedadesCuit(control as TextBox);

            Validador validador = (string txt, out string errMjs) =>
            {
                errMjs = "El campo debe contener 11 carácteres numéricos.";

                bool ok = txt.All(c => char.IsDigit(c) || char.IsWhiteSpace(c) || c == '\b');

                if (obligatorio)
                {
                    errMjs += " El campo es obligatorio.";
                    ok &= obligatorio ? txt != "" : true;
                }

                errMjs += " Ej.: 20123456784";

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

        private void ConfigurarProiedadesCuit(TextBox textBox)
        {
            textBox.MaxLength = 11;
            textBox.TextAlign = HorizontalAlignment.Right;
        }
    }
}
