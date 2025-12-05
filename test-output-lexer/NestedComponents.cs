using Minimact.AspNetCore.Core;
using Minimact.AspNetCore.Extensions;
using MinimactHelpers = Minimact.AspNetCore.Core.Minimact;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinimactTest.Components;

[Component]
public partial class Card : MinimactComponent
{
    protected override VNode Render()
    {
        StateManager.SyncMembersToState(this);

        return new VNull("1");
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
