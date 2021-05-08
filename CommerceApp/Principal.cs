namespace CommerceApp
{
    using System;
    using System.Windows.Forms;
    using Aplicacion.Constantes;
    using Presentacion.Core.Articulo;
    using Presentacion.Core.Caja;
    using Presentacion.Core.Cliente;
    using Presentacion.Core.Comprobantes;
    using Presentacion.Core.CondicionIva;
    using Presentacion.Core.Configuracion;
    using Presentacion.Core.Departamento;
    using Presentacion.Core.Empleado;
    using Presentacion.Core.Localidad;
    using Presentacion.Core.Provincia;
    using Presentacion.Core.Usuario;
    using PresentacionBase.Formularios;
    using IServicios.Caja;
    using StructureMap;
    using Presentacion.Core.FormaPago;
    using Presentacion.Core.Proveedor;

    public partial class Principal : Form
    {
        private readonly ICajaServicio _cajaServicio;

        public Principal()
        {
            InitializeComponent();

            _cajaServicio = ObjectFactory.GetInstance<ICajaServicio>();
        }

        private void consultaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00001_Provincia>().ShowDialog();
        }

        private void consultaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00003_Departamento>().ShowDialog();
        }

        private void consultaToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00005_Localidad>().ShowDialog();
        }

        private void btnAdministrarClientes_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00009_Cliente>().ShowDialog();
        }

        private void btnConfiguracion_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00012_Configuracion>().ShowDialog();

        }

        private void btnCondicionIva_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00013_CondicionIva>().ShowDialog();
        }

        private void btnEmpleado_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00007_Empleado>().ShowDialog();
        }

        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00011_Usuario>().ShowDialog();
        }

        private void btnPuestoTrabajo_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00051_PuestoTrabajo>().ShowDialog();
        }

        private void btnListasPrecio_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00032_ListaPrecio>().ShowDialog();
        }

        private void btnIva_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00025_Iva>().ShowDialog();
        }

        private void btnMarca_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00021_Marca>().ShowDialog();
        }

        private void btnUnidadMedida_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00023_UnidadDeMedida>().ShowDialog();
        }

        private void btnConceptoGasto_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00041_ConceptoGastos>().ShowDialog();
        }

        private void btnGasto_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00043_Gastos>().ShowDialog();
        }

        private void btnArticulo_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00017_Articulo>().ShowDialog();
        }

        private void Principal_Load(object sender, EventArgs e)
        {
            picFotoUsuario.Image = Identidad.Foto;
            lblNombreUsuario.Text = Identidad.Nombre;
            lblApellidoEmpleado.Text = Identidad.Apellido;
        }

        private void lnkCambiarPassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Mjs.Info("Servicio no disponible.");
        }

        private void lnkCambiarFoto_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Mjs.Info("Servicio no disponible.");
        }

        private void btnVenta_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00050_Venta>().ShowDialog();
        }

        private void btnAdministrarCajas_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00038_Caja>().ShowDialog();
        }

        private void btnAperturaCaja_Click(object sender, EventArgs e)
        {
            // Verificar que la caja no este abierta
            if (_cajaServicio.VerificarSiExisteCajaAbierta(Identidad.UsuarioId))
            {
                Mjs.Alerta("El usuario ya tiene una caja abierta");
                return;
            }

            ObjectFactory.GetInstance<_00039_AperturaCaja>().ShowDialog();
        }

        private void btnCerrarCaja_Click(object sender, EventArgs e)
        {
            // Verificar que la caja este abierta
            if (!_cajaServicio.VerificarSiExisteCajaAbierta(Identidad.UsuarioId))
            {
                Mjs.Alerta("No hay una caja abierta.");
                return;
            }

            var cajaActiva = ObjectFactory.GetInstance<ICajaServicio>().ObtenerCajaAciva(Identidad.UsuarioId);
            new _00040_CierreCaja(cajaActiva).ShowDialog();
        }

        private void btnBanco_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00047_Banco>().ShowDialog();
        }

        private void btnTarjeta_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00045_Tarjeta>().ShowDialog();
        }

        private void btnPagosPendientes_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00049_CobroDiferido>().ShowDialog();
        }

        private void btnModificarPrecioArticulos_Click(object sender, EventArgs e)
        {
            new _00031_ActualizarPrecios().ShowDialog();
        }

        private void proveedoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00015_Proveedor>().ShowDialog();

        }

        private void btnComprobanteCompra_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00053_Compra>().ShowDialog();
        }

        private void btnCheque_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00055_Cheque>().ShowDialog();
        }

        private void btnAdministrarDepositos_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00050_Deposito>().ShowDialog();
        }

        private void btnDepositoTransferencias_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00058_Deposito_Transferencia>().ShowDialog();
        }
    }
}
