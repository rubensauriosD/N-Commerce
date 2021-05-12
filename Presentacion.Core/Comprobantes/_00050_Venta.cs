namespace Presentacion.Core.Comprobantes
{
    using System;
    using System.Linq;
    using System.Windows.Forms;
    using StructureMap;
    using Aplicacion.Constantes;
    using IServicio.Articulo;
    using IServicio.Articulo.DTOs;
    using IServicio.Configuracion;
    using IServicio.Configuracion.DTOs;
    using IServicio.ListaPrecio;
    using IServicio.ListaPrecio.DTOs;
    using IServicio.Persona;
    using IServicio.Persona.DTOs;
    using IServicio.PuestoTrabajo;
    using IServicios.Articulo.DTOs;
    using IServicios.Caja;
    using IServicios.Comprobante;
    using IServicios.Comprobante.DTOs;
    using IServicios.Contador;
    using Presentacion.Core.Articulo;
    using Presentacion.Core.Cliente;
    using Presentacion.Core.Comprobantes.Clases;
    using Presentacion.Core.Empleado;
    using Presentacion.Core.FormaPago;
    using PresentacionBase.Formularios;
    using IServicios.FormaPago;
    using IServicios.Informes.DTOs;
    using Microsoft.Reporting.WinForms;
    using System.Collections.Generic;

    public partial class _00050_Venta : FormBase
    {
        private ConfiguracionDto configuracion;
        private ArticuloVentaDto articuloSeleccionado;
        private ListaPrecioDto listaPrecioSeleccionada;
        private FacturaView facturaView;
        private ItemView item;
        private ItemView itemSeleccionado;
        private bool permitirIngresarCantidad;
        private bool tieneAutorizacionListaPrecio;

        private readonly IConfiguracionServicio _configuracionServicio;
        private readonly IClienteServicio _clienteServicio;
        private readonly IEmpleadoServicio _empleadoServicio;
        private readonly IListaPrecioServicio _listaPrecioServicio;
        private readonly IPuestoTrabajoServicio _puestoTrabajoServicio;
        private readonly IArticuloServicio _articuloServicio;
        private readonly IFacturaServicio _facturaServicio;
        private readonly IPresupuestoServicio _presupuestoServicio;
        private readonly IFormaPagoServicios _formaPagoServicio;

        public _00050_Venta(IConfiguracionServicio configuracionServicio,
            IClienteServicio clienteServicio,
            IEmpleadoServicio empleadoServicio,
            IListaPrecioServicio listaPrecioServicio,
            IPresupuestoServicio presupuestoServicio,
            IPuestoTrabajoServicio puestoTrabjoServicio,
            IArticuloServicio articuloServicio,
            IFormaPagoServicios formaPagoServicio,
            IFacturaServicio facturaServicio)
        {
            InitializeComponent();

            item = new ItemView();
            permitirIngresarCantidad = false;
            tieneAutorizacionListaPrecio = false;
            _configuracionServicio = configuracionServicio;
            _empleadoServicio = empleadoServicio;
            _clienteServicio = clienteServicio;
            _listaPrecioServicio = listaPrecioServicio;
            _puestoTrabajoServicio = puestoTrabjoServicio;
            _articuloServicio = articuloServicio;
            _facturaServicio = facturaServicio;
            _presupuestoServicio = presupuestoServicio;
            _formaPagoServicio = formaPagoServicio;

            listaPrecioSeleccionada = new ListaPrecioDto();
            articuloSeleccionado = new ArticuloVentaDto();
            facturaView = new FacturaView();
            facturaView.Cliente = (ClienteDto)_clienteServicio.Obtener(typeof(ClienteDto),
                ConfiguracionPorDefecto.ClienteDni).First();
            facturaView.Vendedor = (EmpleadoDto)_empleadoServicio.Obtener(typeof(EmpleadoDto),
                Identidad.EmpleadoId);
            configuracion = _configuracionServicio.Obtener();

            if (configuracion == null)
            {
                Mjs.Alerta("Debe cargar la configuración antes de poder acceder.");
                Close();
            }

            _articuloServicio = articuloServicio;
        }

        private void _00050_Venta_Load(object sender, EventArgs e)
        {
            if (!ObjectFactory.GetInstance<ICajaServicio>().VerificarSiExisteCajaAbierta(Identidad.UsuarioId))
            {
                Mjs.Alerta($"No hay una caja abierta.{Environment.NewLine}Por favor abra una caja para poder emitir comprobantes.");
                Close();
            }

            // Setear Controles
            CargarCabeceraDelComprobante();

            CargarCuerpoDeComprobante();

            CargarPieDeComprobante();

            txtCodigo.Focus();
        }

        private void CargarPieDeComprobante()
        {
            lblSubTotal.Text = facturaView.SubtotalStr;
            nudDescuento.Value = facturaView.Descuento;
            lblTotal.Text = facturaView.TotalStr;
        }

        //
        // Cuerpo del Comprobante
        //
        private void CargarCuerpoDeComprobante()
        {
            dgvGrilla.DataSource = facturaView.Items.OrderByDescending(x => x.Id).ToList();
            FormatearGrilla(dgvGrilla);

            var ultimoItem = facturaView.Items.LastOrDefault();

            if (ultimoItem == null)
            {
                lblDescripcion.Text = string.Empty;
                lblPrecioPorCantidad.Text = string.Empty;
                return;
            }

            lblDescripcion.Text = ultimoItem.Descripcion;
            lblPrecioPorCantidad.Text = $"{ultimoItem.Cantidad} X {ultimoItem.PrecioStr} = {ultimoItem.SubTotalStr}";
        }

        public override void FormatearGrilla(DataGridView dgv)
        {
            base.FormatearGrilla(dgv);

            dgv.Columns["Codigo"].Visible = true;
            dgv.Columns["Codigo"].DisplayIndex = 1;
            dgv.Columns["Codigo"].HeaderText = "Cod";
            dgv.Columns["Codigo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["Codigo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgv.Columns["Descripcion"].Visible = true;
            dgv.Columns["Descripcion"].DisplayIndex = 2;
            dgv.Columns["Descripcion"].HeaderText = "Descripción";
            dgv.Columns["Descripcion"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["Descripcion"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgv.Columns["Cantidad"].Visible = true;
            dgv.Columns["Cantidad"].DisplayIndex = 3;
            dgv.Columns["Cantidad"].HeaderText = "Cant";
            dgv.Columns["Cantidad"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["Cantidad"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgv.Columns["PrecioStr"].Visible = true;
            dgv.Columns["PrecioStr"].DisplayIndex = 4;
            dgv.Columns["PrecioStr"].HeaderText = "Precio";
            dgv.Columns["PrecioStr"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["PrecioStr"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgv.Columns["Iva"].Visible = true;
            dgv.Columns["Iva"].DisplayIndex = 5;
            dgv.Columns["Iva"].HeaderText = "Iva";
            dgv.Columns["Iva"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["Iva"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgv.Columns["SubtotalStr"].Visible = true;
            dgv.Columns["SubtotalStr"].DisplayIndex = 6;
            dgv.Columns["SubtotalStr"].HeaderText = "Subtotal";
            dgv.Columns["SubtotalStr"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["SubtotalStr"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        //
        // Cabecera del Comprobante
        //
        private void CargarCabeceraDelComprobante()
        {
            CargarDatosCliente();

            lblFechaActual.Text = DateTime.Now.ToString("d");

            PoblarComboBox(
                cmbComprobanteListaPrecio,
                _listaPrecioServicio.Obtener(string.Empty, false),
                "Descripcion", "Id");
            cmbComprobanteListaPrecio.SelectedValue = configuracion.ListaPrecioPorDefectoId;
            listaPrecioSeleccionada = (ListaPrecioDto)cmbComprobanteListaPrecio.SelectedItem;

            PoblarComboBox(cmbComprobanteTipo, Enum.GetValues(typeof(TipoComprobante)));
            cmbComprobanteTipo.SelectedItem = TipoComprobante.B;

            PoblarComboBox(
                cmbPuestoVenta,
                _puestoTrabajoServicio.Obtener(string.Empty, false),
                "Descripcion", "Id"
                );

            CargarDatosVendedor();
        }

        private void CargarDatosVendedor()
        {
            if (facturaView.Vendedor == null)
            {
                Mjs.Alerta("Por favor seleccione un vendedor.");
                return;
            }

            txtVendedor.Text = facturaView.Vendedor.ApyNom;
        }

        private void CargarDatosCliente()
        {
            if (facturaView.Cliente == null)
                return;

            txtClienteDni.Text = facturaView.Cliente.Dni;
            txtClienteNombre.Text = facturaView.Cliente.ApyNom;
            txtClienteDomicilio.Text = facturaView.Cliente.Direccion;
            txtClienteTelefono.Text = facturaView.Cliente.Telefono;
            txtClienteCondicionIva.Text = facturaView.Cliente.CondicionIva;
        }

        //
        // Eventos de Controles
        //
        private void btnClienteBuscar_Click(object sender, EventArgs e)
        {
            var f = new ClienteLookUp();
            f.ShowDialog();

            if (!f.RealizoSeleccion)
                return;

            facturaView.Cliente = (ClienteDto)f.EntidadSeleccionada;
            CargarDatosCliente();
        }

        private void btnBuscarVendedor_Click(object sender, EventArgs e)
        {
            var lookUpEmpleado = ObjectFactory.GetInstance<EmpleadoLookUp>();
            lookUpEmpleado.ShowDialog();

            if (!lookUpEmpleado.RealizoSeleccion)
                return;

            facturaView.Vendedor = (EmpleadoDto)lookUpEmpleado.EntidadSeleccionada;
        }

        private void cmbComprobanteListaPrecio_SelectionChangeCommitted(object sender, EventArgs e)
        {
            var lstPrecio = (ListaPrecioDto)cmbComprobanteListaPrecio.SelectedItem;

            // Solicito autorizacion para la lista precio
            if (lstPrecio.NecesitaAutorizacion)
            { 
                var fPedirAutorizacion = ObjectFactory.GetInstance<ListaPrecioAutorizacion>();
                fPedirAutorizacion.ShowDialog();

                tieneAutorizacionListaPrecio = fPedirAutorizacion.PuedeAcceder;

                // Si no tengo la autorizacion no guardo la seleccion
                if (!tieneAutorizacionListaPrecio)
                {
                    cmbComprobanteListaPrecio.SelectedValue = listaPrecioSeleccionada.Id;
                    Mjs.Alerta("No tiene autorización para utilizar la lista de precio.");
                    return;
                }
            }

            // Guardo la lista de precio seleccionada
            listaPrecioSeleccionada = lstPrecio;
        }

        private void txtClienteDni_Leave(object sender, EventArgs e)
        {
            txtClienteDni.Text = facturaView.Cliente.Dni;
        }

        private void nudCantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                permitirIngresarCantidad = false;
                btnAgregarItem.PerformClick();
            }

        }

        //
        // Eventos de txtCodigo
        //
        private void txtCodigo_KeyPress(object sender, KeyPressEventArgs e)
        {
            string codigo = txtCodigo.Text;

            if (codigo == string.Empty || e.KeyChar != (char)Keys.Enter)
                return;

            // Codigo con *
            if (codigo.Contains("*") && ObtenerArticuloAlternativo(codigo))
            {
                // Agregar item
                item.EsArticuloAlternativo = true;
                btnAgregarItem.PerformClick();
                return;
            }

            // Codigo con Bascula
            if (configuracion.ActivarBascula
                && codigo.Length == 13
                && configuracion.CodigoBascula == codigo.Substring(0, 4)
                && ObtenerArticuloBascula(codigo))
            {
                // Agregar Item
                item.IngresoPorBascula = true;
                btnAgregarItem.PerformClick();
                return;
            }

            // Codigo de Barra
            articuloSeleccionado = _articuloServicio.ObtenerPorCodigo(txtCodigo.Text,
                listaPrecioSeleccionada.Id,
                configuracion.DepositoVentaId);

            if (articuloSeleccionado == null)
            {
                Mjs.Error("Código no encontrado.");
                txtCodigo.Clear();
                txtCodigo.Focus();
                return;
            }

            btnAgregarItem.PerformClick();
        }

        private bool ObtenerArticuloBascula(string codigo)
        {
            if (!int.TryParse(codigo.Substring(4, 3), out int codigoArticulo)
                || !decimal.TryParse(codigo.Substring(7, 5), out decimal precioPeso))
                return false;

            // Intento obtener el aritulo
            articuloSeleccionado = _articuloServicio.ObtenerPorCodigo(
                codigoArticulo.ToString(),
                listaPrecioSeleccionada.Id,
                configuracion.DepositoVentaId);

            if (articuloSeleccionado == null)
                return false;

            if (configuracion.EtiquetaPorPrecio)
                articuloSeleccionado.Precio = precioPeso / 100;

            if (configuracion.EtiquetaPorPeso)
                nudCantidad.Value = precioPeso / 1000;

            return true;
        }

        private bool ObtenerArticuloAlternativo(string codigo)
        {
            var codigoBuscar = codigo.Substring(0, codigo.IndexOf("*"));

            // Si no ingreso el codigo regreso
            if (string.IsNullOrEmpty(codigoBuscar))
                return false;

            // Intento obtener el aritulo
            articuloSeleccionado = _articuloServicio.ObtenerPorCodigo(codigoBuscar,
                listaPrecioSeleccionada.Id,
                configuracion.DepositoVentaId);

            // Si no se pudo obtener el articulo
            if (articuloSeleccionado == null)
                return false;

            // Obtener el precio alternativo
            if (!decimal.TryParse(codigo.Substring(codigo.IndexOf("*") + 1), out decimal precioAlternativo))
                return false;

            articuloSeleccionado.Precio = precioAlternativo;

            return true;
        }

        private void txtCodigo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 116)
                permitirIngresarCantidad = !permitirIngresarCantidad;
    
            if (e.KeyValue == 119)
                EjecutarBusquedaDeArticulo();
        }

        private void EjecutarBusquedaDeArticulo()
        {
            var articuloLookUp = ObjectFactory.GetInstance<ArticuloLookUp>();
            articuloLookUp.ShowDialog();

            if (articuloLookUp.EntidadSeleccionada == null)
                return;

            articuloSeleccionado = _articuloServicio.ObtenerPorCodigo(
                ((ArticuloDto)articuloLookUp.EntidadSeleccionada).Codigo.ToString(),
                listaPrecioSeleccionada.Id,
                configuracion.DepositoVentaId);

            btnAgregarItem.PerformClick();
        }

        //
        // Evnto de BtnAgregar
        //
        private void btnAgregarItem_Click(object sender, EventArgs e)
        {
            // Si no hay un articulo seleccionado
            if (articuloSeleccionado == null)
            {
                Mjs.Alerta("No se seleccionó ningún artículo.");
                return;
            }

            // Si permito modificar la cantidad la habilito
            if (permitirIngresarCantidad)
            {
                nudCantidad.Enabled = permitirIngresarCantidad;
                txtDescripcion.Text = articuloSeleccionado.Descripcion;
                txtPrecioUnitario.Text = articuloSeleccionado.PrecioStr;

                nudCantidad.Focus();
                nudCantidad.Select();
                return;
            }

            // Agregar Items
            AgregarItem(item);

            // Preparar controles para nueva carga
            LimpiarParaNuevoItem();
        }

        private void AgregarItem(ItemView item)
        {
            // Extraer datos
            item.ArticuloId = articuloSeleccionado.Id;
            item.ListaPrecioId = listaPrecioSeleccionada.Id;
            item.Codigo = articuloSeleccionado.Codigo;
            item.Descripcion = articuloSeleccionado.Descripcion;
            item.Precio = articuloSeleccionado.Precio;
            item.Cantidad = nudCantidad.Value;
            item.Iva = articuloSeleccionado.Iva;

            // Chequear restricciones
            if (!ValidarRestricciones(item))
                return;

            // Agregar el item
            facturaView.AgregarItem(item, configuracion.UnificarRenglonesIngresarMismoProducto);
        }

        private bool ValidarRestricciones(ItemView item)
        {
            // Verificar Stock
            var stockDisponible = articuloSeleccionado.Stock
                - facturaView.Items.Where(x => x.ArticuloId == item.ArticuloId).Sum(x => x.Cantidad);

            if (stockDisponible < item.Cantidad && !articuloSeleccionado.PermiteStockNegativo)
            {
                Mjs.Alerta($"Solo hay {stockDisponible} de {item.Descripcion} en stock.");
                return false;
            }

            // Verificar Restriccion de cantidad
            var cantidadActual = facturaView.Items
                .Where(x => x.ArticuloId == articuloSeleccionado.Id)
                .Sum(x => x.Cantidad) + item.Cantidad;

            if (articuloSeleccionado.TieneRestriccionPorCantidad
                && cantidadActual > articuloSeleccionado.LimiteVenta)
            {
                Mjs.Alerta($"El artículo {item.Descripcion} tiene un límite de {articuloSeleccionado.LimiteVenta} por persona.");
                return false;
            }

            // Verificar Restricción Horaria
            int minutosOperacion = DateTime.Now.Minute + DateTime.Now.Hour * 60;
            int minutosDesde = articuloSeleccionado.HoraDesde.Minute + articuloSeleccionado.HoraDesde.Hour * 60;
            int minutosHasta = articuloSeleccionado.HoraHasta.Minute + articuloSeleccionado.HoraHasta.Hour * 60;

            bool estaDentroDeLaRestriccion = minutosDesde > minutosHasta
                ? minutosOperacion > minutosDesde || minutosHasta > minutosOperacion
                : minutosDesde < minutosOperacion ? minutosOperacion < minutosHasta : false;

            if (articuloSeleccionado.TieneRestriccionHorario
                && estaDentroDeLaRestriccion)
            {
                Mjs.Alerta($"La venta de {item.Descripcion} se encuentra restringida en este horario.");
                return false;
            }

            return true;
        }

        private void LimpiarParaNuevoItem()
        {
            itemSeleccionado = null;
            item = new ItemView();
            item.EsArticuloAlternativo = false;
            item.IngresoPorBascula = false;

            CargarCuerpoDeComprobante();
            CargarPieDeComprobante();

            permitirIngresarCantidad = false;
            articuloSeleccionado = null;

            txtDescripcion.Clear();
            txtPrecioUnitario.Clear();
            txtDescripcion.Enabled = false;
            txtPrecioUnitario.Enabled = false;
            nudCantidad.Value = 1;
            nudCantidad.Enabled = false;
            txtSubTotalLinea.Clear();
            txtSubTotalLinea.Enabled = false;

            txtCodigo.Clear();
            txtCodigo.Focus();
        }

        //
        // Evento de Grilla
        //
        private void dgvGrilla_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvGrilla.RowCount < 1)
            {
                itemSeleccionado = null;
                return;
            }

            itemSeleccionado = (ItemView)dgvGrilla.Rows[e.RowIndex].DataBoundItem;
        }

        private void dgvGrilla_DoubleClick(object sender, EventArgs e)
        {
            if (dgvGrilla.RowCount < 1 || itemSeleccionado == null)
                return;

            if (item.EsArticuloAlternativo || item.IngresoPorBascula)
            {
                Mjs.Alerta("El artículo no se puede modificar.");
                return;
            }

            var fModificarItem = new ItemComprobanteModificarEliminar(itemSeleccionado);
            fModificarItem.ShowDialog();

            // Si deseo eliminar el item
            if (fModificarItem.EliminarItemId.HasValue)
                facturaView.EliminarItem(fModificarItem.EliminarItemId);

            LimpiarParaNuevoItem();
        }

        private void btnEliminarItem_Click(object sender, EventArgs e)
        {
            if (dgvGrilla.RowCount < 1 || itemSeleccionado == null)
                return;

            if (!Mjs.Preguntar("¿Seguro que desea elminar el item selecciondo?"))
                return;

            facturaView.EliminarItem(itemSeleccionado.Id);
            LimpiarParaNuevoItem();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (!Mjs.Preguntar($"Todos los datos se perderán.{Environment.NewLine} ¿Seguro que desea cancelar la facturación?"))
                return;

            LimpiarParaNuevaFactura();
        }

        private void LimpiarParaNuevaFactura()
        {
            facturaView = new FacturaView();
            permitirIngresarCantidad = false;
            tieneAutorizacionListaPrecio = false;
            CargarCabeceraDelComprobante();
            CargarCuerpoDeComprobante();
            CargarPieDeComprobante();
            LimpiarParaNuevoItem();
        }

        private void btnFacturar_Click(object sender, EventArgs e)
        {
            facturaView.TipoComprobante = (TipoComprobante)cmbComprobanteTipo.SelectedItem;
            facturaView.PuestoVentaId = (long)cmbPuestoVenta.SelectedValue;
            facturaView.UsuarioId = Identidad.UsuarioId;

            var nuevaFactura = new FacturaDto() {
                ClienteId = facturaView.Cliente.Id,
                PuestoTrabajoId = facturaView.PuestoVentaId,
                // ---
                EmpleadoId = facturaView.Vendedor.Id,
                UsuarioId = facturaView.UsuarioId,
                Descuento = facturaView.Descuento,
                Iva21 = 0m,
                Iva105 = 0m,
                TipoComprobante = facturaView.TipoComprobante,
                Items = facturaView.Items.Select(iv => new DetalleComprobanteDto() {
                    ArticuloId = iv.ArticuloId,
                    Codigo = iv.Codigo,
                    Descripcion = iv.Descripcion,
                    Cantidad = iv.Cantidad,
                    Iva = iv.Iva,
                    Precio = iv.Precio
                }).ToList()
            };

            nuevaFactura.Id = _facturaServicio.Insertar(nuevaFactura);

            if (!configuracion.PuestoCajaSeparado)
            { 
                var fFormaPago = new _00044_FormaPago(facturaView.Total, facturaView.Cliente.Id);
                fFormaPago.ShowDialog();

                if (fFormaPago.RealizoVenta)
                    nuevaFactura.FormasDePagos = fFormaPago.FormasPago;
            }

            _formaPagoServicio.Insertar(nuevaFactura.FormasDePagos, nuevaFactura.Id);

            LimpiarParaNuevaFactura();
        }

        // --- Presupuesto
        private void btnPresupuesto_Click(object sender, EventArgs e)
        {
            if (configuracion.PresupuestoDescuentaStock)
                if (!Mjs.Preguntar($"Emitir el presupuesto puede alterar el stock de algunos productos.{Environment.NewLine}¿Seguro que desea continuar?"))
                    return;

            var nuevoPresupuesto = new PresupuestoDto()
            {
                ClienteId = facturaView.Cliente.Id,
                Cliente = facturaView.Cliente.ApyNom,
                EmpleadoId = facturaView.Vendedor.Id,
                UsuarioId = Identidad.UsuarioId,
                Descuento = facturaView.Descuento,
                Iva21 = 0m,
                Iva105 = 0m,
                TipoComprobante = TipoComprobante.Presupuesto,
                Items = facturaView.Items.Select(iv => new DetalleComprobanteDto()
                {
                    ArticuloId = iv.ArticuloId,
                    Codigo = iv.Codigo,
                    Descripcion = iv.Descripcion,
                    Cantidad = iv.Cantidad,
                    Iva = iv.Iva,
                    Precio = iv.Precio
                }).ToList()
            };

            nuevoPresupuesto.Id = _presupuestoServicio.Insertar(nuevoPresupuesto);
            
            nuevoPresupuesto.Numero = _presupuestoServicio.ObtenerNumeroPresupuesto(nuevoPresupuesto.Id);

            ImprimirPresupuesto(nuevoPresupuesto);
            LimpiarParaNuevaFactura();
        }

        private void ImprimirPresupuesto(PresupuestoDto presupuesto)
        {
            var items = presupuesto.Items
                .Select(i => new InformePresupuestoDetalleDto()
                {
                    Cantidad = i.Cantidad.ToString(),
                    Descripcion = i.Descripcion,
                    Precio = i.Precio.ToString("c"),
                    Subtotal = i.SubTotal.ToString("c")
                })
                .ToList();

            var parametros = new List<ReportParameter>() {
                new ReportParameter("nombreSujeto", presupuesto.Cliente.ToUpper()),
                new ReportParameter("NumeroPresupuesto", presupuesto.Numero.ToString("00000")),
                new ReportParameter("SubTotal", presupuesto.SubTotal.ToString("c")),
                new ReportParameter("Iva", (presupuesto.Iva105 + presupuesto.Iva21).ToString("c")),
                new ReportParameter("Descuento", presupuesto.Descuento.ToString("c")),
                new ReportParameter("Total", presupuesto.Total.ToString("C2"))
            };
            
            var form = new FormBase();
            form.MostrarInforme(
                @"D:\Code\N-Commerce\Presentacion.Core\Informes\InformePresupesto.rdlc",
                @"InformePresupuestoDto",
                items,
                parametros);
            
            form.ShowDialog();
        }
    }
}
