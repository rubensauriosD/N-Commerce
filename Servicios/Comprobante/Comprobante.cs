namespace Servicios.Comprobante
{
    using Dominio.UnidadDeTrabajo;
    using IServicios.Comprobante.DTOs;
    using StructureMap;

    public class Comprobante
    {
        protected readonly IUnidadDeTrabajo _unidadDeTrabajo;

        public Comprobante()
        {
            _unidadDeTrabajo = ObjectFactory.GetInstance<IUnidadDeTrabajo>();
        }

        public virtual long Insertar(ComprobanteDto comprobante)
        {
            return 0;
        }
    }
}
