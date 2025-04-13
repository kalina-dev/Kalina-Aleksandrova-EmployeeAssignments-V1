using EmployeeAssignments.API.Dtos;

namespace EmployeeAssignments.API.Services
{
    public interface IEmployeeProjectService
    {
        Task<EmployeePairResultDto?> GetLongestWorkingPairAsync();
    }
}