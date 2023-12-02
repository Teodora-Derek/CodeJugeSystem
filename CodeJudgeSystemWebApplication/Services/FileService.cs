using CodeJudgeSystemWebApplication.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis;
using System.IO.Compression;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;

namespace CodeJudgeSystemWebApplication.Services
{
    public interface IFileService
    {
        Task<bool> RunFileDynamicallyAsync(IFormFile file);
        Task<bool> SaveFileInFileSystemAsync(string unzipFolderPath, IFormFile file);
    }

    public class FileService : IFileService
    {
        public async Task<bool> RunFileDynamicallyAsync(IFormFile file)
        {
            var zipArchive = new ZipArchive(file.OpenReadStream());

            //var references = AppDomain.CurrentDomain.GetAssemblies()
            //    .Where(a => !a.IsDynamic && !string.IsNullOrEmpty(a.Location))
            //    .Select(a => MetadataReference.CreateFromFile(a.Location))
            //    .ToList();

            var compilation = CSharpCompilation.Create(
                "MyDynamicCompilation",
                zipArchive.Entries.Select(entry => CSharpSyntaxTree.ParseText(SourceText.From(entry.Open()))));

            using var streamIL = new MemoryStream();
            EmitResult result = compilation.Emit(streamIL);

            if (result.Success)
            {
                var assembly = Assembly.Load(streamIL.ToArray());
                var entryPoint = assembly.EntryPoint;
                if (entryPoint is null)
                    return false;

                try
                {
                    //entryPoint.Invoke(null, null);
                    // Redirect console output to capture it
                    using (var consoleOutput = new StringWriter())
                    {
                        Console.SetOut(consoleOutput);

                        // Invoke the entry point
                        entryPoint.Invoke(null, Array.Empty<object>());

                        // Get the captured output
                        var output = consoleOutput.ToString();

                        // Check if the output is as expected
                        if (output.Trim() == "Hello, World!")
                        {
                            Console.WriteLine("Program executed successfully with the expected output.");
                        }
                        else
                        {
                            Console.WriteLine("Program executed, but the output did not match the expected output.");
                        }
                    }
                }
                catch
                {
                    return false;
                }

                return true;
            }
            else
            {
                var failures = result.Diagnostics
                    .Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error);

                StreamWriter stream = File.AppendText("C:\\Users\\Teodora Derek\\Desktop\\output.txt");

                foreach (var failure in failures)
                {
                    stream.WriteLine($"{failure.Id}: {failure.GetMessage()}");
                   // Console.Error.WriteLine($"{failure.Id}: {failure.GetMessage()}");
                }

                return false;
            }
        }

        public async Task<bool> SaveFileInFileSystemAsync(string unzipFolderPath, IFormFile file)
        {
            // Check if the file and filePath are null or empty
            if (file.Length <= 0 || unzipFolderPath.Length == 0)
                return false;

            // Combine with a target directory path
            var filePath = Path.Combine(unzipFolderPath, file.FileName);

            // Save the file to the server
            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return true;
        }
    }
}
