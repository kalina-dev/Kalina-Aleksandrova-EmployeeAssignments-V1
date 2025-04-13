namespace EmployeeAssignments.API.Entities
{
    public class EmployeeProjectMap
    {
        public int EmpID { get; set; }
        public int ProjectID { get; set; }
        public DateTime DateFrom { get; set; } = DateTime.UtcNow;
        public DateTime? DateTo { get; set; }
    }
}
