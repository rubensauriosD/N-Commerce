namespace Dominio.UnidadDeTrabajo
{
    using Dominio.Entidades;
    using Dominio.Repositorio;

    public interface IUnidadDeTrabajo
    {
        // Metodos
        void Commit();

        void Disposed();

        // Propiedades

        IMovimientoCuentaCorrienteProveedorRepositorio MovimientoCuentaCorrienteProveedorRepositorio { get; }
        IFacturaRepositorio FacturaRepositorio { get; }
        IClienteRepositorio ClienteRepositorio { get; }
        IEmpleadoRepositorio EmpleadoRepositorio { get; }
        IRepositorio<Articulo> ArticuloRepositorio { get; }
        IRepositorio<Banco> BancoRepositorio { get; }
        IRepositorio<ConceptoGasto> ConceptoGastoRepositorio { get; }
        IRepositorio<CondicionIva> CondicionIvaRepositorio { get; }
        IRepositorio<Configuracion> ConfiguracionRepositorio { get; }
        IRepositorio<Contador> ContadorRepositorio { get; }
        IRepositorio<Caja> CajaRepositorio { get; }
        IRepositorio<Departamento> DepartamentoRepositorio { get; }
        IRepositorio<Deposito> DepositoRepositorio { get; }
        IRepositorio<DetalleCaja> DetalleCajaRepositorio { get; }
        IRepositorio<Gasto> GastoRepositorio { get; }
        IRepositorio<Iva> IvaRepositorio { get; }
        IRepositorio<Localidad> LocalidadRepositorio { get; }
        IRepositorio<ListaPrecio> ListaPrecioRepositorio { get; }
        IRepositorio<Marca> MarcaRepositorio { get; }
        IRepositorio<MotivoBaja> MotivoBajaRepositorio { get; }
        IRepositorio<Rubro> RubroRepositorio { get; }
        IRepositorio<UnidadMedida> UnidadMedidaRepositorio { get; }
        IRepositorio<Usuario> UsuarioRepositorio { get; }
        IRepositorio<Precio> PrecioRepositorio { get; }
        IRepositorio<Provincia> ProvinciaRepositorio { get; }
        IRepositorio<Proveedor> ProveedorRepositorio { get; }
        IRepositorio<PuestoTrabajo> PuestoTrabajoRepositorio { get; }
        IRepositorio<Stock> StockRepositorio { get; }
        IRepositorio<Tarjeta> TarjetaRepositorio { get; }
    }
}
