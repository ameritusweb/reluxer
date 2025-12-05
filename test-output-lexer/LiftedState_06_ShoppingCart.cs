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
public partial class NavBar : MinimactComponent
{
    protected override VNode Render()
    {
        StateManager.SyncMembersToState(this);

        var _ShoppingCart_items_reader = State["ShoppingCart.items"];

        return MinimactHelpers.createElement("nav", null, new VElement("div", "1.1", new Dictionary<string, string> { ["class"] = "logo" }, "My Store"), new VElement("div", "1.2", new Dictionary<string, string> { ["id"] = "cart-icon", ["class"] = "cart-icon" }, new VNode[]
        {
            new VText("🛒", "1.2.1"),
            (new MObject(cartCount>0)) ? new VElement("span", "1.2.2.1", new Dictionary<string, string> { ["id"] = "cart-badge", ["class"] = "badge" }, new VNode[]
            {
                new VText($"{(cartCount)}", "1.2.2.1.1")
            }) : new VNull("1.2.2")
        }));
    }

    public void Handle0()
    {
        handleRemoveItem(idx);
    }

    /// <summary>
    /// Returns JavaScript event handlers for client-side execution
    /// These execute in the browser with bound hook context
    /// </summary>
    protected override Dictionary<string, string> GetClientHandlers()
    {
        return new Dictionary<string, string>
        {
            ["Handle0"] = @"function () {\n  handleRemoveItem(idx);\n}"
        };
    }
}

[Component]
public partial class ProductList : MinimactComponent
{
    protected override VNode Render()
    {
        StateManager.SyncMembersToState(this);

        return MinimactHelpers.createElement("div", null, new VElement("h2", "1.1", new Dictionary<string, string>(), "Products"), new VText($"{(PRODUCTS.map(product=>(<divkey={product.id}className="product-card"><h3>{product.name}</h3><pclassName="price">${product.price.ToString("F2")}</p><buttontype="button"className="add-to-cart-btn"data-product-id={product.id}onClick={()=>handleAddToCart(product)}>Add to Cart</button></div>)))}", "1.2"));
    }

    public void handleAddToCart()
    {
        constcartItems=state["ShoppingCart.items"]||[];constcartTotal=state["ShoppingCart.total"]||0;SetState(nameof(state), "ShoppingCart.items",[...cartItems,product]);SetState(nameof(state), "ShoppingCart.total",cartTotal+product.price);
    }

    public void Handle0()
    {
        handleRemoveItem(idx);
    }

    /// <summary>
    /// Returns JavaScript event handlers for client-side execution
    /// These execute in the browser with bound hook context
    /// </summary>
    protected override Dictionary<string, string> GetClientHandlers()
    {
        return new Dictionary<string, string>
        {
            ["handleAddToCart"] = @"function () {\n  { constcartItems=state[""ShoppingCart.items""]||[];constcartTotal=state[""ShoppingCart.total""]||0;setState(""ShoppingCart.items"",[...cartItems,product]);setState(""ShoppingCart.total"",cartTotal+product.price); };\n}",
            ["Handle0"] = @"function () {\n  handleRemoveItem(idx);\n}"
        };
    }
}

