using Blazor.Extensions;
using Blazor.Extensions.Canvas;
using Blazor.Extensions.Canvas.WebGL;
using Microsoft.AspNetCore.Components;
using OverlayGrid.Controllers.Interfaces;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace OverlayGrid.Pages
{
    public partial class DrawGrid
    {
        private BECanvas _canvasReference;

        [Parameter]
        public int Height { get; set; }

        [Parameter]
        public int Width { get; set; }

        public void PaintImage(IImageController imageController)
        {
            Height = imageController.Height;
            Width = imageController.Width;
            StateHasChanged();
            var context = _canvasReference.CreateCanvas2D();
            //context.DrawImageAsync(bitmap, 0, 0, 200, 200);
        }
    }
}
