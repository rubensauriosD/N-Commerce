using System.Windows.Forms;
using IServicio.Caja;
using PresentacionBase.Formularios;

namespace Presentacion.Core.Caja
{
    public partial class _00043_Gastos : FormConsulta
    {
        private readonly IGastoServicio _gastoServicio;

        public _00043_Gastos(IGastoServicio gastoServicio)
        {
            InitializeComponent();

            _gastoServicio = gastoServicio;
        }

        public override void ActualizarDatos(DataGridView dgv, string cadenaBuscar)
        {
            dgv.DataSource = _gastoServicio.Obtener(cadenaBuscar);

            base.ActualizarDatos(dgv, cadenaBuscar);
        }

        public override void FormatearGrilla(DataGridView dgv)
        {
            base.FormatearGrilla(dgv);

            dgv.Columns["Descripcion"].Visible = true;
            dgv.Columns["Descripcion"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["Descripcion"].HeaderText = "Gasto";
            dgv.Columns["Descripcion"].DisplayIndex = 1;

            dgv.Columns["Monto"].Visible = true;
            dgv.Columns["Monto"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["Monto"].HeaderText = "Monto";
            dgv.Columns["Monto"].DisplayIndex = 2;

            dgv.Columns["EliminadoStr"].Visible = true;
            dgv.Columns["EliminadoStr"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["EliminadoStr"].HeaderText = "Eliminado";
            dgv.Columns["EliminadoStr"].DisplayIndex = 3;
        }

        public override bool EjecutarComando(TipoOperacion tipoOperacion, long? id = null)
        {
            var form = new _00044_Abm_Gastos(tipoOperacion, id);
            form.ShowDialog();
            return form.RealizoAlgunaOperacion;
        }
    }
}
