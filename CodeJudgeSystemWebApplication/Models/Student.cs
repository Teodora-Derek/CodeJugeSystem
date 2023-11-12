using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace CodeJudgeSystemWebApplication.Models
{
    public class Student
    {
        [Key]
        [NineDigitNumeric]
        [MemberNotNull(nameof(FacultyNumber))]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [StringLength(9, MinimumLength = 9)]
        public string FacultyNumber { get; set; } = string.Empty;

        [Required]
        [MemberNotNull(nameof(FacultyName))]
        [Column(TypeName = "nvarchar(10)")]
        public string FacultyName { get; set; } = string.Empty;

        [Required]
        [MemberNotNull(nameof(FirstName))]
        [Column(TypeName = "nvarchar(100)")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MemberNotNull(nameof(MiddleName))]
        [Column(TypeName = "nvarchar(100)")]
        public string MiddleName { get; set; } = string.Empty;

        [Required]
        [MemberNotNull(nameof(LastName))]
        [Column(TypeName = "nvarchar(100)")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [MemberNotNull(nameof(AcademicDegreeName))]
        [Column(TypeName = "nvarchar(100)")]
        public string AcademicDegreeName { get; set; } = string.Empty;

        [Required]
        [MemberNotNull(nameof(Email))]
        [Column(TypeName = "nvarchar(100)")]
        public string Email { get; set; } = string.Empty;

        [Required]
        public DegreeType AcademicDegreeType { get; set; }

        [Required]
        public short Stream { get; set; }

        [Required]
        public short Group { get; set; }

        [Required]
        public short Semester { get; set; }

        [Required]
        public short Course { get; set; }

        [Required]
        public StudyType StudyType { get; set; }

        [Required]
        public Status Status { get; set; }
    }

    public enum DegreeType
    {
        Bachelor,
        Master
    }

    public enum StudyType
    {
        FullTime,
        PartTime
    }

    public enum Status
    {
        Active,
        Disabled
    }


    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public partial class NineDigitNumericAttribute : ValidationAttribute
    {
        [GeneratedRegex("^[0-9]{9}$")]
        private static partial Regex FacultyNumberValidationRegex();

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (value is string facultyNumber)
            {
                if (FacultyNumberValidationRegex().IsMatch(facultyNumber))
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult("The faculty number must be a string with exactly 9 numeric digits.");
        }
    }
}
