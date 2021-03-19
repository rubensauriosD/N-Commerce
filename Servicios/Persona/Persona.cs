namespace Servicios.Persona
{
    using System.Collections.Generic;
    using Dominio.UnidadDeTrabajo;
    using IServicio.Persona.DTOs;
    using StructureMap;

    public class Persona
    {
        protected readonly IUnidadDeTrabajo _unidadDeTrabajo;

        public Persona()
        {
            _unidadDeTrabajo = ObjectFactory.GetInstance<IUnidadDeTrabajo>();
        }


        public virtual long Insertar(PersonaDto entidad)
        {
            return 0;
        }

        public virtual void Eliminar(long id)
        {

        }

        public virtual IEnumerable<PersonaDto> Obtener(string cadenaBuscar)
        {
            return null;
        }

        public virtual PersonaDto Obtener(long id)
        {
            return null;
        }

        public virtual void Modificar(PersonaDto entidad)
        {

        }
    }
}
