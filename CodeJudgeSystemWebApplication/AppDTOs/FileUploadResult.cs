using Microsoft.CodeAnalysis;

namespace CodeJudgeSystemWebApplication.AppModels;

public class FileUploadResult
{
    public IReadOnlyCollection<Diagnostic>? Diagnostics { get; private init; }
    public Exception? Exception { get; private init; }
    public string? Error { get; private init; }
    public string? Result { get; private init; }
    public bool IsSuccessful { get; private init; }

    public static FileUploadResult FromDiagnostics(IReadOnlyCollection<Diagnostic> diagnostics) => new()
    {
        Diagnostics = diagnostics,
        IsSuccessful = false,
    };

    public static FileUploadResult FromException(Exception exception) => new()
    {
        Exception = exception,
        IsSuccessful = false,
    };

    public static FileUploadResult FromError(string error) => new()
    {
        Error = error,
        IsSuccessful = false,
    };

    public static FileUploadResult FromResult(string result) => new()
    {
        Result = result,
        IsSuccessful = true,
    };
}
