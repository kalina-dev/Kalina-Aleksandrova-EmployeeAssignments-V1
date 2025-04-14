using EmployeeAssignments.API.Entities;
using EmployeeAssignments.API.Models;

namespace EmployeeAssignments.API.Repositories
{
    public interface IEmployeeRepository
    {
        Task<RepositoryResult<IEnumerable<Employee>>> GetAllAsync();
        Task<RepositoryResult<Employee>> GetByIdAsync(int id);
        Task<RepositoryResult<IEnumerable<Employee>>> GetEmployeesByProjectAsync(int projectId);
        Task<RepositoryResult<bool>> AddAsync(Employee employee);
    }
}
