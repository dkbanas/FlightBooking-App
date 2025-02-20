using Microsoft.Extensions.FileProviders;

namespace Presentation.Extensions;

public static class StaticFileExtension
{
    public static IApplicationBuilder UseCustomStaticFiles(this IApplicationBuilder app)
    {
        // Definiowanie ścieżki do folderu IMG/Airports
        var imgPath = Path.Combine(Directory.GetCurrentDirectory(), "IMG", "Airports");

        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(imgPath),
            RequestPath = "/IMG/Airports" 
        });

        return app;
    }
}