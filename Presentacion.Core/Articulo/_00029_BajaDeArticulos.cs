namespace Presentacion.Core.Articulo
{
    using System;
    using System.Linq;
    using System.Windows.Forms;
    using Aplicacion.Constantes;
    using IServicios.Articulo;
    using IServicios.Articulo.DTOs;
    using PresentacionBase.Formularios;

    public partial class _00029_BajaDeArticulos : FormBase
    {
        private readonly IBajaArticuloServicio _bajaArticuloServicio;
        private string _placeHolder => "Buscar";

        private BajaArticuloDto objetoSeleccionado;
        private Validar Validar;

        public _00029_BajaDeArticulos(IBajaArticuloServicio bajaArticuloServicio)
        {
            InitializeComponent();

            _bajaArticuloServicio = bajaArticuloServicio;
            objetoSeleccionado = new BajaArticuloDto();
            Validar = new Validar();
        }

        private void _00029_BajaDeArticulos_Load(object sender, EventArgs e)
        {
            btnEjecutar.Text = "&Nueva Baja";

            Validar.ComoAlfanumerico(txtBuscar, false);
            txtBuscar.KeyPress += txtBuscar_KeyPress;
            txtBuscar.Leave += txtBuscar_Leave;
            txtBuscar.Enter += txtByscar_Enter;

            ActualizarDatos();
        }

        // -- Eventos de grilla
        private void dgvGrilla_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            objetoSeleccionado = (BajaArticuloDto)dgvGrilla.Rows[e.RowIndex].DataBoundItem;
        }

        public void ActualizarDatos()
        {
            string cadenaBuscar = txtBuscar.Text != _placeHolder ? txtBuscar.Text : string.Empty;

            dgvGrilla.DataSource = _bajaArticuloServicio.Obtener(cadenaBuscar)
                .Select(x => (BajaArticuloDto)x)
                .ToList();

            FormatearGrilla();
        }

        public void FormatearGrilla()
        {
            for (int i = 0; i < dgvGrilla.ColumnCount; i++)
            {
                dgvGrilla.Columns[i].Visible = false;
                dgvGrilla.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dgvGrilla.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            }

            dgvGrilla.Columns["Fecha"].Visible = true;
            dgvGrilla.Columns["Fecha"].HeaderText = "Fecha";
            dgvGrilla.Columns["Fecha"].DefaultCellStyle.Format = "d";
            dgvGrilla.Columns["Fecha"].DisplayIndex = 1;

            dgvGrilla.Columns["Articulo"].Visible = true;
            dgvGrilla.Columns["Articulo"].HeaderText = "Articulo";
            dgvGrilla.Columns["Articulo"].DisplayIndex = 2;

            dgvGrilla.Columns["Cantidad"].Visible = true;
            dgvGrilla.Columns["Cantidad"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvGrilla.Columns["Cantidad"].HeaderText = "Cant";
            dgvGrilla.Columns["Cantidad"].DisplayIndex = 3;

            dgvGrilla.Columns["MotivoBaja"].Visible = true;
            dgvGrilla.Columns["MotivoBaja"].HeaderText = "Motivo";
            dgvGrilla.Columns["MotivoBaja"].DisplayIndex = 4;

            dgvGrilla.Columns["Observacion"].Visible = true;
            dgvGrilla.Columns["Observacion"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvGrilla.Columns["Observacion"].HeaderText = "Observaciones";
            dgvGrilla.Columns["Observacion"].DisplayIndex = 5;
        }

        // -- Evento de busqueda
        private void txtBuscar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                ActualizarDatos();
        }

        private void txtBuscar_Leave(object sender, EventArgs e)
        {
            if (txtBuscar.Text == string.Empty || txtBuscar.TextLength < 3)
                txtBuscar.Text = _placeHolder;
        }

        private void txtByscar_Enter(object sender, EventArgs e)
        {
            if (txtBuscar.Text != _placeHolder)
                return;

            txtBuscar.Text = "";
            txtBuscar.Select();
            txtBuscar.Focus();
        }

        // -- Acciones de botones
        private void btnEjecutar_Click(object sender, EventArgs e)
        {
            var form = new _00030_Abm_BajaArticulos(TipoOperacion.Nuevo);
            form.ShowDialog();

            if (form.RealizoAlgunaOperacion)
                ActualizarDatos();
        }

        private void btnRevertir_Click(object sender, EventArgs e)
        {
            if (!Mjs.Preguntar($"¿Seguro que desar revertir la baja del artículo {objetoSeleccionado.Articulo}?"))
                return;

            if (!_bajaArticuloServicio.RevertirBaja(objetoSeleccionado.Id))
                return;

            Mjs.Info("Baja fue revertida.");
            ActualizarDatos();
        }
    }
}
