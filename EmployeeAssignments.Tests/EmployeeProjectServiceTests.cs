using EmployeeAssignmentsV1.Entities;
using EmployeeAssignmentsV1.Repositories;
using EmployeeAssignmentsV1.Services;
using Moq;
using Xunit;

namespace EmployeeProject.Tests
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

            _repoEmployeeProjectMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(listEmployeeProjects);

            // Act
            var result = await _employeeProjectService.GetLongestWorkingPairAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.EmpID1); // Employee 1
            Assert.Equal(2, result.EmpID2); // Employee 2
            Assert.True(result.TotalDaysWorkedTogether > 0); // Should be more than 0 days
            Assert.Single(result.Projects); // Shared projects count
        }

        [Fact]
        public async Task GetLongestWorkingPairAsync_ReturnsInCorrectPair()
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

            _repoEmployeeProjectMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(listEmployeeProjects);

            // Act
            var result = await _employeeProjectService.GetLongestWorkingPairAsync();

            // Assert
            Assert.Null(result);
            Assert.NotEqual(1, result?.EmpID1); // Employee 1
            Assert.NotEqual(2, result?.EmpID2); // Employee 2
            Assert.False(result?.TotalDaysWorkedTogether == 0); // Should be more than 0 days
            Assert.NotEqual(1, result?.Projects.Count); // Shared projects count
        }
    }
}