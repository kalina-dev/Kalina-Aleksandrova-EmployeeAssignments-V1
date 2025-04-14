using EmployeeAssignments.API.Entities;
using EmployeeAssignments.API.Models;

namespace EmployeeAssignments.API.Repositories;

public interface IEmployeeProjectRepository
{
    Task<RepositoryResult<bool>> ExistsAsync(EmployeeProjectMap ep);
    Task<RepositoryResult<bool>> EmployeeExistsAsync(int empId);
    Task<RepositoryResult<bool>> ProjectExistsAsync(int projectId);
    Task<RepositoryResult<bool>> InsertAsync(EmployeeProjectMap ep);
    Task<RepositoryResult<IEnumerable<EmployeeProjectMap>>> GetAllAsync();
}
