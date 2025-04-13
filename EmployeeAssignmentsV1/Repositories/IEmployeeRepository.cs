using EmployeeAssignmentsV1.Entities;

namespace EmployeeAssignmentsV1.Repositories
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee> GetByIdAsync(int id);
        Task<IEnumerable<Employee>> GetEmployeesByProjectAsync(int projectId);
        Task AddAsync(Employee employee);
    }
}
