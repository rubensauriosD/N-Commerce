namespace Presentacion.Core.Usuario
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using Aplicacion.Constantes;
    using PresentacionBase.Formularios;

    public partial class _0059_Usuario_Cambiar_Foto : FormBase
    {
        public bool RealizoOperacion { get; private set; }
        public Image Foto { get; private set; }

        public _0059_Usuario_Cambiar_Foto(Image foto)
        {
            InitializeComponent();

            RealizoOperacion = false;
            Foto = foto;
        }

        private void _0059_Usuario_Cambiar_Foto_Load(object sender, EventArgs e)
        {
            picImagen.Image = Foto; 
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            RealizoOperacion = true;
            Close();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            ofdBuscarFoto.Title = "Seleccionar Imagen";
            ofdBuscarFoto.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";

            if (ofdBuscarFoto.ShowDialog() != DialogResult.OK || !ofdBuscarFoto.CheckFileExists)
                return;

            try
            {
                Foto = Image.FromFile(ofdBuscarFoto.FileName);
                picImagen.Image = Foto;
            }
            catch (Exception)
            {
                Mjs.Alerta("La imagen seleccionada no es válida.");
            }

        }
    }
}
