using Minimact.AspNetCore.Core;
using Minimact.AspNetCore.Extensions;
using MinimactHelpers = Minimact.AspNetCore.Core.Minimact;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minimact.Components;

[LoopTemplate("items", @"{""stateKey"":""items"",""arrayBinding"":""items"",""itemVar"":""user"",""indexVar"":null,""keyBinding"":null,""itemTemplate"":{""type"":""Element"",""tag"":""tr"",""propsTemplates"":null,""childrenTemplates"":[{""type"":""Element"",""tag"":""td"",""propsTemplates"":null,""childrenTemplates"":[{""type"":""Text"",""template"":""{0}"",""bindings"":[""item.id""],""slots"":[0]}]},{""type"":""Element"",""tag"":""td"",""propsTemplates"":null,""childrenTemplates"":[{""type"":""Text"",""template"":""{0}"",""bindings"":[""item.name""],""slots"":[0]}]},{""type"":""Element"",""tag"":""td"",""propsTemplates"":null,""childrenTemplates"":[{""type"":""Text"",""template"":""{0}"",""bindings"":[""item.email""],""slots"":[0]}]},{""type"":""Element"",""tag"":""td"",""propsTemplates"":null,""childrenTemplates"":[{""type"":""Text"",""template"":""{0}"",""bindings"":[""item.role""],""slots"":[0]}]}]}}")]
[Component]
public partial class UserList : MinimactComponent
{

    [ServerTask("serverTask_0")]
    private async Task<List<object>> ServerTask_0(int page, int pageSize, object filters, IProgress<double> progress, CancellationToken cancellationToken)
    {
        var skip = ((page - 1) * pageSize);
        var take = pageSize;
        var allUsers = generateMockUsers(100);
        var filtered = allUsers;
        if (MinimactHelpers.ToBool(filters.role))
        {
            filtered = filtered.Where((u) => (u.role == filters.role));
            
        }
        if (MinimactHelpers.ToBool(filters.search))
        {
            var searchLower = filters.search.ToLower();
            filtered = filtered.Where((u) => (u.name.ToLower().Contains(searchLower) || u.email.ToLower().Contains(searchLower)));
            
        }
        return filtered.Skip(skip, (skip + take));

    }

