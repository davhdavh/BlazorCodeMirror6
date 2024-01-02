using System.Text.Json.Serialization;

namespace CodeMirror6.Models;

/// <summary>
/// Stores the configuration of a CodeMirror instance (what plugins to load).
/// Cannot be changed after the instance is created.
/// </summary>
public readonly record struct CodeMirrorSetup
{
    /// <summary>
    /// Default constructor
    /// </summary>
    public CodeMirrorSetup()
    {
    }

    /// <summary>
    /// Whether to show line numbers to the left of the editor.
    /// </summary>
    [JsonPropertyName("lineNumbers")] public bool LineNumbers { get; init; } = true;

    /// <summary>
    /// Whether to highlight the line gutter when the cursor is on it.
    /// </summary>
    [JsonPropertyName("highlightActiveLineGutter")] public bool HighlightActiveLineGutter { get; init; } = true;

    /// <summary>
    /// Whether to highlight special characters (whitespace, tabs, newlines).
    /// </summary>
    [JsonPropertyName("highlightSpecialChars")] public bool HighlightSpecialChars { get; init; } = true;

    /// <summary>
    /// Whether to enable undo/redo.
    /// </summary>
    [JsonPropertyName("history")] public bool History { get; init; } = true;

    /// <summary>
    /// Whether to enable code folding.
    /// </summary>
    [JsonPropertyName("foldGutter")] public bool FoldGutter { get; init; } = true;

    /// <summary>
    /// Whether to draw the selection when the editor is focused.
    /// </summary>
    [JsonPropertyName("drawSelection")] public bool DrawSelection { get; init; } = true;

    /// <summary>
    /// Whether to show a cursor marker when the editor is focused.
    /// </summary>
    [JsonPropertyName("dropCursor")] public bool DropCursor { get; init; } = true;

    /// <summary>
    /// Whether to allow multiple selections.
    /// </summary>
    [JsonPropertyName("allowMultipleSelections")] public bool AllowMultipleSelections { get; init; } = true;

    /// <summary>
    /// Whether to indent the current line when a character is typed that might cause the line to be re-indented.
    /// </summary>
    [JsonPropertyName("indentOnInput")] public bool IndentOnInput { get; init; } = true;

    /// <summary>
    /// Whether to enable syntax highlighting.
    /// </summary>
    [JsonPropertyName("syntaxHighlighting")] public bool SyntaxHighlighting { get; init; } = true;

    /// <summary>
    /// Whether to highlight matching brackets.
    /// </summary>
    [JsonPropertyName("bracketMatching")] public bool BracketMatching { get; init; } = true;

    /// <summary>
    /// Whether to automatically close brackets.
    /// </summary>
    [JsonPropertyName("closeBrackets")] public bool CloseBrackets { get; init; } = true;

    /// <summary>
    /// Whether to enable autocompletion.
    /// </summary>
    [JsonPropertyName("autocompletion")] public bool Autocompletion { get; init; } = true;

    /// <summary>
    /// Whether to enable rectangular selection.
    /// </summary>
    [JsonPropertyName("rectangularSelection")] public bool RectangularSelection { get; init; } = true;

    /// <summary>
    /// Whether to enable crosshair selection.
    /// </summary>
    [JsonPropertyName("crossHairSelection")] public bool CrossHairSelection { get; init; } = true;

    /// <summary>
    /// Whether to highlight the active line.
    /// </summary>
    [JsonPropertyName("highlightActiveLine")] public bool HighlightActiveLine { get; init; } = true;

    /// <summary>
    /// Whether to highlight selection matches.
    /// </summary>
    [JsonPropertyName("highlightSelectionMatches")] public bool HighlightSelectionMatches { get; init; } = true;

    /// <summary>
    /// Whether to enable preview images.
    /// </summary>
    [JsonPropertyName("previewImages")] public bool PreviewImages { get; init; } = true;

    /// <summary>
    /// Whether to enable mentions.
    /// </summary>
    [JsonPropertyName("allowMentions")] public bool AllowMentions { get; init; } = true;
}