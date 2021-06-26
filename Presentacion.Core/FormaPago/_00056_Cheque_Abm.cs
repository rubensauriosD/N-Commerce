namespace Presentacion.Core.Cliente
{
    using System;
    using System.Data;
    using System.Linq;
    using System.Windows.Forms;
    using Aplicacion.Constantes;
    using IServicio.FormaPago;
    using IServicio.Persona;
    using IServicio.Persona.DTOs;
    using IServicios.FormaPago;
    using IServicios.FormaPago.DTOs;
    using Presentacion.Core.FormaPago;
    using PresentacionBase.Formularios;
    using StructureMap;

    public partial class _00056_Cheque_Abm : FormAbm
    {
        private readonly IChequeServicio _servicio;
        private readonly IBancoServicio _bancoServicio;
        private readonly IClienteServicio _clienteServicio;
        private readonly Validar Validar;
        private ChequeDto cheque;

        public _00056_Cheque_Abm(TipoOperacion tipoOperacion, long? entidadId = null)
            : base(tipoOperacion, entidadId)
        {
            InitializeComponent();

            _servicio = ObjectFactory.GetInstance<IChequeServicio>();
            _clienteServicio = ObjectFactory.GetInstance<IClienteServicio>();
            _bancoServicio = ObjectFactory.GetInstance<IBancoServicio>();
            Validar = new Validar();

            cheque = entidadId.HasValue 
                ? (ChequeDto)_servicio.Obtener((long)entidadId)
                : new ChequeDto();
        }

        private void _00056_Cheque_Abm_Load(object sender, EventArgs e)
        {
            Validar.ComoNumero(txtNumero, true);
        }

        public override void CargarDatos(long? entidadId)
        {
            base.CargarDatos(entidadId);

            if (entidadId.HasValue)
            {
                var resultado = (ChequeDto)_servicio.Obtener(entidadId.Value);

                if (resultado == null)
                {
                    Mjs.Error("Ocurrio un error al obtener el registro seleccionado.");
                    Close();
                }

                txtNumero.Text = resultado.Numero;
                dtpVencimiento.Value = resultado.FechaVencimiento;
                lblCliente.Text = resultado.Cliente;
                lblBanco.Text = resultado.Banco;

                if (TipoOperacion == TipoOperacion.Eliminar)
                    DesactivarControles(this);
            }
        }

        public override bool VerificarDatosObligatorios()
        {
            if (cheque.ClienteId < 1)
            {
                Mjs.Alerta("Seleccione un cliente.");
                return false;
            }

            if (cheque.BancoId < 1)
            {
                Mjs.Alerta("Seleccione un banco.");
                return false;
            }

            return ValidateChildren();
        }

        // --- Acciones de botones
        public override void EjecutarComandoNuevo()
        {
            var nuevoRegistro = new ChequeDto();
            nuevoRegistro.Numero = txtNumero.Text;
            nuevoRegistro.FechaVencimiento = dtpVencimiento.Value;
            nuevoRegistro.FechaDeposito = null;
            nuevoRegistro.Depositado = false;
            nuevoRegistro.ClienteId = cheque.ClienteId;
            nuevoRegistro.BancoId = cheque.BancoId;
            nuevoRegistro.Eliminado = false;

            _servicio.Insertar(nuevoRegistro);
        }

        public override void EjecutarComandoModificar()
        {
            var modificarRegistro = new ChequeDto();
            modificarRegistro.Id = cheque.Id;
            modificarRegistro.Numero = txtNumero.Text;
            modificarRegistro.FechaVencimiento = dtpVencimiento.Value;
            modificarRegistro.ClienteId = cheque.ClienteId;
            modificarRegistro.BancoId = cheque.BancoId;

            _servicio.Modificar(modificarRegistro);
        }

        public override void EjecutarComandoEliminar()
        {
            _servicio.Eliminar(EntidadId.Value);
        }

        public override void LimpiarControles(object obj, bool tieneValorAsociado = false)
        {
            base.LimpiarControles(obj);

            txtNumero.Focus();
        }

        // --- Cliente
        private void btnNuevoCliente_Click(object sender, EventArgs e)
        {
            var fAltaCliente = new _00010_Abm_Cliente(TipoOperacion.Nuevo);

            fAltaCliente.ShowDialog();

            if (!fAltaCliente.RealizoAlgunaOperacion)
                return;

            var cliente = (ClienteDto)_clienteServicio.Obtener(typeof(ClienteDto), "").OrderByDescending(x => x.Id).First();

            cheque.ClienteId = cliente.Id;
            cheque.Cliente = cliente.ApyNom;

            lblCliente.Text = cheque.Cliente;
        }

        private void btnSeleccionarCliente_Click(object sender, EventArgs e)
        {
            var fSeleccionCliente = new FormBusquedaSeleccion(SetClienteDatosGrilla, SetClienteFormatoGrilla);
            fSeleccionCliente.Titulo = "Clientes";

            fSeleccionCliente.ShowDialog();

            if (!fSeleccionCliente.RealizoSeleccion)
                return;

            var cliente = (ClienteDto)fSeleccionCliente.Seleccion;

            cheque.ClienteId = cliente.Id;
            cheque.Cliente = cliente.ApyNom;

            lblCliente.Text = cheque.Cliente;
        }

        private void SetClienteDatosGrilla(DataGridView dgv, string cadenaBuscar)
            => dgv.DataSource = _clienteServicio.Obtener(typeof(ClienteDto), cadenaBuscar).Where(x => x.Dni != "99999999").ToList();

        private void SetClienteFormatoGrilla(DataGridView dgv)
        {
            dgv.Columns["Dni"].Visible = true;
            dgv.Columns["Dni"].HeaderText = @"Dni";
            dgv.Columns["Dni"].DisplayIndex = 1;

            dgv.Columns["ApyNom"].Visible = true;
            dgv.Columns["ApyNom"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["ApyNom"].HeaderText = @"Cliente";
            dgv.Columns["ApyNom"].DisplayIndex = 2;
        }

        // --- Banco
        private void btnNuevoBanco_Click(object sender, EventArgs e)
        {
            var fAltaBanco = new _00048_Abm_Banco(TipoOperacion.Nuevo);

            fAltaBanco.ShowDialog();

            if (!fAltaBanco.RealizoAlgunaOperacion)
                return;

            var banco = (BancoDto)_bancoServicio.Obtener("").OrderByDescending(x => x.Id).First();

            cheque.BancoId = banco.Id;
            cheque.Banco = banco.Descripcion;

            lblBanco.Text = cheque.Banco;
        }

        private void btnSeleccionarBanco_Click(object sender, EventArgs e)
        {
            var fSeleccionBanco = new FormBusquedaSeleccion(SetBancoDatosGrilla, SetBancoFormatoGrilla);
            fSeleccionBanco.Titulo = "Bancos";

            fSeleccionBanco.ShowDialog();

            if (!fSeleccionBanco.RealizoSeleccion)
                return;

            var banco = (BancoDto)fSeleccionBanco.Seleccion;

            cheque.BancoId = banco.Id;
            cheque.Banco = banco.Descripcion;

            lblBanco.Text = cheque.Banco;
        }

        private void SetBancoDatosGrilla(DataGridView dgv, string cadenaBuscar)
            => dgv.DataSource = _bancoServicio.Obtener(cadenaBuscar)
            .Select(x => (BancoDto)x)
            .ToList();

        private void SetBancoFormatoGrilla(DataGridView dgv)
        {
            dgv.Columns["Descripcion"].Visible = true;
            dgv.Columns["Descripcion"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["Descripcion"].HeaderText = @"Banco";
        }
    }
}
