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

            // Assuming the entry data is in base64 format for simplicity
            byte[]? fileData = file.FileData;
            if (fileData == null)
            {
                return BadRequest("Files data is empty!");
            }
            var base64EncodedData = Convert.ToBase64String(fileData);

            return Ok(new
            {
                FileName = file.FileName,
                FileType = file.FileType,
                FileData = base64EncodedData
            });
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IOptions<AppOptions> optAppOptions, [FromForm] FileUploadModel model)
        {
            var appOptions = optAppOptions.Value;

            var isSavedTask = _fileService.SaveFileInFileSystemAsync(appOptions.UnzipFolderPath, model.File);
            var isExecutedTask = _fileService.RunFileDynamicallyAsync(model.File);

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
                : BadRequest(error);
        }


        [HttpPost]
        public async Task<IActionResult> Upload(IOptions<AppOptions> optAppOptions, byte[] zip)
        {
            try
            {
                var appOptions = optAppOptions.Value;

                using (var ms = new MemoryStream(zip))
                {
                    var zipArchive = new ZipArchive(ms);

                    var references = AppDomain.CurrentDomain.GetAssemblies()
                        .Where(a => !a.IsDynamic && !string.IsNullOrEmpty(a.Location))
                        .Select(a => MetadataReference.CreateFromFile(a.Location))
                        .ToList();

                    var compilation = CSharpCompilation.Create("MyDynamicCompilation",
                        syntaxTrees: zipArchive.Entries.Select(entry => CSharpSyntaxTree.ParseText(SourceText.From(entry.Open()))),
                        references: references,
                        options: new CSharpCompilationOptions(OutputKind.ConsoleApplication));

                    EmitResult result = compilation.Emit(ms);

                    if (!result.Success)
                    {
                        var failures = result.Diagnostics
                            .Where(diagnostic => diagnostic.IsWarningAsError || diagnostic.Severity == DiagnosticSeverity.Error);

                        foreach (var diagnostic in failures)
                        {
                            Console.Error.WriteLine($"{diagnostic.Id}: {diagnostic.GetMessage()}");
                        }
                    }
                    else
                    {
                        // Step 4: Execute the application
                        ms.Seek(0, SeekOrigin.Begin);
                        var assembly = Assembly.Load(ms.ToArray());

                        // Find the entry point
                        var entryPoint = assembly.EntryPoint;
                        if (entryPoint != null)
                        {
                            // Create parameters if needed
                            object[]? parameters = entryPoint.GetParameters().Length == 0 ? null : new object[] { new string[] { } };

                            // Invoke the entry point
                            entryPoint.Invoke(null, parameters);
                        }
                    }
                }
                return Ok("Server said: Files uploaded successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
