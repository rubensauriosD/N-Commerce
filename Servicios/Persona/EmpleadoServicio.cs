namespace Servicios.Persona
{
    using System;
    using System.Linq;
    using Aplicacion.Constantes;
    using Dominio.UnidadDeTrabajo;
    using IServicio.Persona;

    public class EmpleadoServicio : PersonaServicio, IEmpleadoServicio
    {
        public EmpleadoServicio(IUnidadDeTrabajo unidadDeTrabajo) 
            : base(unidadDeTrabajo)
        {
        }

        public bool ModificarFoto(long id, byte[] foto)
        {
            try
            {
                var empleado = _unidadDeTrabajo.EmpleadoRepositorio.Obtener(id);
                empleado.Foto = foto;

                if (empleado == null)
                {
                    Mjs.Alerta("Error al encontrar el empleado.");
                    return false;
                }

                _unidadDeTrabajo.EmpleadoRepositorio.Modificar(empleado);
                _unidadDeTrabajo.Commit();

                return true;
            }
            catch (Exception e)
            {
                throw new Exception($"Error en EmpleadoServicio.ModificarFoto:{Environment.NewLine}{e.Message}");
                return false;
            }

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
