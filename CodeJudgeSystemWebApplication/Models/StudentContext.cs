using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System.Configuration;

namespace CodeJudgeSystemWebApplication.Models
{
    public class StudentContext : DbContext
    {
        public DbSet<Student> Students { get; set; } = null!;

        public StudentContext(DbContextOptions<StudentContext> options) : base(options)
        { }

        //public void ConfigureServices(WebApplicationBuilder builder)
        //{
        //    var connetionString = builder.Configuration.GetConnectionString("DBConn");
        //    builder.Services.AddDbContextPool<StudentContext>(
        //            options => options.UseMySql(connetionString, ServerVersion.AutoDetect(connetionString))
        //        );
        //}

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Student>().Property(s => s.FacultyNumber).IsRequired();
        //}
    }
}
