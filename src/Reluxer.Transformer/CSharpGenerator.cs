using System.Text;
using Reluxer.Transformer.Models;

namespace Reluxer.Transformer;

/// <summary>
/// Generates C# code from parsed component models.
/// </summary>
public class CSharpGenerator
{
    private readonly TransformOptions _options;
    private readonly StringBuilder _sb = new();
    private int _indentLevel;

    public CSharpGenerator(TransformOptions options)
    {
        _options = options;
    }

    public string Generate(List<ComponentModel> components)
    {
        _sb.Clear();
        _indentLevel = 0;

        if (_options.IncludeUsings)
        {
            WriteUsings();
            WriteLine();
        }

        // Use file-scoped namespace (C# 10+)
        WriteLine($"namespace {_options.Namespace};");
        WriteLine();

        for (int i = 0; i < components.Count; i++)
        {
            GenerateComponent(components[i]);
            if (i < components.Count - 1)
                WriteLine();
        }

        return _sb.ToString();
    }

    private void WriteUsings()
    {
        WriteLine("using Minimact.AspNetCore.Core;");
        WriteLine("using Minimact.AspNetCore.Extensions;");
        WriteLine("using MinimactHelpers = Minimact.AspNetCore.Core.Minimact;");
        WriteLine("using System.Collections.Generic;");
        WriteLine("using System.Linq;");
        WriteLine("using System.Threading.Tasks;");
    }

    private void GenerateComponent(ComponentModel component)
    {
        WriteLine("[Component]");
        var partial = _options.GeneratePartialClasses ? "partial " : "";
        WriteLine($"public {partial}class {component.Name} : MinimactComponent");
        WriteLine("{");
        _indentLevel++;

        // State fields
        foreach (var state in component.StateFields)
        {
            WriteLine("[State]");
            var csharpType = ConvertTypeToCSharp(state.Type);
            var initialValue = ConvertInitialValue(state.InitialValue, state.Type);
            WriteLine($"private {csharpType} {state.Name} = {initialValue};");
            WriteLine();
        }

        // Render method
        WriteLine("protected override VNode Render()");
        WriteLine("{");
        _indentLevel++;

        WriteLine("StateManager.SyncMembersToState(this);");
        WriteLine();

        // Local variables
        foreach (var local in component.LocalVariables)
        {
            var keyword = local.IsConst ? "var" : "var";
            WriteLine($"{keyword} {local.Name} = {ConvertExpression(local.Expression)};");
        }

        if (component.LocalVariables.Count > 0)
            WriteLine();

        // Render tree
        if (component.RenderTree != null)
        {
            WriteIndent();
            Write("return ");
            GenerateVNode(component.RenderTree, isReturn: true);
            _sb.AppendLine(";");
        }
        else
        {
            WriteLine("return new VNull(\"1\");");
        }

        _indentLevel--;
        WriteLine("}");

        // Event handlers
        foreach (var handler in component.EventHandlers)
        {
            WriteLine();
            GenerateEventHandler(handler);
        }

        // GetClientHandlers() - returns JS handlers for client-side execution
        if (component.EventHandlers.Count > 0)
        {
            WriteLine();
            GenerateGetClientHandlers(component);
        }

        _indentLevel--;
        WriteLine("}");
    }

    private void GenerateGetClientHandlers(ComponentModel component)
    {
        WriteLine("/// <summary>");
        WriteLine("/// Returns JavaScript event handlers for client-side execution");
        WriteLine("/// These execute in the browser with bound hook context");
        WriteLine("/// </summary>");
        WriteLine("protected override Dictionary<string, string> GetClientHandlers()");
        WriteLine("{");
        _indentLevel++;

        WriteLine("return new Dictionary<string, string>");
        WriteLine("{");
        _indentLevel++;

        for (int i = 0; i < component.EventHandlers.Count; i++)
        {
            var handler = component.EventHandlers[i];
            var jsBody = FormatJsHandler(handler.OriginalExpression ?? handler.Body);
            var comma = i < component.EventHandlers.Count - 1 ? "," : "";
            WriteLine($"[\"{handler.GeneratedName}\"] = @\"{jsBody}\"{comma}");
        }

        _indentLevel--;
        WriteLine("};");

        _indentLevel--;
        WriteLine("}");
    }

