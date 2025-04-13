using Dapper;
using EmployeeAssignmentsV1.Entities;
using EmployeeAssignmentsV1.Infrastructure.Connection;

namespace EmployeeAssignmentsV1.Repositories;

public class EmployeeProjectRepository(IDbConnectionFactory db) : IEmployeeProjectRepository
{
    private readonly IDbConnectionFactory db = db;

    public async Task<bool> ExistsAsync(EmployeeProjectMap ep)
    {
        using var con = await db.OpenConnectionAsync();
        var exists = await con.ExecuteScalarAsync<bool>(
            "SELECT 1 FROM EmployeeProjects WHERE EmpID = @EmpID AND ProjectID = @ProjectID AND DateFrom = @DateFrom", ep);
        return exists;
    }

    public async Task<bool> EmployeeExistsAsync(int empId)
    {
        using var con = await db.OpenConnectionAsync();
        return await con.ExecuteScalarAsync<bool>(
            "SELECT 1 FROM Employees WHERE EmpID = @empId", new { empId });
    }

    public async Task<bool> ProjectExistsAsync(int projectId)
    {
        using var con = await db.OpenConnectionAsync();
        return await con.ExecuteScalarAsync<bool>(
            "SELECT 1 FROM Projects WHERE ProjectID = @projectId", new { projectId });
    }

    public async Task InsertAsync(EmployeeProjectMap ep)
    {
        using var con = await db.OpenConnectionAsync();
        await con.ExecuteAsync(@"
            INSERT INTO EmployeeProjects (EmpID, ProjectID, DateFrom, DateTo)
            VALUES (@EmpID, @ProjectID, @DateFrom, @DateTo)", ep);
    }

    public async Task<IEnumerable<EmployeeProjectMap>> GetAllAsync()
    {
        using var con = await db.OpenConnectionAsync();
        return await con.QueryAsync<EmployeeProjectMap>("SELECT * FROM EmployeeProjects");
    }
}
