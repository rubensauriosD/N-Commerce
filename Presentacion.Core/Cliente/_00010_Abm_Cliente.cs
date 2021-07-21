namespace Presentacion.Core.Cliente
{
    using System.Collections.Generic;
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
    using Presentacion.Core.CondicionIva;
    using Presentacion.Core.Departamento;
    using Presentacion.Core.Localidad;
    using Presentacion.Core.Provincia;
    using PresentacionBase.Formularios;
    using StructureMap;

    public partial class _00010_Abm_Cliente : FormAbm
    {
        private readonly IClienteServicio _servicio;
        private readonly IProvinciaServicio _provinciaServicio;
        private readonly IDepartamentoServicio _departamentoServicio;
        private readonly ILocalidadServicio _localidadServicio;
        private readonly ICondicionIvaServicio _condicionIvaServicio;
        private readonly Validar Validar;

        public _00010_Abm_Cliente(TipoOperacion tipoOperacion, long? entidadId = null)
            : base(tipoOperacion, entidadId)
        {
            InitializeComponent();

            _servicio = ObjectFactory.GetInstance<IClienteServicio>();
            _provinciaServicio = ObjectFactory.GetInstance<IProvinciaServicio>();
            _departamentoServicio = ObjectFactory.GetInstance<IDepartamentoServicio>();
            _localidadServicio = ObjectFactory.GetInstance<ILocalidadServicio>();
            _condicionIvaServicio = ObjectFactory.GetInstance<ICondicionIvaServicio>();
            Validar = new Validar();
        }

        private void _00010_Abm_Cliente_Load(object sender, System.EventArgs e)
        {
            ActualizarProvincia();
            ActualizarDepartamento();
            ActualizarLocalidad();

            PoblarComboBox(cmbCondicionIva, _condicionIvaServicio.Obtener(string.Empty, false),"Descripcion","Id");
            cmbCondicionIva.SelectedValue = 2;

            chkActivarCuentaCorriente.Checked = false;
            chkLimiteCompra.Checked = false;
            chkLimiteCompra.Enabled = false;
            nudLimiteCompra.Enabled = false;
            nudLimiteCompra.Value = 0;

            Validar.ComoTexto(txtApellido, true);
            Validar.ComoTexto(txtNombre, true);
            Validar.ComoDni(txtDni, true);
            Validar.ComoDomicilio(txtDomicilio, true);
            Validar.ComoMail(txtMail, true);
            Validar.ComoTelefono(txtTelefono);

            CargarDatos();
        }

        public void CargarDatos()
        {
            if (!EntidadId.HasValue)
                return;

            var resultado = (ClienteDto)_servicio.Obtener(typeof(ClienteDto),EntidadId.Value);

            if (resultado == null)
            {
                MessageBox.Show("Ocurrio un error al obtener el registro seleccionado.");
                Close();
            }

            // Cargar Datos en controles

            txtApellido.Text = resultado.Apellido;
            txtNombre.Text = resultado.Nombre;
            txtDni.Text = resultado.Dni;
            txtTelefono.Text = resultado.Telefono;
            txtDomicilio.Text = resultado.Direccion;
            txtMail.Text = resultado.Mail;
            chkLimiteCompra.Checked = resultado.TieneLimiteCompra;
            chkActivarCuentaCorriente.Checked = resultado.ActivarCtaCte;
            nudLimiteCompra.Value = resultado.MontoMaximoCtaCte;
            cmbCondicionIva.SelectedValue = resultado.CondicionIvaId;

            ActualizarProvincia(resultado.ProvinciaId);
            ActualizarDepartamento(resultado.DepartamentoId);
            ActualizarLocalidad(resultado.LocalidadId);
        }

        public override bool VerificarDatosObligatorios()
        {
            if (cmbLocalidad.Items.Count < 1)
            {
                Validar.SetErrorProvider(cmbLocalidad, "Debe seleccionar una localidad.");
                return false;
            }
            else Validar.ClearErrorProvider(cmbLocalidad);

            return ValidateChildren();
        }

        public override bool VerificarSiExiste(long? id = null)
        {
            return _servicio.VerificarSiExisteDni(txtDni.Text, id);
        }

        // --- Acciones de botones
        public override void EjecutarComandoNuevo()
        {
            var nuevoRegistro = new ClienteDto();
            nuevoRegistro.Nombre = txtNombre.Text;
            nuevoRegistro.Apellido = txtApellido.Text;
            nuevoRegistro.Dni = txtDni.Text;
            nuevoRegistro.Telefono = txtTelefono.Text;
            nuevoRegistro.Direccion = txtDomicilio.Text;
            nuevoRegistro.Mail = txtMail.Text;
            nuevoRegistro.LocalidadId = (long)cmbLocalidad.SelectedValue;
            nuevoRegistro.CondicionIvaId = (long)cmbCondicionIva.SelectedValue;
            nuevoRegistro.ActivarCtaCte = chkActivarCuentaCorriente.Checked;
            nuevoRegistro.TieneLimiteCompra = chkLimiteCompra.Checked;
            nuevoRegistro.MontoMaximoCtaCte = nudLimiteCompra.Value;

            _servicio.Insertar(nuevoRegistro);
        }

        public override void EjecutarComandoModificar()
        {
            var modificarRegistro = new ClienteDto();
            modificarRegistro.Id = EntidadId.Value;
            modificarRegistro.Nombre = txtNombre.Text;
            modificarRegistro.Apellido = txtApellido.Text;
            modificarRegistro.Dni = txtDni.Text;
            modificarRegistro.Telefono = txtTelefono.Text;
            modificarRegistro.Direccion = txtDomicilio.Text;
            modificarRegistro.Mail = txtMail.Text;
            modificarRegistro.LocalidadId = (long)cmbLocalidad.SelectedValue;
            modificarRegistro.CondicionIvaId = (long)cmbCondicionIva.SelectedValue;
            modificarRegistro.ActivarCtaCte = chkActivarCuentaCorriente.Checked;
            modificarRegistro.TieneLimiteCompra = chkLimiteCompra.Checked;
            modificarRegistro.MontoMaximoCtaCte = nudLimiteCompra.Value;

            _servicio.Modificar(modificarRegistro);
        }

        public override void EjecutarComandoEliminar()
        {
            _servicio.Eliminar(typeof(ClienteDto),EntidadId.Value);
        }

        public override void LimpiarControles(object obj, bool tieneValorAsociado = false)
        {
            base.LimpiarControles(obj);

            chkActivarCuentaCorriente.Checked = false;
            chkLimiteCompra.Checked = false;
            nudLimiteCompra.Value = 0;
            txtApellido.Focus();
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

        // --- Provincia Departamento Localidad
        private void cmbProvincia_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            ActualizarDepartamento();
        }

        private void cmbDepartamento_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            ActualizarLocalidad();
        }

        private void btnNuevaProvincia_Click(object sender, System.EventArgs e)
        {
            var form = new _00002_Abm_Provincia(TipoOperacion.Nuevo);
            form.ShowDialog();

            if (!form.RealizoAlgunaOperacion)
                return;

            ActualizarProvincia();
        }

        private void btnNuevoDepartamento_Click(object sender, System.EventArgs e)
        {
            var form = new _00004_Abm_Departamento(TipoOperacion.Nuevo);
            form.ShowDialog();

            if (!form.RealizoAlgunaOperacion)
                return;

            ActualizarDepartamento();
        }

        private void btnNuevaLocalidad_Click(object sender, System.EventArgs e)
        {
            var form = new _00006_AbmLocalidad(TipoOperacion.Nuevo);
            form.ShowDialog();

            if (!form.RealizoAlgunaOperacion)
                return;

            ActualizarLocalidad();
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

            if (id != 0)
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

            var provinciaId = (long)cmbProvincia.SelectedValue;
            var lstDepartamentos = _departamentoServicio.ObtenerPorProvincia(provinciaId)
                .OrderBy(x => x.Descripcion)
                .ToList();

            PoblarComboBox(cmbDepartamento, lstDepartamentos, "Descripcion", "Id");

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

            var departamentId = (long)cmbDepartamento.SelectedValue;
            var lstLocalidades = _localidadServicio.ObtenerPorDepartamento(departamentId)
                .OrderBy(x => x.Descripcion)
                .ToList();

            PoblarComboBox(cmbLocalidad, lstLocalidades, "Descripcion", "Id");

            if (id != 0)
                cmbLocalidad.SelectedValue = id;
        }

        private void chkActivarCuentaCorriente_CheckedChanged(object sender, System.EventArgs e)
        {
            chkLimiteCompra.Enabled = chkActivarCuentaCorriente.Checked;
            chkLimiteCompra.Checked = false;
            nudLimiteCompra.Enabled = chkActivarCuentaCorriente.Checked;
            nudLimiteCompra.Value = 0;
        }
    }
}
