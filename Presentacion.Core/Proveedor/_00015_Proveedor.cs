namespace Presentacion.Core.Proveedor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;
    using Aplicacion.Constantes;
    using IServicio.Persona;
    using IServicio.Persona.DTOs;
    using PresentacionBase.Formularios;

    public partial class _00015_Proveedor : FormConsulta
    {
        private readonly IProveedorServicio _servicio;
        private ProveedorDto proveedor;

        public _00015_Proveedor(IProveedorServicio provinciaServicio)
        {
            InitializeComponent();

            _servicio = provinciaServicio;
            proveedor = new ProveedorDto();

            SetearControles();
        }

        private void SetearControles()
        {
            AgregarSeparadorAlMenu();

            var btnCuentaCorriente = new ToolStripButton();
            btnCuentaCorriente.Text = @"Cuenta Corriente";
            btnCuentaCorriente.Image = Imagen.Pago;
            btnCuentaCorriente.Click += MostrarCuentaCuente;

            AgregarBotonAlMenu(btnCuentaCorriente);
        }

        // Cuenta Corriente
        private void MostrarCuentaCuente(object sender, EventArgs e)
        {
            if (proveedor.Id == 0)
            {
                Mjs.Alerta("No se seleccionó ningún proveedor.");
                return;
            }

            var fCuentaCorriente = new _00036_ProveedorCtaCte(proveedor.Id);
            fCuentaCorriente.ShowDialog();
        }

        public override void ActualizarDatos(DataGridView dgv, string cadenaBuscar)
        {
            dgv.DataSource = ((List<ProveedorDto>)_servicio.Obtener(cadenaBuscar))
                .Where(x => x.CUIT != "99999999")
                .ToList();

            base.ActualizarDatos(dgv, cadenaBuscar);
        }

        public override void FormatearGrilla(DataGridView dgv)
        {
            base.FormatearGrilla(dgv);

            dgv.Columns["Cuit"].Visible = true;
            dgv.Columns["Cuit"].DisplayIndex = 1;
            dgv.Columns["Cuit"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["Cuit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns["Cuit"].HeaderText = @"CUIT";

            dgv.Columns["RazonSocial"].Visible = true;
            dgv.Columns["RazonSocial"].DisplayIndex = 2;
            dgv.Columns["RazonSocial"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["RazonSocial"].HeaderText = @"Proveedor";

            dgv.Columns["EliminadoStr"].Visible = true;
            dgv.Columns["EliminadoStr"].DisplayIndex = 3;
            dgv.Columns["EliminadoStr"].HeaderText = @"Eliminado";
            dgv.Columns["EliminadoStr"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns["EliminadoStr"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }

        public override bool EjecutarComando(TipoOperacion tipoOperacion, long? id = null)
        {
            var formulario = new _00016_Abm_Proveedor(tipoOperacion, id);

            formulario.ShowDialog();

            return formulario.RealizoAlgunaOperacion;
        }

        public override void dgvGrilla_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            base.dgvGrilla_RowEnter(sender, e);

            proveedor = (ProveedorDto)EntidadSeleccionada;
        }
    }
}
