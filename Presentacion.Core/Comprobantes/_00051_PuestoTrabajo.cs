using System.Windows.Forms;
using IServicio.PuestoTrabajo;
using PresentacionBase.Formularios;

namespace Presentacion.Core.Comprobantes
{
    public partial class _00051_PuestoTrabajo : FormConsulta
    {
        private readonly IPuestoTrabajoServicio _servicio;

        public _00051_PuestoTrabajo(IPuestoTrabajoServicio puestoTrabajoServicio)
        {
            InitializeComponent();

            _servicio = puestoTrabajoServicio;
        }

        public override void ActualizarDatos(DataGridView dgv, string cadenaBuscar)
        {
            dgv.DataSource = _servicio.Obtener(cadenaBuscar);

            base.ActualizarDatos(dgv, cadenaBuscar);
        }

        public override void FormatearGrilla(DataGridView dgv)
        {
            base.FormatearGrilla(dgv);

            dgv.Columns["Codigo"].Visible = true;
            dgv.Columns["Codigo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["Codigo"].HeaderText = "Código";
            dgv.Columns["Codigo"].DisplayIndex = 0;

            dgv.Columns["Descripcion"].Visible = true;
            dgv.Columns["Descripcion"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["Descripcion"].HeaderText = "Descripción";
            dgv.Columns["Descripcion"].DisplayIndex = 1;
        }

        public override bool EjecutarComando(TipoOperacion tipoOperacion, long? id = null)
        {
            var form = new _00052_Abm_PuestoTrabajo(tipoOperacion, id);
            form.ShowDialog();
            return form.RealizoAlgunaOperacion;
        }
    }
}
