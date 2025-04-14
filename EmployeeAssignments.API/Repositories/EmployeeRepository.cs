using Dapper;
using EmployeeAssignments.API.Entities;
using EmployeeAssignments.API.Infrastructure.Connection;
using EmployeeAssignments.API.Models;
using System.Net;

namespace EmployeeAssignments.API.Repositories
{
    public class EmployeeRepository(IDbConnectionFactory db) : IEmployeeRepository
    {
        private readonly IDbConnectionFactory db = db;

        public async Task<RepositoryResult<IEnumerable<Employee>>> GetAllAsync()
        {
            try
            {
                using var con = await db.OpenConnectionAsync();
                var data = await con.QueryAsync<Employee>("SELECT * FROM Employees");
                return RepositoryResult<IEnumerable<Employee>>.Ok(data);
            }
            catch (Exception ex)
            {
                return RepositoryResult<IEnumerable<Employee>>.Error(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public async Task<RepositoryResult<Employee>> GetByIdAsync(int id)
        {
            try
            {
                using var con = await db.OpenConnectionAsync();
                var employee = await con.QuerySingleOrDefaultAsync<Employee>(
                    "SELECT * FROM Employees WHERE EmpID = @EmpID", new { EmpID = id });

                if (employee is null)
                    return RepositoryResult<Employee>.Error(HttpStatusCode.NotFound, "Employee not found.");

                return RepositoryResult<Employee>.Ok(employee);
            }
            catch (Exception ex)
            {
                return RepositoryResult<Employee>.Error(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public async Task<RepositoryResult<IEnumerable<Employee>>> GetEmployeesByProjectAsync(int projectId)
        {
            try
            {
                using var con = await db.OpenConnectionAsync();
                var data = await con.QueryAsync<Employee>(
                    "SELECT * FROM Employees WHERE ProjectID = @ProjectID", new { ProjectID = projectId });

                return RepositoryResult<IEnumerable<Employee>>.Ok(data);
            }
            catch (Exception ex)
            {
                return RepositoryResult<IEnumerable<Employee>>.Error(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public async Task<RepositoryResult<bool>> AddAsync(Employee employee)
        {
            try
            {
                using var con = await db.OpenConnectionAsync();
                const string sql = """
                    INSERT INTO Employees (EmpID, ProjectID, DateFrom, DateTo)
                    VALUES (@EmpID, @ProjectID, @DateFrom, @DateTo)
                """;
                var affected = await con.ExecuteAsync(sql, employee);

                if (affected > 0)
                    return RepositoryResult<bool>.Ok(true, "Employee added.");

                return RepositoryResult<bool>.Error(HttpStatusCode.BadRequest, "Insert failed.");
            }
            catch (Exception ex)
            {
                return RepositoryResult<bool>.Error(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
