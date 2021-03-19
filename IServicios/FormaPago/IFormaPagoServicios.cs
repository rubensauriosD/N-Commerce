namespace IServicios.FormaPago
{
    using System.Collections.Generic;
    using IServicios.FormaPago.DTOs;

    public interface IFormaPagoServicios
    {
        void Insertar(List<FormaPagoDto> pagos, long facturaId);
    }
}
