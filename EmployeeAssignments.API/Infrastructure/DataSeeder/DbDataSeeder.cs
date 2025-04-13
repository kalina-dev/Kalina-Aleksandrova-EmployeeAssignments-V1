using Bogus;
using EmployeeAssignments.API.Entities;

namespace EmployeeAssignments.API.Infrastructure.DataSeeder;
public class DbDataSeeder : IDbDataSeeder
{
    public List<EmployeeProjectMap> GenerateData(int employeeProjectsCount, List<Employee> employees, List<Project> projects)
    {
        var empProjFaker = new Faker<EmployeeProjectMap>()
          .RuleFor(ep => ep.EmpID, f => f.PickRandom(employees).EmpID)
          .RuleFor(ep => ep.ProjectID, f => f.PickRandom(projects).ProjectId)
          .RuleFor(ep => ep.DateFrom, f => f.Date.Past(2))
          .RuleFor(ep => ep.DateTo, (f, ep) =>
          {
              // 70% chance of DateTo being set
              return f.Random.Bool(0.7f) ? f.Date.Between(ep.DateFrom, DateTime.UtcNow) : null;
          });
        var employeeProjectsMap = empProjFaker.Generate(employeeProjectsCount);
        return employeeProjectsMap;
    }

    (List<Employee>, List<Project>) IDbDataSeeder.GenerateData(int employeeCount, int maxProjectsCount)
    {
        var employeeFaker = new Faker<Employee>()
           .RuleFor(e => e.Name, f => f.Name.FullName());

        var projectFaker = new Faker<Project>()
           .RuleFor(p => p.Name, f => f.Company.CompanyName());

        var employees = employeeFaker.Generate(employeeCount);
        var projects = projectFaker.Generate(maxProjectsCount);

        return (employees, projects);
    }
}