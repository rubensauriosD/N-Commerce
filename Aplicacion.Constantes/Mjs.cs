namespace Aplicacion.Constantes
{
    using System.Windows.Forms;

    public static class Mjs
    {
        // ##### ERROR
        public static bool Error(string txt = @"Error inesperado.")
        {
            return MessageBox.Show(txt, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.Yes;
        }

        // ##### ALERTA
        public static bool Alerta(string txt = @"Algo inesperado ocurrió.")
        {
            return MessageBox.Show(txt, @"Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning) == DialogResult.Yes;
        }

        // ##### INFORMACION
        public static bool Info(string txt = @"Información importante.")
        {
            return MessageBox.Show(txt, @"Información", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.Yes;
        }

        // ##### PREGUNTA
        public static bool Preguntar(string txt = @"Elija una opción.")
        {
            return MessageBox.Show(txt, @"Consulta", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

    }
}
