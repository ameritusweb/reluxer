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
public partial class ShoppingCart : MinimactComponent
{
    protected override VNode Render()
    {
        StateManager.SyncMembersToState(this);

        return MinimactHelpers.createElement("div", null, new VElement("h1", "1.1", new Dictionary<string, string>(), "Shopping Cart"), new VElement("div", "1.2", new Dictionary<string, string> { ["class"] = "items" }, new VNode[]
        {
                        MinimactHelpers.createFragment(items.Select((item) =>
                new VElement("div", "1.2.1.item", new Dictionary<string, string> { ["key"] = $"{(item.id)}", ["class"] = "cart-item" }, new VNode[]
                {
                    new VElement("img", "1.2.1.item.1", new Dictionary<string, string> { ["src"] = $"{(item.image)}", ["alt"] = $"{(item.name)}" }),
                    new VElement("div", "1.2.1.item.2", new Dictionary<string, string> { ["class"] = "details" }, new VNode[]
                    {
                        new VElement("h3", "1.2.1.item.2.1", new Dictionary<string, string>(), new VNode[]
                        {
                            new VText($"{(item.name)}", "1.2.1.item.2.1.1")
                        }),
                        new VElement("p", "1.2.1.item.2.2", new Dictionary<string, string>(), new VNode[]
                        {
                            new VText("Quantity:", "1.2.1.item.2.2.1"),
                            new VText($"{(item.quantity)}", "1.2.1.item.2.2.2")
                        }),
                        new VElement("p", "1.2.1.item.2.3", new Dictionary<string, string>(), new VNode[]
                        {
                            new VText("$", "1.2.1.item.2.3.1"),
                            new VText($"{(item.price)}", "1.2.1.item.2.3.2")
                        })
                    }),
                    new VElement("button", "1.2.1.item.3", new Dictionary<string, string> { ["onclick"] = "Handle0" }, "Remove")
                })
            ).ToArray())
        }), new VElement("div", "1.3", new Dictionary<string, string> { ["class"] = "summary" }, new VNode[]
        {
            new VElement("p", "1.3.1", new Dictionary<string, string>(), new VNode[]
            {
                new VText("Subtotal: $", "1.3.1.1"),
                new VText($"{(total)}", "1.3.1.2")
            }),
            (new MObject(discount>0)) ? new VElement("p", "1.3.2.1", new Dictionary<string, string>(), new VNode[]
            {
                new VText("Discount:", "1.3.2.1.1"),
                new VText($"{(discount)}", "1.3.2.1.2"),
                new VText("%", "1.3.2.1.3")
            }) : new VNull("1.3.2"),
            new VElement("h2", "1.3.3", new Dictionary<string, string>(), new VNode[]
            {
                new VText("Total: $", "1.3.3.1"),
                new VText($"{(finalPrice.ToString("F2"))}", "1.3.3.2")
            }),
            new VElement("button", "1.3.4", new Dictionary<string, string> { ["onclick"] = "checkout", ["disabled"] = $"{(items.length===0)}" }, "Checkout")
        }));
    }

    public void Handle0()
    {
        removeItem(item.id);
    }

    public void Handle1()
    {
        removeItem(item.id);
    }

    /// <summary>
    /// Returns JavaScript event handlers for client-side execution
    /// These execute in the browser with bound hook context
    /// </summary>
    protected override Dictionary<string, string> GetClientHandlers()
    {
        return new Dictionary<string, string>
        {
            ["Handle0"] = @"function () {\n  removeItem(item.id);\n}",
            ["Handle1"] = @"function () {\n  removeItem(item.id);\n}"
        };
    }
}
