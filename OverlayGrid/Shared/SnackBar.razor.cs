using Microsoft.AspNetCore.Components;

namespace OverlayGrid.Shared
{
    public partial class SnackBar
    {
        [Parameter]
        public bool IsOpen { get; set; }

        [Parameter]
        public string Message { get; set; }

        public void ShowSnackBar(string message)
        {
            Message = message;
            IsOpen = true;
            StateHasChanged();
        }

        void Close()
        {
            IsOpen = false;
            StateHasChanged();
        }
    }
}
