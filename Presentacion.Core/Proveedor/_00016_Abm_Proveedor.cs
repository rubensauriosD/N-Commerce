namespace Presentacion.Core.Proveedor
{
    using System.Windows.Forms;
    using IServicio.Departamento;
    using IServicio.Departamento.DTOs;
    using IServicio.Localidad;
    using IServicio.Persona;
    using IServicio.Persona.DTOs;
    using IServicio.Provincia;
    using Presentacion.Core.CondicionIva;
    using Presentacion.Core.Departamento;
    using Presentacion.Core.Localidad;
    using Presentacion.Core.Provincia;
    using PresentacionBase.Formularios;
    using StructureMap;

    public partial class _00016_Abm_Proveedor : FormAbm
    {
        private readonly IProveedorServicio _servicio;
        private readonly IProvinciaServicio _provinciaServicio;
        private readonly IDepartamentoServicio _departamentoServicio;
        private readonly ILocalidadServicio _localidadServicio;
        private readonly ICondicionIvaServicio _condicionIvaServicio;

        public _00016_Abm_Proveedor(TipoOperacion tipoOperacion, long? entidadId = null)
            : base(tipoOperacion, entidadId)
        {
            InitializeComponent();

            _servicio = ObjectFactory.GetInstance<IProveedorServicio>();
            _provinciaServicio = ObjectFactory.GetInstance<IProvinciaServicio>();
            _departamentoServicio = ObjectFactory.GetInstance<IDepartamentoServicio>();
            _localidadServicio = ObjectFactory.GetInstance<ILocalidadServicio>();
            _condicionIvaServicio = ObjectFactory.GetInstance<ICondicionIvaServicio>();

            // Poblar controles
            PoblarComboBox(cmbCondicionIva, _condicionIvaServicio.Obtener(string.Empty, false), "Descripcion", "Id");
            PoblarComboBox(cmbLocalidad, _localidadServicio.Obtener(string.Empty, false), "Descripcion", "Id");
            PoblarComboBox(cmbDepartamento, _departamentoServicio.Obtener(string.Empty, false), "Descripcion", "Id");
            PoblarComboBox(cmbProvincia, _provinciaServicio.Obtener(string.Empty, false), "Descripcion", "Id");
        }

        public override void CargarDatos(long? entidadId)
        {
            if (entidadId.HasValue)
            {
                var resultado = (ProveedorDto)_servicio.Obtener(entidadId.Value);

                if (resultado == null)
                {
                    MessageBox.Show("Ocurrio un error al obtener el registro seleccionado.");
                    Close();
                }

                // Cargar Datos en controles
                var departamentoId = ((DepartamentoDto)_departamentoServicio.Obtener(resultado.LocalidadId)).Id;
                var provinciaId = _provinciaServicio.ObtenerPorDepartamento(departamentoId).Id;
                
                txtRazonSocial.Text = resultado.RazonSocial;
                txtCUIT.Text = resultado.CUIT;
                txtTelefono.Text = resultado.Telefono;
                txtDomicilio.Text = resultado.Direccion;
                txtMail.Text = resultado.Mail;
                cmbCondicionIva.SelectedValue = resultado.CondicionIvaId;
                cmbLocalidad.SelectedValue = resultado.LocalidadId;
                cmbDepartamento.SelectedValue = departamentoId;
                cmbProvincia.SelectedValue = provinciaId;

                if (TipoOperacion == TipoOperacion.Eliminar)
                    DesactivarControles(this);
            }
        }

        public override bool VerificarDatosObligatorios()
        {
            return true;
        }

        public override bool VerificarSiExiste(long? id = null)
        {
            return _servicio.VerificarSiExisteCuit(txtCUIT.Text, id);
        }

        //
        // Acciones de botones
        //
        public override void EjecutarComandoNuevo()
        {
            var nuevoRegistro = new ProveedorDto();
            nuevoRegistro.RazonSocial = txtRazonSocial.Text;
            nuevoRegistro.CUIT = txtCUIT.Text;
            nuevoRegistro.Telefono = txtTelefono.Text;
            nuevoRegistro.Direccion = txtDomicilio.Text;
            nuevoRegistro.Mail = txtMail.Text;
            nuevoRegistro.LocalidadId = (long)cmbLocalidad.SelectedValue;
            nuevoRegistro.CondicionIvaId = (long)cmbCondicionIva.SelectedValue;

            _servicio.Insertar(nuevoRegistro);
        }

        public override void EjecutarComandoModificar()
        {
            var modificarRegistro = new ProveedorDto();
            modificarRegistro.Id = (long)EntidadId;
            modificarRegistro.RazonSocial = txtRazonSocial.Text;
            modificarRegistro.CUIT = txtCUIT.Text;
            modificarRegistro.Telefono = txtTelefono.Text;
            modificarRegistro.Direccion = txtDomicilio.Text;
            modificarRegistro.Mail = txtMail.Text;
            modificarRegistro.LocalidadId = (long)cmbLocalidad.SelectedValue;
            modificarRegistro.CondicionIvaId = (long)cmbCondicionIva.SelectedValue;

            _servicio.Modificar(modificarRegistro);
        }

        public override void EjecutarComandoEliminar()
        {
            _servicio.Eliminar(EntidadId.Value);
        }

        public override void LimpiarControles(object obj, bool tieneValorAsociado = false)
        {
            base.LimpiarControles(obj);

            txtRazonSocial.Focus();
        }

        private void _00010_Abm_Proveedor_Load(object sender, System.EventArgs e)
        {
            // Cargar Combos
            PoblarComboBox(
                cmbCondicionIva,
                _condicionIvaServicio.Obtener(string.Empty, false),
                "Descripcion",
                "Id"
                );

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
            form.ShowDialog();

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
            form.ShowDialog();

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
            form.ShowDialog();

            if (!form.RealizoAlgunaOperacion)
                return;

            if (cmbDepartamento.Items.Count > 0)
                PoblarComboBox(
                    cmbLocalidad,
                    _localidadServicio.ObtenerPorDepartamento((long)cmbDepartamento.SelectedValue),
                    "Descripcion",
                    "Id");
        }

        private void btnNuevaCondicionIva_Click(object sender, System.EventArgs e)
        {
            var form = new _00014_Abm_CondicionIva(TipoOperacion.Nuevo);
            form.ShowDialog();

            if (!form.RealizoAlgunaOperacion)
                return;

            PoblarComboBox(
                cmbCondicionIva,
                _condicionIvaServicio.Obtener(string.Empty, false),
                "Descripcion",
                "Id"
                );
        }
    }
}
