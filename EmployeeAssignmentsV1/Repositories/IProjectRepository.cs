using EmployeeAssignmentsV1.Entities;

namespace EmployeeAssignmentsV1.Repositories
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetAllAsync();
        Task<Project> GetByIdAsync(int id);
        Task AddAsync(Project project);
    }
}
