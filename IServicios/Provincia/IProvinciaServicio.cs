using IServicio.Provincia.DTOs;

namespace IServicio.Provincia
{
    public interface IProvinciaServicio : Base.IServicio
    {
        bool VerificarSiExiste(string datoVerificar, long? entidadId = null);

        ProvinciaDto ObtenerPorDepartamento(long id);
    }
}
