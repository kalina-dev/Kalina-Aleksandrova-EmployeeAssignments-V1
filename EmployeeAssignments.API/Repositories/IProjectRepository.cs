using EmployeeAssignments.API.Entities;
using EmployeeAssignments.API.Models;

namespace EmployeeAssignments.API.Repositories
{
    public interface IProjectRepository
    {
        Task<RepositoryResult<IEnumerable<Project>>> GetAllAsync();
        Task<RepositoryResult<Project>> GetByIdAsync(int id);
        Task<RepositoryResult<bool>> AddAsync(Project project);
    }
}
