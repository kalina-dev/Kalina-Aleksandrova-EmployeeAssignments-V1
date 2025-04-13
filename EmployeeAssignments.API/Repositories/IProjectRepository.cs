using EmployeeAssignments.API.Entities;

namespace EmployeeAssignments.API.Repositories
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetAllAsync();
        Task<Project> GetByIdAsync(int id);
        Task AddAsync(Project project);
    }
}
