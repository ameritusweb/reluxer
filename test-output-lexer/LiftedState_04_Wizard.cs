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
public partial class Step1 : MinimactComponent
{
    protected override VNode Render()
    {
        StateManager.SyncMembersToState(this);

        return MinimactHelpers.createElement("div", null, new VElement("h2", "1.1", new Dictionary<string, string>(), "Step 1: Basic Information"), new VElement("input", "1.2", new Dictionary<string, string> { ["id"] = "step1-name", ["type"] = "text", ["placeholder"] = "Name", ["value"] = $"{(data.name||'')}", ["oninput"] = "Handle0" }), new VElement("input", "1.3", new Dictionary<string, string> { ["id"] = "step1-email", ["type"] = "email", ["placeholder"] = "Email", ["value"] = $"{(data.email||'')}", ["oninput"] = "Handle1" }), (new MObject(complete)) ? new VElement("span", "1.4.1", new Dictionary<string, string> { ["class"] = "check" }, "✓ Complete") : new VNull("1.4"));
    }

    public void handleChange()
    {
        constnewData={...data,[field]:value};SetState(nameof(state), 'data',newData);constisComplete=newData.name&&newData.email;if(isComplete!==complete){SetState(nameof(state), 'complete',isComplete);}
    }

    public void Handle0()
    {
        handleChange('name',e.target.value);
    }

    public void Handle1()
    {
        handleChange('email',e.target.value);
    }

    /// <summary>
    /// Returns JavaScript event handlers for client-side execution
    /// These execute in the browser with bound hook context
    /// </summary>
    protected override Dictionary<string, string> GetClientHandlers()
    {
        return new Dictionary<string, string>
        {
            ["handleChange"] = @"function () {\n  { constnewData={...data,[field]:value};setState('data',newData);constisComplete=newData.name&&newData.email;if(isComplete!==complete){setState('complete',isComplete);} };\n}",
            ["Handle0"] = @"function () {\n  handleChange('name',e.target.value);\n}",
            ["Handle1"] = @"function () {\n  handleChange('email',e.target.value);\n}"
        };
    }
}

[Component]
public partial class Step2 : MinimactComponent
{
    protected override VNode Render()
    {
        StateManager.SyncMembersToState(this);

        return MinimactHelpers.createElement("div", null, new VElement("h2", "1.1", new Dictionary<string, string>(), "Step 2: Address"), new VElement("input", "1.2", new Dictionary<string, string> { ["id"] = "step2-address", ["type"] = "text", ["placeholder"] = "Address", ["value"] = $"{(data.address||'')}", ["oninput"] = "Handle0" }), new VElement("input", "1.3", new Dictionary<string, string> { ["id"] = "step2-city", ["type"] = "text", ["placeholder"] = "City", ["value"] = $"{(data.city||'')}", ["oninput"] = "Handle1" }), (new MObject(complete)) ? new VElement("span", "1.4.1", new Dictionary<string, string> { ["class"] = "check" }, "✓ Complete") : new VNull("1.4"));
    }

    public void handleChange()
    {
        constnewData={...data,[field]:value};SetState(nameof(state), 'data',newData);constisComplete=newData.address&&newData.city;if(isComplete!==complete){SetState(nameof(state), 'complete',isComplete);}
    }

    public void Handle0()
    {
        handleChange('address',e.target.value);
    }

    public void Handle1()
    {
        handleChange('city',e.target.value);
    }

    /// <summary>
    /// Returns JavaScript event handlers for client-side execution
    /// These execute in the browser with bound hook context
    /// </summary>
    protected override Dictionary<string, string> GetClientHandlers()
    {
        return new Dictionary<string, string>
        {
            ["handleChange"] = @"function () {\n  { constnewData={...data,[field]:value};setState('data',newData);constisComplete=newData.address&&newData.city;if(isComplete!==complete){setState('complete',isComplete);} };\n}",
            ["Handle0"] = @"function () {\n  handleChange('address',e.target.value);\n}",
            ["Handle1"] = @"function () {\n  handleChange('city',e.target.value);\n}"
        };
    }
}

