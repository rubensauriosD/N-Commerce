using Aplicacion.Constantes;
using Infraestructura.UnidadDeTrabajo;
using IServicios.Contador;
using System.Linq;

namespace Servicios.Contador
{
    public class ContadorServicio : IContadorServicio
    {
        private readonly UnidadDeTrabajo _udt;

        public ContadorServicio(UnidadDeTrabajo udt)
        {
            _udt = udt;
        }

        public int ObtenerSiguienteNumero(TipoComprobante tipoComprobante)
        {
            var entidad = _udt.ContadorRepositorio
                .Obtener(x => !x.EstaEliminado && x.TipoComprobante == tipoComprobante)
                .FirstOrDefault();

            if (entidad == null)
                throw new System.Exception("No se encontro el contador para el tipo de comprobante.");

            entidad.Valor++;

            _udt.ContadorRepositorio.Modificar(entidad);
            _udt.Commit();

            return entidad.Valor;
        }
    }
}
