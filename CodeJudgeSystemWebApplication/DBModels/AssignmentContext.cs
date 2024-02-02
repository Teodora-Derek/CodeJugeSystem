using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CodeJudgeSystemWebApplication.Models
{
    public class AssignmentContext : DbContext
    {
        public DbSet<AssignmentModel> Assignments { get; set; }

        public AssignmentContext(DbContextOptions<AssignmentContext> options) : base(options)
        {

        }
    }
}
