using System.Text;
using Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);
//Services
    //Extensions
    builder.Services.AddSwaggerConfiguration();
    builder.Services.AddCorsConfiguration("CorsPolicy");
    builder.Services.AddJwtConfiguration(builder.Configuration);
    //Needed
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddAuthorization();
    builder.Services.AddInfrastructure(builder.Configuration);
//Application
    var app = builder.Build();
    //Seeder
    await app.Services.RunSeedDataAsync();


    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseCustomStaticFiles();
    app.UseHttpsRedirection();
    app.UseCors("CorsPolicy");
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();