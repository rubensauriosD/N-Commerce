using IServicio.Persona;
using IServicio.Persona.DTOs;
using IServicio.Usuario;
using PresentacionBase.Formularios;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Presentacion.Core.Empleado
{
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
            dgv.DataSource = (List<EmpleadoDto>)_servicio
                                    .Obtener(typeof(EmpleadoDto), cadenaBuscar);

            base.ActualizarDatos(dgv, cadenaBuscar);
        }
    }
}
