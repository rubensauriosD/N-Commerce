namespace Aplicacion.Constantes
{
    using System;
    using System.Windows.Forms;

    public partial class Validar : IDisposable
    {
        public delegate bool Validador(string txt, out string errorMsg);

        private ErrorProvider errorProvider;

        public Validar()
        {
            errorProvider = new ErrorProvider();
        }

        public void SetErrorProvider(Control control, string txtErr)
        {
            errorProvider.SetError(control, txtErr);
        }

        public void ClearErrorProvider(Control control)
        {
            errorProvider.SetError(control, "");
        }

        public string EliminarEspaciosEnBlanco(string str)
        {
            str = str.Trim();

            while (str.Contains("  "))
                str = str.Replace("  ", " ");

            return str;
        }

        public void Dispose()
        {
            Dispose();
        }

    }
}