namespace IServicio.Persona
{
    using IServicios.Persona.DTOs;
    using System.Collections.Generic;

    public interface IClienteServicio : IPersonaServicio
    {
        bool VerificarSiExisteDni(string datoVerificar, long? entidadId = null);

        decimal SaldoCuentaCorriente(long clienteId);

        bool AgregarPagoCuentaCorriente(MovimientoCuentaCorrienteClienteDto pago);

        bool RevertirPagoCuentaCorriente(MovimientoCuentaCorrienteClienteDto pago);

        List<MovimientoCuentaCorrienteClienteDto> ObtenerMovimientosCuentaCorriente(long id);
    }
}
