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
public partial class ConditionalRenderingTest : MinimactComponent
{
    [State]
    private bool isLoggedIn = false;

    [State]
    private object user = null;

    [State]
    private int count = 0;

    protected override VNode Render()
    {
        StateManager.SyncMembersToState(this);

        return MinimactHelpers.createElement("div", null, new VText($"{(isLoggedIn?(<div><h1>Welcome back!</h1><buttononClick={()=>setIsLoggedIn(false)}>Logout</button></div>):(<div><h1>Please log in</h1><buttononClick={()=>setIsLoggedIn(true)}>Login</button></div>))}", "1.1"), (new MObject(user)) ? new VElement("p", "1.2.1", new Dictionary<string, string>(), new VNode[]
        {
            new VText("User:", "1.2.1.1"),
            new VText($"{(user.name)}", "1.2.1.2")
        }) : new VNull("1.2"), (new MObject(count>0)) ? new VElement("p", "1.3.1", new Dictionary<string, string>(), new VNode[]
        {
            new VText("Count is positive:", "1.3.1.1"),
            new VText($"{(count)}", "1.3.1.2")
        }) : new VNull("1.3"));
    }
}
