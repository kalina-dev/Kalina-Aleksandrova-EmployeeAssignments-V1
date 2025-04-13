using Dapper;
using EmployeeAssignmentsV1.Entities;
using EmployeeAssignmentsV1.Infrastructure.Connection;

namespace EmployeeAssignmentsV1.Repositories
{
    public class EmployeeRepository(IDbConnectionFactory db) : IEmployeeRepository
    {
        private readonly IDbConnectionFactory db = db;

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            using var con = await db.OpenConnectionAsync();
            return await con.QueryAsync<Employee>("SELECT * FROM Employees");
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            using var con = await db.OpenConnectionAsync();
            return await con.QuerySingleOrDefaultAsync<Employee>(
                "SELECT * FROM Employees WHERE EmpID = @EmpID", new { EmpID = id });
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByProjectAsync(int projectId)
        {
            using var con = await db.OpenConnectionAsync();
            return await con.QueryAsync<Employee>(
                "SELECT * FROM Employees WHERE ProjectID = @ProjectID", new { ProjectID = projectId });
        }

        public async Task AddAsync(Employee employee)
        {
            using var con = await db.OpenConnectionAsync();
            const string sql = "INSERT INTO Employees (EmpID, ProjectID, DateFrom, DateTo) VALUES (@EmpID, @ProjectID, @DateFrom, @DateTo)";
            await con.ExecuteAsync(sql, employee);
        }
    }
}