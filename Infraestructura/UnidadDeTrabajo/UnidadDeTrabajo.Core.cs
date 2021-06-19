namespace Infraestructura.UnidadDeTrabajo
{
    using Dominio.Entidades;
    using Dominio.Repositorio;
    using Infraestructura.Repositorio;

    public partial class UnidadDeTrabajo
    {

        // ============================================================================================================ //

        private IPresupuestoRepositorio presupuestoRepositorio;

        public IPresupuestoRepositorio PresupuestoRepositorio => presupuestoRepositorio
                                                               ?? (presupuestoRepositorio =
                                                                   new PresupuestoRepositorio(_context));

        // ============================================================================================================ //

        private ICompraRepositorio compraRepositorio;

        public ICompraRepositorio CompraRepositorio => compraRepositorio
                                                               ?? (compraRepositorio =
                                                                   new CompraRepositorio(_context));

        // ============================================================================================================ //

        private IFacturaRepositorio facturaRepositorio;

        public IFacturaRepositorio FacturaRepositorio => facturaRepositorio
                                                               ?? (facturaRepositorio =
                                                                   new FacturaRepositorio(_context));

        // ============================================================================================================ //

        private IRepositorio<BajaArticulo> bajaArticuloRepositorio;

        public IRepositorio<BajaArticulo> BajaArticuloRepositorio => bajaArticuloRepositorio
                                                               ?? (bajaArticuloRepositorio =
                                                                   new Repositorio<BajaArticulo>(_context));

        // ============================================================================================================ //

        private IRepositorio<Caja> cajaRepositorio;

        public IRepositorio<Caja> CajaRepositorio => cajaRepositorio
                                                               ?? (cajaRepositorio =
                                                                   new Repositorio<Caja>(_context));

        // ============================================================================================================ //

        private IRepositorio<DetalleCaja> detalleCajaRepositorio;

        public IRepositorio<DetalleCaja> DetalleCajaRepositorio => detalleCajaRepositorio
                                                               ?? (detalleCajaRepositorio =
                                                                   new Repositorio<DetalleCaja>(_context));

        // ============================================================================================================ //

        private IRepositorio<Proveedor> proveedorRepositorio;

        public IRepositorio<Proveedor> ProveedorRepositorio => proveedorRepositorio
                                                               ?? (proveedorRepositorio =
                                                                   new Repositorio<Proveedor>(_context));

        // ============================================================================================================ //

        private IEmpleadoRepositorio empleadoRepositorio;

        public IEmpleadoRepositorio EmpleadoRepositorio => empleadoRepositorio
                                                           ?? (empleadoRepositorio = 
                                                               new EmpleadoRepositorio(_context));

        // ============================================================================================================ //

        private IClienteRepositorio clienteRepositorio;

        public IClienteRepositorio ClienteRepositorio => clienteRepositorio
                                                         ?? (clienteRepositorio =
                                                             new ClienteRepositorio(_context));

        // ============================================================================================================ //

        private IRepositorio<Configuracion> configuracionRepositorio;

        public IRepositorio<Configuracion> ConfiguracionRepositorio => configuracionRepositorio
                                                                       ?? (configuracionRepositorio =
                                                                           new Repositorio<Configuracion>(_context));

        // ============================================================================================================ //

        private IRepositorio<ListaPrecio> listaPrecioRepositorio;

        public IRepositorio<ListaPrecio> ListaPrecioRepositorio => listaPrecioRepositorio
                                                                   ?? (listaPrecioRepositorio =
                                                                       new Repositorio<ListaPrecio>(_context));

        // ============================================================================================================ //

        private IRepositorio<Articulo> articuloRepositorio;

        public IRepositorio<Articulo> ArticuloRepositorio => articuloRepositorio
                                                             ?? (articuloRepositorio =
                                                                 new Repositorio<Articulo>(_context));

        // ============================================================================================================ //
    }
}
