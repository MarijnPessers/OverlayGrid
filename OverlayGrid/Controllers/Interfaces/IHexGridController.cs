using System.Collections.Generic;
using System.Drawing;

namespace OverlayGrid.Controllers.Interfaces
{
    public interface IHexGridController
    {
        int CanvasWidth { get; set; }
        int CanvasHeight { get; set; }
        double Diameter { get; set; }
        int Columns { get; set; }
        int Rows { get; set; }
        IDictionary<int, Point> GetCoordinates(int row, int column);
    }
}
