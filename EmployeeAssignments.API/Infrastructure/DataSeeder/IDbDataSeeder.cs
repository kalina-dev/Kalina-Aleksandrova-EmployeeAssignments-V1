using EmployeeAssignments.API.Entities;

namespace EmployeeAssignments.API.Infrastructure.DataSeeder;

public interface IDbDataSeeder
{
    (List<Employee>, List<Project>) GenerateData(int employeeCount, int maxProjectsCount);
    List<EmployeeProjectMap> GenerateData(int employeeProjectsCount, List<Employee> employees, List<Project> projects);
}