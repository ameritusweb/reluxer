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
            Traverse(_componentBody,
                nameof(VisitUseState),
                nameof(VisitUseMvcStateImmutable),
                nameof(VisitUseMvcStateMutable),
                nameof(VisitUseMvcViewModel),
                nameof(VisitHelperFunction),
                nameof(VisitHelperFunctionNoParams),
                nameof(VisitLiftedState));
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

    // Match: const [name] = useMvcState<Type>('key') - immutable
    // Use \go and \gc for generic type angle brackets, \o for operator =
    [TokenPattern(@"\k""const"" ""["" (\i) ""]"" \o""="" \i""useMvcState"" \go (\tn) \gc ""("" (\s)")]
    public void VisitUseMvcStateImmutable(TokenMatch match, string localName, string typeName, string viewModelKey)
    {
        var cleanKey = viewModelKey.Trim('\'', '"');

        // Skip if already added (prevent duplicates)
        if (_component.MvcStateFields.Any(f => f.LocalName == localName))
            return;

        _component.MvcStateFields.Add(new MvcStateField
        {
            LocalName = localName,
            ViewModelKey = cleanKey,
            Type = MapTsTypeToCSharp(typeName),
            SetterName = "" // No setter for immutable
        });
    }

    // Match: const [name, setName] = useMvcState<Type>('key'...) - mutable
    [TokenPattern(@"\k""const"" ""["" (\i) "","" (\i) ""]"" \o""="" \i""useMvcState"" \go (\tn) \gc ""("" (\s)")]
    public void VisitUseMvcStateMutable(TokenMatch match, string localName, string setterName, string typeName, string viewModelKey)
    {
        var cleanKey = viewModelKey.Trim('\'', '"');

        // Skip if already added (prevent duplicates)
        if (_component.MvcStateFields.Any(f => f.LocalName == localName))
            return;

        _component.MvcStateFields.Add(new MvcStateField
        {
            LocalName = localName,
            ViewModelKey = cleanKey,
            Type = MapTsTypeToCSharp(typeName),
            SetterName = setterName
        });
    }

    // Match: const viewModel = useMvcViewModel<Type>()
    [TokenPattern(@"\k""const"" (\i) \o""="" \i""useMvcViewModel"" \go")]
    public void VisitUseMvcViewModel(TokenMatch match, string varName)
    {
        _component.HasMvcViewModel = true;
    }

    // Match: const name = (params) => { body } - helper function with parameters
    // Use \fa (Arrow type) since tokenizer outputs => as Arrow when params present
    [TokenPattern(@"\k""const"" (\i) \o""="" (\Bp) \fa (\Bb)")]
    public void VisitHelperFunction(TokenMatch match, string funcName, Token[] paramsTokens, Token[] bodyTokens)
    {
        AddHelperFunction(funcName, paramsTokens, bodyTokens);
    }

    // Match: const name = () => { body } - helper function without parameters
    // Use \fa (Arrow type) - tokenizer now consistently outputs => as Arrow
    [TokenPattern(@"\k""const"" (\i) \o""="" \p""("" \p"")"" \fa (\Bb)", Priority = 90)]
    public void VisitHelperFunctionNoParams(TokenMatch match, string funcName, Token[] bodyTokens)
    {
        AddHelperFunction(funcName, Array.Empty<Token>(), bodyTokens);
    }

    private void AddHelperFunction(string funcName, Token[] paramsTokens, Token[] bodyTokens)
    {
        // Only capture helper functions (lowercase start)
        if (char.IsUpper(funcName[0])) return;

        // Skip if already added (prevent duplicates)
        if (_component.HelperFunctions.Any(h => h.Name == funcName))
            return;

        var helper = new HelperFunction
        {
            Name = funcName,
            Body = TokensToString(bodyTokens)
        };

        // Extract parameter names
        var paramIdentifiers = paramsTokens.Where(t => t.Type == TokenType.Identifier).ToList();
        foreach (var param in paramIdentifiers)
        {
            // Skip type annotations
            if (param.Value != "number" && param.Value != "string" && param.Value != "boolean")
                helper.Parameters.Add(param.Value);
        }

        _component.HelperFunctions.Add(helper);
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

    private string MapTsTypeToCSharp(string tsType)
    {
        return tsType switch
        {
            "string" => "string",
            "number" => "double",
            "boolean" => "bool",
            _ => "object"
        };
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
