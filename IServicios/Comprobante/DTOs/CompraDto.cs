namespace IServicios.Comprobante.DTOs
{
    public class CompraDto : ComprobanteDto
    {
        public long ProveedorId { get; set; }

        public decimal Iva27 { get; set; }

        public decimal PrecepcionTem { get; set; }

        public decimal PrecepcionPyP { get; set; }

        public decimal PrecepcionIva { get; set; }

        public decimal PrecepcionIB { get; set; }

        public decimal ImpuestosInternos { get; set; }


    }
}
