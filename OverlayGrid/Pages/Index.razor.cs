using MatBlazor;
using OverlayGrid.Shared;
using System;

namespace OverlayGrid.Pages
{
    public partial class Index
    {
        private int Height { get; set; } = 200;
        private int Width { get; set; } = 200;

        private string ErrorMessage { get; set; }

        private SnackBar _snackBar { get; set; }

        private void DrawCanvas()
        {
            StateHasChanged();
        }


        async System.Threading.Tasks.Task UploadImageAsync(IMatFileUploadEntry[] files)
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
                        _snackBar.ShowSnackBar("Image file uploaded!");
                    }
                }
            }
            catch (Exception ex)
            {
                _snackBar.ShowSnackBar("Something went wrong. Try it again!");
            }
            finally
            {
                await InvokeAsync(() => { StateHasChanged(); });
            }

        }
    }
}
