namespace Presentacion.Core.Articulo
{
    using System;
    using System.Linq;
    using System.Windows.Forms;
    using StructureMap;
    using Aplicacion.Constantes;
    using IServicio.Articulo;
    using IServicio.Articulo.DTOs;
    using IServicio.Deposito;
    using IServicio.Deposito.DTOs;
    using IServicios.Deposito.DTOs;
    using PresentacionBase.Formularios;

    public partial class _00058_Deposito_Transferencia : FormBase
    {
        private readonly IArticuloServicio _articuloServicio;
        private readonly IDepositoSevicio _depositoServicio;

        private TransferenciaDepositoDto transferencia;

        public _00058_Deposito_Transferencia()
        {
            InitializeComponent();

            _articuloServicio = ObjectFactory.GetInstance<IArticuloServicio>();
            _depositoServicio = ObjectFactory.GetInstance<IDepositoSevicio>();

            transferencia = new TransferenciaDepositoDto();
        }

        private void _00058_Deposito_Transferencia_Load(object sender, EventArgs e)
        {
            lblDepositoDestino.Text = transferencia.Destino;
            lblDepositoOrigen.Text = transferencia.Origen;
            lblArticulo.Text = transferencia.Articulo;
        }

        // --- Acciones de controles
        private void nudCantidad_ValueChanged(object sender, EventArgs e)
        {
            transferencia.Cantidad = nudCantidad.Value;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!DatosDeSeleccionCorrectos())
                return;

            if (!_depositoServicio.TransferirArticulos(transferencia))
                return;

            Mjs.Info("Operación realizada con éxito.");
            Close();
        }

        private bool DatosDeSeleccionCorrectos()
        {
            bool ok = true;

            ok &= transferencia.ArticuloId != 0
                && transferencia.DestinoId != 0
                && transferencia.OrigenId != 0
                && transferencia.Cantidad > 0;

            if (!ok)
                Mjs.Alerta("Los datos seleccionados no son correctos");

            return ok;
        }

        // --- Articulo Seleccion
        private void btnArticuloSeleccion_Click(object sender, EventArgs e)
        {
            var fSeleccionArticulo = new FormBusquedaSeleccion(
                setArticuloSeleccionDatosGrilla,
                setArticuloSeleccionFormatoGrilla
                );

            fSeleccionArticulo.Titulo = "Articulos";
            fSeleccionArticulo.ShowDialog();

            if (!fSeleccionArticulo.RealizoSeleccion)
                return;

            ArticuloDto articulo = (ArticuloDto)fSeleccionArticulo.Seleccion;

            transferencia = new TransferenciaDepositoDto();

            transferencia.ArticuloId = articulo.Id;
            transferencia.Articulo = articulo.Descripcion;

            lblArticulo.Text = transferencia.Articulo;
            lblDepositoDestino.Text = "";
            lblDepositoOrigen.Text = "";
        }

        private void setArticuloSeleccionFormatoGrilla(DataGridView grilla)
        {
            grilla.Columns["Descripcion"].Visible = true;
            grilla.Columns["Descripcion"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            grilla.Columns["Descripcion"].DisplayIndex = 1;

            grilla.Columns["StockActual"].Visible = true;
            grilla.Columns["StockActual"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grilla.Columns["StockActual"].DisplayIndex = 2;
        }

        private void setArticuloSeleccionDatosGrilla(DataGridView grilla, string cadenaBuscar)
        {
            grilla.DataSource = _articuloServicio.Obtener(cadenaBuscar)
                .Select(x => (ArticuloDto)x)
                .Where(x => x.Id != transferencia.ArticuloId && x.StockActual > 0)
                .OrderBy(x => x.Descripcion)
                .ToList();
        }

        // --- Deposito Origen Seleccion
        private void btnDepositoOrigenSeleccion_Click(object sender, EventArgs e)
        {
            var fSeleccionDeposito = new FormBusquedaSeleccion(
                setDepositoOrigenSeleccionDatosGrilla,
                setDepositoSeleccionFormatoGrilla
                );

            fSeleccionDeposito.Titulo = "Depositos";
            fSeleccionDeposito.ShowDialog();

            if (!fSeleccionDeposito.RealizoSeleccion)
                return;

            DepositoDto articulo = (DepositoDto)fSeleccionDeposito.Seleccion;
            decimal stock = articulo.Stocks.First(s => s.ArticuloId == transferencia.ArticuloId).Cantidad;

            transferencia.OrigenId = articulo.Id;
            transferencia.Origen = articulo.Descripcion;
                
            lblDepositoOrigen.Text = transferencia.Origen;

            lblDepositoDestino.Text = "";
            transferencia.Destino = "";
            transferencia.DestinoId = 0;
            nudCantidad.Maximum = stock;
            nudCantidad.Value = stock;
        }

        private void setDepositoSeleccionFormatoGrilla(DataGridView grilla)
        {
            grilla.Columns["Descripcion"].Visible = true;
            grilla.Columns["Descripcion"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            grilla.Columns["Descripcion"].DisplayIndex = 1;
        }

        private void setDepositoOrigenSeleccionDatosGrilla(DataGridView grilla, string cadenaBuscar)
        {
            grilla.DataSource = _depositoServicio.Obtener(cadenaBuscar)
                .Select(x => (DepositoDto)x)
                .Where(x => x.Stocks.First(s => s.ArticuloId == transferencia.ArticuloId).Cantidad > 0)
                .OrderBy(x => x.Descripcion)
                .ToList();
        }

        // --- Deposito Destino Seleccion
        private void btnDepositoDestinoSeleccion_Click(object sender, EventArgs e)
        {
            var fSeleccionDeposito = new FormBusquedaSeleccion(
                setDepositoDestinoSeleccionDatosGrilla,
                setDepositoSeleccionFormatoGrilla
                );

            fSeleccionDeposito.Titulo = "Depositos";
            fSeleccionDeposito.ShowDialog();

            if (!fSeleccionDeposito.RealizoSeleccion)
                return;

            var articulo = (DepositoDto)fSeleccionDeposito.Seleccion;

            transferencia.DestinoId = articulo.Id;
            transferencia.Destino = articulo.Descripcion;

            lblDepositoDestino.Text = transferencia.Destino;
        }

        private void setDepositoDestinoSeleccionDatosGrilla(DataGridView grilla, string cadenaBuscar)
        {
            grilla.DataSource = _depositoServicio.Obtener(cadenaBuscar)
                .Select(x => (DepositoDto)x)
                .Where(x => x.Id != transferencia.OrigenId)
                .OrderBy(x => x.Descripcion)
                .ToList();
        }

    }
}
