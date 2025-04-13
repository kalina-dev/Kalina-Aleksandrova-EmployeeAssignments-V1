namespace EmployeeAssignmentsV1.Infrastructure.DbInitializer;
public interface IDbFactory
{
    Task<IDbFactory> EnsureTablesCreationAsync();
    Task SeedAsync();
}