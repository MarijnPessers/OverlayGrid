using Microsoft.Extensions.DependencyInjection;
using OverlayGrid.Controllers;
using OverlayGrid.Controllers.Interfaces;

namespace OverlayGrid
{
    public static class Initializer
    {
        public static void Initialize(this IServiceCollection services)
        {
            services.AddScoped<IImageController, ImageController>();
            services.AddScoped<IHexGridController, HexGridController>();
        }
    }
}
