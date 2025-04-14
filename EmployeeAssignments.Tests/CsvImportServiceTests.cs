using Moq;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using EmployeeAssignments.API.Repositories;
using EmployeeAssignments.API.Entities;
using EmployeeAssignments.API.Services;
using EmployeeAssignments.API.Models;

namespace EmployeeAssignments.Tests
{
    public class CsvImportServiceTests
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly Mock<IEmployeeProjectRepository> _repoMock;

        public CsvImportServiceTests()
        {
            var services = new ServiceCollection();

            _repoMock = new Mock<IEmployeeProjectRepository>();

            services.AddSingleton(_repoMock.Object);
            services.AddTransient<CsvImportService>();

            _serviceProvider = services.BuildServiceProvider();
        }

        private CsvImportService GetService() => _serviceProvider.GetRequiredService<CsvImportService>();

        private static Stream ToStream(string csv)
        {
            var bytes = Encoding.UTF8.GetBytes(csv);
            return new MemoryStream(bytes);
        }

        [Fact]
        public async Task Import_WithValidCsv_ShouldInsertOnce()
        {
            _repoMock.Setup(r => r.EmployeeExistsAsync(1))
                .ReturnsAsync(RepositoryResult<bool>.Ok(true));
            _repoMock.Setup(r => r.ProjectExistsAsync(10))
                .ReturnsAsync(RepositoryResult<bool>.Ok(true));
            _repoMock.Setup(r => r.ExistsAsync(It.IsAny<EmployeeProjectMap>()))
                .ReturnsAsync(RepositoryResult<bool>.Ok(false));
            _repoMock.Setup(r => r.InsertAsync(It.IsAny<EmployeeProjectMap>()))
                .ReturnsAsync(RepositoryResult<bool>.Ok(true));

            var csv = "EmpID,ProjectID,DateFrom,DateTo\n1,10,2022-01-01,2022-12-31";
            var service = GetService();

            var result = await service.ImportEmployeeProjectsAsync(ToStream(csv));

            Assert.Empty(result);
            _repoMock.Verify(r => r.InsertAsync(It.IsAny<EmployeeProjectMap>()), Times.Once);
        }

        [Fact]
        public async Task Import_WithInvalidDateRange_ShouldFail()
        {
            var csv = "EmpID,ProjectID,DateFrom,DateTo\n1,10,2023-01-01,2022-01-01";
            var service = GetService();

            var result = await service.ImportEmployeeProjectsAsync(ToStream(csv));

            Assert.Single(result);
            Assert.Contains("DateTo must be after DateFrom", result.First());
            _repoMock.Verify(r => r.InsertAsync(It.IsAny<EmployeeProjectMap>()), Times.Never);
        }

        [Fact]
        public async Task Import_WithMissingFK_ShouldSkip()
        {
            _repoMock.Setup(r => r.EmployeeExistsAsync(1))
                .ReturnsAsync(RepositoryResult<bool>.Ok(false));
            _repoMock.Setup(r => r.ProjectExistsAsync(10))
                .ReturnsAsync(RepositoryResult<bool>.Ok(true));

            var csv = "EmpID,ProjectID,DateFrom,DateTo\n1,10,2022-01-01,2022-12-31";
            var service = GetService();

            var result = await service.ImportEmployeeProjectsAsync(ToStream(csv));

            Assert.Single(result);
            Assert.Contains("Employee not found", result.First());
            _repoMock.Verify(r => r.InsertAsync(It.IsAny<EmployeeProjectMap>()), Times.Never);
        }

        [Fact]
        public async Task Import_WithDuplicate_ShouldSkip()
        {
            _repoMock.Setup(r => r.EmployeeExistsAsync(1))
                .ReturnsAsync(RepositoryResult<bool>.Ok(true));
            _repoMock.Setup(r => r.ProjectExistsAsync(10))
                .ReturnsAsync(RepositoryResult<bool>.Ok(true));
            _repoMock.Setup(r => r.ExistsAsync(It.IsAny<EmployeeProjectMap>()))
                .ReturnsAsync(RepositoryResult<bool>.Ok(true));

            var csv = "EmpID,ProjectID,DateFrom,DateTo\n1,10,2022-01-01,2022-12-31";
            var service = GetService();

            var result = await service.ImportEmployeeProjectsAsync(ToStream(csv));

            Assert.Single(result);
            Assert.Contains("Duplicate", result.First());
            _repoMock.Verify(r => r.InsertAsync(It.IsAny<EmployeeProjectMap>()), Times.Never);
        }

        [Fact]
        public async Task Import_WhenRepositoryFails_ShouldReturnError()
        {
            _repoMock.Setup(r => r.EmployeeExistsAsync(1))
                .ReturnsAsync(RepositoryResult<bool>.Error(System.Net.HttpStatusCode.InternalServerError, "Database error"));

            var csv = "EmpID,ProjectID,DateFrom,DateTo\n1,10,2022-01-01,2022-12-31";
            var service = GetService();

            var result = await service.ImportEmployeeProjectsAsync(ToStream(csv));

            Assert.Single(result);
            Assert.Contains("Error checking employee", result.First());
        }
    }
}
