using CodeJudgeSystemWebApplication.AppModels;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Text;
using System.IO.Compression;
using System.Reflection;

namespace CodeJudgeSystemWebApplication.Services;

public interface IFileService
{
    FileUploadResult RunFileDynamically(IFormFile file);
    Task<bool> SaveFileInFileSystemAsync(string unzipFolderPath, IFormFile file);
}

public class FileService : IFileService
{
    private static readonly string[] _fileExtensions = new[] { ".cs" };
    private static readonly IReadOnlyCollection<MetadataReference> _references;

    static FileService()
    {
        _references = AppDomain.CurrentDomain
            .GetAssemblies()
            .Where(a => !a.IsDynamic)
            .Select(a => MetadataReference.CreateFromFile(a.Location))
            .ToList();
    }

    public FileUploadResult RunFileDynamically(IFormFile file)
    {
        var syntaxTrees = new List<SyntaxTree>();
        var options = new CSharpCompilationOptions(OutputKind.ConsoleApplication);

        using (var zipArchive = new ZipArchive(file.OpenReadStream()))
        {
            syntaxTrees.AddRange(zipArchive.Entries
                .Where(entry => _fileExtensions.All(x => entry.Name.EndsWith(x)))
                .Select(entry =>
                {
                    using var stream = entry.Open();
                    return CSharpSyntaxTree.ParseText(SourceText.From(stream));
                }));
        }

        var compilation = CSharpCompilation.Create(
            "MyDynamicCompilation",
            syntaxTrees,
            _references,
            options
        );

        Assembly assembly;
        using (var streamIL = new MemoryStream())
        {
            EmitResult result = compilation.Emit(streamIL);

            if (!result.Success)
                return FileUploadResult.FromDiagnostics(result.Diagnostics);

            assembly = Assembly.Load(streamIL.ToArray());
        }

        var entryPoint = assembly.EntryPoint;

        if (entryPoint is null)
            return FileUploadResult.FromError($"{assembly.EntryPoint} was null");

        string output;
        var stdOut = Console.Out;
        using (var consoleOutput = new StringWriter())
        {
            Console.SetOut(consoleOutput);

            try
            {
                entryPoint.Invoke(null, null);
            }
            catch (Exception e)
            {
                return FileUploadResult.FromException(e);
            }

            output = consoleOutput.ToString();
        }

        Console.SetOut(stdOut);
        return FileUploadResult.FromResult(output);
    }

    public async Task<bool> SaveFileInFileSystemAsync(string unzipFolderPath, IFormFile file)
    {
        // Check if the file and filePath are null or empty
        if (file.Length <= 0 || unzipFolderPath.Length == 0)
            return false;

        // Combine with a target directory path
        var filePath = Path.Combine(unzipFolderPath, file.FileName);

        // Save the file to the server
        using var stream = File.Open(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        return true;
    }
}
