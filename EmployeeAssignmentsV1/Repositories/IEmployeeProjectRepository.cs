using EmployeeAssignmentsV1.Entities;

namespace EmployeeAssignmentsV1.Repositories
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
