using System.Windows.Forms;
using IServicio.FormaPago;
using PresentacionBase.Formularios;

namespace Presentacion.Core.FormaPago
{
    public partial class _00045_Tarjeta : FormConsulta
    {
        private readonly ITarjetaServicio _marcaServicio;

        public _00045_Tarjeta(ITarjetaServicio marcaServicio)
        {
            InitializeComponent();

            _marcaServicio = marcaServicio;
        }

        public override void ActualizarDatos(DataGridView dgv, string cadenaBuscar)
        {
            dgv.DataSource = _marcaServicio.Obtener(cadenaBuscar);

            base.ActualizarDatos(dgv, cadenaBuscar);
        }

        public override void FormatearGrilla(DataGridView dgv)
        {
            base.FormatearGrilla(dgv);

            dgv.Columns["Descripcion"].Visible = true;
            dgv.Columns["Descripcion"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["Descripcion"].HeaderText = "Tarjeta";
            dgv.Columns["Descripcion"].DisplayIndex = 1;

            dgv.Columns["EliminadoStr"].Visible = true;
            dgv.Columns["EliminadoStr"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["EliminadoStr"].HeaderText = "Eliminado";
            dgv.Columns["EliminadoStr"].DisplayIndex = 2;
        }

        public override bool EjecutarComando(TipoOperacion tipoOperacion, long? id = null)
        {
            var form = new _00046_Abm_tarjeta(tipoOperacion, id);
            form.ShowDialog();
            return form.RealizoAlgunaOperacion;
        }
    }
}
