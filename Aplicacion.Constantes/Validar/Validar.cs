namespace Aplicacion.Constantes
{
    using System;

    public partial class Validar : IDisposable
    {
        public delegate bool Validador(string txt, out string errorMsg);

        public void Dispose()
        {
            Dispose();
        }

    }
}