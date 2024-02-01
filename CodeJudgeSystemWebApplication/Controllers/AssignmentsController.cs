using CodeJudgeSystemWebApplication.AppModels;
using CodeJudgeSystemWebApplication.Models;
using Microsoft.AspNetCore.Mvc;


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
        public void AddAssignment(AssignmentDTO assignmentDTO)
        {
            AssignmentModel assignment = new AssignmentModel
            {
                Subject = assignmentDTO.Subject,
                AssignmentName = assignmentDTO.AssignmentName,
                DueDate = assignmentDTO.DueDate,
                Course = assignmentDTO.Course,
                Semester = assignmentDTO.Semester,
                TargetGroup = assignmentDTO.TargetGroup,
                Description = assignmentDTO.Description,
                ExpectedInputAndOutputPairs = assignmentDTO.ExpectedInputAndOutputPairs,

            };
            _context.Assignments.Add(assignment);
            _context.SaveChanges();
        }

        [HttpPut]
        public void UpdateAssignment(AssignmentDTO updatedAssignmentDTO)
        {
            var existingAssignment = _context.Assignments.Find(updatedAssignmentDTO.Id);

            if (existingAssignment != null)
            {
                existingAssignment.Subject = updatedAssignmentDTO.Subject;
                existingAssignment.AssignmentName = updatedAssignmentDTO.AssignmentName;
                existingAssignment.DueDate = updatedAssignmentDTO.DueDate;
                existingAssignment.Course = updatedAssignmentDTO.Course;
                existingAssignment.Semester = updatedAssignmentDTO.Semester;
                existingAssignment.TargetGroup = updatedAssignmentDTO.TargetGroup;
                existingAssignment.Description = updatedAssignmentDTO.Description;
                existingAssignment.ExpectedInputAndOutputPairs = updatedAssignmentDTO.ExpectedInputAndOutputPairs;

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
