# Refactoring Plan: Declarative Loop Context Handling

## Problem

The current JsxVisitor uses imperative state tracking for loop context:

```csharp
// CURRENT (WRONG): Mutable state tracking
private string? _currentLoopItemName;

private VNodeModel? ParseMapCallback(...)
{
    _currentLoopItemName = itemName;  // Set state
    var template = ParseBranch(bodyTokens, path);
    _currentLoopItemName = null;      // Clear state
    return ...;
}

private (string, bool, string?) ParseEventHandler(Token[] tokens)
{
    // Read mutable state
    return (handlerName, true, _currentLoopItemName);
}
```

This violates the Reluxer principle: **The pattern IS the specification. The handler IS the implementation. Nothing else.**

## Solution

Use declarative patterns with:
1. **Named captures** to extract context from patterns
2. **VisitorContext** to share state between pattern handlers
3. **Traverse with nameof** to dispatch to nested handlers

## Refactoring Steps

### Step 1: Define Pattern for Map Expression

Create a pattern that captures the loop item name declaratively:

```csharp
// Pattern: array.map((item) => body) or array.map((item, index) => body)
[TokenPattern(@"(.*?) ""."" \i""map"" ""("" (?<callback>\Bp) "")""", Priority = 90)]
public void VisitMapExpression(TokenMatch match, Token[] arrayExpr)
{
    var callbackTokens = match.GetNamedCapture("callback");

    // Dispatch to callback parser
    Traverse(callbackTokens, nameof(VisitMapCallback));
}
```

### Step 2: Pattern for Map Callback with Named Captures

```csharp
// Pattern: (item) => body or (item, index) => body
[TokenPattern(@"""("" (?<item>\i) (?:"","" (?<index>\i))? "")"" \fa (?<body>.*)", Priority = 80)]
public void VisitMapCallback(TokenMatch match)
{
    var itemName = match.GetNamedCapture("item")?.FirstOrDefault()?.Value ?? "item";
    var indexName = match.GetNamedCapture("index")?.FirstOrDefault()?.Value;
    var bodyTokens = match.GetNamedCapture("body");

    // Store loop context in VisitorContext
    Context.Set("LoopItemName", itemName);
    if (indexName != null)
        Context.Set("LoopIndexName", indexName);

    // Traverse body to find JSX elements
    Traverse(bodyTokens, nameof(VisitJsxElement));

    // Build VListModel from results
    var template = Context.Get<VNodeModel>("ParsedTemplate");

    // Clear context after processing
    Context.Remove("LoopItemName");
    Context.Remove("LoopIndexName");
}
```

### Step 3: Pattern for Event Handler in Loop Context

Instead of checking mutable state, the handler pattern reads from Context:

```csharp
// Pattern: onClick={expression} or similar event attributes
[TokenPattern(@"(\ja) ""="" (\Bb)", Priority = 80)]
public void VisitExpressionAttribute(TokenMatch match, Token attrNameToken, Token[] exprContent)
{
    if (IsEventAttribute(attrNameToken.Value))
    {
        // Get loop context from VisitorContext (set by enclosing map pattern)
        var loopItemName = Context.Get<string>("LoopItemName");

        Traverse(exprContent, nameof(VisitEventHandler));

        var handlerName = Context.Get<string>("LastHandlerName");
        var handlerRef = loopItemName != null
            ? $"{handlerName}:{{{loopItemName}}}"
            : handlerName;

        // Set attribute...
    }
}
```

### Step 4: Event Handler Pattern Reads Context

```csharp
// Pattern: () => body (arrow function handler)
[TokenPattern(@"(\Bp)? \fa (.*)", Priority = 70)]
public void VisitArrowHandler(TokenMatch match, Token[]? params, Token[] body)
{
    var handlerName = $"Handle{_handlerCounter++}";
    var loopItemName = Context.Get<string>("LoopItemName");

    _component.EventHandlers.Add(new EventHandler
    {
        GeneratedName = handlerName,
        Body = TokensToString(body),
        LoopItemName = loopItemName  // From Context, not mutable field
    });

    Context.Set("LastHandlerName", handlerName);
}
```

## Key Changes Summary

| Before (Imperative) | After (Declarative) |
|---------------------|---------------------|
| `private string? _currentLoopItemName` | `Context.Get<string>("LoopItemName")` |
| Set/clear mutable state around calls | Context set in pattern handler, read by nested handlers |
| `ParseMapCallback()` method with manual parsing | `[TokenPattern]` attribute with named captures |
| `ParseEventHandler()` reads field | Pattern handler reads Context |

## Files to Modify

1. **JsxVisitor.cs**
   - Remove `_currentLoopItemName` field
   - Replace `TryParseMapCall` and `ParseMapCallback` with pattern handlers
   - Update `ParseEventHandler` to use Context instead of field
   - Add new `[TokenPattern]` methods for map expressions

2. **VisitorContext.cs** (if needed)
   - Ensure `Get<T>`, `Set`, `Remove` methods exist
   - Consider adding scoped context for nested patterns

## Testing

After refactoring, run:
```bash
cd src/Reluxer.Transformer.Tests
dotnet run -- TodoList.tsx
```

Expected: Same output as before, but with declarative implementation.

## Benefits

1. **No mutable state** - Context is explicit and scoped
2. **Pattern declares context** - Named captures show what context is needed
3. **Composable** - Nested patterns naturally inherit context
4. **Testable** - Each pattern handler can be tested independently
5. **Matches Reluxer philosophy** - Pattern IS the specification
