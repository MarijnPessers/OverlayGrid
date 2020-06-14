using Blazor.Extensions;
using Blazor.Extensions.Canvas;
using Blazor.Extensions.Canvas.Canvas2D;
using Blazor.Extensions.Canvas.WebGL;
using Microsoft.AspNetCore.Components;
using OverlayGrid.Controllers.Interfaces;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace OverlayGrid.Pages
{
    public partial class DrawGrid
    {
        private BECanvas _canvasReference;

        private ElementReference imageToBeLoaded { get; set; }
        private string imageSource { get; set; }
        private Canvas2DContext _canvas2DContext { get; set; }
        private int Height { get; set; }
        private int Width { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            _canvas2DContext = await _canvasReference.CreateCanvas2DAsync();
            await base.OnAfterRenderAsync(firstRender);
        }

        public async Task PlaceImage(IImageController imageController)
        {
            Height = imageController.Height;
            Width = imageController.Width;
            imageController.SetImageFormat(ImageFormat.Png);
            imageSource = imageController.GetBase64Image();
            StateHasChanged();
            await _canvas2DContext.DrawImageAsync(imageToBeLoaded, 0, 0, Width, Height);
            StateHasChanged();
        }
    }
}
