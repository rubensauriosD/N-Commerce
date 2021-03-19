namespace PresentacionBase.Formularios
{
    using System;
    using System.Windows.Forms;

    public partial class FormConsultaConDetalle : FormBase
    {
        private long? entidadId;
        protected object EntidadSeleccionada;

        public FormConsultaConDetalle()
        {
            InitializeComponent();

            entidadId = null;
        }

        private void FormConsulta_Load(object sender, EventArgs e)
        {
            ActualizarDatos(dgvGrilla, string.Empty);
        }

        public virtual void ActualizarDatos(DataGridView dgv, string cadenaBuscar)
        {
            FormatearGrilla(dgv);
        }

        private void txtBuscar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char) Keys.Enter)
            {
                e.Handled = true; // Quita Ruido molesto enter
                ActualizarDatos(dgvGrilla, txtBuscar.Text);
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            ActualizarDatos(dgvGrilla,string.Empty);
            txtBuscar.Clear();
            txtBuscar.Focus();
        }

        public virtual bool EjecutarComando(TipoOperacion tipoOperacion, long? id = null)
        {
            return false;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            if (EjecutarComando(TipoOperacion.Nuevo))
            {
                ActualizarDatos(dgvGrilla, string.Empty);
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (dgvGrilla.RowCount < 1)
            {
                MessageBox.Show("No hay registros Cargados");
                return;
            }

            if (entidadId.HasValue)
            {
                MessageBox.Show("Por favor seleccione un registro");
                return;
            }

            if (EjecutarComando(TipoOperacion.Modificar, entidadId))
                ActualizarDatos(dgvGrilla, string.Empty);
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvGrilla.RowCount < 1)
            {
                MessageBox.Show("No hay registros Cargados");
                return;
            }

            if (entidadId.HasValue)
            {
                MessageBox.Show("Por favor seleccione un registro");
                return;
            }

            if (EjecutarComando(TipoOperacion.Eliminar, entidadId))
                ActualizarDatos(dgvGrilla, string.Empty);
        }

        public virtual void dgvGrilla_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvGrilla.RowCount <= 0) return;

            entidadId = (long) dgvGrilla["Id", e.RowIndex].Value;

            EntidadSeleccionada = dgvGrilla.Rows[e.RowIndex].DataBoundItem;
            
            btnModificar.Enabled = !(bool)dgvGrilla["Eliminado", e.RowIndex].Value;
        }
    }
}
