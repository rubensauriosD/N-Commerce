namespace Presentacion.Core.Articulo
{
    using System;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;
    using Aplicacion.Constantes;
    using IServicio.Articulo;
    using IServicio.Articulo.DTOs;
    using IServicio.Iva;
    using IServicio.Iva.DTOs;
    using IServicio.Marca;
    using IServicio.Marca.DTOs;
    using IServicio.Rubro;
    using IServicio.Rubro.DTOs;
    using IServicio.UnidadMedida;
    using IServicio.UnidadMedida.DTOs;
    using PresentacionBase.Formularios;
    using StructureMap;

    public partial class _00018_Abm_Articulo : FormAbm
    {
        private readonly IArticuloServicio _articuloServicio;
        private readonly IMarcaServicio _marcaServicio;
        private readonly IRubroServicio _rubroServicio;
        private readonly IUnidadMedidaServicio _unidadMedidaServicio;
        private readonly IIvaServicio _ivaServicio;
        private readonly Validar Validar;
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
            Validar = new Validar();
        }

        private void _00018_Abm_Articulo_Load(object sender, System.EventArgs e)
        {
            Validar.ComoCodigoBarra(txtcodigoBarra);

            Validar.ComoAlfanumerico(txtDescripcion, true);
            Validar.ComoNumero(txtCodigo, true);
            Validar.ComoAlfanumerico(txtAbreviatura);
            txtAbreviatura.MaxLength = 10;
            Validar.ComoAlfanumerico(txtUbicacion);
            Validar.ComoAlfanumerico(txtDetalle);
            Validar.ComoPrecio(nudPrecioCosto,true);

            CargarComboMarca();
            CargarComboRubro();
            CargarComboUnidadMedida();
            CargarComboIva();

            imgFoto.Image = ImagenProductoPorDefecto;

            if (EntidadId.HasValue)
                CargarDatos();
        }

        private void CargarDatos()
        {
            groupPrecio.Enabled = false;
            nudStock.Enabled = false;

            var resultado = (ArticuloDto)_articuloServicio.Obtener(EntidadId.Value);

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

            CargarComboMarca(resultado.MarcaId);
            CargarComboRubro(resultado.RubroId);
            CargarComboUnidadMedida(resultado.UnidadMedidaId);
            CargarComboIva(resultado.IvaId);

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

        private void CargarComboIva(long ivaId = 0)
        {
            var lstIva = _ivaServicio.Obtener(string.Empty, false)
                .Select(x => (IvaDto)x)
                .ToList();

            if (ivaId != 0)
            {
                var iva = (IvaDto)_ivaServicio.Obtener(ivaId);
                lstIva.Add(iva);
            }

            PoblarComboBox(cmbIva, lstIva, "Descripcion", "Id");
        }

        private void CargarComboUnidadMedida(long unidadMedidaId = 0)
        {
            var lstUnidadMedida = _unidadMedidaServicio.Obtener(string.Empty, false)
                .Select(x => (UnidadMedidaDto)x)
                .ToList();

            if (unidadMedidaId != 0)
            {
                var unidadMedida = (UnidadMedidaDto)_unidadMedidaServicio.Obtener(unidadMedidaId);
                lstUnidadMedida.Add(unidadMedida);
            }

            PoblarComboBox(cmbUnidad, lstUnidadMedida, "Descripcion", "Id");
        }

        private void CargarComboRubro(long rubroId = 0)
        {
            var lstRubro = _rubroServicio.Obtener(string.Empty, false)
                .Select(x => (RubroDto)x)
                .ToList();

            if (rubroId != 0)
            {
                var rubro = (RubroDto)_rubroServicio.Obtener(rubroId);
                lstRubro.Add(rubro);
            }

            PoblarComboBox(cmbRubro, lstRubro, "Descripcion", "Id");
        }

        private void CargarComboMarca(long marcaId = 0)
        {
            var lstMarca = _marcaServicio.Obtener(string.Empty, false)
                .Select(x => (MarcaDto)x)
                .ToList();

            if (marcaId != 0)
            {
                var marca = (MarcaDto)_marcaServicio.Obtener(marcaId);
                lstMarca.Add(marca);
            }

            PoblarComboBox(cmbMarca, lstMarca, "Descripcion", "Id");
        }

        public override bool VerificarDatosObligatorios()
        {
            bool ok = ValidateChildren();

            if (cmbMarca.Items.Count < 1)
            {
                Validar.SetErrorProvider(cmbMarca, "Debes seleccionar una marca.");
                ok = false;
            }
            else
                Validar.ClearErrorProvider(cmbMarca);


            if (cmbRubro.Items.Count < 1)
            {
                Validar.SetErrorProvider(cmbRubro, "Debes seleccionar un rubro.");
                ok = false;
            }
            else
                Validar.ClearErrorProvider(cmbRubro);


            if (cmbUnidad.Items.Count < 1)
            {
                Validar.SetErrorProvider(cmbUnidad, "Debes seleccionar una unidad.");
                ok = false;
            }
            else
                Validar.ClearErrorProvider(cmbUnidad);


            if (cmbIva.Items.Count < 1)
            {
                Validar.SetErrorProvider(cmbIva, "Debes seleccionar una alicuota iva.");
                ok = false;
            }
            else
                Validar.ClearErrorProvider(cmbIva);

            return ok;
        }

        public override bool VerificarSiExiste(long? id = null)
        {
            bool ok = false;

            int.TryParse(txtCodigo.Text, out int nuevoCodigo);

            if (!string.IsNullOrEmpty(txtCodigo.Text))
            {
                bool existeCodigo = TipoOperacion == TipoOperacion.Modificar && EntidadId.HasValue
                    ? _articuloServicio.VerificarSiExisteCodigo(nuevoCodigo, EntidadId)
                    : _articuloServicio.VerificarSiExisteCodigo(nuevoCodigo);

                if (existeCodigo)
                {
                    Validar.SetErrorProvider(txtCodigo, "El código pertenece a otro artículo.");
                    tabDatosArticulo.SelectedIndex = 0;
                    ok = true;
                }
            }
            else
                Validar.ClearErrorProvider(txtCodigo);


            if (!string.IsNullOrEmpty(txtcodigoBarra.Text))
            {
                bool existeCodigo = TipoOperacion == TipoOperacion.Modificar && EntidadId.HasValue
                    ? _articuloServicio.VerificarSiExisteCodigoBarra(txtcodigoBarra.Text, EntidadId)
                    : _articuloServicio.VerificarSiExisteCodigoBarra(txtcodigoBarra.Text);

                if (existeCodigo)
                {
                    Validar.SetErrorProvider(txtcodigoBarra, "El código de barra pertenece a otro artículo.");
                    tabDatosArticulo.SelectedIndex = 0;
                    ok = true;
                }
            }
            else
                Validar.ClearErrorProvider(txtcodigoBarra);


            if (!string.IsNullOrEmpty(txtDescripcion.Text))
            {
                bool existeCodigo = TipoOperacion == TipoOperacion.Modificar && EntidadId.HasValue
                    ? _articuloServicio.VerificarSiExiste(txtDescripcion.Text, id)
                    : _articuloServicio.VerificarSiExisteCodigoBarra(txtDescripcion.Text);

                if (existeCodigo)
                {
                    Validar.SetErrorProvider(txtDescripcion, "La descripción pertenece a otro artículo.");
                    tabDatosArticulo.SelectedIndex = 0;
                    ok = true;
                }
            }
            else
                Validar.ClearErrorProvider(txtDescripcion);


            return ok;
        }

        // --- Sobrescritura de comportamiento de botones
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

        // --- Eventos de Botones
        private void btnNuevaMarca_Click(object sender, EventArgs e)
        {
            var fNuevaMarca = new _00022_Abm_Marca(TipoOperacion.Nuevo);
            fNuevaMarca.ShowDialog();
            if (fNuevaMarca.RealizoAlgunaOperacion)
            {
                PoblarComboBox(cmbMarca, _marcaServicio.Obtener(string.Empty, false), "Descipcion", "Id");
            }
        }
        
        private void btnNuevoRubro_Click(object sender, EventArgs e)
        {
            var fNuevoRubro = new _00020_Abm_Rubro(TipoOperacion.Nuevo);
            fNuevoRubro.ShowDialog();
            if (fNuevoRubro.RealizoAlgunaOperacion)
            {
                PoblarComboBox(cmbRubro, _rubroServicio.Obtener(string.Empty, false), "Descipcion", "Id");
            }
        }
        
        private void btnNuevoIva_Click(object sender, EventArgs e)
        {
            var fNuevaIva = new _00026_Abm_Iva(TipoOperacion.Nuevo);
            fNuevaIva.ShowDialog();

            if (fNuevaIva.RealizoAlgunaOperacion)
            {
                PoblarComboBox(cmbIva, _ivaServicio.Obtener(string.Empty, false), "Descipcion","Id");
            }
        }
        
        private void btnNuevaUnidad_Click(object sender, EventArgs e)
        {
            var fNuevaUnidad = new _00024_Abm_UnidadDeMedida(TipoOperacion.Nuevo);
            fNuevaUnidad.ShowDialog();

            if (fNuevaUnidad.RealizoAlgunaOperacion)
            {
                PoblarComboBox(cmbUnidad, _unidadMedidaServicio.Obtener(string.Empty, false), "Descipcion", "Id");
            }
        }

        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            if (openFile.ShowDialog() != DialogResult.OK || !openFile.CheckFileExists)
                return;

            try
            {
                imgFoto.Image = Image.FromFile(openFile.FileName);
            }
            catch (System.Exception)
            {
                Mjs.Error("La imagen seleccionada no es válida.");
            }
        }

        private void chkActivarHoraVenta_CheckedChanged(object sender, EventArgs e)
        {
            dtpHoraDesde.Enabled = chkActivarHoraVenta.Checked;
            dtpHoraHasta.Enabled = chkActivarHoraVenta.Checked;
        }

        private void chkActivarLimite_CheckedChanged(object sender, EventArgs e)
        {
            nudLimiteVenta.Enabled = chkActivarLimite.Checked;
        }

    }
}
