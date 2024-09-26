using CodeJudgeSystemWebApplication.Models;
using CodeJudgeSystemWebApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace CodeJudgeSystemWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentsController : ControllerBase
    {
        private readonly AssignmentContext _context;

        public AssignmentsController(AssignmentContext context)
        {
            _context = context;
        }
        

        [HttpGet("{assignmentId}")]
        public AssignmentModel GetAssignmentById(int assignmentId)
        {
            return _context.Assignments.Find(assignmentId);
        }

        [HttpGet]
        public List<AssignmentModel> GetAllAssignments()
        {
            return _context.Assignments.ToList();
        }

        [HttpPost]
        public void AddAssignment(AssignmentModel assignment)
        {
            _context.Assignments.Add(assignment);
            _context.SaveChanges();
        }

        [HttpPut]
        public void UpdateAssignment(AssignmentModel updatedAssignment)
        {
            var existingAssignment = _context.Assignments.Find(updatedAssignment.Id);

            if (existingAssignment != null)
            {
                existingAssignment.Subject = updatedAssignment.Subject;
                existingAssignment.AssignmentName = updatedAssignment.AssignmentName;
                existingAssignment.DueDate = updatedAssignment.DueDate;
                existingAssignment.Course = updatedAssignment.Course;
                existingAssignment.Semester = updatedAssignment.Semester;
                existingAssignment.TargetGroup = updatedAssignment.TargetGroup;
                existingAssignment.Description = updatedAssignment.Description;
                existingAssignment.ExpectedInputAndOutputPairs = updatedAssignment.ExpectedInputAndOutputPairs;

                _context.SaveChanges();
            }
        }

        [HttpDelete("{assignmentId}")]
        public void DeleteAssignment(int assignmentId)
        {
            var assignmentToDelete = _context.Assignments.Find(assignmentId);

            if (assignmentToDelete != null)
            {
                _context.Assignments.Remove(assignmentToDelete);
                _context.SaveChanges();
            }
        }
    }
}