    private string FormatJsHandler(string originalExpr)
    {
        if (string.IsNullOrWhiteSpace(originalExpr))
            return "function () {}";

        // Escape quotes for verbatim string
        var escaped = originalExpr.Replace("\"", "\"\"");

        // Format as function with newlines
        if (escaped.Contains("=>"))
        {
            // Arrow function: () => setCount(count + 1)
            var parts = escaped.Split("=>", 2);
            if (parts.Length == 2)
            {
                var body = parts[1].Trim();
                // Normalize spacing in the body
                body = System.Text.RegularExpressions.Regex.Replace(body, @"(\w+)\s*([+\-*/])\s*(\d+)", "$1 $2 $3");
                // Wrap in function syntax
                return $"function () {{\\n  {body};\\n}}";
            }
        }

        return $"function () {{\\n  {escaped};\\n}}";
    }

    private void GenerateVNode(VNodeModel node, bool isReturn = false)
    {
        switch (node)
        {
            case VElementModel element:
                GenerateVElement(element, isRoot: isReturn);
                break;

            case VTextModel text:
                GenerateVText(text);
                break;

            case VNullModel:
                Write($"new VNull(\"{node.HexPath}\")");
                break;

            case VConditionalModel conditional:
                GenerateConditional(conditional);
                break;

            case VComponentWrapperModel wrapper:
                GenerateComponentWrapper(wrapper);
                break;

            case VListModel list:
                GenerateList(list);
                break;
        }
    }

    private void GenerateVElement(VElementModel element, bool isRoot = false)
    {
        var tag = element.TagName;
        var path = element.HexPath;

        // Generate attributes dictionary
        var attrs = GenerateAttributesDictionary(element.Attributes);

        if (element.Children.Count == 0 && !HasTextContent(element))
        {
            // Self-closing or empty element
            Write($"new VElement(\"{tag}\", \"{path}\", {attrs})");
        }
        else if (element.Children.Count == 1 && element.Children[0] is VTextModel textChild && !textChild.IsDynamic)
        {
            // Single static text child - inline it
            var escapedText = EscapeString(textChild.Text);
            Write($"new VElement(\"{tag}\", \"{path}\", {attrs}, \"{escapedText}\")");
        }
        else if (isRoot)
        {
            // Root element uses MinimactHelpers.createElement for varargs children
            Write($"MinimactHelpers.createElement(\"{tag}\", null, ");

            for (int i = 0; i < element.Children.Count; i++)
            {
                GenerateVNode(element.Children[i]);
                if (i < element.Children.Count - 1)
                    Write(", ");
            }

            Write(")");
        }
        else
        {
            // Multiple children
            Write($"new VElement(\"{tag}\", \"{path}\", {attrs}, new VNode[]");
            _sb.AppendLine();
            WriteIndent();
            _sb.AppendLine("{");
            _indentLevel++;

            for (int i = 0; i < element.Children.Count; i++)
            {
                WriteIndent();
                GenerateVNode(element.Children[i]);
                if (i < element.Children.Count - 1)
                    _sb.AppendLine(",");
                else
                    _sb.AppendLine();
            }

            _indentLevel--;
            WriteIndent();
            Write("})");
        }
    }

    private void GenerateVText(VTextModel text)
    {
        if (text.IsDynamic)
        {
            var binding = ConvertExpression(text.Binding ?? "");
            Write($"new VText($\"{{({binding})}}\", \"{text.HexPath}\")");
        }
        else
        {
            var escapedText = EscapeString(text.Text);
            Write($"new VText(\"{escapedText}\", \"{text.HexPath}\")");
        }
    }

    private void GenerateConditional(VConditionalModel conditional)
    {
        var condition = ConvertCondition(conditional.Condition);

        if (conditional.IsSimpleAnd)
        {
            // {x && <elem>} -> (condition) ? elem : VNull
            Write($"({condition}) ? ");
            if (conditional.TrueNode != null)
                GenerateVNode(conditional.TrueNode);
            else
                Write($"new VNull(\"{conditional.HexPath}\")");
            Write($" : new VNull(\"{conditional.HexPath}\")");
        }
        else
        {
            // Ternary: condition ? a : b
            Write($"({condition}) ? ");
            if (conditional.TrueNode != null)
                GenerateVNode(conditional.TrueNode);
            else
                Write($"new VNull(\"{conditional.HexPath}.1\")");
            Write(" : ");
            if (conditional.FalseNode != null)
                GenerateVNode(conditional.FalseNode);
            else
                Write($"new VNull(\"{conditional.HexPath}.2\")");
        }
    }

