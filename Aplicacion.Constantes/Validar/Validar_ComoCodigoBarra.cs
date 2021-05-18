namespace Aplicacion.Constantes
{
    using System;
    using System.Linq;
    using System.Windows.Forms;

    public partial class Validar
    {
        public void ComoCodigoBarra(Control control, bool obligatorio = false)
        {
            var errorProvider = new ErrorProvider();

            if (control is TextBox)
                ConfigurarProiedadesCodigoBarra(control as TextBox);

            Validador validador = (string txt, out string errMjs) =>
            {
                errMjs = "Código barra debe una cadena númerica de 100 dígitos como máximo.";

                bool ok = txt.All(c => char.IsDigit(c) || char.IsWhiteSpace(c) || c == '\b');

                ok &= txt.Length <= 100;

                if (obligatorio)
                {
                    errMjs += " El campo es obligatorio.";
                    ok &= obligatorio ? txt != "" : true;
                }

                return ok;
            };

            Validador validadorLetra = (string txt, out string errMjs) =>
            {
                errMjs = "Código barra debe una cadena númerica de 100 dígitos como máximo.";

                bool ok = txt.All(c => char.IsDigit(c) || char.IsWhiteSpace(c) || c == '\b');

                if (obligatorio)
                {
                    errMjs += " El campo es obligatorio.";
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
                    control.Select();
                    errorProvider.SetError(control, errorMsg);
                }
            };

            control.Validated += (object sender, EventArgs e)
                => errorProvider.SetError((Control)sender, "");
        }

        private void ConfigurarProiedadesCodigoBarra(TextBox textBox)
        {
            textBox.MaxLength = 100;
            textBox.TextAlign = HorizontalAlignment.Left;
        }
    }
}
