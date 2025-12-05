using Minimact.AspNetCore.Core;
using Minimact.AspNetCore.Extensions;
using MinimactHelpers = Minimact.AspNetCore.Core.Minimact;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minimact.Components;

[Component]
public partial class Gallery : MinimactComponent
{
    protected override VNode Render()
    {
        StateManager.SyncMembersToState(this);

        return MinimactHelpers.createElement("div", new { id = "gallery-root", ref = el => section.attachElement(el) }, new VElement("h2", "1.1", new Dictionary<string, string> { ["id"] = "gallery-title" }, "Image Gallery"), (new MObject(section.isIntersecting)) ? new VElement("div", "1.2.1", new Dictionary<string, string> { ["id"] = "gallery-images", ["class"] = "images" }, new VNode[]
            {
                new VElement("img", "1.2.1.1", new Dictionary<string, string> { ["src"] = "photo1.jpg", ["alt"] = "Photo 1" }),
                new VElement("img", "1.2.1.2", new Dictionary<string, string> { ["src"] = "photo2.jpg", ["alt"] = "Photo 2" })
            }) : new VNull("1.2"), (section.childrenCount > 5) ? new VElement("button", "1.3.1", new Dictionary<string, string> { ["id"] = "collapse-btn", ["type"] = "button" }, "Collapse") : new VNull("1.3"), (new MObject(section.isIntersecting)) ? new VElement("span", "1.4.1", new Dictionary<string, string> { ["id"] = "visible-badge", ["class"] = "badge" }, "Visible") : new VNull("1.4"));
    }
}
