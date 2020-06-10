using Blazor.Extensions.Canvas;
using Microsoft.AspNetCore.Components;

namespace OverlayGrid.Pages
{
    public partial class DrawGrid
    {
        private BECanvas _canvasReference;

        [Parameter]
        public int Height { get; set; }

        [Parameter]
        public int Width { get; set; }
    }
}
