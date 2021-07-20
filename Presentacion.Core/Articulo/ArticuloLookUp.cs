namespace Presentacion.Core.Articulo
{
    using System.Linq;
    using System.Windows.Forms;
    using PresentacionBase.Formularios;
    using IServicio.Articulo.DTOs;
    using IServicio.Articulo;

    public partial class ArticuloLookUp : FormLookUp
    {
        private readonly IArticuloServicio _servicio;

        public ArticuloLookUp(IArticuloServicio servicio)
        {
            InitializeComponent();

            _servicio = servicio;
        }

        public override void FormatearGrilla(DataGridView dgv)
        {
            base.FormatearGrilla(dgv);

            dgv.Columns["Codigo"].Visible = true;
            dgv.Columns["Codigo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["Codigo"].HeaderText = @"Código";
            dgv.Columns["Codigo"].DisplayIndex = 1;

            dgv.Columns["Descripcion"].Visible = true;
            dgv.Columns["Descripcion"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["Descripcion"].HeaderText = @"Articulo";
            dgv.Columns["Descripcion"].DisplayIndex = 2;
        }

        public override void ActualizarDatos(DataGridView dgv, string cadenaBuscar)
        {
            dgv.DataSource = _servicio.Obtener(cadenaBuscar)
                .Select(x => (ArticuloDto)x)
                .OrderBy(x => x.Descripcion)
                .ToList();

            base.ActualizarDatos(dgv, cadenaBuscar);
        }
    }
}
