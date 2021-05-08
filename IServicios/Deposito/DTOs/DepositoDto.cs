namespace IServicio.Deposito.DTOs
{
    using System.Collections.Generic;
    using IServicio.BaseDto;
    using IServicios.Deposito.DTOs;

    public class DepositoDto : DtoBase
    {
        public string Descripcion { get; set; }

        public string Ubicacion { get; set; }

        // --- Colecciones
        public List<StockDto> Stocks { get; set; }
    }
}
