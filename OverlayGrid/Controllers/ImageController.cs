using OverlayGrid.Controllers.Interfaces;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace OverlayGrid.Controllers
{
    public class ImageController : IImageController
    {
        private Bitmap _bitmap;

        private MemoryStream MemoryStream { get; set; }
        public Bitmap Bitmap
        {
            get
            {
                if (_bitmap == null) _bitmap = new Bitmap(MemoryStream);
                return _bitmap;
            }
            set => _bitmap = value;
        }

        public int Height => Bitmap?.Height ?? 0;
        public int Width => Bitmap?.Width ?? 0;

        public void SetImage(MemoryStream memoryStream)
        {
            _bitmap = null;
            MemoryStream = memoryStream;
        }

        public MemoryStream GetMemoryStream()
        {
            return MemoryStream;
        }

        public void SetImageFormat(ImageFormat imageFormat)
        {
            using MemoryStream emoryStreams = new MemoryStream();
            Bitmap.Save(emoryStreams, imageFormat);
            _bitmap = null;
            MemoryStream = emoryStreams;
        }

        public string GetBase64Image()
        {
            return System.Convert.ToBase64String(MemoryStream.ToArray());
        }
    }
}
