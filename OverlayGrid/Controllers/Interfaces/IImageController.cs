using System.IO;

namespace OverlayGrid.Controllers.Interfaces
{
    public interface IImageController
    {
        int Height { get; }
        int Width { get; }
        void SetImage(MemoryStream memoryStream);
    }
}