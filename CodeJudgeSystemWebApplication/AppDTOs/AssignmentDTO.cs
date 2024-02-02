namespace CodeJudgeSystemWebApplication.AppModels;

public class AssignmentDTO
{
    public int Id { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string AssignmentName { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public string Course { get; set; } = string.Empty;
    public string Semester { get; set; } = string.Empty;
    public string TargetGroup { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ExpectedInputAndOutputPairs { get; set; } = string.Empty;

}
