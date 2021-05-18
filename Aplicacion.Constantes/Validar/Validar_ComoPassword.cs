namespace Aplicacion.Constantes
{
    using System;
    using System.Linq;
    using System.Windows.Forms;

    public partial class Validar
    {
        public void ComoPassword(Control control, bool obligatorio = false)
        {
            var errorProvider = new ErrorProvider();

            if (control is TextBox)
                ConfigurarProiedadesPassword(control as TextBox);

            Validador validador = (string txt, out string errMjs) =>
            {
                errMjs = "Password debe tener entre 6 y 20 caracteres alfanuméricos.";

                bool ok = txt.All(c => char.IsDigit(c) || char.IsWhiteSpace(c) || c == '\b');

                ok &= txt.Length >= 6;
                ok &= txt.Length <= 20;

                if (obligatorio)
                {
                    errMjs += " El campo es obligatorio.";
                    ok &= obligatorio ? txt != "" : true;
                }

                return ok;
            };

            Validador validadorLetra = (string txt, out string errMjs) =>
            {
                errMjs = "Password debe tener entre 6 y 12 caracteres alfanuméricos.";

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

        private void ConfigurarProiedadesPassword(TextBox textBox)
        {
            textBox.MaxLength = 20;
            textBox.UseSystemPasswordChar = true;
            textBox.TextAlign = HorizontalAlignment.Center;
        }
    }
}
