using Minimact.AspNetCore.Core;
using Minimact.AspNetCore.Extensions;
using MinimactHelpers = Minimact.AspNetCore.Core.Minimact;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinimactTest.Components;

[Component]
public partial class ListRenderingTest : MinimactComponent
{
    [State]
    private List<object> items = new List<object> { {id:1,name:'Apple',price:1.99},{id:2,name:'Banana',price:0.99},{id:3,name:'Orange',price:1.49} };

    [State]
    private List<object> users = new List<object> { {id:1,name:'Alice',email:'alice@example.com'},{id:2,name:'Bob',email:'bob@example.com'} };

    protected override VNode Render()
    {
        StateManager.SyncMembersToState(this);

        return MinimactHelpers.createElement("div", null, new VElement("h1", "1.1", new Dictionary<string, string>(), "Products"), new VElement("ul", "1.2", new Dictionary<string, string>(), new VNode[]
        {
                        MinimactHelpers.createFragment(items.Select((item) =>
                new VElement("li", "1.2.1.item", new Dictionary<string, string> { ["key"] = $"{(item.id)}" }, new VNode[]
                {
                    new VText($"{(item.name)}", "1.2.1.item.1"),
                    new VText("- $", "1.2.1.item.2"),
                    new VText($"{(item.price)}", "1.2.1.item.3")
                })
            ).ToArray())
        }), new VElement("h1", "1.3", new Dictionary<string, string>(), "Users"), new VElement("div", "1.4", new Dictionary<string, string>(), new VNode[]
        {
                        MinimactHelpers.createFragment(users.Select((user, index) =>
                new VElement("div", "1.4.1.item", new Dictionary<string, string> { ["key"] = $"{(user.id)}", ["class"] = "user-card" }, new VNode[]
                {
                    new VElement("h3", "1.4.1.item.1", new Dictionary<string, string>(), new VNode[]
                    {
                        new VText($"{(index+1)}", "1.4.1.item.1.1"),
                        new VText(".", "1.4.1.item.1.2"),
                        new VText($"{(user.name)}", "1.4.1.item.1.3")
                    }),
                    new VElement("p", "1.4.1.item.2", new Dictionary<string, string>(), new VNode[]
                    {
                        new VText($"{(user.email)}", "1.4.1.item.2.1")
                    })
                })
            ).ToArray())
        }));
    }
}
