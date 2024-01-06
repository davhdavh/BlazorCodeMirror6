using CodeMirror6.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace CodeMirror6;

/// <summary>
/// Code-behind for the CodeMirror6Wrapper component
/// </summary>
public partial class CodeMirror6Wrapper : ComponentBase, IAsyncDisposable
{
    /// <summary>
    /// State of the CodeMirror6 editor
    /// </summary>
    /// <returns></returns>
    public CodeMirrorState State = new();

    /// <summary>
    /// The document contents has changed
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    [JSInvokable] public async Task DocChangedFromJS(string value)
    {
        if (Doc?.Replace("\r", "") == value?.Replace("\r", "")) return;
        Doc = value?.Replace("\r", "") ?? "";
        Config.Doc = Doc;
        await DocChanged.InvokeAsync(Doc);
    }

    /// <summary>
    /// The document focus has changed
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    [JSInvokable] public async Task FocusChangedFromJS(bool value)
    {
        if (State.HasFocus == value) return;
        State.HasFocus = value;
        await FocusChanged.InvokeAsync(State.HasFocus);
        // if (!State.HasFocus) await SelectionChanged.InvokeAsync(null);
    }

    /// <summary>
    /// The cursor position or selections have changed
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    [JSInvokable] public async Task SelectionSetFromJS(IEnumerable<SelectionRange>? values)
    {
        Selection = values?.ToList();
        await SelectionChanged.InvokeAsync(Selection);
    }

    /// <summary>
    /// The active markdown styles at the current selection(s) have changed
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    [JSInvokable] public async Task MarkdownStyleChangedFromJS(IEnumerable<string>? values)
    {
        State.MarkdownStylesAtSelections = new(values?.ToList() ?? []);
        await MarkdownStylesAtSelectionsChanged.InvokeAsync(State.MarkdownStylesAtSelections);
    }

    /// <summary>
    /// CodeMirror requested linting of the document
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    [JSInvokable] public async Task<List<CodeMirrorDiagnostic>> LintingRequestedFromJS(string code)
    {
        try {
            LinterCancellationTokenSource.Cancel();
        }
        catch (ObjectDisposedException) { }
        LinterCancellationTokenSource = new();
        var token = LinterCancellationTokenSource.Token;
        try {
            return await LintDocument(code, token);
        }
        catch (OperationCanceledException) {
            return [];
        }
    }

    private CancellationTokenSource LinterCancellationTokenSource = new();

    /// <summary>
    /// A file upload was initiated from the editor
    /// </summary>
    /// <param name="fileBytes"></param>
    /// <param name="fileName"></param>
    /// <param name="contentType"></param>
    /// <param name="lastModified"></param>
    /// <returns></returns>
    [JSInvokable] public async Task<string?> UploadFileFromJS(byte[] fileBytes, string fileName, string contentType, DateTime lastModified)
    {
        using var fileStream = new MemoryStream(fileBytes);
        var customFormFile = new CustomFormFile(fileStream, fileName, contentType);
        var customBrowserFile = new CustomBrowserFile(fileStream, fileName, contentType, lastModified);
        var fileUrl = UploadFile is not null
            ? await UploadFile(customFormFile)
            : UploadBrowserFile is not null
                ? await UploadBrowserFile(customBrowserFile)
                : null;
        if (!string.IsNullOrEmpty(fileUrl)) {
            var imageChar = contentType.StartsWith("image/") ? "!" : string.Empty;
            var imageLink = $"\n{imageChar}[{fileName}]({fileUrl})\n";
            //await CmJsInterop!.Commands.Dispatch(CodeMirrorCommandOneParameter.InsertTextAbove, imageLink);
        }
        return fileUrl;
    }

    /*
    [JSInvokable] public async Task<string> RequestPasteAction(string[] options) => await DoRequestPasteAction(options);
    */
}
