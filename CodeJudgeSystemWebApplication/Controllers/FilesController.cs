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
using static System.Runtime.InteropServices.JavaScript.JSType;
using CodeJudgeSystemWebApplication.AppModels;

namespace CodeJudgeSystemWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly FileContext _context;
        private readonly IFileService _fileService;

        public FilesController(FileContext context, IFileService fileService)
        {
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

            if (file == null)
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
        public async Task<IActionResult> Upload(IOptions<AppOptions> optAppOptions, [FromForm] FileModelDTO model)
        {
            var file = new FileModel
            {
                FileName = model.File.FileName,
                FileExtention = Path.GetExtension(model.File.FileName),
                UploadTime = DateTime.Now,
            };

            _context.Files.Add(file);
            await _context.SaveChangesAsync();

            var fileUploadResult = await Task.Run(() => _fileService.RunFileDynamically(model.File));
            var fileSaveResult = await _fileService.SaveFileInFileSystemAsync(optAppOptions.Value.UnzipFolderPath, model.File);

            if (fileUploadResult.IsSuccessful && fileSaveResult)
                return Ok(fileUploadResult.Result);

            var problem = ((object?)fileUploadResult.Diagnostics
                    ?? fileUploadResult.Exception)
                    ?? fileUploadResult.Error;

            return BadRequest(problem);

            /*
            var appOptions = optAppOptions.Value;

            var isSavedTask = _fileService.SaveFileInFileSystemAsync(appOptions.UnzipFolderPath, model.File);
            var isExecutedTask = _fileService.RunFileDynamically(model.File);

            await Task.WhenAll(isSavedTask, isExecutedTask);

            var saveFileError = "Cannot save file!";
            var runFileError = "Cannot run file!";

            var error = (isSavedTask.Result, isExecutedTask.Result) switch
            {
                (false, false) => string.Empty,
                (true, false) => saveFileError,
                (false, true) => runFileError,
                (true, true) => saveFileError + Environment.NewLine + runFileError,
            };

            return error == string.Empty
                ? Ok()
                : BadRequest(error);*/
        }
    }
}