[Component]
public partial class Step3 : MinimactComponent
{
    protected override VNode Render()
    {
        StateManager.SyncMembersToState(this);

        return MinimactHelpers.createElement("div", null, new VElement("h2", "1.1", new Dictionary<string, string>(), "Step 3: Payment"), new VElement("input", "1.2", new Dictionary<string, string> { ["id"] = "step3-card", ["type"] = "text", ["placeholder"] = "Card Number", ["value"] = $"{(data.cardNumber||'')}", ["oninput"] = "Handle0" }), new VElement("input", "1.3", new Dictionary<string, string> { ["id"] = "step3-cvv", ["type"] = "text", ["placeholder"] = "CVV", ["value"] = $"{(data.cvv||'')}", ["oninput"] = "Handle1" }), (new MObject(complete)) ? new VElement("span", "1.4.1", new Dictionary<string, string> { ["class"] = "check" }, "✓ Complete") : new VNull("1.4"));
    }

    public void handleChange()
    {
        constnewData={...data,[field]:value};SetState(nameof(state), 'data',newData);constisComplete=newData.cardNumber&&newData.cvv;if(isComplete!==complete){SetState(nameof(state), 'complete',isComplete);}
    }

    public void Handle0()
    {
        handleChange('cardNumber',e.target.value);
    }

    public void Handle1()
    {
        handleChange('cvv',e.target.value);
    }

    /// <summary>
    /// Returns JavaScript event handlers for client-side execution
    /// These execute in the browser with bound hook context
    /// </summary>
    protected override Dictionary<string, string> GetClientHandlers()
    {
        return new Dictionary<string, string>
        {
            ["handleChange"] = @"function () {\n  { constnewData={...data,[field]:value};setState('data',newData);constisComplete=newData.cardNumber&&newData.cvv;if(isComplete!==complete){setState('complete',isComplete);} };\n}",
            ["Handle0"] = @"function () {\n  handleChange('cardNumber',e.target.value);\n}",
            ["Handle1"] = @"function () {\n  handleChange('cvv',e.target.value);\n}"
        };
    }
}

[Component]
public partial class WizardPage : MinimactComponent
{
    protected override VNode Render()
    {
        StateManager.SyncMembersToState(this);

        var _Step1_complete_reader = State["Step1.complete"];
        var _Step2_complete_reader = State["Step2.complete"];
        var _Step3_complete_reader = State["Step3.complete"];

        return MinimactHelpers.createElement("div", null, new VElement("h1", "1.1", new Dictionary<string, string>(), "Setup Wizard"), new VElement("div", "1.2", new Dictionary<string, string> { ["class"] = "progress-bar" }, new VNode[]
        {
            new VElement("div", "1.2.1", new Dictionary<string, string> { ["id"] = "progress-1", ["class"] = $"{($"step {step1Complete ? 'complete' : currentStep === 1 ? 'active' : ''}")}" }, "1"),
            new VElement("div", "1.2.2", new Dictionary<string, string> { ["id"] = "progress-2", ["class"] = $"{($"step {step2Complete ? 'complete' : currentStep === 2 ? 'active' : ''}")}" }, "2"),
            new VElement("div", "1.2.3", new Dictionary<string, string> { ["id"] = "progress-3", ["class"] = $"{($"step {step3Complete ? 'complete' : currentStep === 3 ? 'active' : ''}")}" }, "3")
        }), (new MObject(currentStep>=1)) ?         new VComponentWrapper
                {
                        ComponentName = "Component",
                        ComponentType = "Component",
                        HexPath = "1.3.1",
            InitialState = new Dictionary<string, object> {              }
        } : new VNull("1.3"), (new MObject(currentStep>=2)) ?         new VComponentWrapper
                {
                        ComponentName = "Component",
                        ComponentType = "Component",
                        HexPath = "1.4.1",
            InitialState = new Dictionary<string, object> {              }
        } : new VNull("1.4"), (new MObject(currentStep>=3)) ?         new VComponentWrapper
                {
                        ComponentName = "Component",
                        ComponentType = "Component",
                        HexPath = "1.5.1",
            InitialState = new Dictionary<string, object> {              }
        } : new VNull("1.5"), (new MObject(currentStep===4)) ? new VElement("div", "1.6.1", new Dictionary<string, string> { ["id"] = "completion-message", ["class"] = "completion" }, new VNode[]
        {
            new VElement("h2", "1.6.1.1", new Dictionary<string, string>(), "All Steps Complete!"),
            new VElement("p", "1.6.1.2", new Dictionary<string, string>(), "You have completed all wizard steps."),
            new VElement("button", "1.6.1.3", new Dictionary<string, string> { ["id"] = "complete-btn", ["type"] = "button", ["onclick"] = "handleComplete" }, "Finish")
        }) : new VNull("1.6"), new VElement("div", "1.7", new Dictionary<string, string> { ["class"] = "wizard-nav" }, new VNode[]
        {
            new VElement("button", "1.7.1", new Dictionary<string, string> { ["id"] = "back-btn", ["type"] = "button", ["onclick"] = "handleBack", ["disabled"] = $"{(currentStep===1)}" }, "← Back"),
            new VElement("button", "1.7.2", new Dictionary<string, string> { ["id"] = "next-btn", ["type"] = "button", ["onclick"] = "handleNext", ["disabled"] = $"{(currentStep===4)}" }, "Next →")
        }), new VElement("div", "1.8", new Dictionary<string, string> { ["id"] = "status", ["class"] = "status" }, new VNode[]
        {
            new VElement("p", "1.8.1", new Dictionary<string, string>(), new VNode[]
            {
                new VText("Current Step:", "1.8.1.1"),
                new VElement("span", "1.8.1.2", new Dictionary<string, string> { ["id"] = "current-step" }, new VNode[]
                {
                    new VText($"{(currentStep)}", "1.8.1.2.1")
                })
            }),
            new VElement("p", "1.8.2", new Dictionary<string, string>(), new VNode[]
            {
                new VText("Step 1 Complete:", "1.8.2.1"),
                new VElement("span", "1.8.2.2", new Dictionary<string, string> { ["id"] = "step1-status" }, new VNode[]
                {
                    new VText($"{(step1Complete?'Yes':'No')}", "1.8.2.2.1")
                })
            }),
            new VElement("p", "1.8.3", new Dictionary<string, string>(), new VNode[]
            {
                new VText("Step 2 Complete:", "1.8.3.1"),
                new VElement("span", "1.8.3.2", new Dictionary<string, string> { ["id"] = "step2-status" }, new VNode[]
                {
                    new VText($"{(step2Complete?'Yes':'No')}", "1.8.3.2.1")
                })
            }),
            new VElement("p", "1.8.4", new Dictionary<string, string>(), new VNode[]
            {
                new VText("Step 3 Complete:", "1.8.4.1"),
                new VElement("span", "1.8.4.2", new Dictionary<string, string> { ["id"] = "step3-status" }, new VNode[]
                {
                    new VText($"{(step3Complete?'Yes':'No')}", "1.8.4.2.1")
                })
            })
        }));
    }

