using Xunit;
using Moq;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using EmployeeAssignmentsV1.Repositories;
using EmployeeAssignmentsV1.Services;
using EmployeeAssignmentsV1.Entities;

namespace EmployeeProject.Tests
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
            services.AddTransient<CsvImportService>(); // 💡 The real service

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
            // Arrange
            _repoMock.Setup(r => r.EmployeeExistsAsync(1)).ReturnsAsync(true);
            _repoMock.Setup(r => r.ProjectExistsAsync(10)).ReturnsAsync(true);
            _repoMock.Setup(r => r.ExistsAsync(It.IsAny<EmployeeProjectMap>())).ReturnsAsync(false);

            var csv = "EmpID,ProjectID,DateFrom,DateTo\n1,10,2022-01-01,2022-12-31";
            var service = GetService();

            // Act
            var result = await service.ImportEmployeeProjectsAsync(ToStream(csv));

            // Assert
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
            _repoMock.Setup(r => r.EmployeeExistsAsync(1)).ReturnsAsync(false);
            _repoMock.Setup(r => r.ProjectExistsAsync(10)).ReturnsAsync(true);

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
            _repoMock.Setup(r => r.EmployeeExistsAsync(1)).ReturnsAsync(true);
            _repoMock.Setup(r => r.ProjectExistsAsync(10)).ReturnsAsync(true);
            _repoMock.Setup(r => r.ExistsAsync(It.IsAny<EmployeeProjectMap>())).ReturnsAsync(true);

            var csv = "EmpID,ProjectID,DateFrom,DateTo\n1,10,2022-01-01,2022-12-31";
            var service = GetService();

            var result = await service.ImportEmployeeProjectsAsync(ToStream(csv));

            Assert.Single(result);
            Assert.Contains("Duplicate", result.First());
            _repoMock.Verify(r => r.InsertAsync(It.IsAny<EmployeeProjectMap>()), Times.Never);
        }
    }
}