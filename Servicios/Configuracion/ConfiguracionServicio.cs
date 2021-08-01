namespace Servicios.Configuracion
{
    using System.Linq;
    using Dominio.UnidadDeTrabajo;
    using IServicio.Configuracion;
    using IServicio.Configuracion.DTOs;

    public class ConfiguracionServicio : IConfiguracionServicio
    {
        private readonly IUnidadDeTrabajo _unidadDeTrabajo;

        public ConfiguracionServicio(IUnidadDeTrabajo unidadDeTrabajo)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
        }

        public void Grabar(ConfiguracionDto configuracionDto)
        {
            var config = configuracionDto.EsPrimeraVez
                ? new Dominio.Entidades.Configuracion()
                : _unidadDeTrabajo.ConfiguracionRepositorio.Obtener(configuracionDto.Id);


            config.Id = configuracionDto.Id;
            config.EstaEliminado = false;
            config.LocalidadId = configuracionDto.LocalidadId;
            config.PermitirArqueoNegativo = configuracionDto.PermitirArqueoNegativo;
            config.ActualizaCostoDesdeCompra = configuracionDto.ActualizaCostoDesdeCompra;
            config.Celular = configuracionDto.Celular;
            config.Cuit = configuracionDto.Cuit;
            config.Direccion = configuracionDto.Direccion;
            config.Email = configuracionDto.Email;
            config.FacturaDescuentaStock = configuracionDto.FacturaDescuentaStock;
            config.IngresoManualCajaInicial = configuracionDto.IngresoManualCajaInicial;
            config.ListaPrecioPorDefectoId = configuracionDto.ListaPrecioPorDefectoId;
            config.NombreFantasia = configuracionDto.NombreFantasia;
            config.ObservacionEnPieFactura = configuracionDto.ObservacionEnPieFactura;
            config.PresupuestoDescuentaStock = configuracionDto.PresupuestoDescuentaStock;
            config.PuestoCajaSeparado = configuracionDto.PuestoCajaSeparado;
            config.RazonSocial = configuracionDto.RazonSocial;
            config.RemitoDescuentaStock = configuracionDto.RemitoDescuentaStock;
            config.Telefono = configuracionDto.Telefono;
            config.TipoFormaPagoPorDefectoCompra = configuracionDto.TipoFormaPagoPorDefectoCompra;
            config.TipoFormaPagoPorDefectoVenta = configuracionDto.TipoFormaPagoPorDefectoVenta;
            config.UnificarRenglonesIngresarMismoProducto = configuracionDto.UnificarRenglonesIngresarMismoProducto;
            config.DepositoNuevoArticuloId = configuracionDto.DepositoNuevoArticuloId;
            config.DepositoVentaId = configuracionDto.DepositoVentaId;
            config.ActivarBascula = configuracionDto.ActivarBascula;
            config.EtiquetaPorPeso = configuracionDto.EtiquetaPorPeso;
            config.EtiquetaPorPrecio = configuracionDto.EtiquetaPorPrecio;
            config.CodigoBascula = configuracionDto.CodigoBascula;
            
            if (configuracionDto.EsPrimeraVez)
            {
                _unidadDeTrabajo.ConfiguracionRepositorio.Insertar(config);
            }
            else
            {
                _unidadDeTrabajo.ConfiguracionRepositorio.Modificar(config);
            }

            _unidadDeTrabajo.Commit();
        }

        public ConfiguracionDto Obtener()
        {
            var config = _unidadDeTrabajo.ConfiguracionRepositorio.Obtener(null, "ListaPrecioPorDefecto, Localidad, Localidad.Departamento, Localidad.Departamento.Provincia")
                .FirstOrDefault();

            if (config == null) return null;

            return new ConfiguracionDto
            {
                Id = config.Id,
                Eliminado = false,
                LocalidadId = config.LocalidadId,
                DepartamentoId = config.Localidad.DepartamentoId,
                ProvinciaId = config.Localidad.Departamento.ProvinciaId,
                PermitirArqueoNegativo = config.PermitirArqueoNegativo,
                ActualizaCostoDesdeCompra = config.ActualizaCostoDesdeCompra,
                Celular = config.Celular,
                Cuit = config.Cuit,
                Direccion = config.Direccion,
                Email = config.Email,
                FacturaDescuentaStock = config.FacturaDescuentaStock,
                IngresoManualCajaInicial = config.IngresoManualCajaInicial,
                ListaPrecioPorDefectoId = config.ListaPrecioPorDefectoId,
                NombreFantasia = config.NombreFantasia,
                ObservacionEnPieFactura = config.ObservacionEnPieFactura,
                PresupuestoDescuentaStock = config.PresupuestoDescuentaStock,
                PuestoCajaSeparado = config.PuestoCajaSeparado,
                RazonSocial = config.RazonSocial,
                RemitoDescuentaStock = config.RemitoDescuentaStock,
                Telefono = config.Telefono,
                TipoFormaPagoPorDefectoCompra = config.TipoFormaPagoPorDefectoCompra,
                TipoFormaPagoPorDefectoVenta = config.TipoFormaPagoPorDefectoVenta,
                UnificarRenglonesIngresarMismoProducto = config.UnificarRenglonesIngresarMismoProducto,
                DepositoNuevoArticuloId = config.DepositoNuevoArticuloId,
                DepositoVentaId = config.DepositoVentaId,
                ActivarBascula = config.ActivarBascula,
                EtiquetaPorPeso = config.EtiquetaPorPeso,
                EtiquetaPorPrecio = config.EtiquetaPorPrecio,
                CodigoBascula = config.CodigoBascula
            };
        }

        public ConfiguracionDto ConfiguracionPorDefecto()
        {
            return new ConfiguracionDto
            {
                Id = 0,
                Eliminado = false,
                EsPrimeraVez = true,

                // TODO: Borrar los datos generales de la configuracion para que los cargue el usuario
                // Genereales
                LocalidadId = 12,
                RazonSocial = "RAZÓN SOCIAL",
                NombreFantasia = "NOMBRE FANTASÍA",
                Cuit = "30991236549",
                Direccion = "DIRECCION",
                Telefono = "381 123 1234",
                Celular = "381 999 1234",
                Email = "mail@dominio.com",
                
                // Compra
                ActualizaCostoDesdeCompra = true,
                TipoFormaPagoPorDefectoCompra = Aplicacion.Constantes.TipoPago.Efectivo,

                // Venta
                FacturaDescuentaStock = true,
                PresupuestoDescuentaStock = false,
                RemitoDescuentaStock = false,
                ListaPrecioPorDefectoId = 1,
                ObservacionEnPieFactura = "",
                PuestoCajaSeparado = true,
                TipoFormaPagoPorDefectoVenta = Aplicacion.Constantes.TipoPago.Efectivo,
                UnificarRenglonesIngresarMismoProducto = true,
                DepositoVentaId = 1,

                // Caja
                IngresoManualCajaInicial = true,
                PermitirArqueoNegativo = false,

                // Stock
                DepositoNuevoArticuloId = 1,
                ActivarBascula = false,
                CodigoBascula = "",
                EtiquetaPorPeso = true,
                EtiquetaPorPrecio = false,
            };

        }
    }
}
