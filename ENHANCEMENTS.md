# TokenLexer Enhancement Roadmap

This document outlines potential enhancements to make the token pattern visitor framework more robust, handle more edge cases, and add powerful new features.

---

## 1. Pattern Language Enhancements

### 1.1 Non-Greedy Quantifier Backtracking

**Current Limitation:** Non-greedy quantifiers (`*?`, `+?`) don't properly backtrack when the subsequent pattern fails.

**Enhancement:** Implement proper backtracking for non-greedy matches.

```csharp
// Should match: const x = 5;
// Currently fails because .*? doesn't backtrack to let ";" match
[TokenPattern(@"\k""const"" (\i) ""="" (.*?) "";""")]
```

**Implementation:** Use a continuation-passing style or explicit backtracking stack in `PatternMatcher`.

---

### 1.2 Named Capture Groups

**Enhancement:** Support `(?<name>...)` syntax for named captures that map to method parameters by name.

```csharp
[TokenPattern(@"\k""function"" (?<name>\i) ""("" (?<params>.*?) "")""")]
public void VisitFunction(TokenMatch match, string name, Token[] @params)
{
    // Parameters matched by name, not position
}
```

**Benefits:**
- More readable patterns
- Parameter order doesn't matter
- Self-documenting code

---

### 1.3 Lookahead and Lookbehind

**Enhancement:** Support zero-width assertions.

```csharp
// Positive lookahead: match identifier only if followed by (
[TokenPattern(@"(\i)(?=""("")")]

// Negative lookahead: match identifier NOT followed by (
[TokenPattern(@"(\i)(?!""("")")]

// Positive lookbehind: match identifier preceded by .
[TokenPattern(@"(?<=""."")(\i)")]
```

---

### 1.4 Token Type Negation

**Enhancement:** Match any token EXCEPT a specific type.

```csharp
// Match any token that's not a keyword
[TokenPattern(@"\!k")]

// Match any token that's not punctuation or operator
[TokenPattern(@"[\!p|\!o]")]
```

---

### 1.5 Optional Groups with Default Values

**Enhancement:** Allow optional captures with defaults when not matched.

```csharp
[TokenPattern(@"\k""function"" (\i) ""("" (\i)? "")""")]
public void VisitFunction(string name, string? firstParam = null)
{
    // firstParam is null if no parameter in function
}
```

---

## 2. Lexer Enhancements

### 2.1 Contextual Lexing for JSX

**Current Limitation:** JSX context detection is heuristic-based and can fail in edge cases.

**Enhancement:** Implement a proper state machine for JSX lexing.

```
States:
- Normal (JavaScript)
- JSXElement (inside <tag>)
- JSXAttribute (parsing attributes)
- JSXExpression (inside {})
- JSXText (between tags)
```

**Edge Cases to Handle:**
- JSX in template literals: `` `<div>${jsx}</div>` ``
- Nested JSX expressions: `<div>{items.map(i => <span>{i}</span>)}</div>`
- JSX spread attributes: `<div {...props}>`
- JSX fragments: `<>...</>`

---

### 2.2 TypeScript-Specific Tokens

**Enhancement:** Add token types for TypeScript constructs.

```csharp
public enum TokenType
{
    // Existing...

    // TypeScript additions
    TypeAnnotation,     // : string, : number
    GenericOpen,        // <T>
    GenericClose,       // >
    AsExpression,       // as Type
    TypeKeyword,        // type, interface, enum
    AccessModifier,     // public, private, protected
    Decorator,          // @decorator
}
```

---

### 2.3 Source Location Tracking

**Enhancement:** Track original source locations through transformations.

```csharp
public class Token
{
    // Existing...

    public SourceLocation Location { get; set; }
}

public class SourceLocation
{
    public string FileName { get; set; }
    public int Line { get; set; }
    public int Column { get; set; }
    public int Offset { get; set; }

    // For source maps
    public SourceLocation? OriginalLocation { get; set; }
}
```

---

### 2.4 Incremental Lexing

**Enhancement:** Support re-lexing only changed portions of source code.

```csharp
public class IncrementalLexer
{
    public TokenStream Update(TokenStream previous, TextChange change)
    {
        // Only re-lex affected tokens
        // Reuse unchanged tokens
    }
}
```

**Benefits:**
- Better IDE integration
- Faster re-processing for large files

---

## 3. Visitor Framework Enhancements

### 3.1 Enter/Exit Hooks

**Enhancement:** Call hooks when entering and exiting scopes.

```csharp
public class MyVisitor : TokenVisitor
{
    [TokenPattern(@"\k""function"" (\i)")]
    public void VisitFunction(string name)
    {
        // Called on match
    }

    [OnEnter(nameof(VisitFunction))]
    public void OnEnterFunction(TokenMatch match)
    {
        _scopeStack.Push(new Scope());
    }

    [OnExit(nameof(VisitFunction))]
    public void OnExitFunction(TokenMatch match)
    {
        _scopeStack.Pop();
    }
}
```

