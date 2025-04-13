namespace EmployeeAssignmentsV1.Services
{
    public interface ICsvImportService
    {
        Task<List<string>> ImportEmployeeProjectsAsync(Stream csvStream);
    }
}