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
        [MemberNotNull(nameof(FileType))]
        public string FileType { get; set; } = string.Empty;

        [Required]
        [MemberNotNull(nameof(FileData))]
        public byte[]? FileData { get; set; }
    }
}
