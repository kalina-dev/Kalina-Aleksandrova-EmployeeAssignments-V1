using EmployeeAssignments.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeAssignments.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ImportController(ICsvImportService importService) : ControllerBase
{
    private readonly ICsvImportService _importService = importService;

    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpPost(Name = "employeeprojects")]
    public async Task<IActionResult> ImportEmployeeProjects([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("CSV file is required.");

        using var stream = file.OpenReadStream();
        var errors = await _importService.ImportEmployeeProjectsAsync(stream);

        if (errors.Any())
            return BadRequest(new { message = "Some rows failed", errors });

        return Ok(new { message = "Import successful ✅" });
    }
}
