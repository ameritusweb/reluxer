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
public partial class PersonalInfoForm : MinimactComponent
{
    protected override VNode Render()
    {
        StateManager.SyncMembersToState(this);

        return MinimactHelpers.createElement("div", null, new VElement("h2", "1.1", new Dictionary<string, string>(), "Personal Information"), new VElement("input", "1.2", new Dictionary<string, string> { ["id"] = "name-input", ["type"] = "text", ["placeholder"] = "Name", ["value"] = $"{(data.name||'')}", ["oninput"] = "Handle0" }), new VElement("input", "1.3", new Dictionary<string, string> { ["id"] = "email-input", ["type"] = "email", ["placeholder"] = "Email", ["value"] = $"{(data.email||'')}", ["oninput"] = "Handle1" }), new VElement("input", "1.4", new Dictionary<string, string> { ["id"] = "phone-input", ["type"] = "tel", ["placeholder"] = "Phone", ["value"] = $"{(data.phone||'')}", ["oninput"] = "Handle2" }), (new MObject(isValid)) ? new VElement("span", "1.5.1", new Dictionary<string, string> { ["class"] = "check-mark" }, "✓") : new VNull("1.5"));
    }

    public void validate(dynamic field, dynamic value)
    {
        var newData = {...data, [field]:value};
        var valid = newData.name&&newData.email&&newData.phone;
        setState('isValid', valid);
        setState('data', newData);
    }

    public void Handle0()
    {
        validate('name',e.target.value);
    }

    public void Handle1()
    {
        validate('email',e.target.value);
    }

    public void Handle2()
    {
        validate('phone',e.target.value);
    }

    /// <summary>
    /// Returns JavaScript event handlers for client-side execution
    /// These execute in the browser with bound hook context
    /// </summary>
    protected override Dictionary<string, string> GetClientHandlers()
    {
        return new Dictionary<string, string>
        {
            ["Handle0"] = @"function () {\n  validate('name',e.target.value);\n}",
            ["Handle1"] = @"function () {\n  validate('email',e.target.value);\n}",
            ["Handle2"] = @"function () {\n  validate('phone',e.target.value);\n}"
        };
    }
}

[Component]
public partial class AddressForm : MinimactComponent
{
    protected override VNode Render()
    {
        StateManager.SyncMembersToState(this);

        return MinimactHelpers.createElement("div", null, new VElement("h2", "1.1", new Dictionary<string, string>(), "Address"), new VElement("input", "1.2", new Dictionary<string, string> { ["id"] = "street-input", ["type"] = "text", ["placeholder"] = "Street", ["value"] = $"{(data.street||'')}", ["oninput"] = "Handle0" }), new VElement("input", "1.3", new Dictionary<string, string> { ["id"] = "city-input", ["type"] = "text", ["placeholder"] = "City", ["value"] = $"{(data.city||'')}", ["oninput"] = "Handle1" }), new VElement("input", "1.4", new Dictionary<string, string> { ["id"] = "zip-input", ["type"] = "text", ["placeholder"] = "ZIP Code", ["value"] = $"{(data.zip||'')}", ["oninput"] = "Handle2" }), (new MObject(isValid)) ? new VElement("span", "1.5.1", new Dictionary<string, string> { ["class"] = "check-mark" }, "✓") : new VNull("1.5"));
    }

    public void validate(dynamic field, dynamic value)
    {
        var newData = {...data, [field]:value};
        var valid = newData.street&&newData.city&&newData.zip;
        setState('isValid', valid);
        setState('data', newData);
    }

    public void Handle0()
    {
        validate('street',e.target.value);
    }

    public void Handle1()
    {
        validate('city',e.target.value);
    }

    public void Handle2()
    {
        validate('zip',e.target.value);
    }

    /// <summary>
    /// Returns JavaScript event handlers for client-side execution
    /// These execute in the browser with bound hook context
    /// </summary>
    protected override Dictionary<string, string> GetClientHandlers()
    {
        return new Dictionary<string, string>
        {
            ["Handle0"] = @"function () {\n  validate('street',e.target.value);\n}",
            ["Handle1"] = @"function () {\n  validate('city',e.target.value);\n}",
            ["Handle2"] = @"function () {\n  validate('zip',e.target.value);\n}"
        };
    }
}

[Component]
public partial class PaymentForm : MinimactComponent
{
    protected override VNode Render()
    {
        StateManager.SyncMembersToState(this);

        return MinimactHelpers.createElement("div", null, new VElement("h2", "1.1", new Dictionary<string, string>(), "Payment"), new VElement("input", "1.2", new Dictionary<string, string> { ["id"] = "card-input", ["type"] = "text", ["placeholder"] = "Card Number", ["value"] = $"{(data.cardNumber||'')}", ["oninput"] = "Handle0" }), new VElement("input", "1.3", new Dictionary<string, string> { ["id"] = "cvv-input", ["type"] = "text", ["placeholder"] = "CVV", ["value"] = $"{(data.cvv||'')}", ["oninput"] = "Handle1" }), new VElement("input", "1.4", new Dictionary<string, string> { ["id"] = "expiry-input", ["type"] = "text", ["placeholder"] = "Expiry (MM/YY)", ["value"] = $"{(data.expiry||'')}", ["oninput"] = "Handle2" }), (new MObject(isValid)) ? new VElement("span", "1.5.1", new Dictionary<string, string> { ["class"] = "check-mark" }, "✓") : new VNull("1.5"));
    }

