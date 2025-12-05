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
public partial class UserProfile : MinimactComponent
{
    protected override VNode Render()
    {
        StateManager.SyncMembersToState(this);

        return MinimactHelpers.createElement("div", null, new VText($"{(loading?(<divclassName="spinner">Loading...</div>):(<divclassName="user-info"><h1>{user.name}</h1><p>{user.email}</p>{user.isAdmin&&<spanclassName="badge">Admin</span>}</div>))}", "1.1"));
    }
}
