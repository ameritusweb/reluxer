using Minimact.AspNetCore.Core;
using Minimact.AspNetCore.Extensions;
using MinimactHelpers = Minimact.AspNetCore.Core.Minimact;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinimactTest.Components;

[Component]
public partial class TernaryTest : MinimactComponent
{
    [State]
    private bool isExpanded = false;

    [State]
    private int count = 0;

    protected override VNode Render()
    {
        StateManager.SyncMembersToState(this);

        return MinimactHelpers.createElement("div", null, new VElement("h1", "1.1", new Dictionary<string, string>(), "Ternary Expression Test"), new VElement("button", "1.2", new Dictionary<string, string> { ["onclick"] = "Handle1" }, new VNode[]
        {
            (new MObject(isExpanded)) ? new VText("Hide", "1.2.1.1") : new VText("Show", "1.2.1.2"),
            new VText("Details", "1.2.2")
        }), new VElement("p", "1.3", new Dictionary<string, string>(), new VNode[]
        {
            new VText("Status:", "1.3.1"),
            (new MObject(isExpanded)) ? new VText("Expanded", "1.3.2.1") : new VText("Collapsed", "1.3.2.2")
        }), new VElement("p", "1.4", new Dictionary<string, string>(), new VNode[]
        {
            new VText("Count:", "1.4.1"),
            new VText($"{(count)}", "1.4.2")
        }), new VElement("p", "1.5", new Dictionary<string, string>(), new VNode[]
        {
            new VText("Total:", "1.5.1"),
            new VText($"{(count.toFixed(2))}", "1.5.2")
        }));
    }

    public void Handle0()
    {
        SetState(nameof(isExpanded), !isExpanded);
    }

    public void Handle1()
    {
        SetState(nameof(isExpanded), !isExpanded);
    }

    /// <summary>
    /// Returns JavaScript event handlers for client-side execution
    /// These execute in the browser with bound hook context
    /// </summary>
    protected override Dictionary<string, string> GetClientHandlers()
    {
        return new Dictionary<string, string>
        {
            ["Handle0"] = @"function () {\n  setIsExpanded(!isExpanded);\n}",
            ["Handle1"] = @"function () {\n  setIsExpanded(!isExpanded);\n}"
        };
    }
}
