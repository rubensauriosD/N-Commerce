namespace Presentacion.Core.Cliente
{
    using System;
    using System.Linq;
    using System.Windows.Forms;
    using Aplicacion.Constantes;
    using IServicio.Persona;
    using IServicio.Persona.DTOs;
    using PresentacionBase.Formularios;

    public partial class _00009_Cliente : FormConsulta
    {
        private readonly IClienteServicio _servicio;

        public _00009_Cliente(IClienteServicio provinciaServicio)
        {
            InitializeComponent();

            _servicio = provinciaServicio;
        }

        private void _00009_Cliente_Load(object sender, System.EventArgs e)
        {
            var btnCuentaCorrienteCliente = new ToolStripButton();
            btnCuentaCorrienteCliente.Text = "Cuenta Corriente";
            btnCuentaCorrienteCliente.Image = Imagen.Pago;
            btnCuentaCorrienteCliente.Click += 
                (object objeto, EventArgs eveto) => new _00034_ClienteCtaCte(((ClienteDto)EntidadSeleccionada).Id).ShowDialog();

            AgregarSeparadorAlMenu();
            AgregarBotonAlMenu(btnCuentaCorrienteCliente);
        }

        private void clickCC(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public override void ActualizarDatos(DataGridView dgv, string cadenaBuscar)
        {
            dgv.DataSource = _servicio.Obtener(typeof(ClienteDto), cadenaBuscar).Where(x => x.Dni != "99999999").ToList();

            base.ActualizarDatos(dgv, cadenaBuscar);
        }

        public override void FormatearGrilla(DataGridView dgv)
        {
            base.FormatearGrilla(dgv);

            dgv.Columns["ApyNom"].Visible = true;
            dgv.Columns["ApyNom"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["ApyNom"].HeaderText = @"Cliente";
            dgv.Columns["ApyNom"].DisplayIndex = 1;

            dgv.Columns["EliminadoStr"].Visible = true;
            dgv.Columns["EliminadoStr"].Width = 100;
            dgv.Columns["EliminadoStr"].HeaderText = @"Eliminado";
            dgv.Columns["EliminadoStr"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns["EliminadoStr"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["EliminadoStr"].DisplayIndex = 2;
        }

        public override void dgvGrilla_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            base.dgvGrilla_RowEnter(sender, e);

            var cliente = (ClienteDto)EntidadSeleccionada;

            btnModificar.Enabled = cliente.Dni != "99999999";
            btnEliminar.Enabled = cliente.Dni != "99999999";
        }

        public override bool EjecutarComando(TipoOperacion tipoOperacion, long? id = null)
        {
            var formulario = new _00010_Abm_Cliente(tipoOperacion, id);

            formulario.ShowDialog();

            return formulario.RealizoAlgunaOperacion;
        }
    }
}
