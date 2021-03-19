namespace Servicios.Persona
{
    using System;
    using System.Linq;
    using Dominio.UnidadDeTrabajo;
    using IServicio.Persona;

    public class EmpleadoServicio : PersonaServicio, IEmpleadoServicio
    {
        public EmpleadoServicio(IUnidadDeTrabajo unidadDeTrabajo) 
            : base(unidadDeTrabajo)
        {
        }

        public int ObtenerSiguienteLegajo()
        {
            var empleados = _unidadDeTrabajo.EmpleadoRepositorio.Obtener();

            return empleados.Any()
                ? empleados.Max(x => x.Legajo) + 1
                : 1;
        }

        public bool VerificarSiExisteDni(string dni, long? entidadId = null)
        {
            return entidadId.HasValue
                ? _unidadDeTrabajo.EmpleadoRepositorio.Obtener(x => !x.EstaEliminado
                                                                        && x.Id != entidadId.Value
                                                                        && x.Dni.Equals(dni,
                                                                            StringComparison.CurrentCultureIgnoreCase))
                    .Any()
                : _unidadDeTrabajo.EmpleadoRepositorio.Obtener(x => !x.EstaEliminado
                                                                        && x.Dni.Equals(dni,
                                                                            StringComparison.CurrentCultureIgnoreCase))
                    .Any();
        }
    }
}
