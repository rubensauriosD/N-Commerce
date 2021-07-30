namespace Presentacion.Core.Localidad
{
    using System.Linq;
    using Aplicacion.Constantes;
    using StructureMap;
    using IServicio.Departamento;
    using IServicio.Departamento.DTOs;
    using IServicio.Localidad;
    using IServicio.Localidad.DTOs;
    using IServicio.Provincia;
    using IServicio.Provincia.DTOs;
    using IServicios.Localidad.DTOs;
    using Presentacion.Core.Departamento;
    using Presentacion.Core.Provincia;
    using PresentacionBase.Formularios;

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
            CargarComboProvincia();
            CargarComboDepartamento();

            Validar.ComoAlfanumerico(txtDescripcion, true);

            if (EntidadId.HasValue)
            {
                var localidad = (LocalidadDto)_localidadServicio.Obtener(EntidadId.Value);
                var provincia = (ProvinciaDto)_provinciaServicio.Obtener(localidad.ProvinciaId);
                var departamento = (DepartamentoDto)_departamentoServicio.Obtener(localidad.DepartamentoId);

                if (provincia.Eliminado)
                    CargarComboProvincia(provincia.Id);

                cmbProvincia.SelectedValue = localidad.ProvinciaId;

                if (departamento.Eliminado)
                    CargarComboDepartamento(departamento.Id);
                else
                    CargarComboDepartamento();

                cmbDepartamento.SelectedValue = localidad.DepartamentoId;

                txtDescripcion.Text = localidad.Descripcion;
            }
        }

        private void CargarComboProvincia(long idElemento = 0)
        {
            var lstProvincias = _provinciaServicio.Obtener(string.Empty, false)
                .Select(x => (ProvinciaDto)x)
                .ToList();
            
            if (idElemento != 0)
            {
                var provincia = (ProvinciaDto)_provinciaServicio.Obtener(idElemento);
                lstProvincias.Add(provincia);
            }

            PoblarComboBox(cmbProvincia, lstProvincias, "Descripcion", "id");
        }

        private void CargarComboDepartamento(long idElemento = 0)
        {
            if (cmbProvincia.Items.Count < 1 || cmbProvincia.SelectedValue == null)
                return;

            var lstDepartamentos = _departamentoServicio.ObtenerPorProvincia((long)cmbProvincia.SelectedValue)
                .Select(x => (DepartamentoDto)x)
                .ToList();

            if (idElemento != 0)
            {
                var departamento = (DepartamentoDto)_departamentoServicio.Obtener(idElemento);
                lstDepartamentos.Add(departamento);
            }

            PoblarComboBox(cmbDepartamento, lstDepartamentos, "Descripcion", "id");
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
            bool ok = true;

            if (cmbProvincia.Items.Count <= 0 || cmbProvincia.SelectedValue == null)
            {
                Validar.SetErrorProvider(cmbProvincia, "Obligatorio.");
                ok = false;
            }
            else
                Validar.ClearErrorProvider(cmbProvincia);

            if (cmbDepartamento.Items.Count <= 0 || cmbDepartamento.SelectedValue == null)
            {
                Validar.SetErrorProvider(cmbDepartamento, "Obligatorio.");
                ok = false;
            }
            else
                Validar.ClearErrorProvider(cmbDepartamento);


            ok &= ValidateChildren();

            return ok;
        }

        public override bool VerificarSiExiste(long? id = null)
        {
            return _localidadServicio.VerificarSiExiste(txtDescripcion.Text, (long) cmbDepartamento.SelectedValue, id);
        }

        private void cmbProvincia_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            CargarComboDepartamento();
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
