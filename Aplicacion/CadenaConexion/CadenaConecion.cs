namespace Aplicacion.CadenaConexion
{
    public static class CadenaConecion
    {
        // Atributos
        private const string Servidor = @"FEDEPC"; // Cambia
        private const string BaseDatos = @"N-Commerce";
        private const string Usuario = @"Fede";
        private const string Password = @""; // Cambia

        // Propiedad
        public static string ObtenerCadenaSql => $"Data Source={Servidor}; " +
                                                 $"Initial Catalog={BaseDatos}; " +
                                                 $"User Id={Usuario}; " +
                                                 $"Password={Password};";

        public static string ObtenerCadenaWin => $"Data Source={Servidor}; " +
                                                 $"Initial Catalog={BaseDatos}; " +
                                                 $"Integrated Security=true;";
    }
}
