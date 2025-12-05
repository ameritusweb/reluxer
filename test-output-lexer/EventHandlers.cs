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
public partial class InteractiveForm : MinimactComponent
{
    protected override VNode Render()
    {
        StateManager.SyncMembersToState(this);

        return MinimactHelpers.createElement("form", null, new VElement("div", "1.1", new Dictionary<string, string> { ["class"] = "form-group" }, new VNode[]
        {
            new VElement("label", "1.1.1", new Dictionary<string, string> { ["for"] = "name" }, "Name:"),
            new VElement("input", "1.1.2", new Dictionary<string, string> { ["id"] = "name", ["type"] = "text", ["onchange"] = "handleNameChange", ["onblur"] = "validateName", ["onfocus"] = "clearError" })
        }), new VElement("div", "1.2", new Dictionary<string, string> { ["class"] = "form-group" }, new VNode[]
        {
            new VElement("label", "1.2.1", new Dictionary<string, string> { ["for"] = "email" }, "Email:"),
            new VElement("input", "1.2.2", new Dictionary<string, string> { ["id"] = "email", ["type"] = "email", ["onchange"] = "handleEmailChange", ["onblur"] = "validateEmail" })
        }), new VElement("div", "1.3", new Dictionary<string, string> { ["class"] = "actions" }, new VNode[]
        {
            new VElement("button", "1.3.1", new Dictionary<string, string> { ["type"] = "submit", ["onclick"] = "handleClick" }, "Submit"),
            new VElement("button", "1.3.2", new Dictionary<string, string> { ["type"] = "button", ["onclick"] = "handleCancel" }, "Cancel")
        }));
    }
}
