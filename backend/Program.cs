using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using EFCore.NamingConventions;

namespace backend;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var cs = builder.Configuration.GetConnectionString("Default");
        builder.Services.AddDbContext<AppDbContext>(opt =>
            opt.UseNpgsql(cs).UseSnakeCaseNamingConvention());

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Taratarikor API v1");
                c.RoutePrefix = "swagger";
            });
        }

        // (Add auth later if needed)
        // app.UseAuthentication();
        app.UseAuthorization();

        // Map attribute-routed controllers
        app.MapControllers();

        app.MapFallback(async context =>
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsJsonAsync(new { message = "Route not found" });
        });

        app.Run();
    }
}
