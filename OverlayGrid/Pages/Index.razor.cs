namespace OverlayGrid.Pages
{
    public partial class Index
    {
        private int Height { get; set; } = 200;
        private int Width { get; set; } = 200;

        private void DrawCanvas()
        {
            StateHasChanged();
        }
    }
}