---

### 3.2 Pattern Composition

**Enhancement:** Allow patterns to reference other patterns by name.

```csharp
[PatternDefinition("FunctionParams", @"""("" (.*?) "")""")]
[PatternDefinition("FunctionBody", @"""{"" (.*?) ""}""")]

[TokenPattern(@"\k""function"" (\i) @FunctionParams @FunctionBody")]
public void VisitFunction(string name, Token[] @params, Token[] body) { }
```

---

### 3.3 Async Visitor Support

**Enhancement:** Support async visitor methods for I/O operations.

```csharp
[TokenPattern(@"\k""import"" .*? \s""(.*)""")]
public async Task VisitImportAsync(TokenMatch match, string path)
{
    var resolved = await _resolver.ResolveAsync(path);
    var tokens = await _lexer.LexFileAsync(resolved);
    // Process imported file
}
```

---

### 3.4 Visitor Inheritance and Composition

**Enhancement:** Support visitor inheritance with override semantics.

```csharp
public class BaseVisitor : TokenVisitor
{
    [TokenPattern(@"\k""function"" (\i)", Virtual = true)]
    public virtual void VisitFunction(string name) { }
}

public class ExtendedVisitor : BaseVisitor
{
    [TokenPattern(@"\k""function"" (\i)", Override = true)]
    public override void VisitFunction(string name)
    {
        // Custom handling
        base.VisitFunction(name);  // Optionally call base
    }
}
```

---

### 3.5 Conditional Pattern Activation

**Enhancement:** Enable/disable patterns based on runtime conditions.

```csharp
[TokenPattern(@"\k""async"" \k""function""", When = nameof(IsAsyncEnabled))]
public void VisitAsyncFunction() { }

private bool IsAsyncEnabled => _options.SupportAsync;
```

---

### 3.6 Match Context Information

**Enhancement:** Provide richer context to visitor methods.

```csharp
public class MatchContext
{
    public TokenMatch Match { get; }
    public Token[] PrecedingTokens { get; }    // Tokens before match
    public Token[] FollowingTokens { get; }    // Tokens after match
    public int Depth { get; }                   // Nesting depth
    public IReadOnlyList<string> CallStack { get; }
    public IDictionary<string, object> State { get; }  // Shared state
}

[TokenPattern(@"(\i)")]
public void VisitIdentifier(MatchContext ctx, string name)
{
    if (ctx.PrecedingTokens.LastOrDefault()?.Value == ".")
    {
        // This is a member access
    }
}
```

---

## 4. Error Handling & Diagnostics

### 4.1 Pattern Validation at Compile Time

**Enhancement:** Use source generators to validate patterns at compile time.

```csharp
// This would produce a compile-time error:
[TokenPattern(@"\k""function"" ((\i)")]  // Unbalanced parentheses
public void VisitFunction() { }

// Error: CS9001: Invalid token pattern: Unbalanced parentheses at position 18
```

---

### 4.2 Detailed Match Failure Diagnostics

**Enhancement:** Provide detailed information about why a pattern didn't match.

```csharp
if (!matcher.TryMatch(tokens, index, out var match, out var diagnostic))
{
    Console.WriteLine(diagnostic.Message);
    // "Pattern failed at position 3: expected keyword 'function', got identifier 'foo'"
    Console.WriteLine(diagnostic.PartialMatch);
    // Shows how far the pattern got before failing
}
```

---

### 4.3 Ambiguity Detection

**Enhancement:** Warn when multiple patterns could match the same tokens.

```csharp
[TokenPattern(@"(\i) ""(""")]           // Pattern A
[TokenPattern(@"\i""useState"" ""(""")] // Pattern B - overlaps with A!

// Warning: Patterns on VisitCall and VisitUseState may match the same tokens.
// Consider using Priority to disambiguate.
```

---

### 4.4 Performance Profiling

**Enhancement:** Built-in profiling for pattern matching performance.

```csharp
visitor.EnableProfiling = true;
visitor.Visit(tokens);

var report = visitor.GetProfilingReport();
// Pattern: \k"function" (\i) - 15 matches, 2.3ms avg, 34.5ms total
// Pattern: (\i) "(" - 142 matches, 0.1ms avg, 14.2ms total
```

---

## 5. Advanced Matching Features

### 5.1 Recursive Pattern Matching

**Enhancement:** Support recursive patterns for nested structures.

```csharp
// Match balanced parentheses with any content
[TokenPattern(@"""("" (?<inner>(?:(?!""("")|"")"").|@inner)*) "")""")]

// Match nested JSX elements
[TokenPattern(@"<(\i)> @JsxContent </\1>")]
[PatternDefinition("JsxContent", @"(?:@JsxElement|@JsxText|@JsxExpr)*")]
```

