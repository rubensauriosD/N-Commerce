namespace PresentacionBase.Formularios
{
    using System;
    using System.Windows.Forms;
    using Aplicacion.Constantes;

    public partial class FormBusquedaSeleccion : FormBase
    {
        public delegate void SetDatosGrilla(DataGridView grilla, string cadenaBuscar);
        public delegate void SetFormatoGrilla(DataGridView grilla);

        private Validar Validar;
        private SetDatosGrilla datosGrilla;
        private SetFormatoGrilla formatoGrilla;
        private string _placeHolder => "Buscar";
        public object Seleccion { get; private set; }
        public bool RealizoSeleccion { get; private set; }
        public string Titulo { set { Text = value; } }

        public FormBusquedaSeleccion(SetDatosGrilla setDatosGrilla, SetFormatoGrilla setFormatoGrilla) : base()
        {
            InitializeComponent();

            datosGrilla = setDatosGrilla;
            formatoGrilla = setFormatoGrilla;

            Seleccion = new object();
            Validar = new Validar();
        }

        public FormBusquedaSeleccion(IConfiguracionBusquedaListado config)
            : this(config.SetDatosGrilla, config.SetFormatoGrilla) 
        {
            Text = config.Titulo;
        }

        private void FormBusquedaSeleccion_Load(object sender, EventArgs e)
        {
            Validar.ComoAlfanumerico(txtBuscar);
            txtBuscar.Text = _placeHolder;

            ActualizarGrilla();

            if (dgvGrilla.RowCount > 0)
                return;

            Mjs.Info("No hay elementos para seleccionar.");
            Close();
        }

        private void ActualizarGrilla()
        {
            string cadenaBuscar = txtBuscar.Text != _placeHolder ? txtBuscar.Text : string.Empty;

            datosGrilla(dgvGrilla, cadenaBuscar);

            for (int i = 0; i < dgvGrilla.ColumnCount; i++)
            {
                dgvGrilla.Columns[i].Visible = false;
                dgvGrilla.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dgvGrilla.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            }

            formatoGrilla(dgvGrilla);
        }

        // --- Acciones de controles
        private void txtBuscar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                ActualizarGrilla();
        }

        private void txtBuscar_Leave(object sender, EventArgs e)
        {
            if (txtBuscar.Text == string.Empty || txtBuscar.TextLength < 3)
                txtBuscar.Text = _placeHolder;
        }

        private void dgvGrilla_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvGrilla.RowCount <= 0) return;
        
            Seleccion = dgvGrilla.Rows[e.RowIndex].DataBoundItem;
        }

        private void dgvGrilla_KeyPress(object sender, KeyPressEventArgs e)
        {
            char letra = e.KeyChar;

            if (letra == (char)Keys.Escape)
                Close();

            // Si presiono una tecla
            if (char.IsLetterOrDigit(letra))
            {
                if (txtBuscar.Text.Length == txtBuscar.MaxLength)
                    return;

                // Agregar texto al filtro
                txtBuscar.Text = txtBuscar.Text != _placeHolder
                    ? txtBuscar.Text += letra
                    : string.Empty + letra;

                // Mostrar resultado filtrado
                ActualizarGrilla();
            }

            // Si presiona borrar
            if (letra == (char)Keys.Back) 
            {
                if (txtBuscar.Text == _placeHolder)
                    return;

                // Quitar el ultimo caracter del filtro
                var largo = txtBuscar.Text.Length;
                txtBuscar.Text = txtBuscar.Text.Remove(largo - 1, 1);

                if (txtBuscar.Text == "")
                    txtBuscar.Text = _placeHolder;

                // MostrarResultados filtrados
                ActualizarGrilla();
            }
        }

        private void dgvGrilla_KeyDown(object sender, KeyEventArgs e)
        {
            string letra = e.KeyCode.ToString();

            if (letra == "F12")
            {
                RealizoSeleccion = Seleccion != null;
                Close();
                return;
            }
        }

        private void btnSeleccionar_Click(object sender, EventArgs e)
        {
            RealizoSeleccion = Seleccion != null;
            Close();
            return;
        }
    }
}
