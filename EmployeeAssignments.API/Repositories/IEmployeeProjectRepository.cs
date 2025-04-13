using EmployeeAssignments.API.Entities;

namespace EmployeeAssignments.API.Repositories
{
    public interface IEmployeeProjectRepository
    {
        Task<bool> ExistsAsync(EmployeeProjectMap ep);
        Task<bool> EmployeeExistsAsync(int empId);
        Task<bool> ProjectExistsAsync(int projectId);
        Task InsertAsync(EmployeeProjectMap ep);
        Task<IEnumerable<EmployeeProjectMap>> GetAllAsync();
    }
}
