using System.Threading.Tasks;

namespace EmployeeAssignments.API.Infrastructure.DbInitializer;
public interface IDbFactory
{
    Task<IDbFactory> EnsureTablesCreationAsync();
    Task SeedAsync();
}