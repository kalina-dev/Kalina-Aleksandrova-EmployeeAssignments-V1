using EmployeeAssignmentsV1.Extensions;
using EmployeeAssignmentsV1.Infrastructure.Connection;
using EmployeeAssignmentsV1.Infrastructure.DataSeeder;
using EmployeeAssignmentsV1.Infrastructure.DbInitializer;
using EmployeeAssignmentsV1.Repositories;
using EmployeeAssignmentsV1.Services;
using Microsoft.OpenApi.Models;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

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
        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}