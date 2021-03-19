using IServicio.BaseDto;

namespace IServicio.PuestoTrabajo.DTOs
{
    public class PuestoTrabajoDto : DtoBase
    {
        public int Codigo { get; set; }

        public string Descripcion { get; set; }
    }
}
