namespace Presentacion.Core.Cliente
{
    using IServicio.Persona;
    using IServicio.Persona.DTOs;
    using PresentacionBase.Formularios;
    using StructureMap;
    using System.Linq;
    using System.Windows.Forms;

    public partial class ClienteLookUp : FormLookUp
    {
        private readonly IClienteServicio _servicio;

        public ClienteLookUp()
        {
            InitializeComponent();

            _servicio = ObjectFactory.GetInstance<IClienteServicio>();
        }

        public override void ActualizarDatos(DataGridView dgv, string cadenaBuscar)
        {
            dgv.DataSource = _servicio.Obtener(typeof(ClienteDto), cadenaBuscar)
                .Where(x => !x.Eliminado)
                .ToList();

            FormatearGrilla(dgv);
        }

        public override void FormatearGrilla(DataGridView dgv)
        {
            base.FormatearGrilla(dgv);

            dgv.Columns["ApyNom"].Visible = true;
            dgv.Columns["ApyNom"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["ApyNom"].HeaderText = @"Cliente";
            dgv.Columns["ApyNom"].DisplayIndex = 1;
        }
    }
}
