using Blazor.Extensions;
using Blazor.Extensions.Canvas;
using Blazor.Extensions.Canvas.Canvas2D;
using Microsoft.AspNetCore.Components;
using OverlayGrid.Controllers.Interfaces;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace OverlayGrid.Pages
{
    public partial class DrawGrid
    {
        private BECanvas _canvasReference;
        private ElementReference ImageToBeLoaded { get; set; }
        private string ImageSource { get; set; }
        private Canvas2DContext Canvas2DContext { get; set; }
        private int Height { get; set; }
        private int Width { get; set; }

        [Inject]
        public IHexGridController HexGridController { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            Canvas2DContext = await _canvasReference.CreateCanvas2DAsync();
            await base.OnAfterRenderAsync(firstRender);
        }

        public async Task PlaceImage(IImageController imageController)
        {
            Height = imageController.Height;
            Width = imageController.Width;
            imageController.SetImageFormat(ImageFormat.Png);
            ImageSource = imageController.GetBase64Image();
            StateHasChanged();
            await Canvas2DContext.DrawImageAsync(ImageToBeLoaded, 0, 0, Width, Height);
            StateHasChanged();
        }

        public async Task Draw()
        {
            HexGridController.CanvasHeight = Height;
            HexGridController.CanvasWidth = Width;
            HexGridController.Diameter = 100;
            await Canvas2DContext.SetLineWidthAsync(1);
            await Canvas2DContext.SetStrokeStyleAsync("#666666");
            await Canvas2DContext.SetFillStyleAsync("#666666");
            
            for (var row = 0; row < HexGridController.Rows; row++)
            {
                for (var col = 0; col < HexGridController.Columns; col++)
                {
                    await Canvas2DContext.BeginPathAsync();
                    var point = HexGridController.GetCoordinates(row, col)[6];
                    await Canvas2DContext.MoveToAsync(point.X, point.Y);
                    for (var pos = 1; pos <= 6; pos++)
                    {
                        point = HexGridController.GetCoordinates(row, col)[pos];
                        await Canvas2DContext.LineToAsync(point.X, point.Y);
                    }
                    await Canvas2DContext.StrokeAsync();
                }
            }
        }
    }
}
