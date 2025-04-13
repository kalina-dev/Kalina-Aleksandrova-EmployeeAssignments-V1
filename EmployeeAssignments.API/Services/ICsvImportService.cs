namespace EmployeeAssignments.API.Services
{
    public interface ICsvImportService
    {
        Task<List<string>> ImportEmployeeProjectsAsync(Stream csvStream);
    }
}