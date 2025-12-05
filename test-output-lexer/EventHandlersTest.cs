using Minimact.AspNetCore.Core;
using Minimact.AspNetCore.Extensions;
using MinimactHelpers = Minimact.AspNetCore.Core.Minimact;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinimactTest.Components;

[Component]
public partial class EventHandlersTest : MinimactComponent
{
    [State]
    private int count = 0;

    [State]
    private string message = "";

    protected override VNode Render()
    {
        StateManager.SyncMembersToState(this);

        return MinimactHelpers.createElement("div", null, new VElement("h1", "1.1", new Dictionary<string, string>(), new VNode[]
        {
            new VText("Count:", "1.1.1"),
            new VText($"{(count)}", "1.1.2")
        }), new VElement("button", "1.2", new Dictionary<string, string> { ["onclick"] = "Handle0" }, "Increment"), new VElement("button", "1.3", new Dictionary<string, string> { ["onclick"] = "Handle1" }, "Decrement"), new VElement("button", "1.4", new Dictionary<string, string> { ["onclick"] = "Handle2" }, "Reset"), new VElement("input", "1.5", new Dictionary<string, string> { ["value"] = $"{(message)}", ["onchange"] = "Handle3", ["placeholder"] = "Type something..." }));
    }

    public void Handle0()
    {
        SetState(nameof(count), count + 1);
    }

    public void Handle1()
    {
        SetState(nameof(count), count - 1);
    }

    public void Handle2()
    {
        SetState(nameof(count), 0);
    }

    public void Handle3()
    {
        SetState(nameof(message), e.target.value);
    }

    /// <summary>
    /// Returns JavaScript event handlers for client-side execution
    /// These execute in the browser with bound hook context
    /// </summary>
    protected override Dictionary<string, string> GetClientHandlers()
    {
        return new Dictionary<string, string>
        {
            ["Handle0"] = @"function () {\n  setCount(count + 1);\n}",
            ["Handle1"] = @"function () {\n  setCount(count - 1);\n}",
            ["Handle2"] = @"function () {\n  setCount(0);\n}",
            ["Handle3"] = @"function () {\n  setMessage(e.target.value);\n}"
        };
    }
}
