using Dapper;
using EmployeeAssignmentsV1.Entities;
using EmployeeAssignmentsV1.Infrastructure.Connection;
using System.Data;

namespace EmployeeAssignmentsV1.Repositories
{
    public class ProjectRepository(IDbConnectionFactory db) : IProjectRepository
    {
        private readonly IDbConnectionFactory db = db;

        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            using var con = await db.OpenConnectionAsync();
            return await con.QueryAsync<Project>("SELECT * FROM Projects");
        }

        public async Task<Project> GetByIdAsync(int id)
        {
            using var con = await db.OpenConnectionAsync();
            return await con.QuerySingleOrDefaultAsync<Project>(
                "SELECT * FROM Projects WHERE ProjectId = @ProjectId", new { ProjectId = id });
        }

        public async Task AddAsync(Project project)
        {
            using var con = await db.OpenConnectionAsync();
            const string sql = "INSERT INTO Projects (ProjectId, Name) VALUES (@ProjectId, @Name)";
            await con.ExecuteAsync(sql, project);
        }
    }
}
