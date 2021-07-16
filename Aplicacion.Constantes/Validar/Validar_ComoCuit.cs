namespace Aplicacion.Constantes
{
    using System;
    using System.Linq;
    using System.Windows.Forms;

    public partial class Validar
    {
        private const int CuitLength = 11;
        private string cuitErrMjs = $"El campo debe contener 11 carácteres numéricos.{Environment.NewLine}Ej.: 20123456784";

        public bool EsCuit(string txt, out string errMjs)
        {
            bool ok = txt.All(c => char.IsDigit(c) || char.IsWhiteSpace(c) || c == '\b');

            ok &= txt.Length == CuitLength;

            errMjs = cuitErrMjs;

            return ok;
        }

        public void ComoCuit(Control control, bool obligatorio = false)
        {
            if (control is TextBox)
                ConfigurarProiedadesCuit(control as TextBox);

            Validador validador = (string txt, out string errMjs) =>
            {
                errMjs = cuitErrMjs;

                bool ok = txt.All(c => char.IsDigit(c) || char.IsWhiteSpace(c) || c == '\b');

                ok &= txt.Length <= CuitLength;

                if (obligatorio)
                {
                    errMjs += $"{Environment.NewLine}El campo es obligatorio.";
                    ok &= obligatorio ? txt != "" : true;
                }

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
            textBox.MaxLength = CuitLength;
            textBox.TextAlign = HorizontalAlignment.Right;
        }
    }
}
