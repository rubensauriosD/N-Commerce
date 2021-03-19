namespace IServicio.BaseDto
{
    public class DtoBase
    {
        public long Id { get; set; } = 0;

        public bool Eliminado { get; set; } = false;

        public string EliminadoStr => Eliminado ? "SI" : "NO";
    }
}