    public void handleNext()
    {
        if(currentStep===1&&!step1Complete){SetState(nameof(state), "Step1.complete",true);}elseif(currentStep===2&&!step2Complete){SetState(nameof(state), "Step2.complete",true);}elseif(currentStep===3&&!step3Complete){SetState(nameof(state), "Step3.complete",true);}
    }

    public void handleBack()
    {
        if(currentStep===2){SetState(nameof(state), "Step1.complete",false);}elseif(currentStep===3){SetState(nameof(state), "Step2.complete",false);}elseif(currentStep===4){SetState(nameof(state), "Step3.complete",false);}
    }

    public void handleComplete()
    {
        constallData={step1:state["Step1.data"],step2:state["Step2.data"],step3:state["Step3.data"]};console.log('Wizard completed:',allData);alert('Wizard completed successfully!');
    }

    /// <summary>
    /// Returns JavaScript event handlers for client-side execution
    /// These execute in the browser with bound hook context
    /// </summary>
    protected override Dictionary<string, string> GetClientHandlers()
    {
        return new Dictionary<string, string>
        {
            ["handleNext"] = @"function () {\n  { if(currentStep===1&&!step1Complete){setState(""Step1.complete"",true);}elseif(currentStep===2&&!step2Complete){setState(""Step2.complete"",true);}elseif(currentStep===3&&!step3Complete){setState(""Step3.complete"",true);} };\n}",
            ["handleBack"] = @"function () {\n  { if(currentStep===2){setState(""Step1.complete"",false);}elseif(currentStep===3){setState(""Step2.complete"",false);}elseif(currentStep===4){setState(""Step3.complete"",false);} };\n}",
            ["handleComplete"] = @"function () {\n  { constallData={step1:state[""Step1.data""],step2:state[""Step2.data""],step3:state[""Step3.data""]};console.log('Wizard completed:',allData);alert('Wizard completed successfully!'); };\n}"
        };
    }
}
