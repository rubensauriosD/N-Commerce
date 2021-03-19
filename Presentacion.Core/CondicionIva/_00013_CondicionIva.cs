using System.Windows.Forms;
using IServicio.Departamento;
using PresentacionBase.Formularios;

namespace Presentacion.Core.CondicionIva
{
    public partial class _00013_CondicionIva : FormConsulta
    {
        private readonly ICondicionIvaServicio _condicionIvaServicio;

        public _00013_CondicionIva(ICondicionIvaServicio condicionIvaServicio)
        {
            InitializeComponent();

            _condicionIvaServicio = condicionIvaServicio;
        }

        public override void ActualizarDatos(DataGridView dgv, string cadenaBuscar)
        {
            dgv.DataSource = _condicionIvaServicio.Obtener(cadenaBuscar);

            base.ActualizarDatos(dgv, cadenaBuscar);
        }

        public override void FormatearGrilla(DataGridView dgv)
        {
            base.FormatearGrilla(dgv);

            dgv.Columns["Descripcion"].Visible = true;
            dgv.Columns["Descripcion"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["Descripcion"].HeaderText = "Condicion Iva";
            dgv.Columns["Descripcion"].DisplayIndex = 1;

            dgv.Columns["EliminadoStr"].Visible = true;
            dgv.Columns["EliminadoStr"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["EliminadoStr"].HeaderText = "Eliminado";
            dgv.Columns["EliminadoStr"].DisplayIndex = 2;
        }

        public override bool EjecutarComando(TipoOperacion tipoOperacion, long? id = null)
        {
            var form = new _00014_Abm_CondicionIva(tipoOperacion, id);
            form.ShowDialog();
            return form.RealizoAlgunaOperacion;
        }
    }
}
