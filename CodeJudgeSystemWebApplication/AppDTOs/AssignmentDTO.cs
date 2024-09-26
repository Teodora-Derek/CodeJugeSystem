namespace CodeJudgeSystemWebApplication.AppModels;

public class AssignmentDTO
{
    public string Subject { get; set; } = string.Empty;
    public string AssignmentName { get; set; } = string.Empty;
    public string DueDate { get; set; } = string.Empty;
    public short Course { get; set; }
    public short Semester { get; set; }
    public short TargetGroup { get; set; }
    public string Description { get; set; } = string.Empty;

}
