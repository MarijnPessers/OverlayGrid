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
    }
}
