using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace OverlayGrid.Controllers.Interfaces
{
    public interface IImageController
    {
        int Height { get; }
        int Width { get; }
        Bitmap Bitmap { get; }
        void SetImage(MemoryStream memoryStream);
        MemoryStream GetMemoryStream();
        void SetImageFormat(ImageFormat imageFormat);
        string GetBase64Image();
    }
}