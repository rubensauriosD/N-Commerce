namespace Aplicacion.Constantes
{
    using System;
    using System.Linq;
    using System.Windows.Forms;

    public partial class Validar
    {
        public void ComoDomicilio(Control control, bool obligatorio = false)
        {
            if (control is TextBox)
                ConfigurarProiedadesDomicilio(control as TextBox);

            Validador validador = (string txt, out string errMjs) =>
            {
                errMjs = "El campo debe contener hasta 120 carácteres alfanumericos.";

                bool ok = txt.All(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c == '\b');

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
                if (sender is TextBox)
                    (sender as TextBox).Text = EliminarEspaciosEnBlanco((sender as TextBox).Text.Trim());

                if (!validador(control.Text, out string errorMsg))
                {
                    e.Cancel = true;
                    errorProvider.SetError(control, errorMsg);
                }
            };

            control.Validated += (object sender, EventArgs e)
                => errorProvider.SetError((Control)sender, "");
        }

        private void ConfigurarProiedadesDomicilio(TextBox textBox)
        {
            textBox.MaxLength = 120;
            textBox.TextAlign = HorizontalAlignment.Left;
        }
    }
}
