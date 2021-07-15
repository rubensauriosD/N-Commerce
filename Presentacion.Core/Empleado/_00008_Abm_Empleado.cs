namespace Presentacion.Core.Empleado
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;
    using Aplicacion.Constantes;
    using IServicio.Departamento;
    using IServicio.Departamento.DTOs;
    using IServicio.Localidad;
    using IServicio.Localidad.DTOs;
    using IServicio.Persona;
    using IServicio.Persona.DTOs;
    using IServicio.Provincia;
    using IServicio.Provincia.DTOs;
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
        private readonly Validar Validar;

        public _00008_Abm_Empleado(TipoOperacion tipoOperacion, long? entidadId = null)
            : base(tipoOperacion, entidadId)
        {
            InitializeComponent();

            _servicio = ObjectFactory.GetInstance<IEmpleadoServicio>();
            _provinciaServicio = ObjectFactory.GetInstance<IProvinciaServicio>();
            _departamentoServicio = ObjectFactory.GetInstance<IDepartamentoServicio>();
            _localidadServicio = ObjectFactory.GetInstance<ILocalidadServicio>();
            Validar = new Validar();
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

            if (EntidadId.HasValue)
                CargarDatosEntidad();

            Validar.ComoTexto(txtApellido, true);
            Validar.ComoTexto(txtNombre, true);
            Validar.ComoDni(txtDni, true);
            Validar.ComoTelefono(txtTelefono);
            Validar.ComoDomicilio(txtDomicilio, true);
            Validar.ComoMail(txtMail);
        }

        private void CargarDatosEntidad()
        {
            var resultado = (EmpleadoDto)_servicio.Obtener(typeof(EmpleadoDto), EntidadId.Value);

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

            ActualizarProvincia(resultado.ProvinciaId);

            ActualizarDepartamento(resultado.DepartamentoId);

            ActualizarLocalidad(resultado.LocalidadId);
        }

        private void ActualizarProvincia(long id = 0)
        {
            var lstProvincias = _provinciaServicio.Obtener(string.Empty, false)
                .Select(x => (ProvinciaDto)x)
                .OrderBy(x => x.Descripcion)
                .ToList();

            PoblarComboBox(
                cmbProvincia,
                lstProvincias,
                "Descripcion",
                "Id"
                );

            if(id != 0)
                cmbProvincia.SelectedValue = id;

            ActualizarDepartamento();
        }

        private void ActualizarDepartamento(long id = 0)
        {
            if (cmbProvincia.Items.Count < 1)
            {
                cmbDepartamento.DataSource = new List<DepartamentoDto>();
                return;
            } 

            var lstDepartamentos = _departamentoServicio.Obtener(string.Empty, false)
                .Select(x => (DepartamentoDto)x)
                .OrderBy(x => x.Descripcion)
                .ToList();

            PoblarComboBox(
                cmbDepartamento,
                lstDepartamentos,
                "Descripcion",
                "Id"
                );

            if (id != 0)
                cmbDepartamento.SelectedValue = id;

            ActualizarLocalidad();
        }

        private void ActualizarLocalidad(long id = 0)
        {
            if (cmbDepartamento.Items.Count < 1)
            {
                cmbLocalidad.DataSource = new List<LocalidadDto>();
                return;
            }

            var lstLocalidades = _localidadServicio.Obtener(string.Empty, false)
                .Select(x => (LocalidadDto)x)
                .OrderBy(x => x.Descripcion)
                .ToList();

            PoblarComboBox(
                cmbLocalidad,
                lstLocalidades,
                "Descripcion",
                "Id"
                );

            if (id != 0)
                cmbLocalidad.SelectedValue = id;
        }

        public override bool VerificarDatosObligatorios()
        {
            if (cmbProvincia.Items.Count <= 0)
            {
                Validar.SetErrorProvider(cmbProvincia, "Obligatorio");
                return false;
            }
            else
                Validar.ClearErrorProvider(cmbProvincia);

            if (cmbDepartamento.Items.Count <= 0)
            {
                Validar.SetErrorProvider(cmbDepartamento, "Obligatorio");
                return false;
            }
            else
                Validar.ClearErrorProvider(cmbDepartamento);

            if (cmbLocalidad.Items.Count <= 0)
            {
                Validar.SetErrorProvider(cmbLocalidad, "Obligatorio");
                return false;
            }
            else
                Validar.ClearErrorProvider(cmbLocalidad);

            return ValidateChildren();
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
            if (cmbProvincia.Items.Count < 1)
            {
                cmbDepartamento.DataSource = new List<DepartamentoDto>();
                cmbLocalidad.DataSource = new List<LocalidadDto>();
                return;
            }

            PoblarComboBox(
                cmbDepartamento,
                _departamentoServicio.ObtenerPorProvincia((long)cmbProvincia.SelectedValue),
                "Descripcion",
                "Id");

            if (cmbDepartamento.Items.Count < 1)
            {
                cmbLocalidad.DataSource = new List<LocalidadDto>();
                return;
            }

            PoblarComboBox(
                cmbLocalidad,
                _localidadServicio.ObtenerPorDepartamento((long)cmbDepartamento.SelectedValue),
                "Descripcion",
                "Id");
        }

        private void cmbDepartamento_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            if (cmbDepartamento.Items.Count < 1)
            {
                cmbLocalidad.DataSource = new List<LocalidadDto>();
                return;
            }

            PoblarComboBox(
                cmbLocalidad,
                _localidadServicio.ObtenerPorDepartamento((long)cmbDepartamento.SelectedValue),
                "Descripcion",
                "Id");
        }

        private void btnNuevaProvincia_Click(object sender, System.EventArgs e)
        {
            var form = new _00002_Abm_Provincia(TipoOperacion.Nuevo);
            form.ShowDialog();

            if (!form.RealizoAlgunaOperacion)
                return;

            var idProvinciaCreada = _provinciaServicio.Obtener(string.Empty, false).Max(x => x.Id);

            ActualizarProvincia(idProvinciaCreada);
        }

        private void btnNuevoDepartamento_Click(object sender, System.EventArgs e)
        {
            var form = new _00004_Abm_Departamento(TipoOperacion.Nuevo);
            form.ShowDialog();

            if (!form.RealizoAlgunaOperacion)
                return;

            var idDepartamentoCreada = _departamentoServicio.Obtener(string.Empty, false).Max(x => x.Id);
            ActualizarDepartamento(idDepartamentoCreada);
        }

        private void btnNuevaLocalidad_Click(object sender, System.EventArgs e)
        {
            var form = new _00006_AbmLocalidad(TipoOperacion.Nuevo);
            form.ShowDialog();

            if (!form.RealizoAlgunaOperacion)
                return;

            var idLocalidadCreada = _localidadServicio.Obtener(string.Empty, false).Max(x => x.Id);
            ActualizarLocalidad(idLocalidadCreada);
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
