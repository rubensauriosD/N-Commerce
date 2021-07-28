namespace Aplicacion.Constantes
{
    using System;
    using System.Linq;
    using System.Windows.Forms;

    public partial class Validar
    {
        public void ComoPassword(Control control, bool obligatorio = false)
        {
            if (control is TextBox)
                ConfigurarProiedadesPassword(control as TextBox);

            Validador validador = (string txt, out string errMjs) =>
            {
                errMjs = $@"Password debe tener entre 6 y 20 caracteres alfanuméricos.{Environment.NewLine}Al menos una letra, al menos número.";

                bool ok = txt.All(c => char.IsLetterOrDigit(c) || c == '\b');

                ok &= txt.Any(c => char.IsDigit(c));
                ok &= txt.Any(c => char.IsLetter(c));

                ok &=  6 <= txt.Length && txt.Length <= 20;

                if (obligatorio)
                {
                    errMjs += " El campo es obligatorio.";
                    ok &= txt != "";
                }

                return ok;
            };

            Validador validadorLetra = (string txt, out string errMjs) =>
            {
                errMjs = $@"Password debe tener entre 6 y 20 caracteres alfanuméricos.{Environment.NewLine}Al menos una letra, al menos número.";

                bool ok = txt.All(c => char.IsLetterOrDigit(c) || c == '\b');

                return ok;
            };

            control.KeyPress += (object sender, KeyPressEventArgs e) =>
            {
                if (!validadorLetra(e.KeyChar.ToString(), out string errMjs))
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

        private void ConfigurarProiedadesPassword(TextBox textBox)
        {
            textBox.MaxLength = 20;
            textBox.UseSystemPasswordChar = true;
            textBox.TextAlign = HorizontalAlignment.Center;
        }
    }
}
