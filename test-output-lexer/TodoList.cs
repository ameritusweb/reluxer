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
public partial class TodoList : MinimactComponent
{
    protected override VNode Render()
    {
        StateManager.SyncMembersToState(this);

        return MinimactHelpers.createElement("div", null, new VElement("h1", "1.1", new Dictionary<string, string>(), "My Todos"), new VElement("ul", "1.2", new Dictionary<string, string>(), new VNode[]
        {
                        MinimactHelpers.createFragment(todos.Select((todo) =>
                new VElement("li", "1.2.1.item", new Dictionary<string, string> { ["key"] = $"{(todo.id)}" }, new VNode[]
                {
                    new VElement("input", "1.2.1.item.1", new Dictionary<string, string> { ["type"] = "checkbox", ["checked"] = $"{(todo.completed)}" }),
                    new VElement("span", "1.2.1.item.2", new Dictionary<string, string> { ["class"] = $"{(todo.completed?"completed":"")}" }, new VNode[]
                    {
                        new VText($"{(todo.text)}", "1.2.1.item.2.1")
                    }),
                    new VElement("button", "1.2.1.item.3", new Dictionary<string, string> { ["onclick"] = "Handle0" }, "Delete")
                })
            ).ToArray())
        }), new VElement("button", "1.3", new Dictionary<string, string> { ["onclick"] = "addTodo" }, "Add Todo"));
    }

    public void Handle0()
    {
        deleteTodo(todo.id);
    }

    public void Handle1()
    {
        deleteTodo(todo.id);
    }

    /// <summary>
    /// Returns JavaScript event handlers for client-side execution
    /// These execute in the browser with bound hook context
    /// </summary>
    protected override Dictionary<string, string> GetClientHandlers()
    {
        return new Dictionary<string, string>
        {
            ["Handle0"] = @"function () {\n  deleteTodo(todo.id);\n}",
            ["Handle1"] = @"function () {\n  deleteTodo(todo.id);\n}"
        };
    }
}