    private void GenerateComponentWrapper(VComponentWrapperModel wrapper)
    {
        WriteLine($"new VComponentWrapper");
        WriteIndent();
        WriteLine("{");
        _indentLevel++;
        WriteIndent();
        WriteLine($"ComponentName = \"{wrapper.ComponentName}\",");
        WriteIndent();
        WriteLine($"ComponentType = \"{wrapper.ComponentType}\",");
        WriteIndent();
        WriteLine($"HexPath = \"{wrapper.HexPath}\",");
        WriteIndent();
        Write("InitialState = new Dictionary<string, object> { ");

        var stateItems = wrapper.InitialState.ToList();
        for (int i = 0; i < stateItems.Count; i++)
        {
            var kv = stateItems[i];
            Write($"[\"{kv.Key}\"] = {ConvertInitialValue(kv.Value, "object")}");
            if (i < stateItems.Count - 1) Write(", ");
        }

        WriteLine(" }");
        _indentLevel--;
        WriteIndent();
        Write("}");
    }

    private void GenerateList(VListModel list)
    {
        var arrayExpr = ConvertExpression(list.ArrayExpression);

        WriteLine($"MinimactHelpers.createFragment({arrayExpr}.Select(({list.ItemName}{(list.IndexName != null ? $", {list.IndexName}" : "")}) =>");
        _indentLevel++;
        WriteIndent();

        if (list.ItemTemplate != null)
        {
            GenerateVNode(list.ItemTemplate);
        }
        else
        {
            Write($"new VNull(\"{list.HexPath}\")");
        }

        _indentLevel--;
        WriteLine();
        WriteIndent();
        Write(").ToArray())");
    }

    private void GenerateEventHandler(Models.EventHandler handler)
    {
        WriteLine($"public void {handler.GeneratedName}()");
        WriteLine("{");
        _indentLevel++;

        var body = ConvertHandlerBody(handler.Body);
        if (!string.IsNullOrWhiteSpace(body))
        {
            foreach (var line in body.Split('\n', StringSplitOptions.RemoveEmptyEntries))
            {
                WriteLine(line.Trim());
            }
        }

        _indentLevel--;
        WriteLine("}");
    }

    private string GenerateAttributesDictionary(Dictionary<string, AttributeValue> attributes)
    {
        if (attributes.Count == 0)
            return "new Dictionary<string, string>()";

        var items = new List<string>();
        foreach (var attr in attributes)
        {
            string value;
            if (attr.Value.IsEventHandler)
            {
                value = $"\"{attr.Value.EventHandlerRef}\"";
            }
            else if (attr.Value.IsDynamic)
            {
                // Dynamic attribute - use interpolation
                value = $"$\"{{({ConvertExpression(attr.Value.Binding ?? "")})}}\"";
            }
            else
            {
                value = $"\"{EscapeString(attr.Value.RawValue)}\"";
            }
            items.Add($"[\"{attr.Key}\"] = {value}");
        }

        return $"new Dictionary<string, string> {{ {string.Join(", ", items)} }}";
    }

    #region Conversion Helpers

    private string ConvertTypeToCSharp(string tsType)
    {
        return tsType switch
        {
            "number" or "int" => "int",
            "double" or "float" => "double",
            "string" => "string",
            "boolean" or "bool" => "bool",
            "List<object>" => "List<object>",
            "Dictionary<string, object>" => "Dictionary<string, object>",
            _ => "object"
        };
    }

    private string ConvertInitialValue(string? value, string type)
    {
        if (string.IsNullOrEmpty(value)) return "null";

        // Boolean
        if (value == "true" || value == "false") return value;

        // Number
        if (int.TryParse(value, out _) || double.TryParse(value, out _)) return value;

        // String
        if (value.StartsWith("\"") || value.StartsWith("'"))
            return value.Replace("'", "\"");

        // Array
        if (value.StartsWith("["))
            return $"new List<object> {{ {value.Trim('[', ']')} }}";

        // Object
        if (value.StartsWith("{"))
            return ConvertObjectLiteral(value);

        return value;
    }

