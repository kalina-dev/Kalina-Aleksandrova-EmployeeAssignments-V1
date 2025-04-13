using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using EmployeeAssignments.API.Entities;
using EmployeeAssignments.API.Repositories;
using EmployeeAssignments.API.Dtos;
using EmployeeAssignments.API.Validators;

namespace EmployeeAssignments.API.Services;

public class CsvImportService(IEmployeeProjectRepository repository) : ICsvImportService
{
    private readonly IEmployeeProjectRepository _repository = repository;
    private readonly EmployeeProjectValidator _validator = new();

    public async Task<List<string>> ImportEmployeeProjectsAsync(Stream csvStream)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HeaderValidated = null,
            MissingFieldFound = null,
            BadDataFound = null
        };

        var errors = new List<string>();
        var records = new List<EmployeeProjectsDto>();

        using var reader = new StreamReader(csvStream);
        using var csv = new CsvReader(reader, config);

        try
        {
            records = csv.GetRecords<EmployeeProjectsDto>().ToList();
        }
        catch (Exception ex)
        {
            errors.Add($"CSV parsing error: {ex.Message}");
            return errors;
        }

        foreach (var row in records)
        {
            var result = _validator.Validate(row);
            if (!result.IsValid)
            {
                errors.Add($"Validation failed (EmpID {row.EmpID}, ProjectID {row.ProjectID}): " +
                           string.Join("; ", result.Errors.Select(e => e.ErrorMessage)));
                continue;
            }

            // Check foreign keys
            if (!await _repository.EmployeeExistsAsync(row.EmpID))
            {
                errors.Add($"Employee not found: ID = {row.EmpID}");
                continue;
            }

            if (!await _repository.ProjectExistsAsync(row.ProjectID))
            {
                errors.Add($"Project not found: ID = {row.ProjectID}");
                continue;
            }

            var entity = new EmployeeProjectMap
            {
                EmpID = row.EmpID,
                ProjectID = row.ProjectID,
                DateFrom = row.DateFrom,
                DateTo = row.DateTo
            };

            if (await _repository.ExistsAsync(entity))
            {
                errors.Add($"Duplicate: EmpID {entity.EmpID}, ProjectID {entity.ProjectID}, DateFrom {entity.DateFrom:yyyy-MM-dd}");
                continue;
            }

            await _repository.InsertAsync(entity);
        }

        return errors;
    }
}
