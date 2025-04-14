using EmployeeAssignments.API.Extensions;
using EmployeeAssignments.API.Infrastructure.Connection;
using EmployeeAssignments.API.Infrastructure.DataSeeder;
using EmployeeAssignments.API.Infrastructure.DbInitializer;
using EmployeeAssignments.API.Middleware;
using EmployeeAssignments.API.Repositories;
using EmployeeAssignments.API.Services;
using Microsoft.OpenApi.Models;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Employee Project API",
                Version = "v1",
                Description = "Tracks employee-project assignments and collaboration analytics."
            });
        });

        // Allow CORS for development
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });

        builder.Services.AddSwaggerGen();


        builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
        builder.Services.AddSingleton<IDbDataSeeder, DbDataSeeder>();
        builder.Services.AddSingleton<IDbFactory, DbFactory>();
        builder.Services.AddSingleton<IEmployeeRepository, EmployeeRepository>();
        builder.Services.AddSingleton<IProjectRepository, ProjectRepository>();
        builder.Services.AddSingleton<IEmployeeProjectRepository, EmployeeProjectRepository>();
        builder.Services.AddTransient<IEmployeeProjectService, EmployeeProjectService>();
        builder.Services.AddTransient<ICsvImportService, CsvImportService>();
        builder.Services.AddProblemDetails();


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            await app.SeedDatabaseAsync();
        }
        app.UseMiddleware<GlobalExceptionMiddleware>();
        app.MapControllers();
        // Enable CORS
        app.UseCors("AllowAll");

        // Configure status code pages with default response
        app.UseStatusCodePages(async context =>
        {
            var response = context.HttpContext.Response;
            var statusCode = response.StatusCode;

            var message = statusCode switch
            {
                StatusCodes.Status400BadRequest => "Bad Request",
                StatusCodes.Status401Unauthorized => "Unauthorized",
                StatusCodes.Status403Forbidden => "Forbidden",
                StatusCodes.Status404NotFound => "Not Found",
                StatusCodes.Status500InternalServerError => "Internal Server Error",
                _ => "An unexpected error occurred"
            };

            response.ContentType = "application/json";
            await response.WriteAsJsonAsync(new
            {
                status = statusCode,
                title = message
            });
        });

        // Enable Swagger only in development
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee Project API v1");
                c.RoutePrefix = string.Empty; // Open Swagger at root
            });
        }

        //app.UseHttpsRedirection();
        //app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}