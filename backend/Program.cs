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

        var connectionString = builder.Configuration.GetConnectionString("Default");
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention()
        );

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddOpenApi();

        // For Swagger UI
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger(); // serves /swagger/v1/swagger.json
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Taratarikor API v1");
                c.RoutePrefix = "swagger"; // UI at /swagger
            });
        }

        app.UseAuthorization();

        app.MapControllers();

        // A catch all route showing error if the user tries to access a route that does not exist.
        app.Run(async context =>
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsJsonAsync(new { message = "Route not found" });
        });

        app.Run();
    }
}
