using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using EmployeeAssignments.API.Entities;
using EmployeeAssignments.API.Repositories;
using EmployeeAssignments.API.Validators;

namespace EmployeeAssignments.API.Services
{
    public class CsvImportService(IEmployeeProjectRepository repository) : ICsvImportService
    {
        private readonly IEmployeeProjectRepository _repository = repository;
        private readonly EmployeeProjectValidator _validator = new();

        public async Task<List<string>> ImportEmployeeProjectsAsync(Stream csvStream)
        {
            var errors = new List<string>();
            var records = new List<EmployeeProjectsDto>();

            // CSV Configuration
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null,
                BadDataFound = null
            };

            using var reader = new StreamReader(csvStream);
            using var csv = new CsvReader(reader, config);

            try
            {
                // Read the CSV records
                records = csv.GetRecords<EmployeeProjectsDto>().ToList();
            }
            catch (Exception ex)
            {
                errors.Add($"CSV parsing error: {ex.Message}");
                return errors;
            }

            // Validate and process the records
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
                var employeeExistsResult = await _repository.EmployeeExistsAsync(row.EmpID);
                if (!employeeExistsResult.IsSuccess)
                {
                    errors.Add($"Employee not found: ID = {row.EmpID}");
                    continue;
                }

                var projectExistsResult = await _repository.ProjectExistsAsync(row.ProjectID);
                if (!projectExistsResult.IsSuccess)
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

                // Check if the record already exists
                var existsResult = await _repository.ExistsAsync(entity);
                if (existsResult.IsSuccess)
                {
                    errors.Add($"Duplicate: EmpID {entity.EmpID}, ProjectID {entity.ProjectID}, DateFrom {entity.DateFrom:yyyy-MM-dd}");
                    continue;
                }

                // Insert the entity into the repository
                await _repository.InsertAsync(entity);
            }

            return errors;
        }
    }
}
