using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CodeJudgeSystemWebApplication.Models
{
    public class FileContext : DbContext
    {
        public DbSet<FileModel> Files { get; set; }

        public FileContext(DbContextOptions<FileContext> options) : base(options)
        {
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FileModel>()
                .HasOne(f => f.Assignment)
                .WithMany(a => a.Files)
                .HasForeignKey(f => f.AssignmentId);
        }
    }
}