    private string ConvertObjectLiteral(string obj)
    {
        // Simple conversion - in real implementation, parse properly
        return $"new Dictionary<string, object> {{ /* {obj} */ }}";
    }

    private string ConvertExpression(string expr)
    {
        if (string.IsNullOrWhiteSpace(expr)) return "null";

        // Convert template literals: `text ${var}` -> $"text {var}"
        if (expr.StartsWith("`") && expr.EndsWith("`"))
        {
            var inner = expr[1..^1];
            inner = inner.Replace("${", "{");
            return $"$\"{inner}\"";
        }

        // Convert state access: state["Key"] -> State["Key"]
        expr = expr.Replace("state[", "State[");

        return expr;
    }

    private string ConvertCondition(string condition)
    {
        // Wrap simple identifiers in MObject for truthiness check
        var trimmed = condition.Trim();

        // Check if it's a simple identifier (no operators, no parens, no negation)
        if (!trimmed.Contains(" ") &&
            !trimmed.Contains("(") &&
            !trimmed.Contains(".") &&
            !trimmed.Contains("!") &&
            !trimmed.Contains("&") &&
            !trimmed.Contains("|"))
        {
            return $"new MObject({trimmed})";
        }

        // For complex conditions with && or ||, wrap each part in parentheses
        // myState1 && !myState2 -> (myState1) && (!myState2)
        if (trimmed.Contains("&&") || trimmed.Contains("||"))
        {
            // Already has operators - format properly
            var result = trimmed;
            // Add parentheses around sub-expressions for safety
            result = System.Text.RegularExpressions.Regex.Replace(result, @"(\w+)\s*&&", "($1) &&");
            result = System.Text.RegularExpressions.Regex.Replace(result, @"&&\s*(!?\w+)", "&& ($1)");
            return result;
        }

        // Handle negation: !x -> !x (keep as is)
        return condition;
    }

    private string ConvertHandlerBody(string body)
    {
        if (string.IsNullOrWhiteSpace(body)) return "";

        // Convert setState calls
        // setCount(count + 1) -> SetState(nameof(count), count + 1);
        var result = body.Trim();

        // Simple pattern: setXxx(value) -> SetState(nameof(xxx), value)
        var setterPattern = new System.Text.RegularExpressions.Regex(
            @"set(\w+)\(([^)]+)\)");

        result = setterPattern.Replace(result, match =>
        {
            var fieldName = char.ToLower(match.Groups[1].Value[0]) + match.Groups[1].Value[1..];
            var value = match.Groups[2].Value.Trim();
            // Normalize spacing around operators
            value = System.Text.RegularExpressions.Regex.Replace(value, @"(\w+)\s*([+\-*/])\s*(\d+)", "$1 $2 $3");
            return $"SetState(nameof({fieldName}), {value})";
        });

        // Convert setState("Component.key", value)
        result = result.Replace("setState(", "SetState(");

        // Ensure statements end with semicolons
        if (!string.IsNullOrEmpty(result) && !result.EndsWith(";") && !result.EndsWith("}"))
        {
            result += ";";
        }

        return result;
    }

    private bool HasTextContent(VElementModel element)
    {
        return element.Children.Any(c => c is VTextModel);
    }

    private string EscapeString(string s)
    {
        return s.Replace("\\", "\\\\")
                .Replace("\"", "\\\"")
                .Replace("\n", "\\n")
                .Replace("\r", "\\r")
                .Replace("\t", "\\t");
    }

    #endregion

    #region Output Helpers

    private void Write(string text)
    {
        _sb.Append(text);
    }

    private void WriteLine(string text = "")
    {
        if (!string.IsNullOrEmpty(text))
        {
            WriteIndent();
            _sb.AppendLine(text);
        }
        else
        {
            _sb.AppendLine();
        }
    }

    private void WriteIndent()
    {
        for (int i = 0; i < _indentLevel; i++)
            _sb.Append(_options.IndentString);
    }

    #endregion
}
