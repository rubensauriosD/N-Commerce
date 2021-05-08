namespace Presentacion.Core.FormaPago
{
    using System.Windows.Forms;
    using IServicio.FormaPago;
    using PresentacionBase.Formularios;

    public partial class _00047_Banco : FormConsulta
    {
        private readonly IBancoServicio _bancoServicio;

        public _00047_Banco(IBancoServicio bancoServicio)
        {
            InitializeComponent();

            _bancoServicio = bancoServicio;
        }

        public override void ActualizarDatos(DataGridView dgv, string cadenaBuscar)
        {
            dgv.DataSource = _bancoServicio.Obtener(cadenaBuscar);

            base.ActualizarDatos(dgv, cadenaBuscar);
        }

        public override void FormatearGrilla(DataGridView dgv)
        {
            base.FormatearGrilla(dgv);

            dgv.Columns["Descripcion"].Visible = true;
            dgv.Columns["Descripcion"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["Descripcion"].HeaderText = "Banco";
            dgv.Columns["Descripcion"].DisplayIndex = 1;

            dgv.Columns["EliminadoStr"].Visible = true;
            dgv.Columns["EliminadoStr"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["EliminadoStr"].HeaderText = "Eliminado";
            dgv.Columns["EliminadoStr"].DisplayIndex = 2;
        }

        public override bool EjecutarComando(TipoOperacion tipoOperacion, long? id = null)
        {
            var form = new _00048_Abm_Banco(tipoOperacion, id);
            form.ShowDialog();
            return form.RealizoAlgunaOperacion;
        }
    }
}
