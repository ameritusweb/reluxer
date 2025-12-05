using Minimact.AspNetCore.Core;
using Minimact.AspNetCore.Extensions;
using MinimactHelpers = Minimact.AspNetCore.Core.Minimact;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minimact.Components;

[Component]
public partial class ProductCard : MinimactComponent
{
    protected override VNode Render()
    {
        StateManager.SyncMembersToState(this);

        return new VElement("div", "1", new Dictionary<string, string> { ["class"] = "product-card", ["data-testid"] = "product-card" }, new VNode[]
        {
            new VElement("img", "1.1", new Dictionary<string, string> { ["class"] = "product-image", ["alt"] = "Product" }),
            new VElement("h2", "1.2", new Dictionary<string, string> { ["class"] = "product-name" }, new VNode[]
            {
                new VText($"{(dynamic.getState().product.name)}", "1.2.1")
            }),
            new VElement("div", "1.3", new Dictionary<string, string> { ["class"] = "pricing" }, new VNode[]
            {
                new VElement("span", "1.3.1", new Dictionary<string, string> { ["class"] = "price", ["data-testid"] = "price" }),
                new VElement("span", "1.3.2", new Dictionary<string, string> { ["class"] = "premium-badge" }, "✨ PREMIUM PRICE")
            }),
            new VElement("div", "1.4", new Dictionary<string, string> { ["class"] = "stock-status", ["data-testid"] = "stock-status" }),
            new VElement("div", "1.5", new Dictionary<string, string> { ["class"] = "user-info" }, new VNode[]
            {
                new VElement("span", "1.5.1", new Dictionary<string, string> { ["class"] = "user-badge", ["data-testid"] = "user-badge" })
            }),
            new VElement("div", "1.6", new Dictionary<string, string> { ["class"] = "details-panel", ["data-testid"] = "details-panel" }, new VNode[]
            {
                new VElement("p", "1.6.1", new Dictionary<string, string>(), $"Factory Price: ${(dynamic.getState().product.factoryPrice)}"),
                new VElement("p", "1.6.2", new Dictionary<string, string>(), $"Retail Price: ${(dynamic.getState().product.price)}")
            }),
            new VElement("div", "1.7", new Dictionary<string, string> { ["class"] = "controls" }, new VNode[]
            {
                new VElement("button", "1.7.1", new Dictionary<string, string> { ["data-action"] = "upgrade-premium", ["onclick"] = "Handle0" }, "Upgrade to Premium"),
                new VElement("button", "1.7.2", new Dictionary<string, string> { ["data-action"] = "downgrade-basic", ["onclick"] = "Handle2" }, "Downgrade to Basic"),
                new VElement("button", "1.7.3", new Dictionary<string, string> { ["data-action"] = "set-admin", ["onclick"] = "Handle4" }, "Set Admin"),
                new VElement("button", "1.7.4", new Dictionary<string, string> { ["data-action"] = "toggle-stock", ["onclick"] = "Handle6" }, "Toggle Stock"),
                new VElement("button", "1.7.5", new Dictionary<string, string> { ["data-action"] = "toggle-details", ["onclick"] = "Handle8" }, "Toggle Details"),
                new VElement("button", "1.7.6", new Dictionary<string, string> { ["data-action"] = "toggle-theme", ["onclick"] = "Handle10" }, "Toggle Theme")
            })
        });
    }

    public void Handle0()
    {
        dynamic.setState(new { user = new { isPremium = true, role = "premium" } });
    }

    public void Handle2()
    {
        dynamic.setState(new { user = new { isPremium = false, role = "basic" } });
    }

    public void Handle4()
    {
        dynamic.setState(new { user = new { role = "admin" } });
    }

    public void Handle6()
    {
        dynamic.setState(new { product = new { inStock = !dynamic.getState().product.inStock } });
    }

    public void Handle8()
    {
        dynamic.setState(new { ui = new { showDetails = !dynamic.getState().ui.showDetails } });
    }

    public void Handle10()
    {
        dynamic.setState(new { ui = new { theme = (dynamic.getState().ui.theme == "light") ? "dark" : "light" } });
    }

    /// <summary>
    /// Returns JavaScript event handlers for client-side execution
    /// These execute in the browser with bound hook context
    /// </summary>
    protected override Dictionary<string, string> GetClientHandlers()
    {
        return new Dictionary<string, string>
        {
            ["Handle0"] = @"function () {\n  dynamic.setState({\n    user: {\n      isPremium: true,\n      role: 'premium'\n    }\n  });\n}",
            ["Handle2"] = @"function () {\n  dynamic.setState({\n    user: {\n      isPremium: false,\n      role: 'basic'\n    }\n  });\n}",
            ["Handle4"] = @"function () {\n  dynamic.setState({\n    user: {\n      role: 'admin'\n    }\n  });\n}",
            ["Handle6"] = @"function () {\n  dynamic.setState({\n    product: {\n      inStock: !dynamic.getState().product.inStock\n    }\n  });\n}",
            ["Handle8"] = @"function () {\n  dynamic.setState({\n    ui: {\n      showDetails: !dynamic.getState().ui.showDetails\n    }\n  });\n}",
            ["Handle10"] = @"function () {\n  dynamic.setState({\n    ui: {\n      theme: dynamic.getState().ui.theme === 'light' ? 'dark' : 'light'\n    }\n  });\n}"
        };
    }
}
