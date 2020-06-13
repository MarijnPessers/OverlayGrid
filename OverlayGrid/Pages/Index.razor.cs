using MatBlazor;
using Microsoft.AspNetCore.Components;
using OverlayGrid.Controllers.Interfaces;
using OverlayGrid.Shared;
using System;
using System.IO;
using System.Threading.Tasks;

namespace OverlayGrid.Pages
{
    public partial class Index
    {
        private int Height { get; set; } = 200;
        private int Width { get; set; } = 200;
        private SnackBar _snackBar { get; set; }
        private DrawGrid _drawGrid { get; set; }

        [Inject]
        public IImageController ImageController { get; set; }

        private void DrawCanvas()
        {
            StateHasChanged();
        }

        async Task UploadImageAsync(IMatFileUploadEntry[] files)
        {
            try
            {
                foreach (var file in files)
                {
                    if (!file.Type.StartsWith("image/"))
                    {
                        _snackBar.ShowSnackBar("Not a valid image file. Try a different file!");
                    }
                    else
                    {
                        
                        PlaceImageInCanvas(file);
                        _snackBar.ShowSnackBar("Image file uploaded!");
                    }
                }
            }
            catch (Exception ex)
            {
                _snackBar.ShowSnackBar($"Something went wrong. Try it again! '{ex.Message}'");
            }
            finally
            {
                await InvokeAsync(() => { StateHasChanged(); });
            }

        }

        private async void PlaceImageInCanvas(IMatFileUploadEntry file)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                await file.WriteToStreamAsync(memoryStream);

                ImageController.SetImage(memoryStream);
                
                Height = ImageController.Height;
                Width = ImageController.Width;
                
                _drawGrid.PaintImage(ImageController);
                StateHasChanged();
            }
        }
    }
}
