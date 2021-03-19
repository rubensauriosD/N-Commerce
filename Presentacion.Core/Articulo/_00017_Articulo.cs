namespace Presentacion.Core.Articulo
{
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Aplicacion.Constantes;
    using IServicio.Articulo;
    using IServicio.Articulo.DTOs;
    using PresentacionBase.Formularios;

    public partial class _00017_Articulo : FormConsultaConDetalle
    {
        private readonly IArticuloServicio _articuloServicio;

        public _00017_Articulo(IArticuloServicio articuloServicio)
        {
            InitializeComponent();

            _articuloServicio = articuloServicio;
        }

        public override void ActualizarDatos(DataGridView dgv, string cadenaBuscar)
        {
            dgvStock.DataSource = new List<StockDepositoDto>();
            dgvPrecios.DataSource = new List<PrecioDto>();
            dgv.DataSource = _articuloServicio.Obtener(cadenaBuscar);

            base.ActualizarDatos(dgv, cadenaBuscar);
        }

        public override void FormatearGrilla(DataGridView dgv)
        {
            base.FormatearGrilla(dgv);

            FormatearGrillaStock(dgvStock);
            FormatearGrillaPrecios(dgvPrecios);

            dgv.Columns["Codigo"].Visible = true;
            dgv.Columns["Codigo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["Codigo"].HeaderText = "Código";

            dgv.Columns["CodigoBarra"].Visible = true;
            dgv.Columns["CodigoBarra"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["CodigoBarra"].HeaderText = "Código Barra";

            dgv.Columns["Descripcion"].Visible = true;
            dgv.Columns["Descripcion"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["Descripcion"].HeaderText = @"Descripción";

            dgv.Columns["EliminadoStr"].Visible = true;
            dgv.Columns["EliminadoStr"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["EliminadoStr"].HeaderText = "Eliminado";
            dgv.Columns["EliminadoStr"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        public override bool EjecutarComando(TipoOperacion tipoOperacion, long? id = null)
        {
            var form = new _00018_Abm_Articulo(tipoOperacion, id);
            form.ShowDialog();
            return form.RealizoAlgunaOperacion;
        }

        public override void dgvGrilla_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            base.dgvGrilla_RowEnter(sender, e);

            if (EntidadSeleccionada == null)
            {
                txtMarca.Clear();
                txtIva.Clear();
                txtRubro.Clear();
                txtUnidad.Clear();
                txtUbicacion.Clear();
                imgFoto.Image = null;

                dgvStock.DataSource = new List<StockDepositoDto>();
                dgvPrecios.DataSource = new List<PrecioDto>();

                return;
            }

            var articulo = (ArticuloDto)EntidadSeleccionada;

            txtMarca.Text = articulo.Marca;
            txtIva.Text = articulo.Iva;
            txtRubro.Text = articulo.Rubro;
            txtUnidad.Text = articulo.UnidadMedida;
            txtUbicacion.Text = articulo.Ubicacion;
            imgFoto.Image = Imagen.ConvertirImagen(articulo.Foto);

            // ================================================== //
            dgvStock.DataSource = articulo.Stocks;
            lblStockActual.Text = articulo.StockActual.ToString();
            // ================================================== //
            dgvPrecios.DataSource = articulo.Precios;
            // ================================================== //
        }

        private void FormatearGrillaStock(DataGridView dgv)
        {
            base.FormatearGrilla(dgv);

            dgv.Columns["Cantidad"].Visible = true;
            dgv.Columns["Cantidad"].Width = 70;
            dgv.Columns["Cantidad"].HeaderText = "Cantidad";

            dgv.Columns["Desposito"].Visible = true;
            dgv.Columns["Desposito"].HeaderText = "Deposito";
            dgv.Columns["Desposito"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void FormatearGrillaPrecios(DataGridView dgv)
        {
            base.FormatearGrilla(dgv);

            dgv.Columns["ListaPrecio"].Visible = true;
            dgv.Columns["ListaPrecio"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["ListaPrecio"].HeaderText = "Lista Precio";

            dgv.Columns["PrecioStr"].Visible = true;
            dgv.Columns["PrecioStr"].HeaderText = "Precio";
            dgv.Columns["PrecioStr"].Width = 100;

            dgv.Columns["FechaStr"].Visible = true;
            dgv.Columns["FechaStr"].HeaderText = "Fecha Act.";
            dgv.Columns["FechaStr"].Width = 70;
        }

    }
}