    [ServerTask("serverTask_1")]
    private async Task<int> ServerTask_1(object filters, IProgress<double> progress, CancellationToken cancellationToken)
    {
        var allUsers = generateMockUsers(100);
        var filtered = allUsers;
        if (MinimactHelpers.ToBool(filters.role))
        {
            filtered = filtered.Where((u) => (u.role == filters.role));
            
        }
        if (MinimactHelpers.ToBool(filters.search))
        {
            var searchLower = filters.search.ToLower();
            filtered = filtered.Where((u) => (u.name.ToLower().Contains(searchLower) || u.email.ToLower().Contains(searchLower)));
            
        }
        return filtered.length;

    }
    protected override VNode Render()
    {
        StateManager.SyncMembersToState(this);

        return MinimactHelpers.createElement("div", new { className = "user-list" }, new VElement("h1", "1.1", new Dictionary<string, string>(), "User Directory"), new VElement("p", "1.2", new Dictionary<string, string>(), $"Page{(users.page)}of{(users.totalPages)}({(users.total)}total users)"), (new MObject(users.pending)) ? new VElement("div", "1.3.1", new Dictionary<string, string> { ["class"] = "loading" }, "Loading...") : new VNull("1.3"), (new MObject(users.error)) ? new VElement("div", "1.4.1", new Dictionary<string, string> { ["class"] = "error" }, $"Error:{(users.error)}") : new VNull("1.4"), new VElement("table", "1.5", new Dictionary<string, string>(), new VNode[]
            {
                new VElement("thead", "1.5.1", new Dictionary<string, string>(), new VNode[]
                {
                    new VElement("tr", "1.5.1.1", new Dictionary<string, string>(), new VNode[]
                    {
                        new VElement("th", "1.5.1.1.1", new Dictionary<string, string>(), "ID"),
                        new VElement("th", "1.5.1.1.2", new Dictionary<string, string>(), "Name"),
                        new VElement("th", "1.5.1.1.3", new Dictionary<string, string>(), "Email"),
                        new VElement("th", "1.5.1.1.4", new Dictionary<string, string>(), "Role")
                    })
                }),
                MinimactHelpers.createElement("tbody", null, ((IEnumerable<dynamic>)users.items).Select((Func<dynamic, dynamic>)(user => new VElement("tr", "1.5.2.1.1", new Dictionary<string, string>(), new VNode[]
                    {
                        new VElement("td", "1.5.2.1.1.1", new Dictionary<string, string>(), new VNode[]
                        {
                            new VText($"{(user.id)}", "1.5.2.1.1.1.1")
                        }),
                        new VElement("td", "1.5.2.1.1.2", new Dictionary<string, string>(), new VNode[]
                        {
                            new VText($"{(user.name)}", "1.5.2.1.1.2.1")
                        }),
                        new VElement("td", "1.5.2.1.1.3", new Dictionary<string, string>(), new VNode[]
                        {
                            new VText($"{(user.email)}", "1.5.2.1.1.3.1")
                        }),
                        new VElement("td", "1.5.2.1.1.4", new Dictionary<string, string>(), new VNode[]
                        {
                            new VText($"{(user.role)}", "1.5.2.1.1.4.1")
                        })
                    }))).ToArray())
            }), new VElement("div", "1.6", new Dictionary<string, string> { ["class"] = "pagination" }, new VNode[]
            {
                new VElement("button", "1.6.1", new Dictionary<string, string> { ["disabled"] = $"{!users.hasPrev}", ["onclick"] = "Handle0" }, "Previous"),
                new VElement("span", "1.6.2", new Dictionary<string, string>(), $"Page{(users.page)}"),
                new VElement("button", "1.6.3", new Dictionary<string, string> { ["disabled"] = $"{!users.hasNext}", ["onclick"] = "Handle2" }, "Next"),
                new VElement("button", "1.6.4", new Dictionary<string, string> { ["onclick"] = "Handle4" }, "First"),
                new VElement("button", "1.6.5", new Dictionary<string, string> { ["onclick"] = "Handle6" }, "Last"),
                new VElement("button", "1.6.6", new Dictionary<string, string> { ["onclick"] = "Handle8" }, "Refresh")
            }));
    }

    public void Handle0()
    {
        users.prev();
    }

    public void Handle2()
    {
        users.next();
    }

    public void Handle4()
    {
        users.goto(1);
    }

    public void Handle6()
    {
        users.goto(users.totalPages);
    }

    public void Handle8()
    {
        users.refresh();
    }

    /// <summary>
    /// Returns JavaScript event handlers for client-side execution
    /// These execute in the browser with bound hook context
    /// </summary>
    protected override Dictionary<string, string> GetClientHandlers()
    {
        return new Dictionary<string, string>
        {
            ["Handle0"] = @"function () {\n  users.prev();\n}",
            ["Handle2"] = @"function () {\n  users.next();\n}",
            ["Handle4"] = @"function () {\n  users.goto(1);\n}",
            ["Handle6"] = @"function () {\n  users.goto(users.totalPages);\n}",
            ["Handle8"] = @"function () {\n  users.refresh();\n}"
        };
    }

    // Helper function: generateMockUsers
    private static dynamic generateMockUsers(dynamic count)
    {
        var roles = new List<string> { "Admin", "User", "Editor", "Viewer" };
        var firstNames = new List<string> { "Alice", "Bob", "Charlie", "Diana", "Eve", "Frank", "Grace", "Henry" };
        var lastNames = new List<string> { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis" };
        var users = new List<dynamic> {  };
        new VNull("");
        return users;
    }
}
