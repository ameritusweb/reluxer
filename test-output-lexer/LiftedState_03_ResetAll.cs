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
    protected override VNode Render()
    {
        StateManager.SyncMembersToState(this);

        return MinimactHelpers.createElement("div", null, new VElement("h3", "1.1", new Dictionary<string, string>(), "Counter"), new VElement("p", "1.2", new Dictionary<string, string>(), new VNode[]
        {
            new VText("Count:", "1.2.1"),
            new VElement("span", "1.2.2", new Dictionary<string, string> { ["id"] = "counter-value" }, new VNode[]
            {
                new VText($"{(count)}", "1.2.2.1")
            })
        }), new VElement("button", "1.3", new Dictionary<string, string> { ["id"] = "increment-btn", ["type"] = "button", ["onclick"] = "Handle0" }, "Increment"));
    }

    public void Handle0()
    {
        SetState(nameof(state), 'count',count + 1);
    }

    /// <summary>
    /// Returns JavaScript event handlers for client-side execution
    /// These execute in the browser with bound hook context
    /// </summary>
    protected override Dictionary<string, string> GetClientHandlers()
    {
        return new Dictionary<string, string>
        {
            ["Handle0"] = @"function () {\n  setState('count',count + 1);\n}"
        };
    }
}

[Component]
public partial class Timer : MinimactComponent
{
    protected override VNode Render()
    {
        StateManager.SyncMembersToState(this);

        return MinimactHelpers.createElement("div", null, new VElement("h3", "1.1", new Dictionary<string, string>(), "Timer"), new VElement("p", "1.2", new Dictionary<string, string>(), new VNode[]
        {
            new VText("Seconds:", "1.2.1"),
            new VElement("span", "1.2.2", new Dictionary<string, string> { ["id"] = "timer-value" }, new VNode[]
            {
                new VText($"{(seconds)}", "1.2.2.1")
            })
        }), new VElement("button", "1.3", new Dictionary<string, string> { ["id"] = "tick-btn", ["type"] = "button", ["onclick"] = "handleTick" }, "Tick"));
    }

    public void handleTick()
    {
        SetState(nameof(state), 'seconds',seconds + 1);
    }

    /// <summary>
    /// Returns JavaScript event handlers for client-side execution
    /// These execute in the browser with bound hook context
    /// </summary>
    protected override Dictionary<string, string> GetClientHandlers()
    {
        return new Dictionary<string, string>
        {
            ["handleTick"] = @"function () {\n  { setState('seconds',seconds + 1); };\n}"
        };
    }
}

[Component]
public partial class Form : MinimactComponent
{
    protected override VNode Render()
    {
        StateManager.SyncMembersToState(this);

        return MinimactHelpers.createElement("div", null, new VElement("h3", "1.1", new Dictionary<string, string>(), "Form"), new VElement("input", "1.2", new Dictionary<string, string> { ["id"] = "text-input", ["type"] = "text", ["placeholder"] = "Enter text...", ["value"] = $"{(text)}", ["oninput"] = "Handle0" }), new VElement("p", "1.3", new Dictionary<string, string>(), new VNode[]
        {
            new VText("Length:", "1.3.1"),
            new VElement("span", "1.3.2", new Dictionary<string, string> { ["id"] = "text-length" }, new VNode[]
            {
                new VText($"{(text.length)}", "1.3.2.1")
            })
        }));
    }

    public void Handle0()
    {
        SetState(nameof(state), 'text',e.target.value);
    }

    /// <summary>
    /// Returns JavaScript event handlers for client-side execution
    /// These execute in the browser with bound hook context
    /// </summary>
    protected override Dictionary<string, string> GetClientHandlers()
    {
        return new Dictionary<string, string>
        {
            ["Handle0"] = @"function () {\n  setState('text',e.target.value);\n}"
        };
    }
}

