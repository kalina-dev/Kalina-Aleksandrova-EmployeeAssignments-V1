using Dapper;
using EmployeeAssignments.API.Entities;
using EmployeeAssignments.API.Infrastructure.Connection;
using EmployeeAssignments.API.Models;
using System.Net;

namespace EmployeeAssignments.API.Repositories;

public class EmployeeProjectRepository(IDbConnectionFactory db) : IEmployeeProjectRepository
{
    private readonly IDbConnectionFactory db = db;

    public async Task<RepositoryResult<bool>> ExistsAsync(EmployeeProjectMap ep)
    {
        try
        {
            using var con = await db.OpenConnectionAsync();
            var exists = await con.ExecuteScalarAsync<bool>(
                "SELECT 1 FROM EmployeeProjects WHERE EmpID = @EmpID AND ProjectID = @ProjectID AND DateFrom = @DateFrom", ep);
            return RepositoryResult<bool>.Ok(exists);
        }
        catch (Exception ex)
        {
            return RepositoryResult<bool>.Error(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<RepositoryResult<bool>> EmployeeExistsAsync(int empId)
    {
        try
        {
            using var con = await db.OpenConnectionAsync();
            var exists = await con.ExecuteScalarAsync<bool>(
                "SELECT 1 FROM Employees WHERE EmpID = @empId", new { empId });
            return RepositoryResult<bool>.Ok(exists);
        }
        catch (Exception ex)
        {
            return RepositoryResult<bool>.Error(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<RepositoryResult<bool>> ProjectExistsAsync(int projectId)
    {
        try
        {
            using var con = await db.OpenConnectionAsync();
            var exists = await con.ExecuteScalarAsync<bool>(
                "SELECT 1 FROM Projects WHERE ProjectID = @projectId", new { projectId });
            return RepositoryResult<bool>.Ok(exists);
        }
        catch (Exception ex)
        {
            return RepositoryResult<bool>.Error(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<RepositoryResult<bool>> InsertAsync(EmployeeProjectMap ep)
    {
        try
        {
            using var con = await db.OpenConnectionAsync();
            const string sql = @"
                INSERT INTO EmployeeProjects (EmpID, ProjectID, DateFrom, DateTo)
                VALUES (@EmpID, @ProjectID, @DateFrom, @DateTo)";
            var rows = await con.ExecuteAsync(sql, ep);
            return rows > 0
                ? RepositoryResult<bool>.Ok(true, "Inserted successfully.")
                : RepositoryResult<bool>.Error(HttpStatusCode.BadRequest, "Insert failed.");
        }
        catch (Exception ex)
        {
            return RepositoryResult<bool>.Error(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<RepositoryResult<IEnumerable<EmployeeProjectMap>>> GetAllAsync()
    {
        try
        {
            using var con = await db.OpenConnectionAsync();
            var data = await con.QueryAsync<EmployeeProjectMap>("SELECT * FROM EmployeeProjects");
            return RepositoryResult<IEnumerable<EmployeeProjectMap>>.Ok(data);
        }
        catch (Exception ex)
        {
            return RepositoryResult<IEnumerable<EmployeeProjectMap>>.Error(HttpStatusCode.InternalServerError, ex.Message);
        }
    }
}
