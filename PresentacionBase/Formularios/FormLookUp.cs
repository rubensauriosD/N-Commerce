namespace PresentacionBase.Formularios
{
    using System.Windows.Forms;

    public partial class FormLookUp : FormBase
    {
        public long? entidadId;
        public object EntidadSeleccionada;
        public bool RealizoSeleccion { get; private set; }

        public FormLookUp()
        {
            InitializeComponent();

            AsignarEvento_EnterLeave(this);
            RealizoSeleccion = false;
        }

        public virtual void ActualizarDatos(DataGridView dgv, string cadenaBuscar)
        {
            FormatearGrilla(dgv);
        }

        private void FormLookUp_Load(object sender, System.EventArgs e)
        {
            ActualizarDatos(dgvGrilla, string.Empty);
        }

        private void txtBuscar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char) Keys.Enter)
                ActualizarDatos(dgvGrilla, txtBuscar.Text);
        }

        private void dgvGrilla_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvGrilla.RowCount <= 0) return;

            entidadId = (long)dgvGrilla["Id", e.RowIndex].Value;

            // Obtener el Objeto completo seleccionado
            EntidadSeleccionada = dgvGrilla.Rows[e.RowIndex].DataBoundItem;
        }

        private void btnSalir_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void btnActualizar_Click(object sender, System.EventArgs e)
        {
            ActualizarDatos(dgvGrilla,string.Empty);
        }

        private void btnNuevo_Click(object sender, System.EventArgs e)
        {
            RealizoSeleccion = true;
            Close();
        }

        private void dgvGrilla_DoubleClick(object sender, System.EventArgs e)
        {
            Close();
        }

        private void dgvGrilla_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnNuevo.PerformClick();

            if (e.KeyCode == Keys.Escape)
                btnSalir.PerformClick();
        }
    }
}
