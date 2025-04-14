using EmployeeAssignments.API.Dtos;
using EmployeeAssignments.API.Repositories;

namespace EmployeeAssignments.API.Services
{
    public class EmployeeProjectService(IEmployeeProjectRepository employeeProjectRepository) : IEmployeeProjectService
    {
        private readonly IEmployeeProjectRepository _employeeProjectRepository = employeeProjectRepository;

        public async Task<EmployeePairResultDto?> GetLongestWorkingPairAsync()
        {
            var result = await _employeeProjectRepository.GetAllAsync();

            if (!result.IsSuccess || result.Data == null)
            {
                // Optionally log result.Message for diagnostics
                return null; // or throw if preferred
            }

            var records = result.Data;

            var pairs = new Dictionary<string, (int TotalDays, List<ProjectOverlapDetailDto> Projects)>();

            var groupedByProject = records
                .GroupBy(r => r.ProjectID)
                .ToList();

            foreach (var group in groupedByProject)
            {
                var projectRecords = group.ToList();
                for (int i = 0; i < projectRecords.Count; i++)
                {
                    for (int j = i + 1; j < projectRecords.Count; j++)
                    {
                        var a = projectRecords[i];
                        var b = projectRecords[j];

                        var overlapStart = a.DateFrom > b.DateFrom ? a.DateFrom : b.DateFrom;
                        var overlapEnd = a.DateTo < b.DateTo ? a.DateTo : b.DateTo;
                        DateTime endDay = overlapEnd ?? DateTime.Now;

                        if (overlapStart < overlapEnd)
                        {
                            var overlapDays = (endDay - overlapStart).Days;

                            var key = a.EmpID < b.EmpID ? $"{a.EmpID}-{b.EmpID}" : $"{b.EmpID}-{a.EmpID}";
                            if (!pairs.ContainsKey(key))
                                pairs[key] = (0, new List<ProjectOverlapDetailDto>());

                            pairs[key] = (
                                pairs[key].TotalDays + overlapDays,
                                pairs[key].Projects.Append(new ProjectOverlapDetailDto
                                {
                                    ProjectID = group.Key,
                                    OverlapDays = overlapDays
                                }).ToList()
                            );
                        }
                    }
                }
            }

            var best = pairs.OrderByDescending(p => p.Value.TotalDays).FirstOrDefault();

            if (best.Value.TotalDays == 0)
                return null;

            var empIDs = best.Key.Split('-').Select(int.Parse).ToArray();

            return new EmployeePairResultDto
            {
                EmpID1 = empIDs[0],
                EmpID2 = empIDs[1],
                TotalDaysWorkedTogether = best.Value.TotalDays,
                Projects = best.Value.Projects
            };
        }
    }
}
