using CsvHelper.Configuration.Attributes;

public class EmployeeProjectsDto
{
    [Name("EmpID")]
    public int EmpID { get; set; }

    [Name("ProjectID")]
    public int ProjectID { get; set; }

    [Name("DateFrom")]
    [TypeConverter(typeof(CustomDateTimeConverter))]
    public DateTime DateFrom { get; set; }

    [Name("DateTo")]
    [TypeConverter(typeof(CustomDateTimeConverter))]
    public DateTime DateTo { get; set; }
}
