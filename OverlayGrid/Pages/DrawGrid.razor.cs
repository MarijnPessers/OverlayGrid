using Blazor.Extensions;
using Blazor.Extensions.Canvas;
using Blazor.Extensions.Canvas.Canvas2D;
using Microsoft.AspNetCore.Components;
using OverlayGrid.Controllers.Interfaces;
using System;
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

        private int LineThinkness { get; set; } = 1;
        private int Diameter { get; set; } = 100;
        private string LineColor { get; set; } = "#FF0000";

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

        public async Task DrawHorizontal()
        {
            await Draw(false);
        }

        public async Task DrawVertical()
        {
            await Draw(true);
        }

        public async Task Draw(bool vertical)
        {
            await ResetCanvas();

            if (vertical)
            {
                HexGridController.CanvasHeight = Height;
                HexGridController.CanvasWidth = Width;
            }
            else
            {
                HexGridController.CanvasHeight = Width;
                HexGridController.CanvasWidth = Height;
            }
            HexGridController.Diameter = Diameter;
            await Canvas2DContext.SetLineWidthAsync(LineThinkness);
            await Canvas2DContext.SetStrokeStyleAsync(LineColor);

            for (var row = 0; row < HexGridController.Rows; row++)
            {
                for (var col = 0; col < HexGridController.Columns; col++)
                {
                    await Canvas2DContext.BeginPathAsync();
                    var point = HexGridController.GetCoordinates(row, col)[6];
                    if (vertical)
                    {
                        await Canvas2DContext.MoveToAsync(point.X, point.Y);
                    }
                    else
                    {
                        await Canvas2DContext.MoveToAsync(point.Y, point.X);
                    }
                        
                    for (var pos = 1; pos <= 6; pos++)
                    {
                        point = HexGridController.GetCoordinates(row, col)[pos];
                        if (vertical)
                        {
                            await Canvas2DContext.LineToAsync(point.X, point.Y);
                        }
                        else
                        {
                            await Canvas2DContext.LineToAsync(point.Y, point.X);
                        }
                    }
                    await Canvas2DContext.StrokeAsync();
                }
            }
        }

        private async Task ResetCanvas()
        {
            await Canvas2DContext.DrawImageAsync(ImageToBeLoaded, 0, 0, Width, Height);
            StateHasChanged();
        }
    }
}
