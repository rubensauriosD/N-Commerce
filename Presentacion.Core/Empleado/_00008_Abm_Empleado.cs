namespace Presentacion.Core.Empleado
{
    using System.Drawing;
    using System.Windows.Forms;
    using Aplicacion.Constantes;
    using IServicio.Departamento;
    using IServicio.Localidad;
    using IServicio.Persona;
    using IServicio.Persona.DTOs;
    using IServicio.Provincia;
    using Presentacion.Core.Departamento;
    using Presentacion.Core.Localidad;
    using Presentacion.Core.Provincia;
    using PresentacionBase.Formularios;
    using StructureMap;

    public partial class _00008_Abm_Empleado : FormAbm
    {
        private readonly IEmpleadoServicio _servicio;
        private readonly IProvinciaServicio _provinciaServicio;
        private readonly IDepartamentoServicio _departamentoServicio;
        private readonly ILocalidadServicio _localidadServicio;
        private readonly ICondicionIvaServicio _condicionIvaServicio;

        public _00008_Abm_Empleado(TipoOperacion tipoOperacion, long? entidadId = null)
            : base(tipoOperacion, entidadId)
        {
            InitializeComponent();

            _servicio = ObjectFactory.GetInstance<IEmpleadoServicio>();
            _provinciaServicio = ObjectFactory.GetInstance<IProvinciaServicio>();
            _departamentoServicio = ObjectFactory.GetInstance<IDepartamentoServicio>();
            _localidadServicio = ObjectFactory.GetInstance<ILocalidadServicio>();
            _condicionIvaServicio = ObjectFactory.GetInstance<ICondicionIvaServicio>();
        }

        private void _00010_Abm_Empleado_Load(object sender, System.EventArgs e)
        {
            imgFoto.Image = Imagen.ImagenEmpleadoPorDefecto;
            txtLegajo.Text = _servicio.ObtenerSiguienteLegajo().ToString("0000");

            PoblarComboBox(
                cmbProvincia,
                _provinciaServicio.Obtener(string.Empty, false),
                "Descripcion",
                "Id"
                );

            if (cmbProvincia.Items.Count > 0)
                PoblarComboBox(
                    cmbDepartamento,
                    _departamentoServicio.ObtenerPorProvincia((long)cmbProvincia.SelectedValue),
                    "Descripcion",
                    "Id");

            if (cmbDepartamento.Items.Count > 0)
                PoblarComboBox(
                    cmbLocalidad,
                    _localidadServicio.ObtenerPorDepartamento((long)cmbDepartamento.SelectedValue),
                    "Descripcion",
                    "Id");
        }

        public override void CargarDatos(long? entidadId)
        {
            base.CargarDatos(entidadId);

            var resultado = (EmpleadoDto)_servicio.Obtener(typeof(EmpleadoDto), entidadId.Value);

            if (resultado == null)
            {
                MessageBox.Show("Ocurrio un error al obtener el registro seleccionado.");
                Close();
            }

            // Cargar Datos en controles
            txtLegajo.Text = resultado.Legajo.ToString("0000");
            txtApellido.Text = resultado.Apellido;
            txtNombre.Text = resultado.Nombre;
            txtDni.Text = resultado.Dni;
            txtTelefono.Text = resultado.Telefono;
            txtDomicilio.Text = resultado.Direccion;
            txtMail.Text = resultado.Mail;
            imgFoto.Image = Imagen.ConvertirImagen(resultado.Foto);

            PoblarComboBox(cmbProvincia, _provinciaServicio.Obtener(string.Empty, false), "Descripcion", "Id");
            cmbProvincia.SelectedValue = resultado.ProvinciaId;

            PoblarComboBox(cmbDepartamento, _departamentoServicio.ObtenerPorProvincia(resultado.ProvinciaId), "Descripcion", "Id");
            cmbDepartamento.SelectedValue = resultado.DepartamentoId;

            PoblarComboBox(cmbLocalidad, _localidadServicio.ObtenerPorDepartamento(resultado.DepartamentoId), "Descripcion", "Id");
            cmbLocalidad.SelectedValue = resultado.LocalidadId;

            if (TipoOperacion == TipoOperacion.Eliminar)
                DesactivarControles(this);
        }

        public override bool VerificarDatosObligatorios()
        {
            return !string.IsNullOrEmpty(txtApellido.Text)
                && !string.IsNullOrEmpty(txtNombre.Text)
                && !string.IsNullOrEmpty(txtDni.Text);
        }

        public override bool VerificarSiExiste(long? id = null)
        {
            return _servicio.VerificarSiExisteDni(txtDni.Text, id);
        }

        // --- Modificacion de metodos
        public override void EjecutarComandoNuevo()
        {
            var nuevoRegistro = new EmpleadoDto();
            nuevoRegistro.Legajo = int.Parse(txtLegajo.Text);
            nuevoRegistro.Nombre = txtNombre.Text;
            nuevoRegistro.Apellido = txtApellido.Text;
            nuevoRegistro.Dni = txtDni.Text;
            nuevoRegistro.Telefono = txtTelefono.Text;
            nuevoRegistro.Direccion = txtDomicilio.Text;
            nuevoRegistro.Mail = txtMail.Text;
            nuevoRegistro.LocalidadId = (long)cmbLocalidad.SelectedValue;
            nuevoRegistro.Foto = Imagen.ConvertirImagen(imgFoto.Image);
            nuevoRegistro.Eliminado = false;

            _servicio.Insertar(nuevoRegistro);
        }

        public override void EjecutarComandoModificar()
        {
            var modificarRegistro = new EmpleadoDto();
            modificarRegistro.Id = EntidadId.Value;
            modificarRegistro.Legajo = int.Parse(txtLegajo.Text);
            modificarRegistro.Nombre = txtNombre.Text;
            modificarRegistro.Apellido = txtApellido.Text;
            modificarRegistro.Dni = txtDni.Text;
            modificarRegistro.Telefono = txtTelefono.Text;
            modificarRegistro.Direccion = txtDomicilio.Text;
            modificarRegistro.Mail = txtMail.Text;
            modificarRegistro.LocalidadId = (long)cmbLocalidad.SelectedValue;
            modificarRegistro.Eliminado = false;
            modificarRegistro.Foto = Imagen.ConvertirImagen(imgFoto.Image);

            _servicio.Modificar(modificarRegistro);
        }

        public override void EjecutarComandoEliminar()
        {
            _servicio.Eliminar(typeof(EmpleadoDto), EntidadId.Value);
        }

        public override void LimpiarControles(object obj, bool tieneValorAsociado = false)
        {
            base.LimpiarControles(obj);

            txtLegajo.Focus();
        }

        // --- Eventos de controles
        private void cmbProvincia_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            if (cmbProvincia.Items.Count > 0)
                PoblarComboBox(
                    cmbDepartamento,
                    _departamentoServicio.ObtenerPorProvincia((long)cmbProvincia.SelectedValue),
                    "Descripcion",
                    "Id");
        }

        private void cmbDepartamento_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            if (cmbDepartamento.Items.Count > 0)
                PoblarComboBox(
                    cmbLocalidad,
                    _localidadServicio.ObtenerPorDepartamento((long)cmbDepartamento.SelectedValue),
                    "Descripcion",
                    "Id");
        }

        private void btnNuevaProvincia_Click(object sender, System.EventArgs e)
        {
            var form = new _00002_Abm_Provincia(TipoOperacion.Nuevo);

            if (!form.RealizoAlgunaOperacion)
                return;

            PoblarComboBox(
                cmbProvincia,
                _provinciaServicio.Obtener(string.Empty, false),
                "Descripcion",
                "Id"
                );
        }

        private void btnNuevoDepartamento_Click(object sender, System.EventArgs e)
        {
            var form = new _00004_Abm_Departamento(TipoOperacion.Nuevo);

            if (!form.RealizoAlgunaOperacion)
                return;

            if (cmbProvincia.Items.Count > 0)
                PoblarComboBox(
                    cmbDepartamento,
                    _departamentoServicio.ObtenerPorProvincia((long)cmbProvincia.SelectedValue),
                    "Descripcion",
                    "Id");
        }

        private void btnNuevaLocalidad_Click(object sender, System.EventArgs e)
        {
            var form = new _00006_AbmLocalidad(TipoOperacion.Nuevo);

            if (!form.RealizoAlgunaOperacion)
                return;

            if (cmbDepartamento.Items.Count > 0)
                PoblarComboBox(
                    cmbLocalidad,
                    _localidadServicio.ObtenerPorDepartamento((long)cmbDepartamento.SelectedValue),
                    "Descripcion",
                    "Id");
        }

        private void btnImagen_Click(object sender, System.EventArgs e)
        {
            ofdBuscarFoto.Title = "Seleccionar Imagen";
            ofdBuscarFoto.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
            
            if (ofdBuscarFoto.ShowDialog() != DialogResult.OK
                || !ofdBuscarFoto.CheckFileExists)
            return;

            try
            {
                imgFoto.Image = Image.FromFile(ofdBuscarFoto.FileName);
            }
            catch (System.Exception)
            {
                Mjs.Error("La imagen seleccionada no es válida.");
            }
        }
    }
}
