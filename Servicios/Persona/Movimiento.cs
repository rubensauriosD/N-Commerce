namespace Servicios.Persona
{
    using System.Collections.Generic;
    using Dominio.UnidadDeTrabajo;
    using IServicios.Persona.DTOs;
    using StructureMap;

    public class Movimiento
    {
        protected readonly IUnidadDeTrabajo _unidadDeTrabajo;

        public Movimiento()
        {
            _unidadDeTrabajo = ObjectFactory.GetInstance<IUnidadDeTrabajo>();
        }


        public virtual long Insertar(MovimientoDto entidad)
        {
            return 0;
        }

        public virtual void Eliminar(long id)
        {

        }

        public virtual IEnumerable<MovimientoDto> Obtener(string cadenaBuscar)
        {
            return null;
        }

        public virtual MovimientoDto Obtener(long id)
        {
            return null;
        }

        public virtual void Modificar(MovimientoDto entidad)
        {

        }
    }
}
