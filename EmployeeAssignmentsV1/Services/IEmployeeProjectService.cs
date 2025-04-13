using EmployeeAssignmentsV1.Dtos;

namespace EmployeeAssignmentsV1.Services
{
    public interface IEmployeeProjectService
    {
        Task<EmployeePairResultDto?> GetLongestWorkingPairAsync();
    }
}