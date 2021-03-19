namespace Servicios.Persona
{
    using Dominio.UnidadDeTrabajo;
    using IServicio.Persona;

    public class MovimientoCuentaCorrienteProveedorRepositorioServicio : MovimientoServicio, IMovimientoCuentaCorrienteProveedorServicio
    {
        public MovimientoCuentaCorrienteProveedorRepositorioServicio(IUnidadDeTrabajo unidadDeTrabajo) 
            : base(unidadDeTrabajo)
        {

        }

    }
}
