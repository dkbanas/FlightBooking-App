namespace Presentation.Extensions;

public static class CorsExtension
{
    public static void AddCorsConfiguration(this IServiceCollection services, string corsPolicy)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(corsPolicy, policyBuilder =>
            {
                policyBuilder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200");
            });
        });
    }
}