[Component]
public partial class ShoppingCart : MinimactComponent
{
    protected override VNode Render()
    {
        StateManager.SyncMembersToState(this);

        return MinimactHelpers.createElement("div", null, new VElement("h2", "1.1", new Dictionary<string, string>(), "Cart"),         MinimactHelpers.createFragment(items.length===0?(<pid="empty-cart-msg">Cart is empty</p>):(<><divid="cart-items">{items.Select((item, any) =>
            new VElement("div", "1.2.item", new Dictionary<string, string> { ["key"] = $"{(idx)}", ["class"] = "cart-item", ["data-item-index"] = $"{(idx)}" }, new VNode[]
            {
                new VElement("span", "1.2.item.1", new Dictionary<string, string>(), new VNode[]
                {
                    new VText($"{(item.name)}", "1.2.item.1.1")
                }),
                new VElement("span", "1.2.item.2", new Dictionary<string, string>(), new VNode[]
                {
                    new VText("$", "1.2.item.2.1"),
                    new VText($"{(item.price.ToString("F2"))}", "1.2.item.2.2")
                }),
                new VElement("button", "1.2.item.3", new Dictionary<string, string> { ["type"] = "button", ["class"] = "remove-item-btn", ["onclick"] = "Handle0" }, "Remove")
            })
        ).ToArray()));
    }

    public void handleRemoveItem()
    {
        constitemToRemove=items[index];constnewItems=items.filter((_:any,i:number)=>i!==index);constnewTotal=total-itemToRemove.price;SetState(nameof(state), 'items',newItems);SetState(nameof(state), 'total',newTotal);
    }

    public void handleClear()
    {
        SetState(nameof(state), 'items',[]);SetState(nameof(state), 'total',0);
    }

    public void Handle0()
    {
        handleRemoveItem(idx);
    }

    public void Handle1()
    {
        handleRemoveItem(idx);
    }

    /// <summary>
    /// Returns JavaScript event handlers for client-side execution
    /// These execute in the browser with bound hook context
    /// </summary>
    protected override Dictionary<string, string> GetClientHandlers()
    {
        return new Dictionary<string, string>
        {
            ["handleRemoveItem"] = @"function () {\n  { constitemToRemove=items[index];constnewItems=items.filter((_:any,i:number)=>i!==index);constnewTotal=total-itemToRemove.price;setState('items',newItems);setState('total',newTotal); };\n}",
            ["handleClear"] = @"function () {\n  { setState('items',[]);setState('total',0); };\n}",
            ["Handle0"] = @"function () {\n  handleRemoveItem(idx);\n}",
            ["Handle1"] = @"function () {\n  handleRemoveItem(idx);\n}"
        };
    }
}

[Component]
public partial class ProductPage : MinimactComponent
{
    protected override VNode Render()
    {
        StateManager.SyncMembersToState(this);

        var _ShoppingCart_items_reader = State["ShoppingCart.items"];
        var _ShoppingCart_total_reader = State["ShoppingCart.total"];

        return MinimactHelpers.createElement("div", null,         new VComponentWrapper
                {
                        ComponentName = "Component",
                        ComponentType = "Component",
                        HexPath = "1.1",
            InitialState = new Dictionary<string, object> {              }
        }, new VElement("div", "1.2", new Dictionary<string, string> { ["class"] = "content" }, new VNode[]
        {
                        new VComponentWrapper
                        {
                                ComponentName = "Component",
                                ComponentType = "Component",
                                HexPath = "1.2.1",
                InitialState = new Dictionary<string, object> {                  }
            },
                        new VComponentWrapper
                        {
                                ComponentName = "Component",
                                ComponentType = "Component",
                                HexPath = "1.2.2",
                InitialState = new Dictionary<string, object> {                  }
            }
        }), new VElement("div", "1.3", new Dictionary<string, string> { ["id"] = "status", ["class"] = "status" }, new VNode[]
        {
            new VElement("p", "1.3.1", new Dictionary<string, string>(), new VNode[]
            {
                new VText("Cart Items:", "1.3.1.1"),
                new VElement("span", "1.3.1.2", new Dictionary<string, string> { ["id"] = "cart-item-count" }, new VNode[]
                {
                    new VText($"{(cartItems.length)}", "1.3.1.2.1")
                })
            }),
            new VElement("p", "1.3.2", new Dictionary<string, string>(), new VNode[]
            {
                new VText("Cart Total: $", "1.3.2.1"),
                new VElement("span", "1.3.2.2", new Dictionary<string, string> { ["id"] = "cart-total-value" }, new VNode[]
                {
                    new VText($"{(cartTotal.ToString("F2"))}", "1.3.2.2.1")
                })
            })
        }));
    }

    public void Handle0()
    {
        handleRemoveItem(idx);
    }

    /// <summary>
    /// Returns JavaScript event handlers for client-side execution
    /// These execute in the browser with bound hook context
    /// </summary>
    protected override Dictionary<string, string> GetClientHandlers()
    {
        return new Dictionary<string, string>
        {
            ["Handle0"] = @"function () {\n  handleRemoveItem(idx);\n}"
        };
    }
}
