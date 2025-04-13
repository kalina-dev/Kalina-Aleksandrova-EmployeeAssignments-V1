using EmployeeAssignments.API.Entities;

namespace EmployeeAssignments.API.Repositories
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee> GetByIdAsync(int id);
        Task<IEnumerable<Employee>> GetEmployeesByProjectAsync(int projectId);
        Task AddAsync(Employee employee);
    }
}
