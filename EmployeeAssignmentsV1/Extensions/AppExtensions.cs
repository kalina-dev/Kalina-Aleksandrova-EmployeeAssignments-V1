using EmployeeAssignmentsV1.Infrastructure.DbInitializer;

namespace EmployeeAssignmentsV1.Extensions;

public static class AppExtensions
{
    public async static Task SeedDatabaseAsync(this IApplicationBuilder app)
    {
        var dbFactory = app.ApplicationServices.GetService<IDbFactory>();
        await dbFactory.EnsureTablesCreationAsync();
        await dbFactory.SeedAsync();
    }
}