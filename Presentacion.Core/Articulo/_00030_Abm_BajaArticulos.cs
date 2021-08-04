namespace Presentacion.Core.Articulo
{
    using StructureMap;
    using Aplicacion.Constantes;
    using PresentacionBase.Formularios;
    using IServicios.Articulo;
    using IServicios.Articulo.DTOs;
    using IServicio.Articulo;
    using System;
    using System.Linq;
    using System.Windows.Forms;
    using IServicio.Articulo.DTOs;

    public partial class _00030_Abm_BajaArticulos : FormAbm
    {
        private readonly IBajaArticuloServicio _bajaArticuloServicio;
        private readonly IMotivoBajaServicio _motivoBajaServicio;
        private readonly IArticuloServicio _articuloServicio;
        private readonly Validar Validar;

        private BajaArticuloDto bajaArticuloDto;

        public _00030_Abm_BajaArticulos(TipoOperacion tipoOperacion, long? entidadId = null)
            : base(tipoOperacion, entidadId)
        {
            InitializeComponent();

            _bajaArticuloServicio = ObjectFactory.GetInstance<IBajaArticuloServicio>();
            _motivoBajaServicio = ObjectFactory.GetInstance<IMotivoBajaServicio>();
            _articuloServicio = ObjectFactory.GetInstance<IArticuloServicio>();
            Validar = new Validar();


            bajaArticuloDto = new BajaArticuloDto();
        }

        private void _00030_Abm_BajaArticulos_Load(object sender, EventArgs e)
        {
            Validar.ComoAlfanumerico(txtObservacion, true);
            txtObservacion.MaxLength = 400;

            var lstMotivosBaja = _motivoBajaServicio.Obtener(string.Empty,false)
                .Select(x => (MotivoBajaDto)x)
                .Where(x => x.Id != 1)
                .ToList();

            PoblarComboBox(cmbMotivoBaja, lstMotivosBaja, "Descripcion", "Id");
        }

        public override bool VerificarDatosObligatorios()
        {
            bool ok = ValidateChildren();

            if (bajaArticuloDto.ArticuloId < 1)
            {
                Validar.SetErrorProvider(txtArticulo, "Seleccione un articulo.");
                ok = false;
            }
            else
                Validar.ClearErrorProvider(txtArticulo);


            if (bajaArticuloDto.MotivoBajaId < 1)
            {
                Validar.SetErrorProvider(cmbMotivoBaja, "Seleccione un motivo de baja.");
                ok = false;
            }
            else
                Validar.ClearErrorProvider(cmbMotivoBaja);

            return ok;
        }

        // --- Evento de controles
        public override void EjecutarComandoEliminar()
        {
            _bajaArticuloServicio.Eliminar((long)EntidadId);
        }

        public override void EjecutarComandoModificar()
        {
            bajaArticuloDto.Observacion = txtObservacion.Text;
            bajaArticuloDto.Cantidad = nudCantidadBaja.Value;

            _bajaArticuloServicio.Modificar(bajaArticuloDto);
        }

        public override void EjecutarComandoNuevo()
        {
            bajaArticuloDto.Observacion = txtObservacion.Text;
            bajaArticuloDto.Cantidad = nudCantidadBaja.Value;

            _bajaArticuloServicio.Insertar(bajaArticuloDto);
        }

        private void btnNuevoMotivoBaja_Click(object sender, EventArgs e)
        {
            var fNuevoMotivoBaja = new _00028_Abm_MotivoBaja(TipoOperacion.Nuevo);
            fNuevoMotivoBaja.ShowDialog();

            if (!fNuevoMotivoBaja.RealizoAlgunaOperacion)
                return;

            var lstMotivosBaja = _motivoBajaServicio.Obtener(string.Empty, false)
                .Select(x => (MotivoBajaDto)x)
                .Where(x => x.Id != 1)
                .ToList();

            PoblarComboBox(cmbMotivoBaja, lstMotivosBaja, "Descripcion", "id");

            bajaArticuloDto.MotivoBajaId = lstMotivosBaja.First(x => x.Id == lstMotivosBaja.Max(m => m.Id)).Id;
            cmbMotivoBaja.SelectedItem = bajaArticuloDto.MotivoBajaId;
        }

        private void cmbMotivoBaja_SelectedIndexChanged(object sender, EventArgs e)
        {
            bajaArticuloDto.MotivoBajaId = ((MotivoBajaDto)cmbMotivoBaja.SelectedItem).Id;
        }

        // --- Seleccion de Articulo
        private void btnBuscarArticulo_Click(object sender, EventArgs e)
        {
            var fSeleccionArticulo = new FormBusquedaSeleccion(
                ConfigurarDatosSeleccionArticulo,
                ConfigurarFormatoSeleccionArticulo
                );

            fSeleccionArticulo.ShowDialog();

            if (!fSeleccionArticulo.RealizoSeleccion)
                return;

            var articulo = (ArticuloDto)fSeleccionArticulo.Seleccion;
            bajaArticuloDto.ArticuloId = articulo.Id;
            bajaArticuloDto.Articulo = articulo.Descripcion;
            bajaArticuloDto.Stock = articulo.StockActual;
            bajaArticuloDto.Foto = articulo.Foto;

            imgFotoArticulo.Image = Imagen.ConvertirImagen(bajaArticuloDto.Foto);
            txtArticulo.Text = bajaArticuloDto.Articulo;
            nudStockActual.Maximum = bajaArticuloDto.Stock;
            nudStockActual.Minimum = bajaArticuloDto.Stock;
            nudStockActual.Value = bajaArticuloDto.Stock;
            nudCantidadBaja.Maximum = bajaArticuloDto.Stock;
        }

        private void ConfigurarFormatoSeleccionArticulo(DataGridView grilla)
        {
            grilla.Columns["Codigo"].Visible = true;
            grilla.Columns["Codigo"].DisplayIndex = 1;
            grilla.Columns["Codigo"].HeaderText = "Código";
            grilla.Columns["Codigo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            grilla.Columns["CodigoBarra"].Visible = true;
            grilla.Columns["CodigoBarra"].DisplayIndex = 2;
            grilla.Columns["CodigoBarra"].HeaderText = "Cod. Barra";
            grilla.Columns["CodigoBarra"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            grilla.Columns["Descripcion"].Visible = true;
            grilla.Columns["Descripcion"].DisplayIndex = 3;
            grilla.Columns["Descripcion"].HeaderText = "Descripción";
            grilla.Columns["Descripcion"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

        }

        private void ConfigurarDatosSeleccionArticulo(DataGridView grilla, string cadenaBuscar)
        {
            int.TryParse(cadenaBuscar, out int codigo);

            var lstArticulos = _articuloServicio.Obtener(string.Empty)
                .Select(x => (ArticuloDto)x)
                .Where(x => x.Descripcion.Contains(cadenaBuscar) 
                    || x.CodigoBarra.Contains(cadenaBuscar)
                    || x.Codigo == codigo)
                .Where(x => x.StockActual > 0)
                .ToList();

            grilla.DataSource = lstArticulos;
        }

    }
}
