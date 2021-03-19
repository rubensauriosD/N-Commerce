namespace PresentacionBase.Formularios
{
    using Aplicacion.Constantes;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public partial class FormConsulta : FormBase
    {
        private long? entidadId;
        protected object EntidadSeleccionada;

        public FormConsulta()
        {
            InitializeComponent();

            entidadId = null;

            AsignarEvento_EnterLeave(this);
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormConsulta_Load(object sender, EventArgs e)
        {
            ActualizarDatos(dgvGrilla, string.Empty);

            picBuscar.Image = Imagen.Buscar;
        }

        public virtual void ActualizarDatos(DataGridView dgv, string cadenaBuscar)
        {
            FormatearGrilla(dgv);
            lblRegistros.Text = $"Cant. de Registros: {dgv.RowCount}";
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
            if (dgvGrilla.RowCount > 0)
            {
                if (entidadId.HasValue) // Pregunto si tiene un valor
                {
                    if (EjecutarComando(TipoOperacion.Modificar, entidadId))
                    {
                        ActualizarDatos(dgvGrilla, string.Empty);
                    }
                }
                else
                {
                    MessageBox.Show("Por favor seleccione un registro");
                }
            }
            else
            {
                MessageBox.Show("No hay registros Cargados");
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvGrilla.RowCount > 0)
            {
                if (entidadId.HasValue) // Pregunto si tiene un valor
                {
                    if (EjecutarComando(TipoOperacion.Eliminar, entidadId))
                    {
                        ActualizarDatos(dgvGrilla, string.Empty);
                    }
                }
                else
                {
                    MessageBox.Show("Por favor seleccione un registro");
                }
            }
            else
            {
                MessageBox.Show("No hay registros Cargados");
            }
        }

        public virtual void dgvGrilla_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvGrilla.RowCount <= 0) return;

            // Obtiene el valor de una Celda
            entidadId = (long) dgvGrilla["Id", e.RowIndex].Value;

            // Obtener el Objeto completo seleccionado
            EntidadSeleccionada = dgvGrilla.Rows[e.RowIndex].DataBoundItem;
            
            btnModificar.Enabled = !(bool)dgvGrilla["Eliminado", e.RowIndex].Value;
        }

        // Acciones Sobre el Formulario
        public void AgregarSeparadorAlMenu()
        {
            var separador = new ToolStripSeparator();

            separador.Name = $@"toolStripSeparator{tspMenu.Items.Count}";
            separador.Size = new Size(6, 42);

        }

        public void AgregarBotonAlMenu(ToolStripButton btn)
        {
            btn.ImageTransparentColor = System.Drawing.Color.White;
            btn.Padding = new Padding(3, 0, 3, 0);
            btn.Size = new Size(69, 39);
            btn.TextImageRelation = TextImageRelation.ImageAboveText;

            tspMenu.Items.Add(btn);
        }

    }
}
