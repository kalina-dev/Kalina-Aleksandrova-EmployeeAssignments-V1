using Dapper;
using EmployeeAssignments.API.Entities;
using EmployeeAssignments.API.Infrastructure.Connection;
using EmployeeAssignments.API.Models;
using System.Net;

namespace EmployeeAssignments.API.Repositories
{
    public class ProjectRepository(IDbConnectionFactory db) : IProjectRepository
    {
        private readonly IDbConnectionFactory db = db;

        public async Task<RepositoryResult<IEnumerable<Project>>> GetAllAsync()
        {
            try
            {
                using var con = await db.OpenConnectionAsync();
                var data = await con.QueryAsync<Project>("SELECT * FROM Projects");
                return RepositoryResult<IEnumerable<Project>>.Ok(data);
            }
            catch (Exception ex)
            {
                return RepositoryResult<IEnumerable<Project>>.Error(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public async Task<RepositoryResult<Project>> GetByIdAsync(int id)
        {
            try
            {
                using var con = await db.OpenConnectionAsync();
                var project = await con.QuerySingleOrDefaultAsync<Project>(
                    "SELECT * FROM Projects WHERE ProjectID = @ProjectID", new { ProjectID = id });

                if (project is null)
                    return RepositoryResult<Project>.Error(HttpStatusCode.NotFound, "Project not found.");

                return RepositoryResult<Project>.Ok(project);
            }
            catch (Exception ex)
            {
                return RepositoryResult<Project>.Error(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public async Task<RepositoryResult<bool>> AddAsync(Project project)
        {
            try
            {
                using var con = await db.OpenConnectionAsync();
                const string sql = """
                    INSERT INTO Projects (ProjectID, Name)
                    VALUES (@ProjectID, @Name)
                """;
                var affected = await con.ExecuteAsync(sql, project);

                if (affected > 0)
                    return RepositoryResult<bool>.Ok(true, "Project added.");

                return RepositoryResult<bool>.Error(HttpStatusCode.BadRequest, "Insert failed.");
            }
            catch (Exception ex)
            {
                return RepositoryResult<bool>.Error(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
