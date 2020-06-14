using System.Collections.Generic;
using System.Drawing;

namespace OverlayGrid.Controllers.Interfaces
{
    interface IHexGridController
    {
        int CanvasWidth { get; set; }
        int CanvasHeight { get; set; }
        double Diameter { get; set; }
        IDictionary<int, Point> GetCoordinates(int row, int column);
    }
}
