using System.Globalization;

namespace Aplicacion.Constantes
{
    public static class ConfiguracionPorDefecto
    {
        public static string ClienteDni => "99999999";

        public static TipoComprobante TipoComprobante = TipoComprobante.B;

        public static CultureInfo CultureInfo = new CultureInfo("es-Ar");
    }
}
