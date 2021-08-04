namespace Presentacion.Core.Empleado
{
    using IServicio.Persona;
    using IServicio.Persona.DTOs;
    using PresentacionBase.Formularios;
    using System.Linq;
    using System.Windows.Forms;

    public partial class EmpleadoLookUp : FormLookUp
    {
        private readonly IEmpleadoServicio _servicio;

        public EmpleadoLookUp(IEmpleadoServicio servicio)
        {
            InitializeComponent();

            _servicio = servicio;
        }

        public override void FormatearGrilla(DataGridView dgv)
        {
            base.FormatearGrilla(dgv);

            dgv.Columns["ApyNom"].Visible = true;
            dgv.Columns["ApyNom"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["ApyNom"].HeaderText = @"Empleado";
            dgv.Columns["ApyNom"].DisplayIndex = 1;
        }

        public override void ActualizarDatos(DataGridView dgv, string cadenaBuscar)
        {
            dgv.DataSource = _servicio.Obtener(typeof(EmpleadoDto), cadenaBuscar)
                .Select(x => (EmpleadoDto)x)
                .Where(x => !x.Eliminado)
                .ToList();

            base.ActualizarDatos(dgv, cadenaBuscar);
        }
    }
}
