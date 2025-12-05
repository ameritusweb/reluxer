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
    [State]
    private bool isLoading = false;

    protected override VNode Render()
    {
        StateManager.SyncMembersToState(this);

        return MinimactHelpers.createElement("div", null, new VElement("h3", "1.1", new Dictionary<string, string>(), "User Profile"), new VElement("button", "1.2", new Dictionary<string, string> { ["id"] = "refresh-profile-btn", ["type"] = "button", ["onclick"] = "handleRefresh" }, "Refresh Profile"), (new MObject(isLoading)) ? new VElement("span", "1.3.1", new Dictionary<string, string> { ["class"] = "loading-indicator" }, "Loading...") : new VNull("1.3"));
    }

    public void handleRefresh()
    {
        SetState(nameof(isLoading), true);SetState(nameof(timeout), ()=>SetState(nameof(isLoading), false),2000);
    }

    /// <summary>
    /// Returns JavaScript event handlers for client-side execution
    /// These execute in the browser with bound hook context
    /// </summary>
    protected override Dictionary<string, string> GetClientHandlers()
    {
        return new Dictionary<string, string>
        {
            ["handleRefresh"] = @"function () {\n  { setIsLoading(true);setTimeout(()=>setIsLoading(false),2000); };\n}"
        };
    }
}

[Component]
public partial class ShoppingCart : MinimactComponent
{
    [State]
    private bool isLoading = false;

    protected override VNode Render()
    {
        StateManager.SyncMembersToState(this);

        return MinimactHelpers.createElement("div", null, new VElement("h3", "1.1", new Dictionary<string, string>(), "Shopping Cart"), new VElement("button", "1.2", new Dictionary<string, string> { ["id"] = "refresh-cart-btn", ["type"] = "button", ["onclick"] = "handleRefresh" }, "Refresh Cart"), (new MObject(isLoading)) ? new VElement("span", "1.3.1", new Dictionary<string, string> { ["class"] = "loading-indicator" }, "Loading...") : new VNull("1.3"));
    }

    public void handleRefresh()
    {
        SetState(nameof(isLoading), true);SetState(nameof(timeout), ()=>SetState(nameof(isLoading), false),1500);
    }

    /// <summary>
    /// Returns JavaScript event handlers for client-side execution
    /// These execute in the browser with bound hook context
    /// </summary>
    protected override Dictionary<string, string> GetClientHandlers()
    {
        return new Dictionary<string, string>
        {
            ["handleRefresh"] = @"function () {\n  { setIsLoading(true);setTimeout(()=>setIsLoading(false),1500); };\n}"
        };
    }
}

[Component]
public partial class ContactForm : MinimactComponent
{
    [State]
    private bool isLoading = false;

    protected override VNode Render()
    {
        StateManager.SyncMembersToState(this);

        return MinimactHelpers.createElement("div", null, new VElement("h3", "1.1", new Dictionary<string, string>(), "Contact Form"), new VElement("button", "1.2", new Dictionary<string, string> { ["id"] = "submit-form-btn", ["type"] = "button", ["onclick"] = "handleSubmit" }, "Submit Form"), (new MObject(isLoading)) ? new VElement("span", "1.3.1", new Dictionary<string, string> { ["class"] = "loading-indicator" }, "Loading...") : new VNull("1.3"));
    }

    public void handleSubmit()
    {
        SetState(nameof(isLoading), true);SetState(nameof(timeout), ()=>SetState(nameof(isLoading), false),1000);
    }

    /// <summary>
    /// Returns JavaScript event handlers for client-side execution
    /// These execute in the browser with bound hook context
    /// </summary>
    protected override Dictionary<string, string> GetClientHandlers()
    {
        return new Dictionary<string, string>
        {
            ["handleSubmit"] = @"function () {\n  { setIsLoading(true);setTimeout(()=>setIsLoading(false),1000); };\n}"
        };
    }
}

[Component]
public partial class Dashboard : MinimactComponent
{
    protected override VNode Render()
    {
        StateManager.SyncMembersToState(this);

        var _UserProfile_isLoading_reader = State["UserProfile.isLoading"];
        var _ShoppingCart_isLoading_reader = State["ShoppingCart.isLoading"];
        var _ContactForm_isLoading_reader = State["ContactForm.isLoading"];

        return MinimactHelpers.createElement("div", null, new VElement("h1", "1.1", new Dictionary<string, string>(), "Dashboard"), (new MObject(anyLoading)) ? new VElement("div", "1.2.1", new Dictionary<string, string> { ["id"] = "loading-overlay", ["class"] = "loading-overlay" }, new VNode[]
        {
            new VElement("div", "1.2.1.1", new Dictionary<string, string> { ["class"] = "spinner" }),
            new VElement("p", "1.2.1.2", new Dictionary<string, string>(), "Loading...")
        }) : new VNull("1.2"),         new VComponentWrapper
                {
                        ComponentName = "Component",
                        ComponentType = "Component",
                        HexPath = "1.3",
            InitialState = new Dictionary<string, object> {              }
        },         new VComponentWrapper
                {
                        ComponentName = "Component",
                        ComponentType = "Component",
                        HexPath = "1.4",
            InitialState = new Dictionary<string, object> {              }
        },         new VComponentWrapper
                {
                        ComponentName = "Component",
                        ComponentType = "Component",
                        HexPath = "1.5",
            InitialState = new Dictionary<string, object> {              }
        }, new VElement("div", "1.6", new Dictionary<string, string> { ["id"] = "status", ["class"] = "status" }, new VNode[]
        {
            new VElement("p", "1.6.1", new Dictionary<string, string>(), new VNode[]
            {
                new VText("User Loading:", "1.6.1.1"),
                new VElement("span", "1.6.1.2", new Dictionary<string, string> { ["id"] = "user-loading" }, new VNode[]
                {
                    new VText($"{(userLoading?'Yes':'No')}", "1.6.1.2.1")
                })
            }),
            new VElement("p", "1.6.2", new Dictionary<string, string>(), new VNode[]
            {
                new VText("Cart Loading:", "1.6.2.1"),
                new VElement("span", "1.6.2.2", new Dictionary<string, string> { ["id"] = "cart-loading" }, new VNode[]
                {
                    new VText($"{(cartLoading?'Yes':'No')}", "1.6.2.2.1")
                })
            }),
            new VElement("p", "1.6.3", new Dictionary<string, string>(), new VNode[]
            {
                new VText("Form Loading:", "1.6.3.1"),
                new VElement("span", "1.6.3.2", new Dictionary<string, string> { ["id"] = "form-loading" }, new VNode[]
                {
                    new VText($"{(formLoading?'Yes':'No')}", "1.6.3.2.1")
                })
            }),
            new VElement("p", "1.6.4", new Dictionary<string, string>(), new VNode[]
            {
                new VText("Any Loading:", "1.6.4.1"),
                new VElement("span", "1.6.4.2", new Dictionary<string, string> { ["id"] = "any-loading" }, new VNode[]
                {
                    new VText($"{(anyLoading?'Yes':'No')}", "1.6.4.2.1")
                })
            })
        }));
    }
}
