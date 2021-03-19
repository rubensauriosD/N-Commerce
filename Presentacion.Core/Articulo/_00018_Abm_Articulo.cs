namespace Presentacion.Core.Articulo
{
    using System.Drawing;
    using System.Windows.Forms;
    using Aplicacion.Constantes;
    using IServicio.Articulo;
    using IServicio.Articulo.DTOs;
    using IServicio.Iva;
    using IServicio.Marca;
    using IServicio.Rubro;
    using IServicio.UnidadMedida;
    using PresentacionBase.Formularios;
    using StructureMap;

    public partial class _00018_Abm_Articulo : FormAbm
    {
        private readonly IArticuloServicio _articuloServicio;
        private readonly IMarcaServicio _marcaServicio;
        private readonly IRubroServicio _rubroServicio;
        private readonly IUnidadMedidaServicio _unidadMedidaServicio;
        private readonly IIvaServicio _ivaServicio;
        private Image ImagenProductoPorDefecto => Imagen.ImagenEmpleadoPorDefecto;

        public _00018_Abm_Articulo(TipoOperacion tipoOperacion, long? entidadId = null)
            : base(tipoOperacion, entidadId)
        {
            InitializeComponent();

            _articuloServicio = ObjectFactory.GetInstance<IArticuloServicio>();
            _marcaServicio = ObjectFactory.GetInstance<IMarcaServicio>();
            _rubroServicio = ObjectFactory.GetInstance<IRubroServicio>();
            _unidadMedidaServicio = ObjectFactory.GetInstance<IUnidadMedidaServicio>();
            _ivaServicio = ObjectFactory.GetInstance<IIvaServicio>();

            imgFoto.Image = ImagenProductoPorDefecto;

            PoblarComboBox(cmbMarca, _marcaServicio.Obtener(string.Empty, false), "Descripcion", "Id");
            PoblarComboBox(cmbRubro, _rubroServicio.Obtener(string.Empty, false), "Descripcion", "Id");
            PoblarComboBox(cmbUnidad, _unidadMedidaServicio.Obtener(string.Empty, false), "Descripcion", "Id");
            PoblarComboBox(cmbIva, _ivaServicio.Obtener(string.Empty, false), "Descripcion", "Id");
        }

        public override void CargarDatos(long? entidadId)
        {
            base.CargarDatos(entidadId);

            groupPrecio.Enabled = false;
            nudStock.Enabled = false;

            var resultado = (ArticuloDto)_articuloServicio.Obtener(entidadId.Value);

            if (resultado == null)
            {
                MessageBox.Show("Ocurrio un error al obtenerel registro seleccionado.");
                Close();
            }

            //
            // Datos del Articulo
            //
            txtCodigo.Text = resultado.Codigo.ToString();
            txtcodigoBarra.Text = resultado.CodigoBarra;
            txtDescripcion.Text = resultado.Descripcion;
            txtAbreviatura.Text = resultado.Abreviatura;
            txtDetalle.Text = resultado.Detalle;
            txtUbicacion.Text = resultado.Ubicacion;
            cmbMarca.SelectedValue = resultado.MarcaId;
            cmbRubro.SelectedValue = resultado.RubroId;
            cmbUnidad.SelectedValue = resultado.UnidadMedidaId;
            cmbIva.SelectedValue = resultado.IvaId;

            //
            // Datos Generales
            //
            nudStockMin.Value = resultado.StockMinimo;
            chkDescontarStock.Checked = resultado.DescuentaStock;
            chkPermitirStockNeg.Checked = resultado.PermiteStockNegativo;
            chkActivarLimite.Checked = resultado.ActivarLimiteVenta;
            nudLimiteVenta.Value = resultado.LimiteVenta;
            chkActivarHoraVenta.Checked = resultado.ActivarHoraVenta;
            dtpHoraDesde.Value = resultado.HoraLimiteVentaDesde;
            dtpHoraHasta.Value = resultado.HoraLimiteVentaHasta;

            //
            // Foto del Articulo
            //
            imgFoto.Image = Imagen.ConvertirImagen(resultado.Foto);
        }

        public override bool VerificarDatosObligatorios()
        {
            return !string.IsNullOrEmpty(txtCodigo.Text)
                && !string.IsNullOrEmpty(txtcodigoBarra.Text)
                && !string.IsNullOrEmpty(txtDescripcion.Text)
                && cmbMarca.Items.Count > 0
                && cmbRubro.Items.Count > 0
                && cmbUnidad.Items.Count > 0
                && cmbIva.Items.Count > 0;
        }

        public override bool VerificarSiExiste(long? id = null)
        {
            return _articuloServicio.VerificarSiExiste(txtDescripcion.Text, id);
        }

        //
        // Sobrescritura de comportamiento de botones
        //
        public override void EjecutarComandoNuevo()
        {
            var registro = new ArticuloCrudDto();

            registro.Codigo = int.Parse(txtCodigo.Text);
            registro.Descripcion = txtDescripcion.Text;
            registro.CodigoBarra = txtcodigoBarra.Text;
            registro.Abreviatura = txtAbreviatura.Text;
            registro.Detalle = txtDetalle.Text;
            registro.Ubicacion = txtUbicacion.Text;
            registro.MarcaId = (long)cmbMarca.SelectedValue;
            registro.RubroId = (long)cmbRubro.SelectedValue;
            registro.UnidadMedidaId = (long)cmbUnidad.SelectedValue;
            registro.IvaId = (long)cmbIva.SelectedValue;
            registro.PrecioCosto = nudPrecioCosto.Value;

            registro.StockActual = nudStock.Value;
            registro.StockMinimo = nudStockMin.Value;
            registro.DescuentaStock = chkDescontarStock.Checked;
            registro.PermiteStockNegativo = chkPermitirStockNeg.Checked;
            registro.ActivarLimiteVenta = chkActivarLimite.Checked;
            registro.LimiteVenta = nudLimiteVenta.Value;
            registro.ActivarHoraVenta = chkActivarHoraVenta.Checked;
            registro.HoraLimiteVentaDesde = dtpHoraDesde.Value;
            registro.HoraLimiteVentaHasta = dtpHoraHasta.Value;

            registro.Foto = Imagen.ConvertirImagen(imgFoto.Image);
            registro.Eliminado = false;

            _articuloServicio.Insertar(registro);
        }

        public override void EjecutarComandoModificar()
        {
            var registro = new ArticuloCrudDto();

            registro.Id = EntidadId.Value;
            registro.Codigo = int.Parse(txtCodigo.Text);
            registro.Descripcion = txtDescripcion.Text;
            registro.CodigoBarra = txtcodigoBarra.Text;
            registro.Abreviatura = txtAbreviatura.Text;
            registro.Detalle = txtDetalle.Text;
            registro.Ubicacion = txtUbicacion.Text;
            registro.MarcaId = (long)cmbMarca.SelectedValue;
            registro.RubroId = (long)cmbRubro.SelectedValue;
            registro.UnidadMedidaId = (long)cmbUnidad.SelectedValue;
            registro.IvaId = (long)cmbIva.SelectedValue;
            registro.PrecioCosto = nudPrecioCosto.Value;

            registro.StockActual = nudStock.Value;
            registro.StockMinimo = nudStockMin.Value;
            registro.DescuentaStock = chkDescontarStock.Checked;
            registro.PermiteStockNegativo = chkPermitirStockNeg.Checked;
            registro.ActivarLimiteVenta = chkActivarLimite.Checked;
            registro.LimiteVenta = nudLimiteVenta.Value;
            registro.ActivarHoraVenta = chkActivarHoraVenta.Checked;
            registro.HoraLimiteVentaDesde = dtpHoraDesde.Value;
            registro.HoraLimiteVentaHasta = dtpHoraHasta.Value;

            registro.Foto = Imagen.ConvertirImagen(imgFoto.Image);
            registro.Eliminado = false;

            _articuloServicio.Modificar(registro);
        }

        public override void EjecutarComandoEliminar()
        {
            _articuloServicio.Eliminar(EntidadId.Value);
        }

        public override void LimpiarControles(object obj, bool tieneValorAsociado = false)
        {
            base.LimpiarControles(obj);
            txtCodigo.Text = _articuloServicio.ObtenerSigueinteNroCodigo().ToString();
            txtcodigoBarra.Focus();
        }

        //
        // Eventos de Botones
        //
        private void btnNuevaMarca_Click(object sender, System.EventArgs e)
        {
            var fNuevaMarca = new _00022_Abm_Marca(TipoOperacion.Nuevo);
            fNuevaMarca.ShowDialog();
            if (fNuevaMarca.RealizoAlgunaOperacion)
            {
                PoblarComboBox(cmbMarca, _marcaServicio.Obtener(string.Empty, false), "Descipcion", "Id");
            }
        }
        
        private void btnNuevoRubro_Click(object sender, System.EventArgs e)
        {
            var fNuevoRubro = new _00020_Abm_Rubro(TipoOperacion.Nuevo);
            fNuevoRubro.ShowDialog();
            if (fNuevoRubro.RealizoAlgunaOperacion)
            {
                PoblarComboBox(cmbRubro, _rubroServicio.Obtener(string.Empty, false), "Descipcion", "Id");
            }
        }
        
        private void btnNuevoIva_Click(object sender, System.EventArgs e)
        {
            var fNuevaIva = new _00026_Abm_Iva(TipoOperacion.Nuevo);
            fNuevaIva.ShowDialog();

            if (fNuevaIva.RealizoAlgunaOperacion)
            {
                PoblarComboBox(cmbIva, _ivaServicio.Obtener(string.Empty, false), "Descipcion","Id");
            }
        }
        
        private void btnNuevaUnidad_Click(object sender, System.EventArgs e)
        {
            var fNuevaUnidad = new _00024_Abm_UnidadDeMedida(TipoOperacion.Nuevo);
            fNuevaUnidad.ShowDialog();

            if (fNuevaUnidad.RealizoAlgunaOperacion)
            {
                PoblarComboBox(cmbUnidad, _unidadMedidaServicio.Obtener(string.Empty, false), "Descipcion", "Id");
            }
        }

        private void btnAgregarImagen_Click(object sender, System.EventArgs e)
        {
            openFile.ShowDialog();
            imgFoto.Image = string.IsNullOrEmpty(openFile.FileName)
                ? ImagenProductoPorDefecto
                : Image.FromFile(openFile.FileName);
        }

        private void chkActivarHoraVenta_CheckedChanged(object sender, System.EventArgs e)
        {
            dtpHoraDesde.Enabled = chkActivarHoraVenta.Checked;
            dtpHoraHasta.Enabled = chkActivarHoraVenta.Checked;
        }

        private void chkActivarLimite_CheckedChanged(object sender, System.EventArgs e)
        {
            nudLimiteVenta.Enabled = chkActivarLimite.Checked;
        }
    }
}
