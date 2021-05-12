namespace Servicios.Comprobante
{
    using System;
    using Dominio.UnidadDeTrabajo;
    using IServicios.Comprobante;

    public class PresupuestoServicio : ComprobanteServicio, IPresupuestoServicio
    {
        public PresupuestoServicio(IUnidadDeTrabajo unidadDeTrabajo)
            : base(unidadDeTrabajo)
        {
        }

        public int ObtenerNumeroPresupuesto(long id)
        {
            try
            {
                var presupuesto = _unidadDeTrabajo.PresupuestoRepositorio.Obtener(id);

                if (presupuesto == null)
                    throw new Exception("Error al obtener el presupuesto.");

                return presupuesto.Numero;
            }
            catch (Exception e)
            {
                throw new Exception($"Error en PresupuestoServicio.ObtenerNumeroPresupuesto:{Environment.NewLine}{e.Message}");
            }
        }
    }
}
