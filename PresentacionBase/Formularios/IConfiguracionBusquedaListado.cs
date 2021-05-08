namespace PresentacionBase.Formularios
{
    using System.Windows.Forms;

    public interface IConfiguracionBusquedaListado
    {
        string Titulo { get; set; }

        void SetDatosGrilla (DataGridView grilla, string cadenaBuscar);

        void SetFormatoGrilla (DataGridView grilla);
    }
}
