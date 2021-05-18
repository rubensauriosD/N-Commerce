namespace Presentacion.Core.Articulo
{
    using Aplicacion.Constantes;
    using PresentacionBase.Formularios;

    public partial class _00030_Abm_BajaArticulos : FormAbm
    {
        private readonly Validar Validar;

        public _00030_Abm_BajaArticulos(TipoOperacion tipoOperacion, long? entidadId = null)
            : base(tipoOperacion, entidadId)
        {
            InitializeComponent();
        }

        private void _00030_Abm_BajaArticulos_Load(object sender, System.EventArgs e)
        {
            Validar.ComoAlfanumerico(txtObservacion, false);
            txtObservacion.MaxLength = 400;

            // TODO: BajaMaxLenght < StockActual
        }
    }
}
