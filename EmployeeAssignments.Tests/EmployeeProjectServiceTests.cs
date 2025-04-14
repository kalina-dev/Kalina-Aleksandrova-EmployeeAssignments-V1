using EmployeeAssignments.API.Entities;
using EmployeeAssignments.API.Models;
using EmployeeAssignments.API.Repositories;
using EmployeeAssignments.API.Services;
using Moq;
using Xunit;

namespace EmployeeAssignments.Tests
{
    public class EmployeeProjectServiceTests
    {
        private readonly EmployeeProjectService _employeeProjectService;
        private readonly Mock<IEmployeeProjectRepository> _repoEmployeeProjectMock;

        public EmployeeProjectServiceTests()
        {
            _repoEmployeeProjectMock = new Mock<IEmployeeProjectRepository>();
            _employeeProjectService = new EmployeeProjectService(_repoEmployeeProjectMock.Object);
        }

        [Fact]
        public async Task GetLongestWorkingPairAsync_ReturnsCorrectPair()
        {
            var listEmployeeProjects = new List<EmployeeProjectMap>
            {
                new EmployeeProjectMap
                {
                    EmpID = 1,
                    ProjectID = 101,
                    DateFrom = new DateTime(2022, 01, 10),
                    DateTo = new DateTime(2022, 08, 15)
                },
                new EmployeeProjectMap
                {
                    EmpID = 2,
                    ProjectID = 101,
                    DateFrom = new DateTime(2022, 01, 01),
                    DateTo = new DateTime(2022, 08, 15)
                }
            };

            _repoEmployeeProjectMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(RepositoryResult<IEnumerable<EmployeeProjectMap>>.Ok(listEmployeeProjects));

            var result = await _employeeProjectService.GetLongestWorkingPairAsync();

            Assert.NotNull(result);
            Assert.Equal(1, result.EmpID1);
            Assert.Equal(2, result.EmpID2);
            Assert.True(result.TotalDaysWorkedTogether > 0);
            Assert.Single(result.Projects);
        }

        [Fact]
        public async Task GetLongestWorkingPairAsync_ReturnsNull_WhenNoSharedProjects()
        {
            var listEmployeeProjects = new List<EmployeeProjectMap>
            {
                new EmployeeProjectMap
                {
                    EmpID = 1,
                    ProjectID = 101,
                    DateFrom = new DateTime(2022, 01, 10),
                    DateTo = new DateTime(2022, 08, 15)
                },
                new EmployeeProjectMap
                {
                    EmpID = 2,
                    ProjectID = 102,
                    DateFrom = new DateTime(2022, 01, 01),
                    DateTo = new DateTime(2022, 08, 15)
                }
            };

            _repoEmployeeProjectMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(RepositoryResult<IEnumerable<EmployeeProjectMap>>.Ok(listEmployeeProjects));

            var result = await _employeeProjectService.GetLongestWorkingPairAsync();

            Assert.Null(result);
        }

        [Fact]
        public async Task GetLongestWorkingPairAsync_ReturnsNull_OnRepositoryError()
        {
            _repoEmployeeProjectMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(RepositoryResult<IEnumerable<EmployeeProjectMap>>.Error(System.Net.HttpStatusCode.InternalServerError, "DB error"));

            var result = await _employeeProjectService.GetLongestWorkingPairAsync();

            Assert.Null(result);
        }
    }
}
