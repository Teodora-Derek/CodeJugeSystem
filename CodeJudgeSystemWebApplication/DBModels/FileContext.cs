using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CodeJudgeSystemWebApplication.Models
{
    public class FileContext : DbContext
    {
        public DbSet<FileModel> Files { get; set; }

        public FileContext(DbContextOptions<FileContext> options) : base(options)
        { }
    }
}
