namespace CommerceApp
{
    using System;
    using System.Windows.Forms;
    using Aplicacion.Constantes;
    using Aplicacion.IoC;
    using IServicio.Configuracion;
    using Presentacion.Core.Configuracion;
    using Presentacion.Core.Usuario;
    using StructureMap;

    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Configuracion del Inyector (StructureMap)
            new StructureMapContainer().Configure();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var login = ObjectFactory.GetInstance<Login>();

            while (!login.DeseaSalirDelSistema)
            {
                login.ShowDialog();

                if (!login.PuedeAccedearAlSistema
                    || login.DeseaSalirDelSistema)
                    continue;

                if (ObjectFactory.GetInstance<IConfiguracionServicio>().Obtener() == null)
                {
                    ObjectFactory.GetInstance<_00012_Configuracion>().ShowDialog();
                    login.ResetearCampos();
                    continue;
                }

                Application.Run(new Principal());
                login.ResetearCampos();
            }
        }
    }
}
