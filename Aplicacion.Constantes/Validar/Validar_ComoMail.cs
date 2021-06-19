namespace Aplicacion.Constantes
{
    using System;
    using System.Windows.Forms;
    using System.Text.RegularExpressions;

    public partial class Validar
    {
        public void ComoMail(Control control, bool obligatorio = false)
        {
            if (control is TextBox)
                ConfigurarProiedadesMail(control as TextBox);

            Validador validador = (string txt, out string errMjs) =>
            {
                errMjs = "El correo ingresado no tiene e formato correcto.";

                bool ok = Regex.IsMatch(
                    txt,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase,
                    TimeSpan.FromMilliseconds(250));

                ok &= txt.Length >= 6;
                ok &= txt.Length <= 250;

                if (obligatorio)
                {
                    errMjs += " El campo es obligatorio.";
                    ok &= txt != "";
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

        private void ConfigurarProiedadesMail(TextBox textBox)
        {
            textBox.MaxLength = 40;
            textBox.TextAlign = HorizontalAlignment.Left;
        }
    }
}
