using System.Windows.Forms;
using IServicio.ListaPrecio;
using PresentacionBase.Formularios;

namespace Presentacion.Core.Articulo
{
    public partial class _00032_ListaPrecio : FormConsulta
    {
        private readonly IListaPrecioServicio _marcaServicio;

        public _00032_ListaPrecio(IListaPrecioServicio marcaServicio)
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
            dgv.Columns["Descripcion"].HeaderText = "ListaPrecio";
            dgv.Columns["Descripcion"].DisplayIndex = 1;

            dgv.Columns["AutorizacionStr"].Visible = true;
            dgv.Columns["AutorizacionStr"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["AutorizacionStr"].HeaderText = "Necesita Autorización";
            dgv.Columns["AutorizacionStr"].DisplayIndex = 2;

            dgv.Columns["PorcentajeGananciaStr"].Visible = true;
            dgv.Columns["PorcentajeGananciaStr"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["PorcentajeGananciaStr"].HeaderText = "Porcentaje Ganancia";
            dgv.Columns["PorcentajeGananciaStr"].DisplayIndex = 3;

            dgv.Columns["EliminadoStr"].Visible = true;
            dgv.Columns["EliminadoStr"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["EliminadoStr"].HeaderText = "Eliminado";
            dgv.Columns["EliminadoStr"].DisplayIndex = 4;
        }

        public override bool EjecutarComando(TipoOperacion tipoOperacion, long? id = null)
        {
            var form = new _00033_Abm_ListaPrecio(tipoOperacion, id);
            form.ShowDialog();
            return form.RealizoAlgunaOperacion;
        }
    }
}
