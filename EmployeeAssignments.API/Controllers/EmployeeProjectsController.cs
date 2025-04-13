using EmployeeAssignments.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeAssignments.API.Controllers;
[ApiController]
[Route("[controller]")]
public class EmployeeProjectsController(IEmployeeProjectService analyticsService) : ControllerBase
{
    private readonly IEmployeeProjectService _analyticsService = analyticsService;

    [HttpGet(Name ="longest-coworking-pair")]
    public async Task<IActionResult> GetLongestPair()
    {
        var result = await _analyticsService.GetLongestWorkingPairAsync();
        if (result == null)
            return NotFound("No overlapping work periods found.");

        return Ok(result);
    }
}