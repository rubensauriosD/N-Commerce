namespace Presentacion.Core.Caja
{
    using System.Windows.Forms;
    using IServicio.Caja;
    using IServicio.Caja.DTOs;
    using IServicios.Caja;
    using PresentacionBase.Formularios;

    public partial class _00043_Gastos : FormConsulta
    {
        private readonly IGastoServicio _gastoServicio;
        private readonly ICajaServicio _cajaServicio;

        public _00043_Gastos(IGastoServicio gastoServicio, ICajaServicio cajaServicio)
        {
            InitializeComponent();

            _gastoServicio = gastoServicio;
            _cajaServicio = cajaServicio;
        }

        public override void dgvGrilla_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            base.dgvGrilla_RowEnter(sender, e);

            var gasto = EntidadSeleccionada as GastoDto;

            if (_cajaServicio.VerificarSiCajaFueCerrada(gasto.CajaId))
            { 
                btnEliminar.Enabled = false;
                btnModificar.Enabled = false;
            }
            else
            { 
                btnEliminar.Enabled = true;
                btnModificar.Enabled = true;
            }

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
