namespace Presentacion.Core.Localidad
{
    using Aplicacion.Constantes;
    using IServicio.Departamento;
    using IServicio.Localidad;
    using IServicio.Localidad.DTOs;
    using IServicio.Provincia;
    using IServicios.Localidad.DTOs;
    using Presentacion.Core.Departamento;
    using Presentacion.Core.Provincia;
    using PresentacionBase.Formularios;
    using StructureMap;

    public partial class _00006_AbmLocalidad : FormAbm
    {
        private readonly IProvinciaServicio _provinciaServicio;
        private readonly IDepartamentoServicio _departamentoServicio;
        private readonly ILocalidadServicio _localidadServicio;
        private readonly Validar Validar;

        public _00006_AbmLocalidad(TipoOperacion tipoOperacion, long? entidadId = null)
            : base(tipoOperacion, entidadId)
        {
            InitializeComponent();

            _provinciaServicio = ObjectFactory.GetInstance<IProvinciaServicio>();
            _departamentoServicio = ObjectFactory.GetInstance<IDepartamentoServicio>();
            _localidadServicio = ObjectFactory.GetInstance<ILocalidadServicio>();
            Validar = new Validar();
        }

        private void _00006_AbmLocalidad_Load(object sender, System.EventArgs e)
        {
            PoblarComboBox(
                cmbProvincia,
                _provinciaServicio.Obtener(string.Empty, false),
                "Descripcion",
                "Id");

            PoblarComboBox(
                cmbDepartamento,
                _departamentoServicio.ObtenerPorProvincia((long)cmbProvincia.SelectedValue),
                "Descripcion",
                "Id");

            Validar.ComoAlfanumerico(txtDescripcion, true);

            if (EntidadId.HasValue)
            {
                var localidad = (LocalidadDto)_localidadServicio.Obtener(EntidadId.Value);

                cmbProvincia.SelectedValue = localidad.ProvinciaId;

                PoblarComboBox(
                    cmbDepartamento, 
                    _departamentoServicio.ObtenerPorProvincia(localidad.ProvinciaId),
                    "Descripcion",
                    "Id"
                );

                cmbDepartamento.SelectedValue = localidad.DepartamentoId;

                txtDescripcion.Text = localidad.Descripcion;
            }
        }

        public override void EjecutarComandoNuevo()
        {
            _localidadServicio.Insertar(new LocalidadCrudDto
            {
                Descripcion = txtDescripcion.Text,
                DepartamentoId = (long)cmbDepartamento.SelectedValue
            });
        }

        public override void EjecutarComandoModificar()
        {
            _localidadServicio.Modificar(new LocalidadCrudDto
            {
                Id = EntidadId.Value,
                Descripcion = txtDescripcion.Text,
                DepartamentoId = (long)cmbDepartamento.SelectedValue
            });
        }

        public override void EjecutarComandoEliminar()
        {
            _localidadServicio.Eliminar(EntidadId.Value);
        }

        public override bool VerificarDatosObligatorios()
        {
            if (cmbProvincia.Items.Count <= 0)
            {
                Validar.SetErrorProvider(cmbProvincia, "Obligatorio.");
                return false;
            }
            else
                Validar.ClearErrorProvider(cmbProvincia);

            if (cmbDepartamento.Items.Count <= 0)
            {
                Validar.SetErrorProvider(cmbDepartamento, "Obligatorio.");
                return false;
            }
            else
                Validar.ClearErrorProvider(cmbDepartamento);


            return ValidateChildren();
        }

        public override bool VerificarSiExiste(long? id = null)
        {
            return _localidadServicio.VerificarSiExiste(txtDescripcion.Text, (long) cmbDepartamento.SelectedValue, id);
        }

        private void cmbProvincia_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            PoblarComboBox(cmbDepartamento, 
                _departamentoServicio.ObtenerPorProvincia((long)cmbProvincia.SelectedValue), 
                "Descripcion", 
                "Id");
        }

        private void btnNuevaProvincia_Click(object sender, System.EventArgs e)
        {
            var formNuevaProvincia = new _00002_Abm_Provincia(TipoOperacion.Nuevo);
            formNuevaProvincia.ShowDialog();

            if (formNuevaProvincia.RealizoAlgunaOperacion)
            {
                PoblarComboBox(cmbProvincia,
                    _provinciaServicio.Obtener(string.Empty, false),
                    "Descripcion",
                    "Id");

                if (cmbProvincia.Items.Count > 0) // si tiene algo
                {
                    PoblarComboBox(cmbDepartamento,
                        _departamentoServicio.ObtenerPorProvincia((long)cmbProvincia.SelectedValue)
                        , "Descripcion",
                        "Id");
                }
            }
        }

        private void btnNuevoDepartamento_Click(object sender, System.EventArgs e)
        {
            var formNuevoDepartamento = new _00004_Abm_Departamento(TipoOperacion.Nuevo);
            formNuevoDepartamento.ShowDialog();

            if (formNuevoDepartamento.RealizoAlgunaOperacion)
            {
                if (cmbProvincia.Items.Count > 0) // si tiene algo
                {
                    PoblarComboBox(cmbDepartamento,
                        _departamentoServicio.ObtenerPorProvincia((long)cmbProvincia.SelectedValue)
                        , "Descripcion",
                        "Id");
                }
            }
        }
    }
}
