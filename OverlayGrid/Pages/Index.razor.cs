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
        private SnackBar _snackBar { get; set; } = new SnackBar();
        private DrawGrid _drawGrid { get; set; }

        [Inject]
        public IImageController ImageController { get; set; }

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

                        await PlaceImageInCanvas(file);
                        _snackBar.ShowSnackBar("Image file uploaded!");
                    }
                }
            }
            catch (Exception ex)
            {
                _snackBar.ShowSnackBar($"Something went wrong. Try it again! '{ex.Message}'");
            }
        }

        private async Task PlaceImageInCanvas(IMatFileUploadEntry file)
        {
            using MemoryStream memoryStream = new MemoryStream();
            await file.WriteToStreamAsync(memoryStream);

            ImageController.SetImage(memoryStream);

            await _drawGrid.PlaceImage(ImageController);
        }
    }
}
