namespace IServicio.Deposito
{
    using IServicios.Deposito.DTOs;

    public interface IDepositoSevicio : Base.IServicio
    {
        bool VerificarSiExiste(string datoVerificar, long? entidadId = null);

        bool TransferirArticulos(TransferenciaDepositoDto transferencia);
        bool TieneStokDeArticulos(long? entidadId);
    }
}
