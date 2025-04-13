using System.Data;

namespace EmployeeAssignments.API.Infrastructure.Connection;

public interface IDbConnectionFactory
{
    Task<IDbConnection> OpenConnectionAsync();
}