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
public partial class Card : MinimactComponent
{
    protected override VNode Render()
    {
        StateManager.SyncMembersToState(this);

        return MinimactHelpers.createElement("div", null, new VElement("div", "1.1", new Dictionary<string, string> { ["class"] = "card-header" }, new VNode[]
        {
            new VElement("h2", "1.1.1", new Dictionary<string, string>(), new VNode[]
            {
                new VText($"{(title)}", "1.1.1.1")
            })
        }), new VElement("div", "1.2", new Dictionary<string, string> { ["class"] = "card-body" }, new VNode[]
        {
            new VText($"{(children)}", "1.2.1")
        }));
    }
}

[Component]
public partial class Dashboard : MinimactComponent
{
    protected override VNode Render()
    {
        StateManager.SyncMembersToState(this);

        return MinimactHelpers.createElement("div", null,         new VComponentWrapper
                {
                        ComponentName = "Card",
                        ComponentType = "Card",
                        HexPath = "1.1",
            InitialState = new Dictionary<string, object> {              }
        },         new VComponentWrapper
                {
                        ComponentName = "Card",
                        ComponentType = "Card",
                        HexPath = "1.2",
            InitialState = new Dictionary<string, object> {              }
        });
    }
}
