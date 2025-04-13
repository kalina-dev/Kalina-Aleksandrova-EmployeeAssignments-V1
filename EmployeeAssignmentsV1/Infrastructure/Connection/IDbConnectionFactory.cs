using System.Data;

namespace EmployeeAssignmentsV1.Infrastructure.Connection;

public interface IDbConnectionFactory
{
    Task<IDbConnection> OpenConnectionAsync();
}