using OverlayGrid.Controllers.Interfaces;
using System.Drawing;
using System.IO;

namespace OverlayGrid.Controllers
{
    public class ImageController : IImageController
    {
        private Bitmap Bitmap { get; set; }

        public int Height => Bitmap?.Height ?? 0;
        public int Width => Bitmap?.Width ?? 0;

        public void SetImage(MemoryStream memoryStream)
        {
            Bitmap = new Bitmap(memoryStream);
        }
    }
}
