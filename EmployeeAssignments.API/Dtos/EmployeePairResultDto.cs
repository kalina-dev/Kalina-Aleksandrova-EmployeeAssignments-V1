namespace EmployeeAssignments.API.Dtos
{
    public class EmployeePairResultDto
    {
        public int EmpID1 { get; set; }
        public int EmpID2 { get; set; }
        public int TotalDaysWorkedTogether { get; set; }
        public List<ProjectOverlapDetailDto> Projects { get; set; } = [];
    }
}
