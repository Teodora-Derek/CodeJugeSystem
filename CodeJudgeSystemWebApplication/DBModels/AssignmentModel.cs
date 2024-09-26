using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace CodeJudgeSystemWebApplication.Models
{
    public class AssignmentModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MemberNotNull(nameof(Subject))]
        [Column(TypeName = "nvarchar(100)")]
        public string Subject { get; set; } = string.Empty;

        [Required]
        [MemberNotNull(nameof(AssignmentName))]
        [Column(TypeName = "nvarchar(100)")]
        public string AssignmentName { get; set; } = string.Empty;

        [Required]
        [MemberNotNull(nameof(DueDate))]
        public DateTime DueDate { get; set; }

        [Required]
        public string Course { get; set; } = string.Empty;

        [Required]
        public string Semester { get; set; } = string.Empty;

        [Required]
        public string TargetGroup { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string ExpectedInputAndOutputPairs { get; set; } = string.Empty;

    }

}
