using Reluxer.Attributes;
using Reluxer.Matching;
using Reluxer.Tokens;
using Reluxer.Transformer.Models;
using Reluxer.Visitor;

namespace Reluxer.Transformer.Visitors;

/// <summary>
/// Visitor that extracts useState hooks from component bodies.
/// Matches: const [name, setName] = useState(initialValue);
/// </summary>
public class StateVisitor : TokenVisitor
{
    private readonly ComponentModel _component;
    private Token[]? _componentBody;

    public StateVisitor(ComponentModel component)
    {
        _component = component;
    }

    public override void OnBegin(IReadOnlyList<Token> tokens)
    {
        // Get the component body from shared context (set by ComponentVisitor)
        _componentBody = Context.Get<Token[]>($"ComponentBody:{_component.Name}");

        if (_componentBody != null && _componentBody.Length > 0)
        {
            // Traverse just the component body
            Traverse(_componentBody, nameof(VisitUseState), nameof(VisitLiftedState));
        }
    }

    // Match: const [name, setName] = useState(value)
    [TokenPattern(@"\k""const"" ""["" (\i) "","" (\i) ""]"" ""="" \i""useState"" ""(""")]
    public void VisitUseState(TokenMatch match, string stateName, string setterName)
    {
        // Skip if already added (prevent duplicates)
        if (_component.StateFields.Any(sf => sf.Name == stateName))
            return;

        var stateField = new StateField
        {
            Name = stateName,
            SetterName = setterName
        };

        // Extract initial value from parentheses
        var initValueTokens = ExtractParenthesized(match.MatchedTokens.Length - 1);
        if (initValueTokens.Length > 0)
        {
            stateField.InitialValue = TokensToString(initValueTokens);
            stateField.Type = InferType(initValueTokens);
        }
        else
        {
            stateField.InitialValue = "null";
            stateField.Type = "object";
        }

        _component.StateFields.Add(stateField);

        // Skip past the useState call
        SkipBalanced("(", ")");
    }

    // Match: state["Component.key"] for lifted state reads
    [TokenPattern(@"\i""state"" ""["" \s")]
    public void VisitLiftedState(TokenMatch match)
    {
        // This creates a local variable that reads from state manager
        var stringToken = match.MatchedTokens.Last(t => t.Type == TokenType.String);
        var stateKey = stringToken.Value.Trim('"', '\'');

        _component.LocalVariables.Add(new LocalVariable
        {
            Name = $"_{stateKey.Replace(".", "_")}_reader",
            Expression = $"State[\"{stateKey}\"]",
            IsConst = true
        });
    }

    private string TokensToString(Token[] tokens)
    {
        return string.Join("", tokens.Select(t => t.Value));
    }

    private string InferType(Token[] tokens)
    {
        if (tokens.Length == 0) return "object";

        var first = tokens[0];

        // Check for literals
        if (first.Type == TokenType.Number)
        {
            return first.Value.Contains('.') ? "double" : "int";
        }

        if (first.Type == TokenType.String)
        {
            return "string";
        }

        if (first.Type == TokenType.Keyword)
        {
            return first.Value switch
            {
                "true" or "false" => "bool",
                "null" => "object",
                _ => "object"
            };
        }

        // Array literal
        if (first.Value == "[")
        {
            return "List<object>";
        }

        // Object literal
        if (first.Value == "{")
        {
            return "Dictionary<string, object>";
        }

        return "object";
    }
}
