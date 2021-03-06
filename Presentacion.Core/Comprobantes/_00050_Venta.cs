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
        private readonly Validar Validar;

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
            Validar = new Validar();

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
            Validar.ComoDni(txtClienteDni);
            Validar.ComoCodigoDeVentas(txtCodigo);
            Validar.ComoNumero(txtPrecioUnitario);

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

        // --- Cuerpo del Comprobante
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

        // --- Cabecera del Comprobante
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

        // --- Eventos de Controles
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
            txtVendedor.Text = facturaView.Vendedor.ApyNom.ToUpper();
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

        // --- Eventos de txtCodigo
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
            if (ObtenerCodigoDeBascula(codigo, out string[] codigoBascula))
            {
                // Agregar Item
                if (ObtenerArticuloBascula(codigoBascula[1], codigoBascula[2]))
                { 
                    item.IngresoPorBascula = true;
                    btnAgregarItem.PerformClick();
                }
                return;
            }

            // Codigo de Barra
            if (!int.TryParse(codigo, out int codigoArticulo))
                return;

            // Intento obtener el aritulo
            if (!ObtenerArticuloPorCodigo(codigoArticulo))
            {
                Mjs.Alerta("No se pudo encontrar el árticulo.");
                txtCodigo.Clear();
                txtCodigo.Focus();
                return;
            }

            btnAgregarItem.PerformClick();
        }

        private bool ObtenerCodigoDeBascula(string codigo, out string[] codigoBascula)
        {
            codigoBascula = new string[4];

            if(!configuracion.ActivarBascula || codigo.Length != 13)
                return false;

            codigoBascula[0] = codigo.Substring(0, 4);
            codigoBascula[1] = codigo.Substring(4, 3);
            codigoBascula[2] = codigo.Substring(7, 5);
            codigoBascula[3] = codigo.Substring(12, 1);

            if (codigoBascula[0] != configuracion.CodigoBascula)
                return false;

            return true;
        }

        private bool ObtenerArticuloBascula(string codigoStr, string precioPesoStr)
        {
            if (!int.TryParse(codigoStr, out int codigo))
                return false;

            if (!decimal.TryParse(precioPesoStr, out decimal precioPeso))
                return false;

            // Intento obtener el aritulo
            if (!ObtenerArticuloPorCodigo(codigo))
            {
                Mjs.Alerta("No se pudo encontrar el árticulo.");
                return false;
            }

            if (configuracion.EtiquetaPorPrecio)
                articuloSeleccionado.Precio = precioPeso / 100;

            if (configuracion.EtiquetaPorPeso)
                nudCantidad.Value = precioPeso / 1000;

            return true;
        }

        private bool ObtenerArticuloAlternativo(string codigo)
        {
            var codigoStr = codigo.Substring(0, codigo.IndexOf("*"));
            var precioStr = codigo.Substring(codigo.IndexOf("*") + 1);

            // Obtener el precio alternativo
            if (!decimal.TryParse(precioStr, out decimal precioAlternativo))
            {
                Mjs.Alerta("Error al leer el precio ingresado.");
                return false;
            }

            // Si no ingreso el codigo regreso
            if (string.IsNullOrEmpty(codigoStr))
                return false;

            // Intento obtener el aritulo
            if (!ObtenerArticuloPorCodigo(int.Parse(codigoStr)))
            {
                Mjs.Alerta("No se pudo encontrar el árticulo.");
                return false;
            }

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

            if (!ObtenerArticuloPorCodigo(((ArticuloDto)articuloLookUp.EntidadSeleccionada).Codigo))
            {
                Mjs.Alerta("El código seleccionado no pertenece a un artículo.");
                return;
            }

            btnAgregarItem.PerformClick();
        }

        public bool ObtenerArticuloPorCodigo(int codigo)
        {
            long listaPrecios = listaPrecioSeleccionada.Id;
            long deposito = configuracion.DepositoVentaId;
            ArticuloVentaDto articulo;

            articulo = _articuloServicio.ObtenerPorCodigo(codigo, listaPrecios, deposito);

            if (articulo == null)
                return false;

            articuloSeleccionado = articulo;
            return true;
        }

        // --- Evnto de BtnAgregar
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

            if (!articuloSeleccionado.PermiteStockNegativo
                && stockDisponible < item.Cantidad && !articuloSeleccionado.PermiteStockNegativo)
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
            facturaView.Descuento = 0;

            txtCodigo.Clear();
            txtCodigo.Focus();
        }

        // --- Evento de Grilla
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
            var vendedor = facturaView.Vendedor;

            facturaView = new FacturaView { Vendedor = vendedor };

            facturaView.Cliente = (ClienteDto)_clienteServicio
                .Obtener(typeof(ClienteDto), ConfiguracionPorDefecto.ClienteDni)
                .First();

            permitirIngresarCantidad = false;
            tieneAutorizacionListaPrecio = false;
            CargarCabeceraDelComprobante();
            CargarCuerpoDeComprobante();
            CargarPieDeComprobante();
            LimpiarParaNuevoItem();
        }

        private void btnFacturar_Click(object sender, EventArgs e)
        {
            if (!DatosIngresadosCorrectos())
                return;

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
                Estado = Estado.Pendiente,
                Items = facturaView.Items.Select(iv => new DetalleComprobanteDto() {
                    ArticuloId = iv.ArticuloId,
                    Codigo = iv.Codigo,
                    Descripcion = iv.Descripcion,
                    Cantidad = iv.Cantidad,
                    Iva = iv.Iva,
                    Precio = iv.Precio
                }).ToList()
            };

            var nuevaFacturaId = _facturaServicio.Insertar(nuevaFactura);

            nuevaFactura = _facturaServicio.Obtener(nuevaFacturaId);

            if (!configuracion.PuestoCajaSeparado)
            { 
                var fFormaPago = new _00044_FormaPago(facturaView.Total, facturaView.Cliente.Id, configuracion.TipoFormaPagoPorDefectoVenta);
                fFormaPago.ShowDialog();

                if (fFormaPago.RealizoVenta)
                { 
                    nuevaFactura.FormasDePagos = fFormaPago.FormasPago;
                    _formaPagoServicio.Insertar(nuevaFactura.FormasDePagos, nuevaFactura.Id);
                }
            }

            ImprimirFactura(nuevaFactura, facturaView.Cliente.ApyNom);

            LimpiarParaNuevaFactura();
        }

        private bool DatosIngresadosCorrectos()
        {
            bool ok = true;

            if (cmbComprobanteListaPrecio.Items.Count < 1 || cmbComprobanteListaPrecio.SelectedValue == null)
            {
                Validar.SetErrorProvider(cmbComprobanteListaPrecio, "Seleccione una lista de precios.");
                ok = false;
            }
            else 
                Validar.ClearErrorProvider(cmbComprobanteListaPrecio);

            if (cmbPuestoVenta.Items.Count < 1 || cmbPuestoVenta.SelectedValue == null)
            {
                Validar.SetErrorProvider(cmbPuestoVenta, "Seleccione un puesto de venta.");
                ok = false;
            }
            else 
                Validar.ClearErrorProvider(cmbPuestoVenta);

            if (facturaView.Items.Count < 1)
            {
                Mjs.Alerta("No hay elementos para mostrar.");
                ok = false;
            }

            return ok;
        }

        private void ImprimirFactura(FacturaDto factura, string cliente)
        {
            var observaciones = configuracion.ObservacionEnPieFactura != ""
                ? configuracion.ObservacionEnPieFactura : "---";

            var items = factura.Items
                .Select(i => new InformePresupuestoDetalleDto()
                {
                    Cantidad = i.Cantidad.ToString(),
                    Descripcion = i.Descripcion,
                    Precio = i.Precio.ToString("c"),
                    Subtotal = i.SubTotal.ToString("c")
                })
                .ToList();

            var parametros = new List<ReportParameter>() {
                new ReportParameter("nombreSujeto", cliente.ToUpper()),
                new ReportParameter("NumeroFactura", factura.Numero.ToString("00000")),
                new ReportParameter("SubTotal", factura.SubTotal.ToString("c")),
                new ReportParameter("Iva", (factura.Iva105 + factura.Iva21).ToString("c")),
                new ReportParameter("Descuento", factura.Descuento.ToString("c")),
                new ReportParameter("Total", factura.Total.ToString("C2")),
                new ReportParameter("Observaciones", observaciones)
            };

            var form = new FormBase();
            form.MostrarInforme(
                @"D:\Code\N-Commerce\Presentacion.Core\Informes\InformeFactura.rdlc",
                @"DetalleInforme",
                items,
                parametros);

            form.ShowDialog();
        }

        // --- Presupuesto
        private void btnPresupuesto_Click(object sender, EventArgs e)
        {
            if (facturaView.Items.Count < 1)
            {
                Mjs.Alerta("No hay elementos para mostrar.");
                return;
            }

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
            var observaciones = configuracion.ObservacionEnPieFactura != ""
                ? configuracion.ObservacionEnPieFactura : "---";

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
                new ReportParameter("Total", presupuesto.Total.ToString("C2")),
                new ReportParameter("Observaciones", observaciones)
            };
            
            var form = new FormBase();
            form.MostrarInforme(
                @"D:\Code\N-Commerce\Presentacion.Core\Informes\InformePresupesto.rdlc",
                @"InformePresupuestoDto",
                items,
                parametros);
            
            form.ShowDialog();
        }

        // --- Descuento
        private void nudDescuento_Leave(object sender, EventArgs e)
        {
            facturaView.Descuento = nudDescuento.Value;
            CargarPieDeComprobante();
        }
    }
}
