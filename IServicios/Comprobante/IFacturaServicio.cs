namespace IServicios.Comprobante
{
    using System.Collections.Generic;
    using IServicios.Comprobante.DTOs;

    public interface IFacturaServicio : IComprobanteServicio
    {
        IEnumerable<ComprobantePendienteDto> ObtenerPendientesPago();

        FacturaDto Obtener(long id);
    }
}
