using Minimact.AspNetCore.Core;
using Minimact.AspNetCore.Extensions;
using MinimactHelpers = Minimact.AspNetCore.Core.Minimact;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinimactTest.Components;

[Component]
public partial class ProductCard : MinimactComponent
{
    protected override VNode Render()
    {
        StateManager.SyncMembersToState(this);

        return MinimactHelpers.createElement("div", null, new VElement("img", "1.1", new Dictionary<string, string> { ["class"] = "product-image", ["alt"] = "Product" }), new VElement("h2", "1.2", new Dictionary<string, string> { ["class"] = "product-name" }, new VNode[]
        {
            new VText($"{(dynamic.getState().product.name)}", "1.2.1")
        }), new VElement("div", "1.3", new Dictionary<string, string> { ["class"] = "pricing" }, new VNode[]
        {
            new VElement("span", "1.3.1", new Dictionary<string, string> { ["class"] = "price", ["data-testid"] = "price" }),
            new VElement("span", "1.3.2", new Dictionary<string, string> { ["class"] = "premium-badge" }, "✨ PREMIUM PRICE")
        }), new VElement("div", "1.4", new Dictionary<string, string> { ["class"] = "stock-status", ["data-testid"] = "stock-status" }), new VElement("div", "1.5", new Dictionary<string, string> { ["class"] = "user-info" }, new VNode[]
        {
            new VElement("span", "1.5.1", new Dictionary<string, string> { ["class"] = "user-badge", ["data-testid"] = "user-badge" })
        }), new VElement("div", "1.6", new Dictionary<string, string> { ["class"] = "details-panel", ["data-testid"] = "details-panel" }, new VNode[]
        {
            new VElement("p", "1.6.1", new Dictionary<string, string>(), new VNode[]
            {
                new VText("Factory Price: $", "1.6.1.1"),
                new VText($"{(dynamic.getState().product.factoryPrice)}", "1.6.1.2")
            }),
            new VElement("p", "1.6.2", new Dictionary<string, string>(), new VNode[]
            {
                new VText("Retail Price: $", "1.6.2.1"),
                new VText($"{(dynamic.getState().product.price)}", "1.6.2.2")
            })
        }), new VElement("div", "1.7", new Dictionary<string, string> { ["class"] = "controls" }, new VNode[]
        {
            new VElement("button", "1.7.1", new Dictionary<string, string> { ["onclick"] = "Handle0", ["data-action"] = "upgrade-premium" }, "Upgrade to Premium"),
            new VElement("button", "1.7.2", new Dictionary<string, string> { ["onclick"] = "Handle1", ["data-action"] = "downgrade-basic" }, "Downgrade to Basic"),
            new VElement("button", "1.7.3", new Dictionary<string, string> { ["onclick"] = "Handle2", ["data-action"] = "set-admin" }, "Set Admin"),
            new VElement("button", "1.7.4", new Dictionary<string, string> { ["onclick"] = "Handle3", ["data-action"] = "toggle-stock" }, "Toggle Stock"),
            new VElement("button", "1.7.5", new Dictionary<string, string> { ["onclick"] = "Handle4", ["data-action"] = "toggle-details" }, "Toggle Details"),
            new VElement("button", "1.7.6", new Dictionary<string, string> { ["onclick"] = "Handle5", ["data-action"] = "toggle-theme" }, "Toggle Theme")
        }));
    }

    public void Handle0()
    {
        dynamic.SetState(nameof(state), {user:{isPremium:true,role:'premium'}});
    }

    public void Handle1()
    {
        dynamic.SetState(nameof(state), {user:{isPremium:false,role:'basic'}});
    }

    public void Handle2()
    {
        dynamic.SetState(nameof(state), {user:{role:'admin'}asany});
    }

    public void Handle3()
    {
        dynamic.SetState(nameof(state), {product:{inStock:!dynamic.getState().product.inStock}asany});
    }

    public void Handle4()
    {
        dynamic.SetState(nameof(state), {ui:{showDetails:!dynamic.getState().ui.showDetails}asany});
    }

    public void Handle5()
    {
        dynamic.SetState(nameof(state), {ui:{theme:dynamic.getState().ui.theme==='light'?'dark':'light'}asany});
    }

    /// <summary>
    /// Returns JavaScript event handlers for client-side execution
    /// These execute in the browser with bound hook context
    /// </summary>
    protected override Dictionary<string, string> GetClientHandlers()
    {
        return new Dictionary<string, string>
        {
            ["Handle0"] = @"function () {\n  dynamic.setState({user:{isPremium:true,role:'premium'}});\n}",
            ["Handle1"] = @"function () {\n  dynamic.setState({user:{isPremium:false,role:'basic'}});\n}",
            ["Handle2"] = @"function () {\n  dynamic.setState({user:{role:'admin'}asany});\n}",
            ["Handle3"] = @"function () {\n  dynamic.setState({product:{inStock:!dynamic.getState().product.inStock}asany});\n}",
            ["Handle4"] = @"function () {\n  dynamic.setState({ui:{showDetails:!dynamic.getState().ui.showDetails}asany});\n}",
            ["Handle5"] = @"function () {\n  dynamic.setState({ui:{theme:dynamic.getState().ui.theme==='light'?'dark':'light'}asany});\n}"
        };
    }
}