    public void validate(dynamic field, dynamic value)
    {
        var newData = {...data, [field]:value};
        var valid = newData.cardNumber&&newData.cvv&&newData.expiry;
        setState('isValid', valid);
        setState('data', newData);
    }

    public void Handle0()
    {
        validate('cardNumber',e.target.value);
    }

    public void Handle1()
    {
        validate('cvv',e.target.value);
    }

    public void Handle2()
    {
        validate('expiry',e.target.value);
    }

    /// <summary>
    /// Returns JavaScript event handlers for client-side execution
    /// These execute in the browser with bound hook context
    /// </summary>
    protected override Dictionary<string, string> GetClientHandlers()
    {
        return new Dictionary<string, string>
        {
            ["Handle0"] = @"function () {\n  validate('cardNumber',e.target.value);\n}",
            ["Handle1"] = @"function () {\n  validate('cvv',e.target.value);\n}",
            ["Handle2"] = @"function () {\n  validate('expiry',e.target.value);\n}"
        };
    }
}

[Component]
public partial class RegistrationPage : MinimactComponent
{
    protected override VNode Render()
    {
        StateManager.SyncMembersToState(this);

        var _PersonalInfoForm_isValid_reader = State["PersonalInfoForm.isValid"];
        var _AddressForm_isValid_reader = State["AddressForm.isValid"];
        var _PaymentForm_isValid_reader = State["PaymentForm.isValid"];

        return MinimactHelpers.createElement("div", null, new VElement("h1", "1.1", new Dictionary<string, string>(), "Registration"), (!allValid) ? new VElement("div", "1.2.1", new Dictionary<string, string> { ["id"] = "validation-summary", ["class"] = "validation-summary error" }, new VNode[]
        {
            new VElement("strong", "1.2.1.1", new Dictionary<string, string>(), "Please complete the following sections:"),
            new VElement("ul", "1.2.1.2", new Dictionary<string, string>(), new VNode[]
            {
                new VText($"{(invalidSections.map(section=>(<likey={section}>{section}</li>)))}", "1.2.1.2.1")
            })
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
        }, new VElement("button", "1.6", new Dictionary<string, string> { ["id"] = "submit-btn", ["type"] = "button", ["onclick"] = "handleSubmit", ["disabled"] = $"{(!allValid)}", ["class"] = $"{(allValid?"btn-primary":"btn-disabled")}" }, "Complete Registration"), new VElement("div", "1.7", new Dictionary<string, string> { ["id"] = "status", ["class"] = "status" }, new VNode[]
        {
            new VElement("p", "1.7.1", new Dictionary<string, string>(), new VNode[]
            {
                new VText("Personal Valid:", "1.7.1.1"),
                new VElement("span", "1.7.1.2", new Dictionary<string, string> { ["id"] = "personal-valid" }, new VNode[]
                {
                    new VText($"{(personalValid?'Yes':'No')}", "1.7.1.2.1")
                })
            }),
            new VElement("p", "1.7.2", new Dictionary<string, string>(), new VNode[]
            {
                new VText("Address Valid:", "1.7.2.1"),
                new VElement("span", "1.7.2.2", new Dictionary<string, string> { ["id"] = "address-valid" }, new VNode[]
                {
                    new VText($"{(addressValid?'Yes':'No')}", "1.7.2.2.1")
                })
            }),
            new VElement("p", "1.7.3", new Dictionary<string, string>(), new VNode[]
            {
                new VText("Payment Valid:", "1.7.3.1"),
                new VElement("span", "1.7.3.2", new Dictionary<string, string> { ["id"] = "payment-valid" }, new VNode[]
                {
                    new VText($"{(paymentValid?'Yes':'No')}", "1.7.3.2.1")
                })
            }),
            new VElement("p", "1.7.4", new Dictionary<string, string>(), new VNode[]
            {
                new VText("All Valid:", "1.7.4.1"),
                new VElement("span", "1.7.4.2", new Dictionary<string, string> { ["id"] = "all-valid" }, new VNode[]
                {
                    new VText($"{(allValid?'Yes':'No')}", "1.7.4.2.1")
                })
            })
        }));
    }

    public void handleSubmit()
    {
        if(allValid){constdata={personal:state["PersonalInfoForm.data"],address:state["AddressForm.data"],payment:state["PaymentForm.data"]};console.log('Submitting registration:',data);alert('Registration submitted successfully!');}
    }

    /// <summary>
    /// Returns JavaScript event handlers for client-side execution
    /// These execute in the browser with bound hook context
    /// </summary>
    protected override Dictionary<string, string> GetClientHandlers()
    {
        return new Dictionary<string, string>
        {
            ["handleSubmit"] = @"function () {\n  { if(allValid){constdata={personal:state[""PersonalInfoForm.data""],address:state[""AddressForm.data""],payment:state[""PaymentForm.data""]};console.log('Submitting registration:',data);alert('Registration submitted successfully!');} };\n}"
        };
    }
}
