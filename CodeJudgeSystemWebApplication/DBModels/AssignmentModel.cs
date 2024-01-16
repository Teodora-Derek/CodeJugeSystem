using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using System;

namespace CodeJudgeSystemWebApplication.Models
{
    public class AssignmentModel
    {
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
        [Column(TypeName = "nvarchar(100)")]
        public string DueDate { get; set; } = string.Empty;

        [Required]
        public short Course { get; set;}

        [Required]
        public short Semester { get; set; }

        [Required]      
        public short TargetGroup { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

    }

}   
