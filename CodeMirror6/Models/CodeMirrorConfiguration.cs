using System.Text.Json.Serialization;

namespace CodeMirror6.Models;

/// <summary>
/// Configuration of the CodeMirror editor
/// </summary>
/// <remarks>
/// Constructor
/// </remarks>
/// <param name="doc"></param>
/// <param name="placeholder"></param>
/// <param name="themeName"></param>
/// <param name="tabSize"></param>
/// <param name="indentationUnit"></param>
/// <param name="readOnly"></param>
/// <param name="editable"></param>
public class CodeMirrorConfiguration(
    string? doc,
    string? placeholder,
    string? themeName,
    int tabSize,
    int indentationUnit,
    bool readOnly = false,
    bool editable = true)
{

    /// <summary>
    /// The text to display in the editor
    /// </summary>
    [JsonPropertyName("doc")] public string? Doc { get; set; } = doc;

    /// <summary>
    /// The placeholder to display in the editor
    /// </summary>
    [JsonPropertyName("placeholder")] public string? Placeholder { get; set; } = placeholder;

    /// <summary>
    /// The theme to use for the editor
    /// </summary>
    [JsonPropertyName("themeName")] public string? ThemeName { get; set; } = themeName;

    /// <summary>
    /// The tab size to use for the editor
    /// </summary>
    [JsonPropertyName("tabSize")] public int TabSize { get; set; } = tabSize;

    /// <summary>
    /// The indent unit to use for the editor
    /// </summary>
    [JsonPropertyName("indentationUnit")] public int IndentationUnit { get; set; } = indentationUnit;

    /// <summary>
    /// Determine whether editing functionality should apply.
    /// Not to be confused with editable, which controls whether the editor's DOM is set to be editable (and thus focusable).
    /// </summary>
    [JsonPropertyName("readOnly")] public bool ReadOnly { get; set; } = readOnly;

    /// <summary>
    /// Controls whether the editor content DOM is editable
    /// </summary>
    [JsonPropertyName("editable")] public bool Editable { get; set; } = editable;
}
