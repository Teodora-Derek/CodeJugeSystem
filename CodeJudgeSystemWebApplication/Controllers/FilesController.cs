using Microsoft.AspNetCore.Mvc;
using CodeJudgeSystemWebApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using CodeJudgeSystemWebApplication.Options;
using System.IO.Compression;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis;
using System.Diagnostics;
using System.Reflection;
using Microsoft.CodeAnalysis.Text;
using System.IO;
using CodeJudgeSystemWebApplication.Services;
using CodeJudgeSystemWebApplication.AppModels;
using CodeJudgeSystemWebApplication.Migrations.Assignment;

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
        public async Task<ActionResult<IEnumerable<FileModel>>> GetFiles()
        {
            return await _context.Files.ToListAsync();
        }

        [HttpGet("{fileId}")]
        public IActionResult GetFile(int fileId)
        {
            var file = _context.Files.Find(fileId);

            if (file is null)
            {
                return NotFound();
            }

            return Ok(new
            {
                FileName = file.FileName,
                FileExtention = file.FileExtention,
                UploadTime = file.UploadTime
            });

            /*byte[]? fileData = file.FileData;
            if (fileData == null)
            {
                return BadRequest("Files data is empty!");
            }
            var base64EncodedData = Convert.ToBase64String(fileData);

            return Ok(new
            {
                FileName = file.FileName,
                FileType = file.FileExtention,
                FileData = base64EncodedData,
                UploadTime = file.UploadTime
            });*/
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IOptions<AppOptions> optAppOptions, [FromForm] FileModelDTO model, int assignmentId)
        {
            assignmentId = 1;
            var assignment = _assignmentContext.Assignments.Find(assignmentId);

            if (assignment == null)
                return BadRequest();

            assignment.ExpectedInputAndOutputPairs = "1-2,3-4,5-6";
            List<KeyValuePair<string, string>> inputAndOutputPairs = _fileService.ConvertToInputAndOutput(assignment.ExpectedInputAndOutputPairs);

            var file = new FileModel
            {
                FileName = model.File.FileName,
                FileExtention = Path.GetExtension(model.File.FileName),
                UploadTime = DateTime.Now,
            };

            _context.Files.Add(file);
            await _context.SaveChangesAsync();

            var grade = 2;

            foreach (var iopair in inputAndOutputPairs)
            {
                var fileUploadResult = await Task.Run(() => _fileService.RunFileDynamically(model.File,iopair.Key));

                if (fileUploadResult.ToLower() == iopair.Value.ToLower())
                {
                    grade++;
                }
            }

            var fileSaveResult = await _fileService.SaveFileInFileSystemAsync(optAppOptions.Value.UnzipFolderPath, model.File);

            return Ok(grade);
        }
    }
}
