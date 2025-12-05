using Reluxer.Attributes;
using Reluxer.Matching;
using Reluxer.Tokens;
using Reluxer.Transformer.Models;
using Reluxer.Visitor;

namespace Reluxer.Transformer.Visitors;

/// <summary>
/// Visitor that identifies and extracts component definitions from TSX.
/// Matches:
/// - export function ComponentName() { ... }
/// - export default function ComponentName() { ... }
/// - function ComponentName() { ... }
/// </summary>
public class ComponentVisitor : TokenVisitor
{
    public List<ComponentModel> Components { get; } = new();
    public bool DebugMode { get; set; } = true;

    private IReadOnlyList<Token>? _tokens;
    private int _componentStartIndex;

    public ComponentVisitor()
    {
        if (DebugMode) Console.WriteLine($"[ComponentVisitor] Constructor called");
        Console.Out.Flush();
    }

    public override void OnBegin(IReadOnlyList<Token> tokens)
    {
        if (DebugMode) Console.WriteLine($"[ComponentVisitor] OnBegin ENTRY");
        Console.Out.Flush();
        _tokens = tokens;
        Components.Clear();
        if (DebugMode) Console.WriteLine($"[ComponentVisitor] OnBegin: {tokens.Count} tokens");
        Console.Out.Flush();
    }

    public override void OnUnmatched(Token token)
    {
        if (DebugMode && token.Type != TokenType.Whitespace)
            Console.WriteLine($"[ComponentVisitor] Unmatched: [{token.Type}] \"{token.Value}\" at {token.Start}");
    }

    // Match: export default function Name
    [TokenPattern(@"\k""export"" \k""default"" \k""function"" (\i)", Priority = 100)]
    public void VisitExportDefaultFunction(TokenMatch match, string name)
    {
        if (DebugMode) Console.WriteLine($"[ComponentVisitor] MATCHED ExportDefaultFunction: {name}");

        var component = new ComponentModel
        {
            Name = name,
            IsDefault = true,
            IsExported = true
        };

        _componentStartIndex = match.StartIndex;
        if (DebugMode) Console.WriteLine($"[ComponentVisitor] Extracting body...");
        ExtractComponentBody(component, match.EndIndex);
        if (DebugMode) Console.WriteLine($"[ComponentVisitor] Body extracted, adding component");
        Components.Add(component);

        // Skip past the function body
        if (DebugMode) Console.WriteLine($"[ComponentVisitor] Skipping function body...");
        SkipFunctionBody();
        if (DebugMode) Console.WriteLine($"[ComponentVisitor] Done with {name}");
    }

    // Match: export function Name
    [TokenPattern(@"\k""export"" \k""function"" (\i)", Priority = 90)]
    public void VisitExportFunction(TokenMatch match, string name)
    {
        var component = new ComponentModel
        {
            Name = name,
            IsDefault = false,
            IsExported = true
        };

        _componentStartIndex = match.StartIndex;
        ExtractComponentBody(component, match.EndIndex);
        Components.Add(component);

        SkipFunctionBody();
    }

    // Match: function Name (non-exported)
    [TokenPattern(@"\k""function"" (\i)", Priority = 80)]
    public void VisitFunction(TokenMatch match, string name)
    {
        // Only capture PascalCase names as components
        if (!char.IsUpper(name[0])) return;

        var component = new ComponentModel
        {
            Name = name,
            IsDefault = false,
            IsExported = false
        };

        _componentStartIndex = match.StartIndex;
        ExtractComponentBody(component, match.EndIndex);
        Components.Add(component);

        SkipFunctionBody();
    }

    // Match: const Name = () => { ... } (arrow function components)
    // TODO: This pattern causes infinite loop - disabled for now
    // [TokenPattern(@"<keyword:const> (<identifier>) <operator:=>.+?<operator:=>", Priority = 70)]
    public void VisitArrowComponent_Disabled(TokenMatch match, string name)
    {
        // Only capture PascalCase names as components
        if (!char.IsUpper(name[0])) return;

        var component = new ComponentModel
        {
            Name = name,
            IsDefault = false,
            IsExported = false
        };

        _componentStartIndex = match.StartIndex;

        // For arrow functions, body comes after =>
        var bodyTokens = ExtractFunctionBody(0);
        if (bodyTokens.Length > 0)
        {
            // Store body tokens for later processing
            Context.Set($"ComponentBody:{name}", bodyTokens);
        }

        Components.Add(component);
        SkipFunctionBody();
    }

    private void ExtractComponentBody(ComponentModel component, int startOffset)
    {
        // Extract the function body (everything inside { })
        // startOffset is 0 to search from current match position
        var bodyTokens = ExtractFunctionBody(0);

        if (bodyTokens.Length > 0)
        {
            // Store body tokens in shared context for other visitors
            Context.Set($"ComponentBody:{component.Name}", bodyTokens);
        }
    }
}
