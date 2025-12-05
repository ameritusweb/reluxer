using Minimact.AspNetCore.Core;
using Minimact.AspNetCore.Extensions;
using MinimactHelpers = Minimact.AspNetCore.Core.Minimact;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinimactTest.Components;

[Component]
public partial class ConditionalStateTest : MinimactComponent
{
    [State]
    private bool myState1 = false;

    [State]
    private bool myState2 = false;

    [State]
    private string myState3 = "Initial text";

    protected override VNode Render()
    {
        StateManager.SyncMembersToState(this);

        return MinimactHelpers.createElement("div", null, new VElement("h1", "1.1", new Dictionary<string, string>(), "Conditional State Test"), new VElement("div", "1.2", new Dictionary<string, string> { ["class"] = "controls" }, new VNode[]
        {
            new VElement("button", "1.2.1", new Dictionary<string, string> { ["onclick"] = "Handle0" }, new VNode[]
            {
                new VText("Toggle myState1 (currently:", "1.2.1.1"),
                new VText($"{(myState1?'true':'false')}", "1.2.1.2"),
                new VText(")", "1.2.1.3")
            }),
            new VElement("button", "1.2.2", new Dictionary<string, string> { ["onclick"] = "Handle1" }, new VNode[]
            {
                new VText("Toggle myState2 (currently:", "1.2.2.1"),
                new VText($"{(myState2?'true':'false')}", "1.2.2.2"),
                new VText(")", "1.2.2.3")
            }),
            new VElement("input", "1.2.3", new Dictionary<string, string> { ["type"] = "text", ["value"] = $"{(myState3)}", ["onchange"] = "Handle2", ["placeholder"] = "Edit myState3" })
        }), new VElement("div", "1.3", new Dictionary<string, string> { ["class"] = "state-display" }, new VNode[]
        {
            new VElement("p", "1.3.1", new Dictionary<string, string>(), new VNode[]
            {
                new VText("myState1:", "1.3.1.1"),
                new VText($"{(myState1?'✓ true':'✗ false')}", "1.3.1.2")
            }),
            new VElement("p", "1.3.2", new Dictionary<string, string>(), new VNode[]
            {
                new VText("myState2:", "1.3.2.1"),
                new VText($"{(myState2?'✓ true':'✗ false')}", "1.3.2.2")
            }),
            new VElement("p", "1.3.3", new Dictionary<string, string>(), new VNode[]
            {
                new VText("myState3: \"", "1.3.3.1"),
                new VText($"{(myState3)}", "1.3.3.2"),
                new VText("\"", "1.3.3.3")
            }),
            new VElement("p", "1.3.4", new Dictionary<string, string>(), new VNode[]
            {
                new VText("Condition (myState1 && !myState2):", "1.3.4.1"),
                new VText($"{(myState1&&!myState2?'✓ SHOWN':'✗ HIDDEN')}", "1.3.4.2")
            })
        }), new VElement("hr", "1.4", new Dictionary<string, string>()), ((myState1) && (!myState2)) ? new VElement("div", "1.5.1", new Dictionary<string, string> { ["class"] = "conditional-content" }, new VNode[]
        {
            new VElement("h2", "1.5.1.1", new Dictionary<string, string>(), "Conditionally Rendered Section"),
            new VElement("p", "1.5.1.2", new Dictionary<string, string>(), "This section is only visible when:"),
            new VElement("ul", "1.5.1.3", new Dictionary<string, string>(), new VNode[]
            {
                new VElement("li", "1.5.1.3.1", new Dictionary<string, string>(), new VNode[]
                {
                    new VText("myState1 is", "1.5.1.3.1.1"),
                    new VElement("strong", "1.5.1.3.1.2", new Dictionary<string, string>(), "true")
                }),
                new VElement("li", "1.5.1.3.2", new Dictionary<string, string>(), new VNode[]
                {
                    new VText("myState2 is", "1.5.1.3.2.1"),
                    new VElement("strong", "1.5.1.3.2.2", new Dictionary<string, string>(), "false")
                })
            }),
            new VElement("div", "1.5.1.4", new Dictionary<string, string> { ["class"] = "nested-content" }, new VNode[]
            {
                new VElement("p", "1.5.1.4.1", new Dictionary<string, string>(), "Some nested DOM elements here"),
                new VElement("div", "1.5.1.4.2", new Dictionary<string, string> { ["class"] = "dynamic-value" }, new VNode[]
                {
                    new VText("Dynamic content:", "1.5.1.4.2.1"),
                    new VElement("strong", "1.5.1.4.2.2", new Dictionary<string, string>(), new VNode[]
                    {
                        new VText($"{(myState3)}", "1.5.1.4.2.2.1")
                    })
                })
            })
        }) : new VNull("1.5"), new VElement("div", "1.6", new Dictionary<string, string> { ["class"] = "test-cases" }, new VNode[]
        {
            new VElement("h3", "1.6.1", new Dictionary<string, string>(), "Test Scenarios:"),
            new VElement("ol", "1.6.2", new Dictionary<string, string>(), new VNode[]
            {
                new VElement("li", "1.6.2.1", new Dictionary<string, string>(), new VNode[]
                {
                    new VElement("strong", "1.6.2.1.1", new Dictionary<string, string>(), "Initial State:"),
                    new VText("myState1=false, myState2=false → Section HIDDEN", "1.6.2.1.2"),
                    new VElement("br", "1.6.2.1.3", new Dictionary<string, string>()),
                    new VElement("em", "1.6.2.1.4", new Dictionary<string, string>(), "Tests VNull node generation when condition is initially false")
                }),
                new VElement("li", "1.6.2.2", new Dictionary<string, string>(), new VNode[]
                {
                    new VElement("strong", "1.6.2.2.1", new Dictionary<string, string>(), "Toggle myState1 to true:"),
                    new VText("myState1=true, myState2=false → Section SHOWN", "1.6.2.2.2"),
                    new VElement("br", "1.6.2.2.3", new Dictionary<string, string>()),
                    new VElement("em", "1.6.2.2.4", new Dictionary<string, string>(), "Tests patch to replace VNull with full DOM tree")
                }),
                new VElement("li", "1.6.2.3", new Dictionary<string, string>(), new VNode[]
                {
                    new VElement("strong", "1.6.2.3.1", new Dictionary<string, string>(), "Toggle myState2 to true:"),
                    new VText("myState1=true, myState2=true → Section HIDDEN", "1.6.2.3.2"),
                    new VElement("br", "1.6.2.3.3", new Dictionary<string, string>()),
                    new VElement("em", "1.6.2.3.4", new Dictionary<string, string>(), "Tests patch to replace DOM tree with VNull")
                }),
                new VElement("li", "1.6.2.4", new Dictionary<string, string>(), new VNode[]
                {
                    new VElement("strong", "1.6.2.4.1", new Dictionary<string, string>(), "Toggle myState2 back to false:"),
                    new VText("myState1=true, myState2=false → Section SHOWN", "1.6.2.4.2"),
                    new VElement("br", "1.6.2.4.3", new Dictionary<string, string>()),
                    new VElement("em", "1.6.2.4.4", new Dictionary<string, string>(), "Tests patch to re-show the DOM tree")
                }),
                new VElement("li", "1.6.2.5", new Dictionary<string, string>(), new VNode[]
                {
                    new VElement("strong", "1.6.2.5.1", new Dictionary<string, string>(), "Edit myState3 while visible:"),
                    new VText("Change text in input", "1.6.2.5.2"),
                    new VElement("br", "1.6.2.5.3", new Dictionary<string, string>()),
                    new VElement("em", "1.6.2.5.4", new Dictionary<string, string>(), "Tests state synchronization and dynamic content updates inside conditional")
                })
            })
        }));
    }

    public void Handle0()
    {
        ()=>SetState(nameof(myState1), !myState1);
    }

    public void Handle1()
    {
        ()=>SetState(nameof(myState2), !myState2);
    }

    public void Handle2()
    {
        (e)=>SetState(nameof(myState3), e.target.value);
    }

    /// <summary>
    /// Returns JavaScript event handlers for client-side execution
    /// These execute in the browser with bound hook context
    /// </summary>
    protected override Dictionary<string, string> GetClientHandlers()
    {
        return new Dictionary<string, string>
        {
            ["Handle0"] = @"function () {\n  setMyState1(!myState1);\n}",
            ["Handle1"] = @"function () {\n  setMyState2(!myState2);\n}",
            ["Handle2"] = @"function () {\n  setMyState3(e.target.value);\n}"
        };
    }
}
