namespace Presentacion.Core.Articulo
{
    using System.Windows.Forms;
    using Aplicacion.Constantes;
    using IServicio.Deposito;
    using PresentacionBase.Formularios;

    public partial class _00050_Deposito : FormConsulta
    {
        private readonly IDepositoSevicio _depositoServicio;

        public _00050_Deposito(IDepositoSevicio depositoServicio)
        {
            InitializeComponent();

            _depositoServicio = depositoServicio;
        }

        public override void ActualizarDatos(DataGridView dgv, string cadenaBuscar)
        {
            dgv.DataSource = _depositoServicio.Obtener(cadenaBuscar);

            base.ActualizarDatos(dgv, cadenaBuscar);
        }

        public override void FormatearGrilla(DataGridView dgv)
        {
            base.FormatearGrilla(dgv);


            dgv.Columns["Descripcion"].Visible = true;
            dgv.Columns["Descripcion"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["Descripcion"].HeaderText = "Depósito";
            dgv.Columns["Descripcion"].DisplayIndex = 1;

            dgv.Columns["Ubicacion"].Visible = true;
            dgv.Columns["Ubicacion"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["Ubicacion"].HeaderText = "Ubicación";
            dgv.Columns["Ubicacion"].DisplayIndex = 2;

            dgv.Columns["EliminadoStr"].Visible = true;
            dgv.Columns["EliminadoStr"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["EliminadoStr"].HeaderText = "Eliminado";
            dgv.Columns["EliminadoStr"].DisplayIndex = 3;
        }

        public override bool EjecutarComando(TipoOperacion tipoOperacion, long? id = null)
        {
            if (id != null && id == 1)
            {
                Mjs.Info("No se puede eliminar este depósito.");
                return false;
            }

            if (tipoOperacion == TipoOperacion.Eliminar
                && id != null
                && _depositoServicio.TieneStokDeArticulos(id))
            {
                Mjs.Alerta("No se puede eliminar depósitos con stoks");
                return false;
            }

            var form = new _00051_Abm_Deposito(tipoOperacion, id);
            form.ShowDialog();
            return form.RealizoAlgunaOperacion;
        }
    }
}
