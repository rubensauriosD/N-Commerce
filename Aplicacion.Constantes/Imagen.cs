using System;
using System.Drawing;
using System.IO;

namespace Aplicacion.Constantes
{
    public static class Imagen
    {
        //
        // Iconos
        //
        public static Image Nuevo => Properties.RecursoImagenes.new_b;
        public static Image Editar => Properties.RecursoImagenes.edit_b;
        public static Image Eliminar => Properties.RecursoImagenes.delete_b;
        public static Image Guardar => Properties.RecursoImagenes.save_b;
        public static Image Limpiar => Properties.RecursoImagenes.clean_b;
        public static Image Actualizar => Properties.RecursoImagenes.refresh_b;
        public static Image Buscar => Properties.RecursoImagenes.search_b;
        public static Image Salir => Properties.RecursoImagenes.cancel_b;
        public static Image Bloquear => Properties.RecursoImagenes.lock_b;
        public static Image Usuario => Properties.RecursoImagenes.face_b;
        public static Image Pago => Properties.RecursoImagenes.peso_b;
        public static Image Reversion => Properties.RecursoImagenes.peso_no_b;
        public static Image Imprimir=> Properties.RecursoImagenes.print_b;
        public static Image Ver=> Properties.RecursoImagenes.visibility_b;

        //
        // Imagenes por Defecto
        //
        public static Image ImagenEmpleadoPorDefecto = Properties.RecursoImagenes.Empleado;
        public static Image ImagenUsuarioPorDefecto = Properties.RecursoImagenes.face_b;
        public static Image Logo = Properties.RecursoImagenes.logo;

        //
        // Conversion
        //
        public static byte[] ConvertirImagen(Image img)
        {
            var sTemp = Path.GetTempFileName();

            var fs = new FileStream(sTemp, FileMode.OpenOrCreate, FileAccess.ReadWrite);

            img.Save(fs, System.Drawing.Imaging.ImageFormat.Png);

            fs.Position = 0;

            var imgLength = Convert.ToInt32(fs.Length);

            var bytes = new byte[imgLength];

            fs.Read(bytes, 0, imgLength);

            fs.Close();

            return bytes;
        }

        public static Image ConvertirImagen(byte[] bytes)
        {
            if (bytes == null) return null;

            var ms = new MemoryStream(bytes);

            Bitmap bm = null;

            try
            {
                bm = new Bitmap(ms);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return bm;
        }
    }
}
