namespace CodeJudgeSystemWebApplication.AppModels;

public class StudentDTO
{
    public string FacultyNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string AcademicDegreeName { get; set; } = string.Empty;
    public short Group { get; set; }
    public short Semester { get; set; }
    public short Course { get; set; }
}
