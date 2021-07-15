namespace Presentacion.Core.Departamento
{
    using Aplicacion.Constantes;
    using IServicio.Departamento;
    using IServicio.Departamento.DTOs;
    using IServicio.Provincia;
    using IServicios.Departamento.DTOs;
    using Presentacion.Core.Provincia;
    using PresentacionBase.Formularios;
    using StructureMap;

    public partial class _00004_Abm_Departamento : FormAbm
    {
        private readonly IDepartamentoServicio _departamentoServicio;
        private readonly IProvinciaServicio _provinciaServicio;
        private readonly Validar Validar;

        public _00004_Abm_Departamento(TipoOperacion tipoOperacion, long? entidadId = null)
            : base(tipoOperacion, entidadId)
        {
            InitializeComponent();

            _departamentoServicio = ObjectFactory.GetInstance<IDepartamentoServicio>();
            _provinciaServicio = ObjectFactory.GetInstance<IProvinciaServicio>();
            Validar = new Validar();
        }

        private void _00004_Abm_Departamento_Load(object sender, System.EventArgs e)
        {
            Validar.ComoAlfanumerico(txtDescripcion, true);

            PoblarComboBox(cmbProvincia,
                _provinciaServicio.Obtener(string.Empty),
                "Descripcion",
                "Id");

            if (EntidadId.HasValue)
            {
                var entidad = (DepartamentoDto)_departamentoServicio.Obtener(EntidadId.Value);
                cmbProvincia.SelectedValue = entidad.ProvinciaId;

                txtDescripcion.Text = entidad.Descripcion;
            }
        }

        public override void EjecutarComandoNuevo()
        {
            _departamentoServicio.Insertar(new DepartamentoCrudDto{
                Descripcion = txtDescripcion.Text,
                ProvinciaId = (long)cmbProvincia.SelectedValue
            });
        }

        public override void EjecutarComandoModificar()
        {
            _departamentoServicio.Modificar(new DepartamentoCrudDto
            {
                Id = EntidadId.Value,
                Descripcion = txtDescripcion.Text,
                ProvinciaId = (long)cmbProvincia.SelectedValue
            });
        }

        public override void EjecutarComandoEliminar()
        {
            _departamentoServicio.Eliminar(EntidadId.Value);
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

            return ValidateChildren();
        }

        public override bool VerificarSiExiste(long? id = null)
        {
            return _departamentoServicio
                .VerificarSiExiste(txtDescripcion.Text, (long) cmbProvincia.SelectedValue, id);
        }

        // --- Acciones de controloes
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
            }
        }
    }
}
