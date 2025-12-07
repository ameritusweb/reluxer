using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Minimact.AspNetCore.Core;
using Minimact.AspNetCore.Rendering;
using Minimact.AspNetCore.Extensions;
using MinimactHelpers = Minimact.AspNetCore.Core.Minimact;

namespace MinimactTest.Components;

[Component]
public partial class Counter : MinimactComponent
{
    [State]
    private int count = 0;

    protected override VNode Render()
    {
        StateManager.SyncMembersToState(this);

        return MinimactHelpers.createElement("div", null, new VElement("h3", "1.1", new Dictionary<string, string>(), "Counter"), new VElement("p", "1.2", new Dictionary<string, string> { ["id"] = "counter-display" }, new VNode[]
        {
            new VText("Count:", "1.2.1"),
            new VText($"{(count)}", "1.2.2")
        }), new VElement("button", "1.3", new Dictionary<string, string> { ["id"] = "child-increment-btn", ["type"] = "button", ["onclick"] = "Handle0" }, "Child Increment"));
    }

    public void Handle0()
    {
        SetState(nameof(count), count + 1);
    }

    /// <summary>
    /// Returns JavaScript event handlers for client-side execution
    /// These execute in the browser with bound hook context
    /// </summary>
    protected override Dictionary<string, string> GetClientHandlers()
    {
        return new Dictionary<string, string>
        {
            ["Handle0"] = @"function () {\n  setCount(count + 1);\n}"
        };
    }
}

[Component]
public partial class App : MinimactComponent
{
    protected override VNode Render()
    {
        StateManager.SyncMembersToState(this);

        var counterValue = GetState<dynamic>("Counter.count");

        return MinimactHelpers.createElement("div", null, new VElement("h1", "1.1", new Dictionary<string, string>(), "Lifted State - Simple Example"), new VElement("div", "1.2", new Dictionary<string, string> { ["class"] = "parent-display" }, new VNode[]
        {
            new VElement("p", "1.2.1", new Dictionary<string, string>(), new VNode[]
            {
                new VText("Parent sees counter value:", "1.2.1.1"),
                new VElement("span", "1.2.1.2", new Dictionary<string, string> { ["id"] = "parent-sees" }, new VNode[]
                {
                    new VText($"{(counterValue)}", "1.2.1.2.1")
                })
            })
        }), new VElement("div", "1.3", new Dictionary<string, string> { ["class"] = "parent-controls" }, new VNode[]
        {
            new VElement("button", "1.3.1", new Dictionary<string, string> { ["id"] = "parent-reset-btn", ["type"] = "button", ["onclick"] = "handleParentReset" }, "Parent: Reset to 0"),
            new VElement("button", "1.3.2", new Dictionary<string, string> { ["id"] = "parent-set10-btn", ["type"] = "button", ["onclick"] = "handleParentSetTo10" }, "Parent: Set to 10")
        }),         new VComponentWrapper
                {
                        ComponentName = "Component",
                        ComponentType = "Component",
                        HexPath = "1.4",
            InitialState = new Dictionary<string, object> {              }
        }, new VElement("div", "1.5", new Dictionary<string, string> { ["id"] = "status", ["class"] = "status" }, new VNode[]
        {
            new VElement("p", "1.5.1", new Dictionary<string, string>(), new VNode[]
            {
                new VText("Counter Value:", "1.5.1.1"),
                new VElement("span", "1.5.1.2", new Dictionary<string, string> { ["id"] = "status-value" }, new VNode[]
                {
                    new VText($"{(counterValue)}", "1.5.1.2.1")
                })
            })
        }));
    }

    public void handleParentReset()
    {
        SetState("Counter.count",0);
    }

    public void handleParentSetTo10()
    {
        SetState("Counter.count",10);
    }

    /// <summary>
    /// Returns JavaScript event handlers for client-side execution
    /// These execute in the browser with bound hook context
    /// </summary>
    protected override Dictionary<string, string> GetClientHandlers()
    {
        return new Dictionary<string, string>
        {
            ["handleParentReset"] = @"function () {\n  { setState(""Counter.count"",0); };\n}",
            ["handleParentSetTo10"] = @"function () {\n  { setState(""Counter.count"",10); };\n}"
        };
    }
}
