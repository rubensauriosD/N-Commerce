namespace Servicios.Persona
{
    using Dominio.UnidadDeTrabajo;
    using IServicio.Persona;
    using IServicio.Persona.DTOs;
    using System;
    using System.Collections.Generic;

    public class PersonaServicio : IPersonaServicio
    {
        protected readonly IUnidadDeTrabajo _unidadDeTrabajo;
        private Dictionary<Type, string> _diccionario;

        public PersonaServicio(IUnidadDeTrabajo unidadDeTrabajo)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
            _diccionario = new Dictionary<Type, string>();

            InicializadorDiccionario();
        }

        private void InicializadorDiccionario()
        {
            _diccionario.Add(typeof(EmpleadoDto), "Servicios.Persona.Empleado");
            _diccionario.Add(typeof(ClienteDto), "Servicios.Persona.Cliente");
        }

        public void AgregarOpcionDiccionario(Type type, string value)
        {
            _diccionario.Add(type, value);
        }

        public long Insertar(PersonaDto entidad)
        {
            var persona = InstanciaPersona(entidad);

            return persona.Insertar(entidad);
        }

        public void Eliminar(Type tipoEntidad, long id)
        {
            var persona = InstanciarPersona(tipoEntidad);

            persona.Eliminar(id);
        }

        public IEnumerable<PersonaDto> Obtener(Type tipo, string cadenaBuscar)
        {
            var persona = InstanciarPersona(tipo);

            return persona.Obtener(cadenaBuscar);
        }

        public PersonaDto Obtener(Type tipo, long id)
        {
            var persona = InstanciarPersona(tipo);

            return persona.Obtener(id);
        }

        public void Modificar(PersonaDto entidad)
        {
            var persona = InstanciaPersona(entidad);

            persona.Modificar(entidad);
        }

        // ====================================================================== //
        // =================         Metodos Privados           ================= //
        // ====================================================================== //

        private Persona InstanciarEntidad(string tipoEntidad)
        {
            var tipoObjeto = Type.GetType(tipoEntidad);

            if (tipoObjeto == null) return null;

            var entidad = Activator.CreateInstance(tipoObjeto) as Persona;

            return entidad;
        }

        private Persona InstanciaPersona(PersonaDto entidad)
        {
            if (!_diccionario.TryGetValue(entidad.GetType(), out var tipoEntidad))
                throw new Exception($"No hay {entidad.GetType()} para Instanciar.");

            var persona = InstanciarEntidad(tipoEntidad);

            if (persona == null) throw new Exception($"Ocurrió un error al Instanciar {entidad.GetType()}");

            return persona;
        }

        private Persona InstanciarPersona(Type tipo)
        {
            if (!_diccionario.TryGetValue(tipo, out var tipoEntidad))
                throw new Exception($"No hay {tipoEntidad} para Instanciar.");

            var persona = InstanciarEntidad(tipoEntidad);

            if (persona == null) throw new Exception($"Ocurrió un error al Instanciar {tipo}");

            return persona;
        }
    }
}
