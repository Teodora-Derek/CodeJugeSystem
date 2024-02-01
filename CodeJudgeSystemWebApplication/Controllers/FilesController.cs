using CodeJudgeSystemWebApplication.AppModels;
using CodeJudgeSystemWebApplication.Models;
using CodeJudgeSystemWebApplication.Options;
using CodeJudgeSystemWebApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CodeJudgeSystemWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly FileContext _context;
        private readonly AssignmentContext _assignmentContext;
        private readonly IFileService _fileService;

        public FilesController(FileContext context, AssignmentContext assignmentContext, IFileService fileService)
        {
            _assignmentContext = assignmentContext;
            _context = context;
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FileModel>>> GetFiles([FromQuery] int assignmentId)
        {
            return await _context.Files
                .Where(f => f.AssignmentId == assignmentId).ToListAsync();
        }

        [HttpGet("{grade}")]
        public async Task<ActionResult<string>> GetFinalGrade([FromQuery] int assignmentId)
        {
            var grade = 2;

            string defaultGradeMessage = "N/A";

            var allAssignmentGrades = await _context.Files
                .Where(f => f.AssignmentId == assignmentId).Select(f => f.Grade).ToListAsync();

            if (allAssignmentGrades.IsNullOrEmpty())
            {
                return Ok(defaultGradeMessage);
            }

            foreach (var g in allAssignmentGrades)
            {
                if (g > grade)
                    grade = g;
            }

            return Ok(grade.ToString());

        }


        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IOptions<AppOptions> optAppOptions, [FromForm] FileModelDTO model, [FromQuery] int assignmentId)
        {

            var assignment = _assignmentContext.Assignments.Find(assignmentId);

            if (assignment == null)
                return BadRequest();

            List<KeyValuePair<string, string>> inputAndOutputPairs = _fileService.ConvertToInputAndOutput(assignment.ExpectedInputAndOutputPairs);

            var grade = 2;

            foreach (var iopair in inputAndOutputPairs)
            {
                var fileUploadResult = await Task.Run(() => _fileService.RunFileDynamically(model.File, iopair.Key));

                if (fileUploadResult.ToLower() == iopair.Value.ToLower())
                {
                    grade++;
                }
            }

            var fileSaveResult = await _fileService.SaveFileInFileSystemAsync(optAppOptions.Value.UnzipFolderPath, model.File);

            var file = new FileModel
            {
                FileName = model.File.FileName,
                FileExtention = Path.GetExtension(model.File.FileName),
                UploadTime = DateTime.Now,
                AssignmentId = assignmentId,
                Grade = grade
            };

            var allAssignmentGrades = await _context.Files
                .Where(f => f.AssignmentId == assignmentId).Select(f => f.Grade).ToListAsync();

            foreach (var g in allAssignmentGrades)
            {
                if (g > grade)
                    grade = g;
            }

            _context.Files.Add(file);
            await _context.SaveChangesAsync();

            return Ok(grade);
        }
    }
}
