namespace IServicio.Persona
{
    using IServicio.Persona.DTOs;
    using IServicios.Persona.DTOs;
    using System.Collections.Generic;

    public interface IProveedorServicio : Base.IServicio
    {
        ProveedorDto ObtenerPorCuit(string cuit);

        bool VerificarSiExisteCuit(string datoVerificar, long? entidadId = null);

        bool RevertirPagoCuentaCorriente(MovimientoCuentaCorrienteProveedorDto pago);

        bool AgregarPagoCuentaCorriente(MovimientoCuentaCorrienteProveedorDto pago);

        List<MovimientoCuentaCorrienteProveedorDto> ObtenerMovimientosCuentaCorriente(long id);
    }
}
