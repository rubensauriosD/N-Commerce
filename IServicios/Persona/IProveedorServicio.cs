namespace IServicio.Persona
{
    using IServicios.Persona.DTOs;
    using System.Collections.Generic;

    public interface IProveedorServicio : Base.IServicio
    {
        bool VerificarSiExisteCuit(string datoVerificar, long? entidadId = null);

        bool RevertirPagoCuentaCorriente(MovimientoCuentaCorrienteProveedorDto pago);

        bool AgregarPagoCuentaCorriente(MovimientoCuentaCorrienteProveedorDto pago);

        List<MovimientoCuentaCorrienteProveedorDto> ObtenerMovimientosCuentaCorriente(long id);
    }
}
