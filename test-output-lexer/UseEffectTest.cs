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
public partial class UseEffectTest : MinimactComponent
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
        }), new VElement("p", "1.2", new Dictionary<string, string>(), new VNode[]
        {
            new VText("Message:", "1.2.1"),
            new VText($"{(message)}", "1.2.2")
        }));
    }
}
