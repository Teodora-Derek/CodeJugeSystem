using CodeJudgeSystemWebApplication.AppModels;
using CodeJudgeSystemWebApplication.Models;
using CodeJudgeSystemWebApplication.Services;
using Microsoft.AspNetCore.Mvc;


namespace CodeJudgeSystemWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentsController : ControllerBase
    {
        private readonly AssignmentContext _context;

        private readonly IFileService _fileService;

        public AssignmentsController(AssignmentContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
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
        public IActionResult AddAssignment([FromForm] AssignmentDTO assignmentDTO)
        {
            try
            {
                try
                {
                   var pairs = _fileService.ConvertToInputAndOutput(assignmentDTO.ExpectedInputAndOutputPairs);
                   if(pairs.Count != 4)
                    {
                        throw new Exception("The pairs have to be exactly 4");
                    }    
                }
                catch (Exception)
                {
                    return BadRequest("The ExpectedInputAndOutputPairs are not in a valid format");
                }

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

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest("There was an error trying to add a new assignment");
            }
        }

        [HttpPut]
        public IActionResult UpdateAssignment(AssignmentDTO updatedAssignmentDTO)
        {
            try
            {
                var existingAssignment = _context.Assignments.Find(updatedAssignmentDTO.Id);

                try
                {
                    var pairs = _fileService.ConvertToInputAndOutput(updatedAssignmentDTO.ExpectedInputAndOutputPairs);
                    if (pairs.Count != 4)
                    {
                        throw new Exception("The pairs have to be exactly 4");
                    }
                }
                catch (Exception)
                {
                    return BadRequest("The ExpectedInputAndOutputPairs are not in a valid format");
                }

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

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest("There was an error trying to update the assignment");
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
