namespace Aplicacion.IoC
{
    using System.Data.Entity;
    using Dominio.Repositorio;
    using Dominio.UnidadDeTrabajo;
    using Infraestructura.Repositorio;
    using Infraestructura.UnidadDeTrabajo;
    using IServicio.Articulo;
    using IServicio.Caja;
    using IServicio.Configuracion;
    using IServicio.Departamento;
    using IServicio.Deposito;
    using IServicio.FormaPago;
    using IServicio.Iva;
    using IServicio.ListaPrecio;
    using IServicio.Localidad;
    using IServicio.Marca;
    using IServicio.Persona;
    using IServicio.Provincia;
    using IServicio.PuestoTrabajo;
    using IServicio.Rubro;
    using IServicio.Seguridad;
    using IServicio.UnidadMedida;
    using IServicio.Usuario;
    using IServicios.Caja;
    using IServicios.Comprobante;
    using IServicios.Contador;
    using IServicios.FormaPago;
    using Servicios.Articulo;
    using Servicios.Caja;
    using Servicios.Comprobante;
    using Servicios.ConceptoGasto;
    using Servicios.CondicionIva;
    using Servicios.Configuracion;
    using Servicios.Contador;
    using Servicios.Departamento;
    using Servicios.Deposito;
    using Servicios.FormaPago;
    using Servicios.Iva;
    using Servicios.ListaPrecio;
    using Servicios.Localidad;
    using Servicios.Marca;
    using Servicios.Persona;
    using Servicios.Provincia;
    using Servicios.PuestoTrabajo;
    using Servicios.Rubro;
    using Servicios.Seguridad;
    using Servicios.UnidadMedida;
    using Servicios.Usuario;
    using StructureMap;

    public class StructureMapContainer
    {
        public void Configure()
        {
            ObjectFactory.Configure(x =>
            {
                x.For(typeof(IRepositorio<>)).Use(typeof(Repositorio<>));

                x.ForSingletonOf<DbContext>();

                x.For<IUnidadDeTrabajo>().Use<UnidadDeTrabajo>();

                // =================================================================== //

                x.For<IProvinciaServicio>().Use<ProvinciaServicio>();

                x.For<IDepartamentoServicio>().Use<DepartamentoServicio>();

                x.For<ILocalidadServicio>().Use<LocalidadServicio>();

                x.For<ICondicionIvaServicio>().Use<CondicionIvaServicio>();

                x.For<IPersonaServicio>().Use<PersonaServicio>();

                x.For<IClienteServicio>().Use<ClienteServicio>();

                x.For<IEmpleadoServicio>().Use<EmpleadoServicio>();

                x.For<IUsuarioServicio>().Use<UsuarioServicio>();

                x.For<ISeguridadServicio>().Use<SeguridadServicio>();

                x.For<IConfiguracionServicio>().Use<ConfiguracionServicio>();

                x.For<IListaPrecioServicio>().Use<ListaPrecioServicio>();

                x.For<IDepositoSevicio>().Use<DepositoServicio>();

                x.For<IPuestoTrabajoServicio>().Use<PuestoTrabajoServicio>();

                x.For<IIvaServicio>().Use<IvaServicio>();

                x.For<IMarcaServicio>().Use<MarcaServicio>();

                x.For<IUnidadMedidaServicio>().Use<UnidadMedidaServicio>();

                x.For<IConceptoGastoServicio>().Use<ConceptoGastoServicio>();

                x.For<IGastoServicio>().Use<GastoServicio>();

                x.For<IArticuloServicio>().Use<ArticuloServicio>();

                x.For<IRubroServicio>().Use<RubroServicio>();

                x.For<IContadorServicio>().Use<ContadorServicio>();

                x.For<ICajaServicio>().Use<CajaServicio>();

                x.For<IDetalleCajaServicio>().Use<DetalleCajaServicio>();

                x.For<IBancoServicio>().Use<BancoServicio>();

                x.For<IChequeServicio>().Use<ChequeServicio>();

                x.For<ITarjetaServicio>().Use<TarjetaServicio>();

                x.For<IFacturaServicio>().Use<FacturaServicio>();

                x.For<IPresupuestoServicio>().Use<PresupuestoServicio>();

                x.For<ICompraServicio>().Use<CompraServicio>();

                x.For<IFormaPagoServicios>().Use<FormaPagoServicios>();

                x.For<IProveedorServicio>().Use<ProveedorServicios>();

            });
        }
    }
}
