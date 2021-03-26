namespace Presentacion.Core.Proveedor
{
    using System.Linq;
    using System.Windows.Forms;
    using StructureMap;
    using IServicio.Persona;
    using PresentacionBase.Formularios;
    using IServicio.Persona.DTOs;

    public partial class ProveedorLookUp : FormLookUp
    {
        private readonly IProveedorServicio _servicio;

        public ProveedorLookUp()
        {
            InitializeComponent();

            _servicio = ObjectFactory.GetInstance<IProveedorServicio>();
        }

        public override void ActualizarDatos(DataGridView dgv, string cadenaBuscar)
        {
            dgv.DataSource = _servicio.Obtener(cadenaBuscar)
                .Where(x => !x.Eliminado)
                .Select(x => (ProveedorDto)x)
                .ToList();

            FormatearGrilla(dgv);
        }

        public override void FormatearGrilla(DataGridView dgv)
        {
            base.FormatearGrilla(dgv);

            dgv.Columns["RazonSocial"].Visible = true;
            dgv.Columns["RazonSocial"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["RazonSocial"].HeaderText = @"Proveedor";
            dgv.Columns["RazonSocial"].DisplayIndex = 1;
        }
    }
}
