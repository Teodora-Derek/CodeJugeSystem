using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace CodeJudgeSystemWebApplication.Models
{
    public class FileModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FileID { get; set; }

        [Required]
        [MemberNotNull(nameof(FileName))]
        public string FileName { get; set; } = string.Empty;

        [Required]
        [MemberNotNull(nameof(FileExtention))]
        public string FileExtention { get; set; } = string.Empty;

        [Required]
        public DateTime UploadTime { get; set; }

        public int AssignmentId { get; set; }
        public AssignmentModel Assignment{ get; set; }

        public int Grade { get; set; }
    }
}
