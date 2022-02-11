using System.Drawing;
using System.Runtime.Versioning;

namespace mitoSoft.Common.Webcam.Extensions
{
    internal static class StringExtensions
    {
        [SupportedOSPlatform("windows")]
        public static string ToBase64String(this Image image)
        {
            var bitmap = new System.Drawing.Bitmap(image);

            byte[] bytes;
            using var stream = new System.IO.MemoryStream();
            {
                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);

                bytes = stream.ToArray();
            }

            return bytes.ToBase64String();
        }

        private static string ToBase64String(this byte[] bytes)
        {
            if (bytes != null)
            {
                var base64 = Convert.ToBase64String(bytes);
                return string.Format("data:image/jpg;base64,{0}", base64);
            }
            else
            {
                return string.Empty;
            }
        }
    }
}