---

### 5.2 Semantic Predicates

**Enhancement:** Allow C# code to participate in matching decisions.

```csharp
[TokenPattern(@"(\i)", Where = nameof(IsValidIdentifier))]
public void VisitIdentifier(string name) { }

private bool IsValidIdentifier(Token token)
{
    return !_reservedWords.Contains(token.Value);
}
```

---

### 5.3 Capture Transformations

**Enhancement:** Transform captured values before passing to visitor.

```csharp
[TokenPattern(@"\s""(.*)""")]
public void VisitString(
    [Transform(nameof(UnescapeString))] string value)
{
    // value is already unescaped
}

private string UnescapeString(string raw)
{
    return raw.Replace("\\n", "\n").Replace("\\t", "\t");
}
```

---

## 6. Integration Features

### 6.1 Source Map Generation

**Enhancement:** Generate source maps for transformed output.

```csharp
public class SourceMapGenerator
{
    public void AddMapping(
        SourceLocation original,
        SourceLocation generated,
        string? name = null);

    public string ToJson();  // V3 source map format
}
```

---

### 6.2 LSP Integration

**Enhancement:** Provide Language Server Protocol support for pattern-based analysis.

```csharp
public class TokenPatternLanguageServer
{
    public IEnumerable<Diagnostic> GetDiagnostics(string uri);
    public IEnumerable<Location> FindReferences(Position position);
    public CompletionList GetCompletions(Position position);
    public Hover GetHover(Position position);
}
```

---

### 6.3 Watch Mode

**Enhancement:** File watcher with incremental re-processing.

```csharp
var watcher = new TokenPatternWatcher(visitor, options);
watcher.Watch("./src/**/*.tsx");

watcher.OnChange += (file, tokens) => {
    // Process changed file
};
```

---

## 7. Testing Utilities

### 7.1 Pattern Testing DSL

**Enhancement:** Easy testing of patterns in isolation.

```csharp
[Fact]
public void TestFunctionPattern()
{
    var tester = new PatternTester(@"\k""function"" (\i) ""(""");

    tester.ShouldMatch("function foo()")
          .WithCapture(0, "foo");

    tester.ShouldNotMatch("const foo = ()");
}
```

---

### 7.2 Snapshot Testing

**Enhancement:** Snapshot testing for visitor output.

```csharp
[Fact]
public void TestTransformation()
{
    var visitor = new MyVisitor();
    var result = visitor.Transform(source);

    await Verify(result);  // Compare against stored snapshot
}
```

---

### 7.3 Fuzzing Support

**Enhancement:** Generate random valid/invalid token streams for testing.

```csharp
var fuzzer = new TokenFuzzer(seed: 42);

for (int i = 0; i < 1000; i++)
{
    var tokens = fuzzer.GenerateTokenStream(maxLength: 100);

    // Should not throw
    visitor.Visit(tokens);
}
```

---

## 8. Performance Optimizations

### 8.1 Pattern Compilation to IL

**Enhancement:** Compile patterns to IL for faster matching.

```csharp
var compiled = PatternCompiler.Compile(@"\k""function"" (\i)");
// Generates optimized IL instead of interpretation
```

---

### 8.2 Parallel Visitor Execution

**Enhancement:** Process independent subtrees in parallel.

```csharp
[TokenPattern(@"\k""function"" (\i)", Parallel = true)]
public void VisitFunction(string name)
{
    // Each function body can be processed in parallel
    var body = ExtractFunctionBody();
    Traverse(body, ...);  // Runs on thread pool
}
```

---

### 8.3 Pattern Index/Trie

**Enhancement:** Build an index of patterns for O(1) initial dispatch.

```csharp
// Instead of trying all patterns sequentially,
// use a trie based on first token to quickly eliminate non-matching patterns
```

---

## Implementation Priority

| Priority | Enhancement | Complexity | Impact |
|----------|-------------|------------|--------|
| High | Non-greedy backtracking | Medium | High |
| High | Named capture groups | Low | Medium |
| High | Enter/Exit hooks | Low | High |
| High | Better JSX lexing | High | High |
| Medium | Lookahead/lookbehind | Medium | Medium |
| Medium | Pattern composition | Medium | High |
| Medium | Source maps | Medium | High |
| Medium | Error diagnostics | Low | Medium |
| Low | Async visitors | Low | Low |
| Low | IL compilation | High | Medium |
| Low | Parallel execution | High | Medium |

---

## Contributing

When implementing enhancements:

1. Add tests for edge cases
2. Update documentation
3. Maintain backward compatibility
4. Consider performance implications
5. Add examples to the demo project