[Component]
public partial class Dashboard : MinimactComponent
{
    protected override VNode Render()
    {
        StateManager.SyncMembersToState(this);

        var _Counter_count_reader = State["Counter.count"];
        var _Timer_seconds_reader = State["Timer.seconds"];
        var _Form_text_reader = State["Form.text"];

        return MinimactHelpers.createElement("div", null, new VElement("h1", "1.1", new Dictionary<string, string>(), "Dashboard"), new VElement("div", "1.2", new Dictionary<string, string> { ["class"] = "controls" }, new VNode[]
        {
            new VElement("button", "1.2.1", new Dictionary<string, string> { ["id"] = "reset-all-btn", ["type"] = "button", ["onclick"] = "handleResetAll", ["disabled"] = $"{(!hasChanges)}" }, "Reset All Components"),
            new VElement("button", "1.2.2", new Dictionary<string, string> { ["id"] = "reset-counter-btn", ["type"] = "button", ["onclick"] = "handleResetCounter", ["disabled"] = $"{(counterValue===0)}" }, "Reset Counter Only"),
            new VElement("button", "1.2.3", new Dictionary<string, string> { ["id"] = "reset-timer-btn", ["type"] = "button", ["onclick"] = "handleResetTimer", ["disabled"] = $"{(timerValue===0)}" }, "Reset Timer Only"),
            new VElement("button", "1.2.4", new Dictionary<string, string> { ["id"] = "reset-form-btn", ["type"] = "button", ["onclick"] = "handleResetForm", ["disabled"] = $"{(formValue==="")}" }, "Reset Form Only")
        }),         new VComponentWrapper
                {
                        ComponentName = "Component",
                        ComponentType = "Component",
                        HexPath = "1.3",
            InitialState = new Dictionary<string, object> {              }
        },         new VComponentWrapper
                {
                        ComponentName = "Component",
                        ComponentType = "Component",
                        HexPath = "1.4",
            InitialState = new Dictionary<string, object> {              }
        },         new VComponentWrapper
                {
                        ComponentName = "Component",
                        ComponentType = "Component",
                        HexPath = "1.5",
            InitialState = new Dictionary<string, object> {              }
        }, new VElement("div", "1.6", new Dictionary<string, string> { ["id"] = "status", ["class"] = "status" }, new VNode[]
        {
            new VElement("p", "1.6.1", new Dictionary<string, string>(), new VNode[]
            {
                new VText("Counter:", "1.6.1.1"),
                new VElement("span", "1.6.1.2", new Dictionary<string, string> { ["id"] = "status-counter" }, new VNode[]
                {
                    new VText($"{(counterValue)}", "1.6.1.2.1")
                })
            }),
            new VElement("p", "1.6.2", new Dictionary<string, string>(), new VNode[]
            {
                new VText("Timer:", "1.6.2.1"),
                new VElement("span", "1.6.2.2", new Dictionary<string, string> { ["id"] = "status-timer" }, new VNode[]
                {
                    new VText($"{(timerValue)}", "1.6.2.2.1")
                })
            }),
            new VElement("p", "1.6.3", new Dictionary<string, string>(), new VNode[]
            {
                new VText("Form Text:", "1.6.3.1"),
                new VElement("span", "1.6.3.2", new Dictionary<string, string> { ["id"] = "status-form" }, new VNode[]
                {
                    new VText("\"", "1.6.3.2.1"),
                    new VText($"{(formValue)}", "1.6.3.2.2"),
                    new VText("\"", "1.6.3.2.3")
                })
            }),
            new VElement("p", "1.6.4", new Dictionary<string, string>(), new VNode[]
            {
                new VText("Has Changes:", "1.6.4.1"),
                new VElement("span", "1.6.4.2", new Dictionary<string, string> { ["id"] = "has-changes" }, new VNode[]
                {
                    new VText($"{(hasChanges?'Yes':'No')}", "1.6.4.2.1")
                })
            })
        }));
    }

    public void handleResetAll()
    {
        SetState(nameof(state), "Counter.count",0);SetState(nameof(state), "Timer.seconds",0);SetState(nameof(state), "Form.text","");
    }

    public void handleResetCounter()
    {
        SetState(nameof(state), "Counter.count",0);
    }

    public void handleResetTimer()
    {
        SetState(nameof(state), "Timer.seconds",0);
    }

    public void handleResetForm()
    {
        SetState(nameof(state), "Form.text","");
    }

    /// <summary>
    /// Returns JavaScript event handlers for client-side execution
    /// These execute in the browser with bound hook context
    /// </summary>
    protected override Dictionary<string, string> GetClientHandlers()
    {
        return new Dictionary<string, string>
        {
            ["handleResetAll"] = @"function () {\n  { setState(""Counter.count"",0);setState(""Timer.seconds"",0);setState(""Form.text"",""""); };\n}",
            ["handleResetCounter"] = @"function () {\n  { setState(""Counter.count"",0); };\n}",
            ["handleResetTimer"] = @"function () {\n  { setState(""Timer.seconds"",0); };\n}",
            ["handleResetForm"] = @"function () {\n  { setState(""Form.text"",""""); };\n}"
        };
    }
}
